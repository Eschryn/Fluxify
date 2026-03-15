# Fluxify 
.NET library for building applications that interact with fluxer

[![NuGet Version](https://img.shields.io/nuget/v/Fluxify.Bot?style=flat-square)](https://www.nuget.org/packages/Fluxify.Bot)
[![GitHub License](https://img.shields.io/github/license/Eschryn/Fluxify?style=flat-square)](LICENSE)

## Getting Started
See `Example.cs` for a simple starting point

### Building a Bot
Start with the `Fluxify.Bot` package.
```csharp
var cfg = new FluxerConfig
{
    // for configuring the fluxer instance you can provide the instance option
    // InstanceUri = new Uri("https://api.<your-instance>/"),
    Credentials = new BotTokenCredentials("...")
};

var bot = new Bot("!", cfg)

// the parameters to the command will be resolved from the configured service provider or (still in progress) from the command reader
bot.Commands.Command("ping", (CommandContext ctx) => ctx.ReplyAsync("Pong!"));

bot.Commands.Command("hug", async (CommandContext ctx) => {
    var userMention = ctx.Reader.GetNext<Mentionable.Member>();

    var message = new MessageBuilder($"<@{userMention.Id}> you have been hugged!")
            .WithEmbed(e => e
                .WithImage("https://gifprovider/image.gif")
            .Build());

    await ctx.ReplyAsync(message);
});

await bot.RunAsync();
```

### Logging (Simple)
Fluxify does not privide any logging by itself, to use logging use any logging package that supports the `ILoggerFactory` interface
Using `FluxerConfig` provide a `ILoggerFactory`.

Example with `Microsoft.Extensions.Logging.Console`:
```csharp
var cfg = new FluxerConfig
{
    ...
    LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole())
};
...
```
### Dependency Injection
Example with `Microsoft.Extensions.DependencyInjection`:

*TODO*
