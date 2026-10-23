namespace Otp.Core.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
