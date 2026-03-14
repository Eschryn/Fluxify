using Fluxify.Core.Types;

namespace Fluxify.Application.Model.Messages;

public class AllowedMentions
{
    public AllowedMentionsParse[]? Parse { get; set; }
    public bool? RepliedUser { get; set; }
    public Snowflake[]? Roles  { get; set; }
    public Snowflake[]? Users  { get; set; }
}