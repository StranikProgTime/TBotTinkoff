using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TBotTinkoff.Classes.Helper
{
    public class OrderResult
    {
        public string TerminalKey { get; set; }
        public int Amount { get; set; }
        public string OrderId { get; set; }
        public bool Success { get; set; }
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string ErrorCode { get; set; }
        public string PaymentURL { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
