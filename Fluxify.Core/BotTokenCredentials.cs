namespace Fluxify.Core;

public class BotTokenCredentials(string token)
{
    public string Token { get; } = token;
    
    public bool Validate()
    {
        return true;
    }
}