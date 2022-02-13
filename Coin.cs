using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugaExchange
{
    /// <summary>
    /// Criei esta classe para permitir juntar uma string e um decimal
    /// </summary>
    class Coin 
    {
        public string name { get; set; }

        public decimal price { get; set; }

        public decimal quantity { get; set; }

        public Coin() { } // para o json serializer usar

        public Coin(string name, int price, decimal quantity) // este construtor é para o AddCoin
        {
            this.name = name;
            this.price = price;
            this.quantity = quantity;
        }   
    }
}
