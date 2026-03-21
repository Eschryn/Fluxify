// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Entities.Users;
using Fluxify.Application.Extensions;
using Fluxify.Commands.Model;
using Fluxify.Core.Types;

namespace Fluxify.Commands;

public static class Preconditions
{
    private static readonly Type PermissionEnumType = typeof(Permissions);

    public static Precondition RequireAuthorPermissions(Permissions permissions, string? failMessage = null,
        string? guildFailMessage = null)
        => new(
            $"require_author_permissions${Enum.Format(PermissionEnumType, permissions, "X")}",
            $"Requires permissions: {Enum.Format(PermissionEnumType, permissions, "F")}",
            RequirePermissionTemplate(
                permissions,
                static ctx => (GuildUser)ctx.Author,
                failMessage,
                guildFailMessage ?? "You do not have the required permissions to use this command."
            )
        );

    public static Precondition RequireBotPermissions(Permissions permissions, string? failMessage = null,
        string? guildFailMessage = null)
        => new(
            $"require_bot_permissions${Enum.Format(PermissionEnumType, permissions, "X")}",
            $"Requires permissions: {Enum.Format(PermissionEnumType, permissions, "F")}",
            RequirePermissionTemplate(
                permissions,
                static ctx => ctx.Guild!.CurrentUser,
                failMessage,
                guildFailMessage ?? "The bot does not have the required permissions to use this command."
            )
        );

    private static PreconditionDelegate RequirePermissionTemplate(
        Permissions permissions,
        Func<CommandContext, GuildUser> target,
        string? failMessage,
        string guildFailMessage
    ) => ctx =>
    {
        if (ctx.TextChannel is not IGuildChannel guildChannel)
        {
            return PreconditionResult.Fail(failMessage ?? "This command can only be used in a guild.");
        }

        return (guildChannel.CalculateUserPermissions(target(ctx)) & permissions) == permissions
            ? PreconditionResult.Success
            : PreconditionResult.Fail(guildFailMessage);
    };

    public static Precondition RequireGuildContext(string? failMessage = null)
        => new(
            "require_guild_context",
            failMessage ?? "This command can only be used in a guild.",
            ctx => ctx.Guild != null
                ? PreconditionResult.Success
                : PreconditionResult.Fail(failMessage ?? "This command can only be used in a guild."));
}