# Fluxify 
.NET library for building applications that interact with fluxer

[![NuGet Version](https://img.shields.io/nuget/v/Fluxify.Bot?style=flat-square)](https://www.nuget.org/packages/Fluxify.Bot)
[![GitHub License](https://img.shields.io/github/license/Eschryn/Fluxify?style=flat-square)](LICENSE)

<!-- TOC -->
* [Fluxify](#fluxify-)
  * [Getting Started](#getting-started)
    * [Building a Bot](#building-a-bot)
    * [Preconditions](#preconditions)
    * [Logging (Simple)](#logging-simple)
    * [Dependency Injection](#dependency-injection)
    * [Bot Presence](#bot-presence)
    * [Help I cannot find request X](#help-i-cannot-find-request-x)
<!-- TOC -->

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

bot.Commands.Command("hug", async (CommandContext ctx) =>
{
    var userMention = ctx.Reader.GetNext<Mentionable.Member>();

    var message = new MessageBuilder($"<@{userMention.Id}> you have been hugged!")
            .WithEmbed(e => e
                .WithImage("https://gifprovider/image.gif")
            .Build());

    await ctx.Message.Channel.SendMessageAsync(message);
});

await bot.RunAsync();
```

### Voice
To connect to a voice channel you need the `Fluxify.Voice` package.
Here is an example of how to play a file using ffmpeg.
```csharp
// voice session is per voice channel connection
//   -> you'll have to create them per channel - but you can also have multiple sessions per channel
//      probably what you want is some form of ConcurrentDictionary<Snowflake, VoiceSession> :)
var voiceSession = new VoiceSession(bot.Gateway);

// <prefix>playFile #Voice
bot.Commands.Command("playFile", async (CommandContext ctx) =>
{
    var channel = ctx.Reader.GetNext<Mentionable.Channel>();
    if (!ctx.Guild!.Channels.TryGetValue(channel.Id, out var channelValue)
        || channelValue is not GuildVoiceChannel voiceChannel)
    {
        throw new CommandException("Channel is not a voice channel!");
    }

    await voiceSession.ConnectAsync(voiceChannel);
    
    // here you can pretty much use everything ffmpeg has to offer
    // you could chain the stdout of yt-dlp into ffmpeg using .FromPipeInput(new StreamPipeSource()) :)
    // I would recommend retaining the info about what is playing and the task that plays it in some AudioPlayer class
    await FFMpegArguments
        .FromFileInput("/Users/core/FLOW.mp3")
        .OutputToPipe(voiceSession.AudioSourceSink, args => args
            .WithAudioSamplingRate()
            .ForceFormat(voiceSession.AudioSourceSink.GetFormat())
        )
        .ProcessAsynchronously();
    
    await voiceSession.DisconnectAsync();
}, Preconditions.RequireGuildContext());
```

Alternatively you could also get the voice channel from the author if they are in a voice channel.
```csharp
var voiceChannel = context switch
{
    { Author: GuildUser { VoiceState.VoiceChannel: { } authorChannel } } => authorChannel,
    { Reader: { } reader, Guild.Channels: { } guildChannels }
        when guildChannels.TryGetValue(reader.GetNext<Mentionable.Channel>().Id, out var channel)
        && channel is GuildVoiceChannel guildVoiceChannel => guildVoiceChannel,
        _ => throw new CommandException("The mentioned channel is not a voice channel.")
};
```

### Preconditions
```csharp
var botOwnerPrecondition = new Precondition(
    "bot-owner",
    "User needs to be the bot owner"
    static ctx => ctx.Message.Author.Id == 27842764872883298 
        ? PreconditionResult.Success
        : PreconditionResult.Fail("Youre not the bot owner!"));

bot.Module("secret", m =>
    {
        m.Command("isCool", (CommandContext ctx) => ctx.ReplyAsync("Yes you are cool!"))
    }, botOwnerPrecondition)
    .Command(
        "open-pod-bay-doors",
        (CommandContext ctx) => ctx.ReplyAsync("I'm sorry dave I'm afraid I can't do that"),
        Preconditions.RequireAuthorPermissions(Permissions.Administrator)
    );
```

### Logging (Simple)
Fluxify does not provide any logging by itself. To use logging use any logging package that supports the `ILoggerFactory` interface.

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

### ASP.NET Core
To use Fluxify with ASP.NET Core register the core providers and RestClient in the dependency injection container.
Additionally you need the `Fluxify.AspNetCore.Authentication` for the `AddFluxer` authentication option.
Example setup for a webapp:
```cs
builder.Services.AddFluxifyCore(sp => new FluxerConfig
{
    CredentialProvider = sp.GetRequiredService<IAccessTokenProvider>().GetAuthenticationTokenAsync 
})

builder.Services.AddScoped<RestClient>();

builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultScheme = FluxerAuthenticationDefaults.AuthenticationScheme;
        o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddFluxer(o =>
    {
        o.ClientId = builder.Configuration["Fluxer:ClientId"]!;
        o.ClientSecret = builder.Configuration["Fluxer:ClientSecret"]!;

        // o.Scope.Add("guilds") etc..

        o.SaveTokens = true;
    })  
    .AddCookie();
```
Then configure your secret and client id using the user secrets CLI or Visual Studio / Rider.
```
$ dotnet user-secrets init
$ dotnet user-secrets set "Fluxer:ClientId" "<Client ID here>"
$ dotnet user-secrets set "Fluxer:ClientSecret" "<Client ID here>"
```
Clear the entries from the history file.
You could then just use the RestClient as service using dependency injection.
```razor
@page "/"
@using Fluxify.Dto.OAuth2
@using Fluxify.Rest
@using Microsoft.AspNetCore.Authorization
@inject RestClient FluxerClient
@attribute [Authorize]

Welcome @(Me?.User.Username)

@code {
    [PersistentState]
    public OAuth2MeResponse? Me { get; set; }

    protected override async Task OnInitializedAsync() 
        => Me = await FluxerClient.OAuth2.MeAsync();
}
```

### Help I cannot find request X
Currently not all REST functionality is exposed via the Entities.
This means in cases you need functions not exposed in the higher level entities
you should look into Bot.Rest. It currently implements all Guild and User endpoints and almost all Channel endpoints.
If something is missing, feel free to create an issue or contribute.
