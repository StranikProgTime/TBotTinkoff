using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBotTinkoff.Classes.Helper
{
    public class ParamsOrder
    {
        public string TerminalKey { get; set; }
        public string OrderId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RedirectDueDate { get; set; }
        public Product Product { get; set; }
    }
}
