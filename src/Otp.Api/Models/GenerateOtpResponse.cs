namespace Otp.Api.Models;

public class GenerateOtpResponse
{
    public string Password { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }

    public int RemainingSeconds { get; set; }
}
