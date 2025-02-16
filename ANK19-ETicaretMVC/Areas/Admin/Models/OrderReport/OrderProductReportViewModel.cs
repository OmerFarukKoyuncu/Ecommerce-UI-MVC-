using ANK19_ETicaretMVC.Areas.Admin.Enums;

namespace ANK19_ETicaretMVC.Areas.Admin.Models.OrderReport
{
    public class OrderProductReportViewModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string SellerName { get; set; }
        public OrderStatus Status { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
