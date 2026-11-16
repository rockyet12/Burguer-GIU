namespace Burger.Domain.Enums;

public enum PaymentStatus
{
    Pending = 1,
    Proccesing = 2,
    Approved = 3,
    Rejected = 4,
    Cancelled = 5,
    PartiallyRefunded = 6,
    Refunded = 7,
}
