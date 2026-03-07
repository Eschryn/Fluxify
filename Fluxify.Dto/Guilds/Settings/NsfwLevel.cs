namespace Fluxify.Dto.Guilds.Settings;

public enum NsfwLevel
{
    /// <summary>
    /// Default NSFW level
    /// </summary>
    Default = 0,

    /// <summary>
    /// Guild has explicit NSFW content
    /// </summary>
    Explicit = 1,

    /// <summary>
    /// Guild is safe for work
    /// </summary>
    Safe = 2,

    /// <summary>
    /// Guild is age-restricted
    /// </summary>
    AgeRestricted = 3
}