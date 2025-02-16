
namespace ANK19_ETicaretMVC.ViewModels.ShopCartViewModels
{
    public class ShopCartViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Alışveriş sepetini oluşturan müşteri
        public UserViewModel User { get; set; } // User ile ilişki

         public List<ShopCartItemViewModel> Items { get; set; } // Sepetteki ürünler
    }

}
