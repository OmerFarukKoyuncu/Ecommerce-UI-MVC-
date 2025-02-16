using Microsoft.EntityFrameworkCore;

namespace ANK19_ETicaretMVC.Areas.Admin.Data
{
    public class SchafoldIcinDbContext:DbContext
    {
        public SchafoldIcinDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ANK19_ETicaretMVC.Areas.Seller.Models.PromotionViewModels.PromotionViewModel> PromotionViewModel { get; set; } = default!;
    }
}
