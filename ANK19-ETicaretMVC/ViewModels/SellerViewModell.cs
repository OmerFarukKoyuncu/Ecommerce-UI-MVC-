namespace ANK19_ETicaretMVC.ViewModels
{
    public class SellerViewModell
    {
        public string? CompanyName { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public bool IsApproved { get; set; } = false;
        public string? ProfilePictureUrl { get; set; }
    }
}
