using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBotTinkoff.Classes.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TBotTinkoff.Classes
{
    public class Bot
    {
        private TelegramBotClient _bot;
        private CancellationTokenSource _token;
        private TinkoffLinkGenerator _generator;
        private TextMessageHandler _messageHandler;

        public Bot(string token)
        {
            _bot = new TelegramBotClient(token);
            _token = new CancellationTokenSource();

            _generator = new TinkoffLinkGenerator();
            _messageHandler = new TextMessageHandler(_bot, _generator);
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
                await _messageHandler.OnMessageAsync(
                    update.Message, 
                    cancellationToken
                );
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
