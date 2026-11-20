# Giu's Crew — Hoja de ruta

Este documento es la referencia de trabajo del proyecto. Se actualiza a medida que cada etapa queda revisada y validada.

## 1. Domain

### 1.1 Revisar y cerrar las entidades existentes

- [ ] **Customer**
  - [ ] Corregir `UpdateInformation` para validar todos los datos antes de modificar la entidad.
  - [ ] Revisar normalización y validación de nombre, email y teléfono.
  - [ ] Confirmar reglas y límites de cada dato.
- [ ] **Address**
  - [ ] Revisar validaciones y nombres de constantes/parámetros.
  - [ ] Definir cómo se garantiza que un cliente tenga como máximo una dirección predeterminada.
  - [ ] Definir si una dirección puede modificarse después de creada.
- [ ] **Category**
  - [ ] Definir límites máximos de nombre y descripción.
  - [ ] Revisar reglas de activación y orden de visualización.
  - [ ] Definir dónde se controla la unicidad del nombre.
- [ ] **Product**
  - [ ] Definir límites máximos de nombre y descripción.
  - [ ] Aclarar la diferencia entre `IsActive` e `IsAvailable` y sus combinaciones válidas.
  - [ ] Revisar reglas de precio, categoría y orden.
- [ ] **Order**
  - [ ] Corregir la validación de cantidad en `AddItem` y ejecutarla antes de buscar un ítem existente.
  - [ ] Revisar todas las transiciones de estado y sus restricciones.
  - [ ] Verificar que los estados del enum tengan operaciones correspondientes (por ejemplo, entrega).
  - [ ] Definir desde qué estados se permite cancelar un pedido.
  - [ ] Revisar el cálculo del total y la protección de la colección de ítems.
- [ ] **OrderItem**
  - [ ] Revisar validaciones de producto, precio, nombre y cantidad.
  - [ ] Confirmar que nombre y precio sean snapshots del momento de compra.
  - [ ] Revisar que la creación y modificación respeten las reglas de `Order`.
- [ ] **Payment**
  - [ ] Revisar transiciones permitidas para efectivo y Mercado Pago.
  - [ ] Mantener el ciclo de vida del pago separado del estado del pedido.
  - [ ] Usar una misma marca temporal para cada cambio de estado y su historial.
  - [ ] Revisar cancelaciones y estados terminales.
- [ ] **PaymentHistory**
  - [ ] Revisar integridad de los registros y su creación únicamente desde `Payment`.
  - [ ] Confirmar timestamps y datos registrados por transición.

### 1.2 Entidad pendiente

- [ ] **Receipt**
  - [ ] Definir qué representa el comprobante y cuándo se genera.
  - [ ] Definir su relación con pedido y pago.
  - [ ] Definir los datos históricos que debe conservar.
  - [ ] Implementar sus invariantes y comportamiento.

### 1.3 Entidades complementarias a evaluar e implementar

Implementar solo después de definir sus reglas y relaciones con las entidades principales.

- [ ] **Claim**
- [ ] **Compensation**
- [ ] **LoyaltyAccount**
- [ ] **LoyaltyTransaction**
- [ ] **Reward**
- [ ] **RewardRedemption**
- [ ] **Employee**
- [ ] **EmployeeBenefitRedemption**
- [ ] **CustomerSession**
- [ ] **NotificationPreference**
- [ ] **Notification**
- [ ] **EmailVerification**
- [ ] **PhoneVerification**
- [ ] **Refund**
- [ ] **RefundHistory**

### 1.4 Revisión integral de Domain

- [ ] Revisar todos los enums y sus transiciones.
- [ ] Revisar relaciones, identificadores y responsabilidades entre entidades.
- [ ] Revisar invariantes, encapsulamiento y consistencia ante errores.
- [ ] Identificar Value Objects que realmente aporten valor (sin agregarlos por anticipado).
- [ ] Revisar dependencias: Domain no debe depender de Infrastructure, EF Core ni API.
- [ ] Confirmar que el proyecto Domain compile.
- [ ] Dar por cerrada la etapa de diseño de Domain.

## 2. Tests unitarios de Domain

