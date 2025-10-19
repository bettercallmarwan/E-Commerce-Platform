using LinkDev.Talabat.Core.Domain.Entities.Orders;

public class OrderItem : BaseAuditableEntity<int>
{

    public virtual ProductItemOrdered product { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
