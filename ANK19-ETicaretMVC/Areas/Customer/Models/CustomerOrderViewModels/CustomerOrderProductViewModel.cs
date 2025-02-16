namespace ANK19_ETicaretMVC.Areas.Customer.Models.CustomerOrderViewModels
{
    public class CustomerOrderProductViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }

        public decimal? Price { get; set; }

        public int? CustomerRating { get; set; }
        public string? CustomerEvaluation { get; set; }
    }
}

