namespace ANK19_ETicaretMVC.Areas.Admin.Models.OrderReport
{
    public class OrderCustomerReportViewModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal TotalQuantity { get; set; }
    }
}
