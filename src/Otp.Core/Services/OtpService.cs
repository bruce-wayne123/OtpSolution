using Microsoft.Extensions.Options;
using Otp.Core.Interfaces;
using Otp.Core.Models;
using OtpNet;
using System.Security.Cryptography;
using System.Text;

namespace Otp.Core.Services;

public class OtpService : IOtpService
{
    private readonly OtpSettings _settings;
    private readonly IDateTimeProvider _dateTimeProvider;

    public OtpService(IOptions<OtpSettings> options, IDateTimeProvider dateTimeProvider)
    {
        _settings = options.Value;
        _dateTimeProvider = dateTimeProvider;
    }

    public OtpResult Generate(string userId, DateTime dateTime)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User id is required.", nameof(userId));
        }

        var utcTime = dateTime.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
                    : dateTime.ToUniversalTime();
        var timeStep = GetTimeStep(utcTime);
        var expiresAt = GetStepEndTime(timeStep);


        return new OtpResult
        {
            Password = BuildPassword(userId, utcTime),
            ExpiresAtUtc = expiresAt,
            RemainingSeconds = GetRemainingSeconds(expiresAt)
        };
    }

    public bool Validate(string userId, string password)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        var currentStep = GetTimeStep(_dateTimeProvider.UtcNow);
        var expected = BuildPassword(userId, _dateTimeProvider.UtcNow);

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(password.Trim()));
    }

    private long GetTimeStep(DateTime utcTime)
    {
        var unixSeconds = new DateTimeOffset(utcTime, TimeSpan.Zero).ToUnixTimeSeconds();
        return unixSeconds / _settings.ValidityInSeconds;
    }

    private DateTime GetStepEndTime(long timeStep)
    {
        var endSeconds = (timeStep + 1) * _settings.ValidityInSeconds;
        return DateTimeOffset.FromUnixTimeSeconds(endSeconds).UtcDateTime;
    }

    private int GetRemainingSeconds(DateTime expiresAtUtc)
    {
        var remaining = (int)Math.Ceiling((expiresAtUtc - _dateTimeProvider.UtcNow).TotalSeconds);
        return Math.Max(remaining, 0);
    }

    private string BuildPassword(string userId, DateTime utcTime)
    {
        var key = Encoding.UTF8.GetBytes(_settings.Secret + userId.Trim());
        var totp = new Totp(key, _settings.ValidityInSeconds, OtpHashMode.Sha256, _settings.PasswordLength);

        return totp.ComputeTotp(utcTime);
    }
}
