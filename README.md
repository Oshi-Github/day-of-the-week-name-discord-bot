
# Day of the Week Name Discord Bot

## Description

Discord bot written in C# .NET 6.0 using some external packages for handling interfacing with Discord and consuming information from .env files.

## Packages Used

* Discord.Net 3.9.0
* dotenv.net 3.1.2

## Usage

This is mainly for my own future reference as I don't intend this to be used widely.

1. If you don't have it already, download the [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Download the repository
3. Create a <code>.env</code> file containing two keys (replacing <strong>xXX</strong> with your values) and place it into the root of the directory:
    * <code>NAME_BOT_TOKEN=xXX</code> (This is for your Discord bot token)
    * <code>NAME_BOT_NAME_PATTERN=xXX</code> (This is used as a prefix to find users names to append the date to)
4. Change directory to the project in a terminal, or open the project in an editor with an integrated terminal
5. Run <code>dotnet run</code> in a terminal, this will automatically build if needed

If you would like to have this run automatically, you can make use of tools such as windows task scheduler on the <code>.exe</code> or you can move it over to a server

## Post Project Thoughts

I've moved on from this project at this point, but something that would have been nice to have instead of a pattern search for a name, is a new guild role in the Discord server that can be assigned to interested members, they could then be found via this role instead and either use a replacement token or just append the day onto the name.
