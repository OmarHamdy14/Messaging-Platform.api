using System.ComponentModel.DataAnnotations;

namespace MessagingPlatformAPI.Helpers.DTOs.AccountDTOs
{
    public class LogInDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
