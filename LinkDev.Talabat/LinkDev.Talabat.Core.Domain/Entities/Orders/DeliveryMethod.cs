namespace LinkDev.Talabat.Core.Domain.Entities.Orders
{
    //table
    public class DeliveryMethod : BaseAuditableEntity<int>
    {
        public DeliveryMethod()
        {
            
        }
        public required string ShortName { get; set; }
        public required string Description { get; set; }
        public decimal Cost { get; set; }
        public required string DeliveryTime { get; set; }



    }
}
