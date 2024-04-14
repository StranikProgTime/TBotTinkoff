using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TBotTinkoff.Classes
{
    public class Bot
    {
        private TelegramBotClient _bot;
        private CancellationTokenSource _token;
        private TinkoffLinkGenerator _generator;

        public Bot(string token)
        {
            _bot = new TelegramBotClient(token);
            _token = new CancellationTokenSource();

            _generator = new TinkoffLinkGenerator();
        }

        public void Start()
        {
            _bot.StartReceiving(
                HadleUpdateAsync,
                HadleErrorAsync,
                new Telegram.Bot.Polling.ReceiverOptions()
                {
                    ThrowPendingUpdates = true // Скипает обработку старых событий
                },
                cancellationToken: _token.Token
            );
        }

        public void Stop()
        {
            _token.Cancel();
        }

        public async Task GetInfo()
        {
            var botInfo = await _bot.GetMeAsync(cancellationToken: _token.Token);
            Console.WriteLine($"Бот {botInfo.Username} запущен");
        }

        private async Task HadleUpdateAsync(
            ITelegramBotClient bot, 
            Update update, 
            CancellationToken cancellationToken
        )
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                if (update.Message.Text == "/gen_link")
                {
                    var result = await _generator.GenerateLink();

                    if (!result.Success)
                    {
                        await _bot.SendTextMessageAsync(
                            update.Message.Chat.Id,
                            "Fail. Error info:\n" + result.Details,
                            cancellationToken: cancellationToken
                        );
                    }
                }
                else
                {
                    await _bot.SendTextMessageAsync(
                        update.Message.Chat.Id,
                        update.Message.Text,
                        cancellationToken: cancellationToken
                    );
                }
            }
        }

        private Task HadleErrorAsync(
            ITelegramBotClient bot,
            Exception exception,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("Error: \n" + exception);
            Environment.Exit(1);
            return null;
        }
    }
}
