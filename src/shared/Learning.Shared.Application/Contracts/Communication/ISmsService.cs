using Learning.Shared.Common.Models.Communication;

namespace Learning.Shared.Application.Contracts.Communication;

public interface ISmsService
{
    public Task<SmsResponse> SendOtpAsync(string contactNumberWithoutCountryCode, int otp);
}
