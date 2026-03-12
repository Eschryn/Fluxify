namespace Fluxify.Application.Entities.Users;

[Flags]
public enum PublicUserFlags : uint
{
    Staff = 1,
    CtpMember = 2,
    Partner = 4,
    BugHunter = 8,
    FriendlyBot = 16,
    FriendlyBotManualApproval = 32,
}