using ANK19_ETicaretMVC.ViewModels.OrderViewModels;
using ANK19_ETicaretMVC.ViewModels.ProductViewModels;

namespace ANK19_ETicaretMVC.Areas.Seller.Models.OrderViewModels
{
    public class OrderProductViewModel
    {
        public OrderListViewModel Order { get; set; }
        public int OrderId { get; set; }
        public ProductViewModel Product { get; set; }
        public int ProductId { get; set; }
        public double Quantity { get; set; }
    }

}
