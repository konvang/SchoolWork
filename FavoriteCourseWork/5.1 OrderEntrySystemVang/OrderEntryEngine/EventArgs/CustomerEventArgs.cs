namespace OrderEntryEngine
{
    public class CustomerEventArgs
    {
        public CustomerEventArgs(Customer customer)
        {
            this.Customer = customer;
        }

        public Customer Customer { get; private set; }
    }
}