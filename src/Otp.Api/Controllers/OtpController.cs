using Microsoft.AspNetCore.Mvc;
using Otp.Api.Models;
using Otp.Core.Interfaces;

namespace Otp.Api.Controllers;

/// <summary>
/// Otp Controller handles OTP generation and validation requests.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OtpController : ControllerBase
{
    private readonly IOtpService _otpService;

    public OtpController(IOtpService otpService)
    {
        _otpService = otpService;
    }

    /// <summary>
    /// Generates a new OTP for the specified user and returns the generated password along with its expiry time and remaining seconds.
    /// </summary>
    /// <param name="request">The request containing the user ID and the datetime for which to generate the OTP.</param>
    /// <returns>A response containing the generated OTP, its expiry time, and the remaining seconds until it expires.</returns>
    [HttpPost("generate")]
    public ActionResult<GenerateOtpResponse> Generate(GenerateOtpRequest request)
    {
        var result = _otpService.Generate(request.UserId, request.DateTime);

        return Ok(new GenerateOtpResponse
        {
            Password = result.Password,
            ExpiresAtUtc = result.ExpiresAtUtc,
            RemainingSeconds = result.RemainingSeconds
        });
    }

    /// <summary>
    /// Validates the provided OTP for the specified user and returns whether it is valid or not.
    /// </summary>
    /// <param name="request">The request containing the user ID and the OTP to validate.</param>
    /// <returns>A response indicating whether the OTP is valid or not.</returns>
    [HttpPost("validate")]
    public ActionResult<ValidateOtpResponse> Validate(ValidateOtpRequest request)
    {
        var isValid = _otpService.Validate(request.UserId, request.Password);

        return Ok(new ValidateOtpResponse
        {
            IsValid = isValid,
            Message = isValid ? "Password is valid." : "Password is invalid or has expired."
        });
    }
}
