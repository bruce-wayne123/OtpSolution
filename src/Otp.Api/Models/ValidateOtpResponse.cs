namespace Otp.Api.Models;

public class ValidateOtpResponse
{
    public bool IsValid { get; set; }

    public string Message { get; set; } = string.Empty;
}
