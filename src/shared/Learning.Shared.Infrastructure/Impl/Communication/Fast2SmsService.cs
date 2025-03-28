﻿using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Learning.Shared.Application.Contracts.Communication;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Models.Communication;
using Learning.Shared.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Learning.Shared.Infrastructure.Impl.Communication;

public class Fast2SmsService : ISmsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Fast2SmsService> _logger;

    private const string Fast2SmsApiUrl = "https://www.fast2sms.com/dev/bulkV2";
    private readonly string _apiKey;
    private readonly bool _supperSms;
    public Fast2SmsService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<Fast2SmsService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _supperSms = bool.Parse(_configuration[AppSettingsKeyConstant.Sms_SuppressSms]!);
        _apiKey = _configuration[AppSettingsKeyConstant.Sms_ApiKey]!;
        _logger = logger;
    }

    public async Task<SmsResponse> SendOtpAsync(string contactNumberWithoutCountryCode, int otp, string messageTemplateId)
    {
        try
        {
            if (_supperSms)
            {
                return new SmsResponse
                {
                    Success = true,
                };
            }
            var dltSenderId = _configuration[AppSettingsKeyConstant.Sms_DltSenderId];
            var dltMessageId = messageTemplateId;
            var client = _httpClientFactory.CreateClient(); // Creating HttpClient instance
            client.DefaultRequestHeaders.Add("authorization", _apiKey);
            var requestBody = new
            {
                route = "dlt",
                message = dltMessageId,
                flash = 0,
                numbers = !contactNumberWithoutCountryCode.StartsWith(LocalizationConstant.CountryCode) ? contactNumberWithoutCountryCode : contactNumberWithoutCountryCode.Replace(LocalizationConstant.CountryCode, string.Empty),
                variables_values = otp.ToString(),
                sender_id = dltSenderId
            };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Fast2SmsApiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadFromJsonAsync<SmsApiResponse>();
                return new SmsResponse()
                {
                    Success = true,
                };
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<SmsApiErrorResponse>();
                _logger.LogError("F2Sms Failed: {Message}", responseJson!.Message);
                return new SmsResponse()
                {
                    ErrorMessage = responseJson.Message,
                    Success = responseJson.Return
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OTP sms failed");
            return new SmsResponse()
            {
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    public Task<SmsResponse> SendOtpForAccountVerification(string contactNumberWithoutCountryCode, int otp)
    {
        return SendOtpAsync(contactNumberWithoutCountryCode, otp, _configuration[AppSettingsKeyConstant.Sms_DltAccountVerificationMessageId]!);
    }

    public Task<SmsResponse> SendOtpForPasswordRecovery(string contactNumberWithoutCountryCode, int otp)
    {
        return SendOtpAsync(contactNumberWithoutCountryCode, otp, _configuration[AppSettingsKeyConstant.Sms_DltPasswordRecoveryMessageId]!);
    }
}

public class SmsApiResponse
{
    [JsonPropertyName("return")]
    public bool Return { get; set; }

    [JsonPropertyName("message")]
    public string[]? Message { get; set; }
}

public class SmsApiErrorResponse
{
    [JsonPropertyName("return")]
    public bool Return { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
