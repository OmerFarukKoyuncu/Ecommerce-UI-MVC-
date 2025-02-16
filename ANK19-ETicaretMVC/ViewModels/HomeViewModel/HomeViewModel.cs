using ANK19_ETicaretMVC.ViewModels.ContentViewModels;
using ANK19_ETicaretMVC.ViewModels.CustomerProductViewModels;

namespace ANK19_ETicaretMVC.ViewModels.HomeViewModel
{
    public class HomeViewModel
    {
        public List<CustomerProductViewModel>? Products { get; set; }
        public ContentViewModel? Content { get; set; }
    }
}
