using ANK19_ETicaretMVC.Enums;

namespace ANK19_ETicaretMVC.Areas.Seller.Models.RefundViewModels
{
    public class ApproveRefundChangeForSellerVm
    {     
            public RequestType Request { get; set; }
            public string userId { get; set; }
            public int productId { get; set; }
        
    }
}
