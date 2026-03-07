namespace Fluxify.Dto.Guilds.Emoji;

/// <summary>
/// 
/// </summary>
/// <param name="Image">Base64ImageType</param>
/// <param name="Name"></param>
public record GuildEmojiCreateRequest(string Image, string Name);