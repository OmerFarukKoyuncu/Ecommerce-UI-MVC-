using DAL.Enums;

namespace ANK19_ETicaretMVC.ViewModels.OrderViewModels
{
    public class OrderListViewModel
    {
        public int Id { get; set; }
        public UserViewModel User { get; set; }
        public string UserId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }

               
    }
}
