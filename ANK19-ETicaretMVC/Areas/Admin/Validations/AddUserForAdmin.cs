using System.ComponentModel.DataAnnotations;

namespace ANK19_ETicaretMVC.Areas.Admin.Validations
{
    public class AddUserForAdmin : ValidationAttribute
    {
        private readonly string _fieldType;

        public AddUserForAdmin(string fieldType = "general")
        {
            _fieldType = fieldType;
        }

        public override bool IsValid(object value)
        {
            // Şifre doğrulama kuralları
            if (_fieldType == "password")
            {
                var password = value?.ToString();
                if (string.IsNullOrEmpty(password) || password.Length < 8)
                {
                    ErrorMessage = "Parola en az 8 karakter uzunluğunda olmalıdır.";
                    return false;
                }
                if (!password.Any(char.IsUpper))
                {
                    ErrorMessage = "Parola en az bir büyük harf içermelidir.";
                    return false;
                }
                if (!password.Any(char.IsLower))
                {
                    ErrorMessage = "Parola en az bir küçük harf içermelidir.";
                    return false;
                }
                if (!password.Any(char.IsDigit))
                {
                    ErrorMessage = "Parola en az bir rakam içermelidir.";
                    return false;
                }
                if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                {
                    ErrorMessage = "Parola en az bir özel karakter içermelidir.";
                    return false;
                }
            }

            // Email doğrulama kuralları
            if (_fieldType == "email")
            {
                var email = value?.ToString();
                if (string.IsNullOrEmpty(email))
                {
                    ErrorMessage = "Email boş olamaz!";
                    return false;
                }

                // Temel format kontrolü
                try
                {
                    var mailAddress = new System.Net.Mail.MailAddress(email);
                    if (mailAddress.Address != email)
                    {
                        ErrorMessage = "Geçerli bir email adresi giriniz!";
                        return false;
                    }
                }
                catch
                {
                    ErrorMessage = "Geçerli bir email adresi giriniz!";
                    return false;
                }
            }

            // Kullanıcı adı doğrulama kuralları
            if (_fieldType == "username")
            {
                var username = value?.ToString();
                if (string.IsNullOrEmpty(username))
                {
                    ErrorMessage = "Kullanıcı adı boş olamaz!";
                    return false;
                }

                // Kullanıcı adı sadece harf ve rakamlardan oluşmalı
                if (!username.All(c => char.IsLetterOrDigit(c)))
                {
                    ErrorMessage = "Kullanıcı adı yalnızca harfler ve rakamlar içerebilir.";
                    return false;
                }

                // Türkçe karakterleri kontrol et
                var turkishChars = new[] { 'ç', 'ğ', 'ş', 'ö', 'ü', 'ı', 'İ', 'Ç', 'Ğ', 'Ş', 'Ö', 'Ü' };
                if (username.Any(c => turkishChars.Contains(c)))
                {
                    ErrorMessage = "Kullanıcı adı Türkçe karakterler içeremez.";
                    return false;
                }
            }

            return true;
        }
    }



}
