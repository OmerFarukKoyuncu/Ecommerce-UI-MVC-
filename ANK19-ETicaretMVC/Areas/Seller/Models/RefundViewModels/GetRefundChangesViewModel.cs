using ANK19_ETicaretMVC.Areas.Customer.Models.RefundChangeViewModels;
using ANK19_ETicaretMVC.Enums;
using ANK19_ETicaretMVC.ViewModels;



public class GetRefundChangesViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public virtual ProductViewModelForSeller Product { get; set; }
    public string UserId { get; set; }
    public virtual UserViewModel User { get; set; }
    public RequestType Request { get; set; }
    public DateTime CreatedDate { get; set; }
}

