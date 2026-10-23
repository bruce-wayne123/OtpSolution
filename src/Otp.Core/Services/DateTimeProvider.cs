using Otp.Core.Interfaces;

namespace Otp.Core.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
