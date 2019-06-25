namespace OrderEntryEngine
{
    public class CategoryEventArgs
    {
        public CategoryEventArgs(Category category)
        {
            this.Category = category;
        }

        public Category Category { get; private set; }
    }
}