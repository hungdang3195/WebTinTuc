using System.ComponentModel.DataAnnotations;

namespace ShopOnlineApp.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
