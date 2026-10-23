using System.ComponentModel.DataAnnotations;

namespace Otp.Api.Models;

public class ValidateOtpRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
