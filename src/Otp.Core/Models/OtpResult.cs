namespace Otp.Core.Models;

public class OtpResult
{
    public string Password { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }

    public int RemainingSeconds { get; set; }
}
