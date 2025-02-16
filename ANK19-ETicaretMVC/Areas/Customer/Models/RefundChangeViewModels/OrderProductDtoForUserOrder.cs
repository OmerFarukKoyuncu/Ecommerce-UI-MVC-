namespace ANK19_ETicaretMVC.Areas.Customer.Models.RefundChangeViewModels
{
    public class OrderProductDtoForUserOrder
    {
        public int OrderId { get; set; }
        public virtual ProductViewModelForUser Product { get; set; }
        public int ProductId { get; set; }
        public double Quantity { get; set; }
    }
}
