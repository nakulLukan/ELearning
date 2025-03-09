using Learning.Shared.Common.Models.Communication;

namespace Learning.Shared.Application.Contracts.Communication;

public interface ISmsService
{
    public Task<SmsResponse> SendOtpAsync(string contactNumberWithoutCountryCode, int otp, string messageTemplateId);
    public Task<SmsResponse> SendOtpForAccountVerification(string contactNumberWithoutCountryCode, int otp);
    public Task<SmsResponse> SendOtpForPasswordRecovery(string contactNumberWithoutCountryCode, int otp);
}
