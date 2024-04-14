using TBotTinkoff.Classes;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var token = await File.ReadAllTextAsync("token.txt");

        var bot = new Bot(token);

        bot.Start();
        await bot.GetInfo();

        Console.ReadLine();
        bot.Stop();
    }
}