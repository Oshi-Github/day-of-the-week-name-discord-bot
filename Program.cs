using System;
internal static class Program
{
    private static Task Main(string[] args)
    {
        return Program.MainAsync();
    }

    private static async Task MainAsync()
    {
        Client client = new Client();

        await client.Run();

        await Task.Delay(Timeout.Infinite);
    }
}
