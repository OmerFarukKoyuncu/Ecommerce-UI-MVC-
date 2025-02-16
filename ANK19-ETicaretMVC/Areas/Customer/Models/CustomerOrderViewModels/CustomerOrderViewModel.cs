using ANK19_ETicaretMVC.Areas.Admin.Enums;
using ANK19_ETicaretMVC.Areas.Seller.Models.OrderViewModels;

namespace ANK19_ETicaretMVC.Areas.Customer.Models.CustomerOrderViewModels
{
    public class CustomerOrderViewModel
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalPrice { get; set; } 
        public List<CustomerOrderProductViewModel> OrderProducts { get; set; }
    }
}


