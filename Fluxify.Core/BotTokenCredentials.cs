namespace Fluxify.Core;

public class BotTokenCredentials(string token)
{
    public const string TypeConst = "Bot";
    public string Token { get; } = token;
    
    public bool Validate()
    {
        return true;
    }
    
    public string GetAuthorizationHeaderValue() => $"{TypeConst} {Token}";
}