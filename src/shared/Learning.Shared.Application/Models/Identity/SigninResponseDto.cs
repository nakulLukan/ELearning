namespace Learning.Shared.Application.Models.Identity;

public class SigninResponseDto
{
    public string IdToken { get; init; }
    public string RefreshToken { get; init; }
    public string AccessToken { get; init; }
    public DateTimeOffset ExpiresIn { get; init; }

    public SigninResponseDto(string idToken, string refreshToken, string accessToken, int expiresIn)
    {
        IdToken = idToken;
        RefreshToken = refreshToken;
        AccessToken = accessToken;
        ExpiresIn = DateTimeOffset.UtcNow.AddSeconds(expiresIn);
    }
}
