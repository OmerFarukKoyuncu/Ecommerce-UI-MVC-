using ANK19_ETicaretMVC.Enums;

namespace ANK19_ETicaretMVC.Areas.Seller.Models.PromotionViewModels
{
    public class AddPromotionViewModel
    {
        public string PromotionName { get; set; } // Name of the promotion (e.g., "Summer Sale")
        public PromotionStatus DiscountType { get; set; } // Type of discount: "Percentage" or "FixedAmount"
        public decimal DiscountValue { get; set; } // Discount value (e.g., 10% or $5)
        public DateTime StartDate { get; set; } // Promotion start date
        public DateTime EndDate { get; set; } // Promotion end date

        public int ProductId { get; set; }
        public bool IsActive
        {
            get
            {
                // Check if the promotion is currently active based on dates
                return DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
            }
        }
    }
}
