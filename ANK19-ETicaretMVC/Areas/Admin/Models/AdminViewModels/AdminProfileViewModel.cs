namespace ANK19_ETicaretMVC.Areas.Admin.Models.AdminViewModels
{
    public class AdminProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastname { get; set; }
        public string? ProfilePictureUrl { get; set; } // This could be a URL or base64 string
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
    }
}
