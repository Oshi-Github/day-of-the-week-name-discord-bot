using LogMessage = Discord.LogMessage;
using DiscordSocketClient = Discord.WebSocket.DiscordSocketClient;
using DiscordSocketConfig = Discord.WebSocket.DiscordSocketConfig;
using TokenType = Discord.TokenType;
using SocketGuild = Discord.WebSocket.SocketGuild;
using SocketGuildUser = Discord.WebSocket.SocketGuildUser;
using GuildUserProperties = Discord.GuildUserProperties;
using RestGuildUser = Discord.Rest.RestGuildUser;
using GatewayIntents = Discord.GatewayIntents;

class Client
{
    private const int permissionsInteger = 201326592;
    private readonly DiscordSocketClient discordClient = new DiscordSocketClient(new DiscordSocketConfig()
    {
        GatewayIntents = GatewayIntents.AllUnprivileged
    });

    private const string botTokenKey = "NAME_BOT_TOKEN";
    private const string botNamePattern = "NAME_BOT_NAME_PATTERN";

    public Client()
    {
        this.discordClient.Log += this.Log;
    }

    private Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());

        return Task.CompletedTask;
    }

    public async Task Run()
    {
        string botToken = EnvironmentVariables.GetVariable(botTokenKey);

        await this.discordClient.LoginAsync(TokenType.Bot, botToken);
        await this.discordClient.StartAsync();

        this.discordClient.Ready += this.ModifyUsers;
    }

    private async Task ModifyUsers()
    {
        foreach (SocketGuild guild in this.discordClient.Guilds)
        {
            int botMaxRolePosition = this.GetBotMaxRolePosition(guild);

            IReadOnlyCollection<RestGuildUser> modifiableUsers = await this.GetModifiableUsersInGuild(guild);

            foreach (RestGuildUser modifiableUser in modifiableUsers)
            {
                int userMaxRolePosition = modifiableUser.Hierarchy;

                if (userMaxRolePosition > botMaxRolePosition)
                {
                    continue;
                }

                await modifiableUser.ModifyAsync(this.UpdateNickname);
            }
        }

        await this.CloseClient();
    }

    private int GetBotMaxRolePosition(SocketGuild guild)
    {
        SocketGuildUser bot = guild.GetUser(this.discordClient.CurrentUser.Id);

        return bot.Roles.Max(socketRole => socketRole.Position);
    }

    private async Task<IReadOnlyCollection<RestGuildUser>> GetModifiableUsersInGuild(SocketGuild guild)
    {
        string startsWithQuery = EnvironmentVariables.GetVariable(botNamePattern);

        return await guild.SearchUsersAsync(startsWithQuery);
    }

    private void UpdateNickname(GuildUserProperties userProperties)
    {
        userProperties.Nickname = $"{EnvironmentVariables.GetVariable(botNamePattern)} {this.GetCurrentDay()}";
    }

    private string GetCurrentDay()
    {
        DateTime currentDate = DateTime.Today;

        // Custom format string "dddd" - represents Full Day e.g. Monday
        return currentDate.ToString("dddd");
    }

    private async Task CloseClient()
    {
        await this.discordClient.LogoutAsync();
        await this.discordClient.DisposeAsync();

        Environment.Exit(0);
    }
}
