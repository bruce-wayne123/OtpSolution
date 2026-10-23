using Otp.Core.Models;

namespace Otp.Core.Interfaces;

public interface IOtpService
{
    OtpResult Generate(string userId, DateTime dateTime);

    bool Validate(string userId, string password);
}
