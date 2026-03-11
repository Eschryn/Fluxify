using Fluxify.Core.Types;

namespace Fluxify.Gateway.Model.Data.User;

public record GatewayUserNoteUpdate(Snowflake Id, string Note);