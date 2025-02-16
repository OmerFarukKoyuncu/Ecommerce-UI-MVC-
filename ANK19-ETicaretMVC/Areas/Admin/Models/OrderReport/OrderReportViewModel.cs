using ANK19_ETicaretMVC.Areas.Admin.Enums;

namespace ANK19_ETicaretMVC.Areas.Admin.Models.OrderReport
{
    public class OrderReportViewModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public decimal TotalQuantity { get; set; }
        public string SellerName { get; set; }


        public OrderStatus Status { get; set; }
    }
}
