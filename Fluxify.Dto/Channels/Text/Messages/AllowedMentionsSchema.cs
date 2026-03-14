using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text.Messages;

public record AllowedMentionsSchema(
    AllowedMentionsSchemaParse[]? Parse,
    bool? RepliedUser,
    Snowflake[]? Roles,
    Snowflake[]? Users
);