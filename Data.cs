using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugaExchange
{
    /// <summary>
    /// Classe para guardar o estado atual da TugaExchange, para que esta seja carregada automaticamente quando reiniciar o programa.
    /// </summary>
    class Data
    {
        public List<Coin> coins { get; set; }

        public DateTime lastPriceUpdate { get; set; } //data do último câmbio

        public int priceUpdateInSeconds { get; set; } // n
        
        public decimal totalCommission { get; set; }

        public decimal euroBalance { get; set; }
    }
}
