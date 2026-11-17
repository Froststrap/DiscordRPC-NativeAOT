# DiscordRPC

A high-performance, Native AOT-compatible Discord Rich Presence client for .NET 10.0.

## Features

- Native AOT compatibility with zero reflection.
- System.Text.Json source generation for JSON serialization and deserialization.
- No external dependencies.
- Cross-platform support for Windows, Linux, and macOS.
- Custom JSON converters for Discord API compatibility.
- Improved named pipe connection handling with a connection timeout.
- Compatible with trimming.

## Installation

Install the package using the .NET CLI:

```bash
dotnet add package Froststrap.DiscordRPC
````

## Usage

csharp

```csharp
using DiscordRPC;
using DiscordRPC.Logging;

var client = new DiscordRpcClient("YOUR_APPLICATION_ID")
{
    Logger = new ConsoleLogger() { Level = LogLevel.Info }
};

client.OnReady += (sender, e) =>
{
    Console.WriteLine($"Connected to Discord as {e.User.Username}");
};

client.Initialize();

client.SetPresence(new RichPresence()
{
    Details = "Playing a game",
    State = "In a match",
    Timestamps = new Timestamps()
    {
        Start = DateTime.UtcNow
    },
    Assets = new Assets()
    {
        LargeImageKey = "large_image",
        LargeImageText = "My Game",
        SmallImageKey = "small_image",
        SmallImageText = "Level 10"
    }
});

// Keep the application running
Console.ReadLine();
client.Dispose();
```

Replace `YOUR_APPLICATION_ID` with your Discord application's ID.

## Changes from the Original Project

This project is based on [Lachee/discord-rpc-csharp](<https://github.com/Lachee/discord-rpc-csharp>) and includes modifications to support Native AOT and improve compatibility with Discord's API.

### JSON Deserialization

The original implementation had issues deserializing certain Discord API fields because the API can return numeric values as strings.

Custom JSON converters have been added to handle these fields:

- `UlongStringConverter`: Handles Discord snowflake IDs returned as strings.
- `IntStringConverter`: Handles discriminator values returned as strings.
- `EnumStringConverter<T>`: Handles enum fields such as flags and premium\_type.

These converters improve compatibility with Discord's API while supporting source-generated JSON serialization.

### Named Pipe Connection Timeout

A five-second timeout has been added to named pipe connections to prevent indefinite blocking.

csharp

```csharp
_stream.Connect(5000);
```

Previously, the connection used:

csharp

```csharp
_stream.Connect(0);
```

### Enum Serialization

`JsonStringEnumConverter<T>` has been added to the `Command` and `ServerEvent` enums to ensure proper Discord protocol serialization.

### Main Differences

| Feature | Original | This Fork |
| --- | --- | --- |
| Native AOT | Not supported | Supported |
| JSON Library | Newtonsoft.Json | System.Text.Json |
| JSON Serialization | Runtime-based | Source-generated |
| User Field Handling | Original conversion | Custom JSON converters |
| Pipe Connection | No timeout | Five-second timeout |
| Trimming | Not supported | Supported |

## Requirements

- .NET 10.0 or later.
- Discord Desktop Client running.

## Native AOT Publishing

To publish an application using Native AOT, run:

bash

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

Replace `win-x64` with the appropriate runtime identifier for your target platform.

## Releasing

Two workflows handle releases:

1. `.github/workflows/build.yml` runs on every push and pull request. It restores, builds, tests, and packs the project. When you push a tag that starts with `v` (for example `v1.0.0`) and the build succeeds, it creates a **draft** GitHub Release for that tag.
2. `.github/workflows/publish.yml` runs when a GitHub Release is published. It packs and pushes the package to NuGet.org using OIDC trusted publishing (no stored API key).

To release a new version:

1. Bump `<Version>` in `DiscordRPC-NativeAOT.csproj`.
2. Commit and push.
3. Push a tag matching that version, for example:

   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

This builds and tests the code and creates a **draft** release. Review the draft, then click **Publish release** — that publishes the package to NuGet.org automatically.

Before the first automated release, add a trusted publishing policy on [nuget.org](https://www.nuget.org) pointing at this repository and the `publish.yml` workflow file, and set a `NUGET_USER` repository secret in GitHub to the nuget.org username of the person who created that policy.

## License

This project is licensed under the MIT License, following the original [discord-rpc-csharp](<https://github.com/Lachee/discord-rpc-csharp>) project.

See the LICENSE file for details.

## Credits

Original project: [Lachee/discord-rpc-csharp](<https://github.com/Lachee/discord-rpc-csharp>).

This fork contains modifications for Native AOT compatibility, JSON serialization, and connection handling.

## Issues

Report bugs and compatibility issues through the [GitHub Issues](<https://github.com/Froststrap/DiscordRPC-NativeAOT>) page.
