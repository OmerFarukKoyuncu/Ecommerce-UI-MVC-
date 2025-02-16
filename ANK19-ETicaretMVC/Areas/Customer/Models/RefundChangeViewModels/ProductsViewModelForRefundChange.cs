using ANK19_ETicaretMVC.Areas.Admin.Models.UserViewModels;
using ANK19_ETicaretMVC.Enums;

namespace ANK19_ETicaretMVC.Areas.Customer.Models.RefundChangeViewModels
{
    public class ProductsViewModelForRefundChange
    {
        public virtual GetAllUsersViewModel User { get; set; }
        public string UserId { get; set; }
        //public RequestType Status { get; set; }  
        public int SellerId { get; set; }
        public virtual List<OrderProductDtoForUserOrder> OrderProducts { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
