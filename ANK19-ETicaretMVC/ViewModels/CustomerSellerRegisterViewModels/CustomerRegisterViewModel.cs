namespace ANK19_ETicaretMVC.ViewModels.CustomerSellerRegisterViewModels
{
    public class CustomerRegisterViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? SecondName { get; set; }
        public string? SecondLastname { get; set; }
        public string Address { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; } = false;

        public IFormFile? Photo { get; set; }
    }
}
