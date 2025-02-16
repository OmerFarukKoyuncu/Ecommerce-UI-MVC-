using BLL.DTO.UserDtosForAdmin;

namespace ANK19_ETicaretMVC.ViewModels.CommentViewModels
{
    public class CommentManagerViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? ProductId { get; set; }
        public string UserId { get; set; }
        public GetUserViewModel User { get; set; }
        public string? SellerReply { get; set; }
        public int ProductRate { get; set; }


    }
}
