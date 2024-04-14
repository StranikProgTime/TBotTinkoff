using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBotTinkoff.Classes.Helper;

namespace TBotTinkoff.Classes
{
    public class TinkoffLinkGenerator
    {
        public async Task<OrderResult> GenerateLink()
        {
            using var client = new HttpClient();

            var response = await client.PostAsync(
                "https://securepay.tinkoff.ru/v2/Init",
                null
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Get bad status code");

            var text = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OrderResult>(text);

            if (result == null)
                throw new Exception("Error deserialize object");

            return result;
        }
    }
}
