namespace ANK19_ETicaretMVC.Areas.Admin.Models.OrderReport
{
    public class OrderSellerReportViewModel
    {
        public int SellerId { get; set; }
        public string SellerName { get; set; }



        public decimal? TotalPrice { get; set; }
        public decimal TotalQuantity { get; set; }
    }
}
