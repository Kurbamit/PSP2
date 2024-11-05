namespace ReactApp1.Server.Models.Models.Base;

public class JwtSettings
{
    public string Secret { get; set; }
    public double ExpirationInMinutes { get; set; }
}