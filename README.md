# Giu's Crew

> **Una crew, un sabor, una pasión.**

Plataforma digital para gestionar una hamburguesería enfocada en smash burgers. El objetivo es construir una aplicación web moderna que permita a los clientes consultar el menú, realizar pedidos, pagar, seguir sus compras y acceder a beneficios; mientras que el personal del local podrá administrar pedidos, productos, pagos, reclamos y beneficios.

Este README documenta las decisiones tomadas hasta el momento. Las reglas que todavía no están cerradas se identifican como **pendientes de definición**.

---

## Índice

- [Objetivos del proyecto](#objetivos-del-proyecto)
- [Tecnologías y arquitectura](#tecnologías-y-arquitectura)
- [Funcionalidades previstas](#funcionalidades-previstas)
- [Modelo de dominio](#modelo-de-dominio)
- [Pedidos y pagos](#pedidos-y-pagos)
- [Cancelaciones y reembolsos](#cancelaciones-y-reembolsos)
- [Reclamos y compensaciones](#reclamos-y-compensaciones)
- [Programa de puntos](#programa-de-puntos)
- [Beneficios para empleados](#beneficios-para-empleados)
- [Cuentas y seguridad](#cuentas-y-seguridad)
- [Estructura actual del backend](#estructura-actual-del-backend)
- [Pruebas](#pruebas)
- [Estado y próximos pasos](#estado-y-próximos-pasos)

---

## Objetivos del proyecto

Giu's Crew busca ofrecer:

### Para clientes

- Consultar el menú, categorías y productos disponibles.
- Crear una cuenta y gestionar sus datos.
- Realizar pedidos.
- Elegir entre Mercado Pago y efectivo.
- Consultar el estado de sus pedidos.
- Consultar el historial de pedidos y pagos.
- Solicitar ayuda o presentar reclamos sobre una compra.
- Recibir compensaciones cuando corresponda.
- Acumular puntos por compras completadas.
- Canjear puntos por descuentos o productos gratis.

### Para el local y la administración

- Recibir y gestionar pedidos.
- Administrar productos, categorías y disponibilidad.
- Controlar los estados de los pedidos y pagos.
- Gestionar reclamos y decidir compensaciones.
- Administrar recompensas del programa de puntos.
- Gestionar empleados y sus beneficios.
- Consultar historiales y registros de operaciones.

---

## Tecnologías y arquitectura

### Tecnologías elegidas hasta ahora

- **Backend:** .NET 10 / C#
- **Pruebas de dominio:** xUnit
- **Control de versiones:** Git y GitHub
- **Base de datos:** pendiente de selección.
- **Frontend:** pendiente de selección.
- **Proveedor de pagos:** Mercado Pago y efectivo.

### Arquitectura

El backend se organiza en capas, siguiendo una separación inspirada en Clean Architecture:

| Proyecto | Responsabilidad |
|---|---|
| `Burger.Domain` | Entidades, enumeraciones y reglas centrales del negocio. |
| `Burger.Application` | Casos de uso, coordinación y contratos de aplicación. |
| `Burger.Infrastructure` | Persistencia, servicios externos e implementaciones técnicas. |
| `Burger.API` | Endpoints HTTP y entrada a la aplicación. |

La capa Domain no debe depender de la API, de la base de datos ni de proveedores externos. Las reglas importantes del negocio deben protegerse desde el dominio, no depender exclusivamente de validaciones visuales del frontend.

---

## Funcionalidades previstas

1. Catálogo de productos y categorías.
2. Registro y acceso de clientes.
3. Gestión de direcciones.
4. Carrito y creación de pedidos.
5. Pagos con Mercado Pago o efectivo.
6. Seguimiento del pedido y notificaciones.
7. Historial de pedidos y pagos.
8. Cancelaciones y reembolsos.
9. Reclamos y compensaciones.
10. Programa de puntos y recompensas.
11. Beneficios para empleados.
12. Panel de administración y gestión del local.

---

## Modelo de dominio

Las entidades que ya se trabajaron o se identificaron para el dominio son las siguientes.

### Entidades principales

| Entidad | Responsabilidad |
|---|---|
| `Customer` | Representa al cliente. |
| `Address` | Dirección asociada a un cliente. |
| `Product` | Producto ofrecido por el local. |
| `Category` | Clasificación de productos. |
| `Order` | Pedido realizado por un cliente. |
| `OrderItem` | Producto, cantidad y precio registrado dentro de un pedido. |
| `Payment` | Un intento u operación de pago asociada a un pedido. |
| `PaymentHistory` | Historial de cambios de estado de un pago. |

### Criterios del dominio

- Las entidades protegen sus invariantes mediante constructores y métodos controlados.
- Las propiedades que no deben modificarse libremente utilizan setters privados.
- Los identificadores de las entidades son `Guid`.
- Los importes monetarios se representan con `decimal`.
- Los registros históricos deben conservar lo que ocurrió, aunque después cambien los datos actuales del catálogo.
- Los estados de pedido y de pago son independientes: un pedido no es un pago.
- Un mismo pedido puede tener distintos intentos de pago. Cada intento conserva su propio identificador e historial.
- Los métodos de cambio de estado deben validar las transiciones permitidas.

### Enumeraciones identificadas

- `OrderStatus`
- `PaymentStatus`
- `PaymentMethod`

Los valores concretos y las transiciones deben mantenerse coherentes con las reglas implementadas en el código.

---

## Pedidos y pagos

### Métodos de pago

Se contemplan:

- **Mercado Pago:** la aprobación debe confirmarse a partir de una notificación del proveedor validada de forma segura. No se debe confiar en que el frontend informe que el pago fue aprobado.
- **Efectivo:** el cliente selecciona efectivo al realizar el pedido; el pago queda pendiente hasta que el personal confirme la recepción del dinero.

`PaymentMethod` identifica cómo se paga. `PaymentStatus` identifica en qué estado se encuentra la operación.

### Estados de pago previstos

- `Pending`
- `Processing`
- `Approved`
- `Rejected`
- `Cancelled`
- `PartiallyRefunded`
- `Refunded`

Los estados de reembolso ya están contemplados en el enum de diseño, pero sus operaciones todavía no están implementadas.

### Historial de pagos

Cada `Payment` conserva un historial de cambios mediante `PaymentHistory`. El historial registra, entre otros datos:

- Estado anterior.
- Estado nuevo.
- Descripción del cambio.
- Fecha de creación.

No se debe borrar el historial al cambiar el estado.

### Reintentos

Si un intento de pago falla y el cliente vuelve a intentarlo, se crea una nueva instancia de `Payment` asociada al mismo pedido. No se reutiliza ni se altera el importe histórico del intento anterior.

---

## Cancelaciones y reembolsos

Cancelar un pago pendiente y devolver dinero de un pago aprobado son operaciones distintas.

- La cancelación corresponde a una operación que todavía no fue cobrada o confirmada.
- El reembolso devuelve dinero de una operación ya aprobada.
- Un reembolso debe modelarse como una operación independiente, no como una simple cancelación del pago.
- El resultado del reembolso debe confirmarse con el proveedor cuando corresponda.
- Debe conservarse el historial de la operación.

### Política comercial conversada

Se analizaron solicitudes voluntarias de cancelación o devolución según el avance del pedido:

- Antes de que el local comience a preparar el pedido: se conversó una devolución del 95%.
- Cuando el pedido ya está completo/listo: se conversó una devolución de entre el 88% y el 90%.
- Para pedidos que ya están en preparación: la regla sigue pendiente de definición.

**Importante:** estos porcentajes son ideas comerciales en evaluación, no una regla legal definitiva ni una autorización automática para retener dinero. Cualquier política debe respetar los derechos del consumidor y las obligaciones aplicables. Los costos reales del medio de pago deben analizarse por separado.

Todavía no se implementó una entidad `Refund`.

---

## Reclamos y compensaciones

El cliente podrá presentar un reclamo asociado a una compra desde su historial.

Ejemplos conversados:

- Poco bacon: ofrecer bacon adicional en una próxima compra.
- Poca carne: ofrecer carne adicional en una próxima compra.
- Hamburguesa fría: evaluar un descuento del 10% en una próxima compra.

La administración podrá revisar el caso y elegir una compensación adecuada. La compensación no debe reemplazar una solución o devolución que legalmente corresponda.

### Entidades propuestas

#### `Claim`

Representa el reclamo del cliente. Atributos candidatos:

- `Id`
- `CustomerId`
- `OrderId`
- `Description`
- `Status`
- `CreatedAt`
- `ResolvedAt`

#### `Compensation`

Representa la compensación otorgada por un reclamo. Atributos candidatos:

- `Id`
- `ClaimId`
- `CustomerId`
- `Type`
- `Value`
- `Status`
- `ExpirationDate`
- `RedeemedAt`

Tipos posibles: producto adicional, producto gratis, descuento u otra solución autorizada.

Estas entidades y sus reglas todavía no están implementadas.

---

## Programa de puntos

El programa de puntos es para clientes y se mantiene separado de los beneficios internos de empleados.

### Reglas acordadas

1. El cliente gana **1 punto por cada $100 efectivamente pagados**.
2. El cálculo se realiza sobre el importe después de aplicar descuentos.
3. Los importes inferiores a $100 se acumulan para futuras compras.
4. Los puntos no vencen.
5. Los puntos se acreditan cuando el pedido se completa, no simplemente cuando se paga.
6. Se podrán canjear puntos por descuentos y productos gratis.
7. Cada movimiento debe quedar registrado.
8. Si una compra que generó puntos se reembolsa, se debe evaluar y registrar el ajuste correspondiente sin borrar el historial.
9. Un pedido no debe generar puntos más de una vez.
10. No se pueden canjear más puntos de los disponibles.

### Ejemplo de acumulación

- Compra elegible de $15.950: se acreditan 159 puntos y quedan $50 acumulados.
- Compra elegible posterior de $7.080: se suman los $50 anteriores y se consideran $7.130.
- Se acreditan 71 puntos adicionales y quedan $30 acumulados.

El saldo de puntos de estas dos compras sería de 230 puntos. Los importes acumulados se mantienen para el cálculo de compras futuras.

### Entidades propuestas

#### `LoyaltyAccount`

Cuenta de puntos del cliente.

Atributos candidatos:

- `Id`
- `CustomerId`
- `Balance`
- `AccumulatedAmount`
- `CreatedAt`
- `UpdatedAt`

`AccumulatedAmount` representa el importe restante que todavía no alcanzó otros $100 para generar un punto.

#### `LoyaltyTransaction`

Historial inmutable de movimientos de puntos.

Atributos candidatos:

- `Id`
- `LoyaltyAccountId`
- `OrderId` (opcional, según el tipo de movimiento)
- `Type`
- `Points`
- `Description`
- `CreatedAt`

Debe permitir identificar puntos ganados, canjeados y ajustes.

#### `Reward`

Recompensa disponible para canjear.

Atributos candidatos:

- `Id`
- `Name`
- `Description`
- `Type`
- `PointsCost`
- `DiscountPercentage` (opcional)
- `DiscountAmount` (opcional)
- `ProductId` (opcional)
- `IsActive`
- `CreatedAt`

#### `RewardRedemption`

Registro de un canje.

Atributos candidatos:

- `Id`
- `LoyaltyAccountId`
- `RewardId`
- `PointsSpent`
- `Status`
- `RedeemedAt`
- `ExpiresAt` (opcional)
- `UsedAt` (opcional)

### Pendiente de definición

- Valores de puntos necesarios para cada recompensa.
- Reglas de combinación de recompensas con otros descuentos.
- Tratamiento exacto de puntos ante reembolsos parciales.
- Si los puntos se calculan sobre todos los componentes cobrados del pedido o si hay otros conceptos que se excluyen.

---

## Beneficios para empleados

Los beneficios de empleados son independientes del programa de puntos de clientes.

### Descuento personal del 35%

Reglas acordadas:

1. El descuento es exclusivamente para consumo personal del empleado.
2. Se calcula sobre el subtotal total de los productos elegibles del pedido, no como un beneficio independiente por unidad.
3. Se aplica a todos los productos elegibles de ese pedido.
4. Se excluyen promociones y combos.
5. No se aplica a productos gratuitos.
6. No se aplica al costo de delivery.
7. El descuento no modifica el precio original del producto ni el precio histórico guardado en el pedido.

**Ejemplo:**

Dos hamburguesas elegibles de $10.000 cada una:

- Subtotal elegible: $20.000.
- Descuento del 35%: $7.000.
- Total de esos productos: $13.000.

### Hamburguesa gratuita mensual

Reglas acordadas:

1. Cada empleado puede utilizar el beneficio una vez por mes.
2. Consiste en una hamburguesa mediana gratuita.
3. Incluye sus acompañantes chicos.
4. Puede solicitarse en cualquier momento del mes.
5. El beneficio es para retirar en el local; no incluye delivery.
6. El producto gratuito no recibe además el descuento del 35%.
7. Puede combinarse en un mismo pedido con la compra de otros productos.
8. Si el empleado compra otros productos elegibles, el 35% se calcula sobre el subtotal de esos productos, excluyendo el producto gratuito.

### Ejemplo de uso combinado

Un empleado solicita su hamburguesa mediana gratuita con acompañantes chicos y compra otros productos.

- La hamburguesa y sus acompañantes se registran como beneficio gratuito.
- Los demás productos elegibles reciben el descuento del 35%.
- El envío no se incluye porque el beneficio gratuito es para retirar en el local.

### Entidades propuestas

#### `Employee`

Representa a un empleado.

Atributos candidatos:

- `Id`
- `UserId`
- `FirstName`
- `LastName`
- `IsActive`
- `HiredAt`

El 35% es una política general del negocio; no es necesario guardar el mismo porcentaje repetido en cada empleado.

#### `EmployeeBenefitRedemption`

Registra el uso de un beneficio.

Atributos candidatos:

- `Id`
- `EmployeeId`
- `OrderId`
- `BenefitType`
- `Period`
- `RedeemedAt`

El registro debe permitir comprobar que el beneficio mensual no se utilizó anteriormente en el mismo período.

### Enumeración propuesta

`EmployeeBenefitType`, para identificar el tipo de beneficio utilizado.

---

## Cuentas y seguridad

Se planteó la siguiente regla:

- Una cuenta no debe poder utilizarse simultáneamente desde varios dispositivos.

**Pendiente de precisión:** confirmar si esta restricción se aplica a todas las cuentas (clientes, empleados y administradores) o solamente a las cuentas de empleados.

La restricción debe aplicarse en el backend mediante gestión de sesiones o dispositivos autorizados; no alcanza con ocultar funciones en el frontend. También habrá que definir qué ocurre cuando el usuario cambia de dispositivo, pierde el equipo o necesita cerrar una sesión anterior.

El descuento del empleado es personal y no puede utilizarse para compras destinadas a otras personas.

---

## Estructura actual del backend

La solución se encuentra dentro de `backend/`:

```text
backend/
├── BurgerPlatform.slnx
├── src/
│   ├── Burger.API/
│   ├── Burger.Application/
│   ├── Burger.Domain/
│   │   ├── Entities/
│   │   └── Enums/
│   └── Burger.Infrastructure/
└── tests/
    └── Burger.Domain.Tests/
        └── UnitTest1.cs
```

La estructura de `Burger.Domain` se irá ampliando a medida que se implementen las reglas acordadas. Las entidades de puntos, recompensas, empleados, reclamos, compensaciones y reembolsos todavía son diseños propuestos, no necesariamente clases existentes en el código.

---

## Pruebas

Se utiliza **xUnit** para pruebas automatizadas.

El proyecto de pruebas de dominio está ubicado en:

```text
backend/tests/Burger.Domain.Tests/
```

Se ejecutan desde `backend/` con:

```bash
dotnet test
```

Las pruebas de Domain verifican reglas del negocio sin necesitar levantar la API ni conectarse a una base de datos.

A medida que avance el proyecto se incorporarán pruebas para:

- Entidades y validaciones.
- Transiciones de estado.
- Casos de uso de Application.
- Persistencia e integraciones de Infrastructure.
- Endpoints de API.
- Flujos completos de usuario (E2E), cuando corresponda.

---

## Estado y próximos pasos

### Trabajado hasta ahora

- [x] Solución .NET 10 y proyectos principales creados.
- [x] Entidades iniciales de productos, categorías, clientes, direcciones, pedidos y pagos trabajadas.
- [x] Historial de cambios de estado de pagos diseñado e implementado.
- [x] Proyecto de pruebas de dominio con xUnit creado.
- [x] Primeros tests de Payment ejecutados correctamente.
- [x] Métodos de pago: efectivo y Mercado Pago definidos.
- [x] Reglas iniciales del programa de puntos acordadas.
- [x] Reglas iniciales de beneficios para empleados acordadas.

### Diseñado, pero todavía pendiente de implementar o terminar de definir

- [ ] Terminar y probar las entidades del dominio existentes.
- [ ] Definir por completo los estados y transiciones de pedidos.
- [ ] Definir reglas completas de cancelación y reembolso.
- [ ] Diseñar e implementar `Refund`.
- [ ] Definir estados y flujo de reclamos y compensaciones.
- [ ] Definir catálogo y costos en puntos de las recompensas.
- [ ] Diseñar e implementar las entidades de fidelización.
- [ ] Diseñar e implementar empleados y beneficios.
- [ ] Definir el alcance exacto de la regla de una sesión por cuenta.
- [ ] Elegir base de datos y frontend.
- [ ] Implementar casos de uso, persistencia, API e interfaz.

### Criterio de desarrollo

El proyecto se desarrolla paso a paso. Primero se terminan de entender y definir las reglas del dominio; después se implementan las entidades y sus pruebas; luego se avanza con Application, Infrastructure y API. No se deben agregar entidades solamente por anticipación sin comprender qué responsabilidad tienen y cómo se relacionan con el resto del sistema.

---

## Nota

Este documento es una fotografía del diseño actual de Giu's Crew. Cuando una regla comercial cambie, se debe actualizar el README y mantener el código y las pruebas alineados con la decisión más reciente.
