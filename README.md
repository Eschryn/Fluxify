# Fluxify 
.NET library for building applications that interact with fluxer

[![NuGet Version](https://img.shields.io/nuget/v/Fluxify.Bot?style=flat-square)](https://www.nuget.org/packages/Fluxify.Bot)
[![GitHub License](https://img.shields.io/github/license/Eschryn/Fluxify?style=flat-square)](LICENSE)

## Getting Started
See `Example.cs` for a simple starting point

### Building a Bot
Start with the `Fluxify.Bot` package.
```csharp
var bot = new Bot("!", new FluxerConfig())

bot.Commands.Command("ping", (CommandContext ctx) => ctx.ReplyAsync("Pong!"));

await bot.RunAsync(new BotTokenCredentials("..."));
```

### Logging (Simple)
Using `FluxerConfig` provide a `ILoggerFactory`.

Example with `Microsoft.Extensions.Logging.Console`:
```csharp

var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var bot = new Bot("!", new FluxerConfig(loggerFactory));
...
```
### Dependency Injection
Example with `Microsoft.Extensions.DependencyInjection`:

*TODO*
