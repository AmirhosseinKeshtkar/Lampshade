namespace ShopManagement.Domain.OrderAgg {
    public class OrederItem {
        public long ProductId { get; private set; }
        public int Count { get; private set; }
        public double UnitPrice { get; private set; }
        public int DiscountRate{ get; private set; }
        public long OrderId { get; private set; }
        public Order Order { get; private set; }

        public OrederItem(long productId, int count, double unitPrice, int discountRate, long orderId) {
            ProductId = productId;
            Count = count;
            UnitPrice = unitPrice;
            DiscountRate = discountRate;
            OrderId = orderId;
        }
    }
}
