namespace ANK19_ETicaretMVC.Areas.Seller.Models.SellerProfileViewModels
{
    public class SellerProfileViewModel
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string Name { get; set; }//firma adı
        public string ContactInfo { get; set; }//address
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        

        public IFormFile? ProfilePicture { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;





    }
}
