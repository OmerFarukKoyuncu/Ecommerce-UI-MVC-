namespace ANK19_ETicaretMVC.Areas.Customer.Models.CustomerProfileViewModels
{
    public class CustomerViewModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public IFormFile? ProfilePicture { get; set; }



       


        


    }
}
