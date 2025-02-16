namespace ANK19_ETicaretMVC.ViewModels
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string PictureURL { get; set; }

        public bool isAdmin { get; set; }
        public bool isSeller { get; set; }

    }
}
