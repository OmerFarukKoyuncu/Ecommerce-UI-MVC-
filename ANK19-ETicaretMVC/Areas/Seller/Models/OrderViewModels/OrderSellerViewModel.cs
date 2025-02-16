using ANK19_ETicaretMVC.Areas.Admin.Enums;
using ANK19_ETicaretMVC.Areas.Admin.Models.UserViewModels;
using ANK19_ETicaretMVC.ViewModels.SellerViewModel;

namespace ANK19_ETicaretMVC.Areas.Seller.Models.OrderViewModels
{
    public class OrderSellerViewModel
    {
        public int Id { get; set; }
        public virtual GetAllUsersViewModel User { get; set; }
        public string UserId { get; set; }
        public OrderStatus Status { get; set; }
        public virtual SellerViewModel Seller { get; set; }
        public int SellerId { get; set; }
        public virtual List<OrderProductViewModel> OrderProducts { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
