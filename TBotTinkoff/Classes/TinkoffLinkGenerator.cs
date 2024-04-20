using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TBotTinkoff.Classes.Helper;

namespace TBotTinkoff.Classes
{
    public class TinkoffLinkGenerator
    {
        public async Task<OrderResult> GenerateLink(ParamsOrder paramsOrder)
        {
            var reqParams = new Dictionary<string, dynamic>
            {
                { "TerminalKey", paramsOrder.TerminalKey },
                { "Amount", paramsOrder.Product.ItemAmout },
                { "OrderId", paramsOrder.OrderId },
                { "Description", paramsOrder.Product.Name },
                { "RedirectDueDate", DateTime.Parse(paramsOrder.RedirectDueDate).ToString("yyyy-MM-ddThh:mm:ss+03:00") }
            };

            var listParams = reqParams.ToList().Select(x => (x.Key, x.Value)).ToList();
            listParams.Add(("Password", paramsOrder.Password));
            listParams.Sort((x, y) => string.Compare(x.Key, y.Key, StringComparison.Ordinal));

            var tokenString = "";

            foreach (var item in listParams)
            {
                tokenString += item.Value;
            }

            var token = HashString(tokenString);

            // Add params with nested data
            reqParams.Add("Token", token);
            reqParams.Add("DATA", new Dictionary<string, string>() { { "Phone", paramsOrder.Phone } });
            reqParams.Add("Receipt", new Dictionary<string, dynamic>()
            {
                // User information
                {"Email", paramsOrder.Email},
                {"Phone", paramsOrder.Phone},
                // Taxation
                {"Taxation", "usn_income_outcome"},
                // Payment
                {
                    "Items", new List<dynamic>()
                    {
                        new Dictionary<string, dynamic>()
                        {
                            {"Name", paramsOrder.Product.Name},
                            {"Price", paramsOrder.Product.ItemPrice},
                            {"Quantity", paramsOrder.Product.QuanityValue},
                            {"Amount", paramsOrder.Product.ItemAmout},
                            {"PaymentMethod", "lfull_prepayment"},
                            {"PaymentObject", "commodity"},
                            {"Tax", "none"},

                        }
                    }
                }
            });

            using var client = new HttpClient();
            using var content = new StringContent(JsonConvert.SerializeObject(reqParams));

            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(
                "https://securepay.tinkoff.ru/v2/Init",
                content
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Get bad status code");

            var text = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OrderResult>(text);

            if (result == null)
                throw new Exception("Error deserialize object");

            return result;
        }

        private string HashString(string str)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(str);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
