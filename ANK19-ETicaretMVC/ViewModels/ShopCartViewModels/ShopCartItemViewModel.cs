using ANK19_ETicaretMVC.ViewModels.ProductViewModels;

namespace ANK19_ETicaretMVC.ViewModels.ShopCartViewModels
{
    public class ShopCartItemViewModel
    {
        public int Id { get; set; }
        public int ShopCartId { get; set; } // Hangi sepete ait olduğu
        virtual public ShopCartViewModel ShopCart { get; set; } // ShopCart ile ilişki

        public int ProductId { get; set; } // Sepete eklenen ürün
        virtual public ProductViewModel Product { get; set; } // Product ile ilişki

        public int Quantity { get; set; } // Ürünün sepetteki miktarı
        public decimal TotalPrice => (decimal)(Product.Price * Quantity); // Ürünün toplam fiyatı (miktar * birim fiyat)
    }
}
