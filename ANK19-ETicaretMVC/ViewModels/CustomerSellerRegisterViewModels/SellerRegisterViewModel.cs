using ANK19_ETicaretMVC.ViewModels.ProductViewModels;

namespace ANK19_ETicaretMVC.ViewModels.CustomerSellerRegisterViewModels
{
    public class SellerRegisterViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactInfo { get; set; }
        public string? ContactName { get; set; }
        public string? CompanyAdress { get; set; }
        public bool IsApproved { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public IFormFile? Photo { get; set; }
        public string? LogoPictureUrl { get; set; }

        public string? UserId { get; set; }
    }
}
