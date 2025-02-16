namespace ANK19_ETicaretMVC.ViewModels.CustomerProductViewModels
{
    public class CustomerProductCategoryViewModel
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }
        virtual public CategoryViewModel Category { get; set; }
    }
}
