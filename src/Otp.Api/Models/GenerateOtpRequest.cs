using System.ComponentModel.DataAnnotations;

namespace Otp.Api.Models;

public class GenerateOtpRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public DateTime DateTime { get; set; }
}
