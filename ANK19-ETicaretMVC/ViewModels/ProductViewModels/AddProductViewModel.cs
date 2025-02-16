namespace ANK19_ETicaretMVC.ViewModels.ProductViewModels
{
    public class AddProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int SellerId { get; set; }

        public IFormFile Photo { get; set; }
        public string? PhotoUrl { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
