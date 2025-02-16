using ANK19_ETicaretMVC.Areas.Seller.Models.PromotionViewModels;
using ANK19_ETicaretMVC.ViewModels.CommentViewModels;

namespace ANK19_ETicaretMVC.ViewModels.CustomerProductViewModels
{
    public class CustomerProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string? PhotoUrl { get; set; }
        public int SellerId { get; set; }

        virtual public List<CustomerProductCategoryViewModel> ProductCategories { get; set; }
        virtual public List<PromotionViewModel> Promotions { get; set; }
        virtual public List<CommentManagerViewModel> Comments { get; set; }
    }
}
