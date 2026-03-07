using Fluxify.Core;

namespace Fluxify.Gateway.Model.Dto;

public record GatewayUserNoteUpdate(Snowflake Id, string Note);