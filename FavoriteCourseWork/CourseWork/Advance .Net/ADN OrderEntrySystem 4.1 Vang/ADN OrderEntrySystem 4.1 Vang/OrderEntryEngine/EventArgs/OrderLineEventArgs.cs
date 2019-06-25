namespace OrderEntryEngine
{
    public class OrderLineEventArgs
    {
        public OrderLineEventArgs(OrderLine line)
        {
            this.Line = line;
        }

        public OrderLine Line { get; set; }
    }
}