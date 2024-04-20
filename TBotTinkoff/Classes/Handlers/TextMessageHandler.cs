using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TBotTinkoff.Classes.Handlers
{
    public class TextMessageHandler
    {
        private TelegramBotClient _bot;
        private TinkoffLinkGenerator _generator;

        public TextMessageHandler(TelegramBotClient bot, TinkoffLinkGenerator generator) 
            => (_bot, _generator) = (bot, generator);

        public async Task OnMessageAsync(Message message, CancellationToken token)
        {
            if (message.Text == "/gen_link")
            {
                var result = await _generator.GenerateLink(null);

                if (!result.Success)
                {
                    await _bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Fail. Error info:\n" + result.Details,
                        cancellationToken: token
                    );
                }
                else
                {
                    await _bot.SendTextMessageAsync(
                        message.Chat.Id,
                        $"Link: [*click*]({result.PaymentURL})"
                    );
                }
            }
            else
            {
                await _bot.SendTextMessageAsync(
                    message.Chat.Id,
                    message.Text,
                    cancellationToken: token
                );
            }
        }

    }
}
