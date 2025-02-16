namespace ANK19_ETicaretMVC.ViewModels.ProductViewModels
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        virtual public ProductViewModel Product { get; set; }
        public int CategoryId { get; set; }
        virtual public CategoryViewModel Category { get; set; }
    }
}
