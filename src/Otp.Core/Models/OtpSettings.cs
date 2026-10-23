namespace Otp.Core.Models;

public class OtpSettings
{
    public string Secret { get; set; } = string.Empty;

    public int ValidityInSeconds { get; set; } = 30;

    public int PasswordLength { get; set; } = 6;
}
