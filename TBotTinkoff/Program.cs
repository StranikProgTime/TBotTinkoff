using TBotTinkoff.Classes;
using TBotTinkoff.Classes.Helper;

internal class Program
{
    private static async Task Main(string[] args)
    {
        /*var token = await File.ReadAllTextAsync("token.txt");

        var bot = new Bot(token);

        bot.Start();
        await bot.GetInfo();

        Console.ReadLine();
        bot.Stop();*/

        var tinkoffData = await File.ReadAllLinesAsync("tinkoffData.txt");

        TinkoffLinkGenerator generator = new TinkoffLinkGenerator();
        var result = await generator.GenerateLink(
            new ParamsOrder()
            {
                TerminalKey = tinkoffData[0],
                Password = tinkoffData[1],
                OrderId = "ic-7025",
                Email = "dgarioch0@thetimes.co.uk",
                Phone = "+2616736956782",
                RedirectDueDate = "2024-04-23T23:55:00",
                Product = new Product("Duobam", 45349)
            }
        );
    }
}