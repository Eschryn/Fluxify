#!/usr/bin/env dotnet
#:package Fluxify.Bot@0.2.3-preview
#:package Microsoft.Extensions.Logging.Console@10.0.3

// This is a Single file app -> run with `dotnet run Example.cs`
// you need to provide the FLUXIFY_BOT_TOKEN variable with the bot token

using Fluxify.Bot;
using Fluxify.Commands;
using Fluxify.Core.Credentials;
using Microsoft.Extensions.Logging;

var botConfig = new BotConfig("f!")
{
    Credentials = new BotTokenCredentials(Environment.GetEnvironmentVariable("FLUXIFY_BOT_TOKEN")
                                          ?? throw new Exception("FLUXIFY_BOT_TOKEN environment variable is not set")),
    FluxerConfig =
    {
        LoggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace)),
    },
};

var bot = new Bot(botConfig);

bot.Commands.Command("ping", (CommandContext ctx) => ctx.ReplyAsync("Pong!"));

await bot.RunAsync();