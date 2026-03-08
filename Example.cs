#!/usr/bin/env dotnet
#:project ./Fluxify.Bot/Fluxify.Bot.csproj
#:package Microsoft.Extensions.Logging.Console@10.0.3

// This is a Single file app -> run with `dotnet run Example.cs`
// you need to provide the FLUXIFY_BOT_TOKEN variable with the bot token

using Fluxify.Bot;
using Fluxify.Commands;
using Fluxify.Core;
using Microsoft.Extensions.Logging;

var token = new BotTokenCredentials(Environment.GetEnvironmentVariable("FLUXIFY_BOT_TOKEN")
                                    ?? throw new Exception("FLUXIFY_BOT_TOKEN environment variable is not set"));
var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));

var bot = new Bot("f!", new FluxerConfig(loggerFactory));

bot.Commands.Command("ping", () => Console.WriteLine("pong"));

await bot.RunAsync(token);