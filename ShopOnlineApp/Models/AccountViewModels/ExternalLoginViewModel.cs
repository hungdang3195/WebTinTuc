using System.ComponentModel.DataAnnotations;

namespace ShopOnlineApp.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string DOB { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
