using System.ComponentModel.DataAnnotations;
using ANK19_ETicaretMVC.ViewModels.CommentViewModels;
using System.ComponentModel.DataAnnotations;

namespace ANK19_ETicaretMVC.ViewModels.ProductViewModels
{
    public class ProductDetailsViewModel
    {
        [Display(Name = "Ürün Kodu")]
        public int Id { get; set; }

        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }

        [Display(Name = "Tanım")]
        public string Description { get; set; }

        [Display(Name = "Fiyat")]
        public decimal? Price { get; set; }

        [Display(Name = "Stok")]
        public int? Stock { get; set; }

        [Display(Name = "Fotoğraf")]
        public string? PhotoUrl { get; set; }

        [Display(Name = "Satıcı")]
        public SellerViewModell Seller { get; set; }

        [Display(Name = "Kategoriler")]
        public List<ProductCategoryViewModel> Categories { get; set; }

        [Display(Name = "Yorumlar")]

        public List<CommentManagerViewModel> Comments { get; set; }
    }
}
