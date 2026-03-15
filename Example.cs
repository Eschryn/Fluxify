#!/usr/bin/env dotnet
#:package Fluxify.Bot@0.1.0-preview
#:package Microsoft.Extensions.Logging.Console@10.0.3

// This is a Single file app -> run with `dotnet run Example.cs`
// you need to provide the FLUXIFY_BOT_TOKEN variable with the bot token

using Fluxify.Bot;
using Fluxify.Commands;
using Fluxify.Commands.CommandCollection;
using Fluxify.Core;
using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));
var config = new FluxerConfig(loggerFactory)
{
    Credentials = new BotTokenCredentials(Environment.GetEnvironmentVariable("FLUXIFY_BOT_TOKEN")
                                          ?? throw new Exception("FLUXIFY_BOT_TOKEN environment variable is not set"))
};

var bot = new Bot("f!", config);

bot.Commands.Command("ping", (CommandContext ctx) => ctx.ReplyAsync("Pong!"));

await bot.RunAsync();