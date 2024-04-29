namespace Min_API_DZ
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserDto user);
    }
}
