namespace ANK19_ETicaretMVC.ViewModels.SellerViewModel
{
    public class SellerViewModel
    {
        public int Id { get; set; }

        public string? CompanyName { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public bool IsApproved { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
