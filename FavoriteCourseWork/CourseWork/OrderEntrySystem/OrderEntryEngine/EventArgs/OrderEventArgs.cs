namespace OrderEntryEngine
{
    public class OrderEventArgs
    {
        public OrderEventArgs(Order order)
        {
            this.Order = order;
        }

        public Order Order { get; private set; }
    }
}