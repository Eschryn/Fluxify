# Fluxify
.NET library for building applications that interact with fluxer

## Getting Started
### Building a Bot
Start with the `Fluxify.Bot` package.
```csharp
var bot = new Bot("!", new FluxerConfig())

bot.Commands.Command("ping", () => Console.WriteLine("pong!"));

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