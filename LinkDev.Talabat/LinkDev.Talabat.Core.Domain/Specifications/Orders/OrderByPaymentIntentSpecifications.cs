using LinkDev.Talabat.Core.Domain.Entities.Orders;

namespace LinkDev.Talabat.Core.Domain.Specifications.Orders
{
    public class OrderByPaymentIntentSpecifications : BaseSpecifications<Order, int>
    {
        public OrderByPaymentIntentSpecifications(string paymentIntentId)
            : base(order => order.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
