using ANK19_ETicaretMVC.Areas.Admin.Validations;
using System.ComponentModel.DataAnnotations;

namespace ANK19_ETicaretMVC.Areas.Admin.Models.UserViewModels
{
    public class AddUserViewModel
    {
        // Kullanıcı adı doğrulaması: Yalnızca Required doğrulaması ekleniyor
        [AddUserForAdmin("username", ErrorMessage = "Kullanıcı adı geçerli değil.")]
        [Required(ErrorMessage = "Kullanıcı adı boş olamaz!")]
        public string UserName { get; set; }

        // Şifre doğrulaması: AddUserForAdmin yalnızca password için uygulanır
        [AddUserForAdmin("password", ErrorMessage = "Şifre geçerli değil.")]
        [Required(ErrorMessage = "Şifre boş olamaz!")]
        public string Password { get; set; }

        // Email doğrulaması: AddUserForAdmin yalnızca email için uygulanır
        [AddUserForAdmin("email", ErrorMessage = "Email geçerli değil.")]
        [Required(ErrorMessage = "Email boş olamaz!")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz!")]
        public string Email { get; set; }

        // Diğer alanlar
        [AddUserForAdmin]
        [Required(ErrorMessage = "İlk isim boş olamaz!")]
        public string Firstname { get; set; }

        [AddUserForAdmin]
        [Required(ErrorMessage = "Soyad boş olamaz!")]
        public string Lastname { get; set; }

        public string? SecondName { get; set; }
        public string? SecondLastname { get; set; }

        [AddUserForAdmin]
        [Required(ErrorMessage = "Adres Boş Olamaz")]
        public string Address { get; set; }

        public IFormFile? Photo { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }

}
