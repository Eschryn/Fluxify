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

// the parameters to the command will be resolved from the configured service provider
//   or (still in progress as of 0.1.0-preview) from the command reader
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
Configure your IServiceProvider using the `Services` option. A new service scope will be created per Command execution.

```csharp
var cfg = new FluxerConfig
{
    ...
    Services = ...
};
```

### Bot Presence
This is configured using the gateway configuration so you will have to pass another configuration to the bot class.

```csharp
var gatewayConfig = new GatewayConfig
{
    // IgnoredGatewayEvents = ...,
    DefaultPresence = new PresenceUpdate(
        Status: UserStatus.Online,
        CustomStatus: new CustomStatus(
            Text: "hello world!"))
};

var bot = new Bot(..., gatewayConfig);
...
```
*TODO: BotHosting example once done*
