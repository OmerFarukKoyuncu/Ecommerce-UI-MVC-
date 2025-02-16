namespace ANK19_ETicaretMVC.ViewModels.ProductViewModels
{
    public class UpdateProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public IFormFile? Photo { get; set; }
        public string? PhotoUrl { get; set; }

    }
}
