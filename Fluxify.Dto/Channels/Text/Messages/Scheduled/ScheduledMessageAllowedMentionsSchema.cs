using Fluxify.Core.Types;

namespace Fluxify.Dto.Channels.Text.Messages.Scheduled;

public record ScheduledMessageAllowedMentionsSchema(
    ScheduledMessageAllowedMentionsSchemaParse[]? Parse,
    bool? RepliedUser,
    Snowflake[]? Roles,
    Snowflake[]? Users
);