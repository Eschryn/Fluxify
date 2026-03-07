using Fluxify.Dto.Users;
using Fluxify.Gateway.Model.Data;

namespace Fluxify.Gateway.Model.Dto;

public record Presence(
    User User,
    UserStatus Status,
    bool Mobile,
    bool Afk,
    CustomStatus? CustomStatus
);