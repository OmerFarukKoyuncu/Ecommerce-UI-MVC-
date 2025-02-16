namespace ANK19_ETicaretMVC.ViewModels.ShopCartViewModels
{
    public class ItemRequest
    {
        public int Quantity { get; set; }
        public int Id { get; set; }

        public string? ErrorFallBack { get; set; }
    }
}