Los tests comienzan una vez finalizada la revisión de Domain.

- [ ] Revisar la estructura del proyecto `Burger.Domain.Tests`.
- [ ] Definir convenciones para nombres, organización y datos de prueba.
- [ ] Revisar factories y helpers para evitar duplicación innecesaria.
- [ ] Crear tests de escenarios válidos, inválidos y límites para cada entidad.
- [ ] Verificar que los errores no dejen entidades en estados parcialmente modificados.
- [ ] Ejecutar `dotnet test` y corregir los fallos.
- [ ] Revisar cobertura de reglas importantes sin perseguir tests redundantes.

## 3. Application

- [ ] Definir casos de uso y límites de cada operación.
- [ ] Crear DTOs de entrada y salida donde correspondan.
- [ ] Definir interfaces/contratos necesarios para persistencia y servicios externos.
- [ ] Implementar casos de uso de clientes, catálogo, direcciones y pedidos.
- [ ] Implementar coordinación de pagos y reembolsos.
- [ ] Implementar notificaciones y operaciones complementarias según alcance.
- [ ] Validar reglas de aplicación sin trasladar reglas de negocio fuera de Domain.
- [ ] Crear tests unitarios de Application.

## 4. Infrastructure

- [ ] Elegir y configurar PostgreSQL.
- [ ] Configurar EF Core y el proveedor correspondiente.
- [ ] Definir mappings de entidades y relaciones.
- [ ] Implementar repositorios/consultas según los contratos de Application.
- [ ] Configurar migraciones y crear el esquema inicial.
- [ ] Preparar una base de datos de desarrollo/prueba.
- [ ] Cargar datos de prueba controlados.
- [ ] Verificar altas, consultas, modificaciones y transacciones.
- [ ] Inspeccionar los datos y movimientos con una herramienta de base de datos.
- [ ] Crear tests de integración de persistencia.

## 5. API

- [ ] Definir endpoints y contratos HTTP.
- [ ] Implementar autenticación y autorización según roles.
- [ ] Conectar endpoints con casos de uso de Application.
- [ ] Definir respuestas, códigos HTTP y manejo consistente de errores.
- [ ] Documentar la API con OpenAPI/Swagger.
- [ ] Probar endpoints y validaciones HTTP.
- [ ] Implementar y probar integración con Mercado Pago, incluyendo notificaciones/webhooks.
- [ ] Verificar que los secretos y credenciales no queden en el repositorio.

## 6. Frontend e integración

- [ ] Definir la estructura y tecnología del frontend.
- [ ] Implementar catálogo y navegación de productos.
- [ ] Implementar carrito y checkout.
- [ ] Integrar el frontend con la API mediante HTTP/JSON.
- [ ] Implementar selección de pago (Mercado Pago o efectivo).
- [ ] Implementar confirmación y seguimiento del pedido.
- [ ] Implementar interfaz administrativa para recibir y gestionar pedidos.
- [ ] Implementar notificación posterior a la compra.
- [ ] Probar flujos completos de cliente y administración.
- [ ] Realizar pruebas end-to-end.

## 7. Criterio de avance

- Trabajar una entidad o caso de uso por vez.
- Entender la responsabilidad y las reglas antes de escribir código.
- Explicar las decisiones de diseño y las herramientas utilizadas.
- No avanzar de etapa hasta revisar y cerrar la anterior.
- Mantener los tests alineados con el comportamiento esperado.
- No agregar complejidad o entidades sin una necesidad del negocio.
- Registrar aquí los avances y decisiones importantes.

## Arquitectura objetivo

- **Domain:** entidades, Value Objects, enums e invariantes del negocio.
- **Application:** casos de uso, coordinación, DTOs y contratos.
- **Infrastructure:** base de datos, EF Core, repositorios e integraciones técnicas.
- **API:** endpoints, HTTP, autenticación y documentación.
- **Frontend:** experiencia de cliente y administración, consumiendo la API.

La base de datos de prueba y la visualización de movimientos se implementarán en la etapa de Infrastructure; antes podemos usar ejemplos controlados para comprender el comportamiento del dominio, sin adelantar la configuración de persistencia.
