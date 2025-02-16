using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DAL.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Hazırlanıyor")]
        Preparing,
        [Display(Name = "Kargoya Verildi")]
        Shipped,
        [Display(Name = "Teslim Edildi")]
        Delivered,
        [Display(Name = "İptal Edildi")]
        Canceled,
        [Display(Name = "İade Edildi")]
        Returned,
    }
}

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetField(enumValue.ToString())
            .GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? enumValue.ToString();
    }
}
