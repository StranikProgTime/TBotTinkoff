using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBotTinkoff.Classes.Helper
{
    public class Product
    {
        public string Name { get; set; } // Название продукта
        public int QuanityValue { get; set; } // Колчество продуктов
        public int ItemPrice { get; set; } // Цена одного продукта (коп.)
        public int ItemAmout { get; set; } // Сумма стоимости всех продуктов (коп.)
                                           
        public Product(string name, int itemPrice) // itemPrice - руб.
        {
            Name = name;
            QuanityValue = 1;
            ItemPrice = itemPrice * 100;
            ItemAmout = QuanityValue * ItemPrice;
        }
    }
}
