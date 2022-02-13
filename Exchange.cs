using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TugaExchange
{
    class Exchange
    {
        readonly string filePath = @"..\..\..\DataFile.json";
        Data data;

        /// <summary>
        /// Método serve para atualizar os preços a cada n segundos
        /// </summary>
        public void UpdatePrices()
        {
            TimeSpan diff = DateTime.UtcNow - data.lastPriceUpdate; // diff é quanto tempo passou desde a ultima atualização q é igual à diferença entre o tempo atual e a última atualização 
            var updateCount = diff.TotalSeconds / data.priceUpdateInSeconds; // ver quantos intervalos de n é que cabem em diff, se o último intervalo não for completo é descartado no for loop

            for (int i = 1; i < updateCount; i++) // exemplo: updateCount = 3.5, logo queremos 3 updates, o ciclo vai ser executado com i=1,2,3. Para não fazer updateCount - 1
            {
                UpdatePricesOnce();
            }
            data.lastPriceUpdate = DateTime.UtcNow;
            Save();
        }

        /// <summary>
        /// Método que faz 1 atualização só, é chamado pelo método acima
        /// </summary>
        private void UpdatePricesOnce()
        {
            foreach (Coin coin in data.coins)
            {
                var min = decimal.ToDouble(coin.price * 0.995m); // 0.995m é o resultado de 1 - 0.5%
                var max = decimal.ToDouble(coin.price * 1.005m);
                coin.price = Convert.ToDecimal(RandomDouble(min, max));
            }
        }

        // Método para tirar nº à sorte entre min e máx
        private static double RandomDouble(double min, double max) //https://stackoverflow.com/questions/9021344/c-sharp-generating-random-decimals-between-two-decimals
        {
            var random = new Random();
            return (random.NextDouble() * Math.Abs(max - min)) + min;
        }

        /// <summary>
        /// Adiciona coin à Exchange, lança excepção se a coin já existir
        /// </summary>  
        public void AddCoin(string coinName)
        {
            var coinToAdd = data.coins.SingleOrDefault(c => c.name == coinName);
            if (coinToAdd != null)
            {
                throw new Exception();
            }
            // só podemos adicionar se for null, ou seja, se a coin não foi encontrada
            data.coins.Add(new Coin(coinName, 1, 0)); // o preço inicial é 1 euro e quantidade inicial é 0
            Save();
        }

        // https://stackoverflow.com/questions/3801748/select-method-in-listt-collection
        public List<string> GetCoins() 
        {
            return data.coins.Select(c => c.name).ToList();
        }

        public void Deposit(decimal amount)
        {
            data.euroBalance += amount;
            Save();
        }

        public decimal GetEuroBalance()
        {
            return data.euroBalance;
        }
        
        /// <summary>
        /// Remove coin da Exchange, lança excepção se a coin não existir ou tiver um investidor
        /// </summary>  
        public void RemoveCoin(string coinName)
        {
            // Single encontra um elemento da lista onde a condição (c.name == coinName) é verdadeira ou lança excepção se não houver nenhum
            var coinToRemove = data.coins.Single(c => c.name == coinName);
            if (coinToRemove.quantity > 0) // não deixa remover a coin se o investidor tiver essa coin no seu portfolio
            {
                throw new Exception();
            }
            // só podemos remover se a coin for encontrada
            data.coins.Remove(coinToRemove);
            Save();
        }

        public List<Coin> GetPrices()
        {
            UpdatePrices(); // antes de mostrar os preços ao investidor, atualizo
            return data.coins;
        }

        public void DefinePriceUpdateInSeconds (int seconds)
        {
            data.priceUpdateInSeconds = seconds;
            Save();
        }

        public int GetPriceUpdateInSeconds()
        {
            return data.priceUpdateInSeconds;
        }
 
        /// <summary>
        /// Compra coins, lança excepção caso não haja fundos
        /// </summary>
        public void BuyCoin (string coinName, decimal coinQuantity)
        {
            UpdatePrices();
            var coin = data.coins.SingleOrDefault(c => c.name == coinName); //SingleOrDefault é um select com limite de 1, devolve o primeiro elemento da lista que encontra
            var price = coin.price;
            var cost = price * coinQuantity;
            var commission = cost * 0.01m; //m serve para especificar que é decimal, assumiu que é float
            var totalCost = cost + commission; // valor bruto que o investidor paga

            if (data.euroBalance < totalCost) //verificar se o investidor tem fundos
            {
                throw new Exception();
            }

            data.euroBalance -= totalCost;
            coin.quantity += coinQuantity;
            data.totalCommission += commission;
            Save();
        }

        public void SellCoin(string coinName, decimal coinQuantity)
        {
            UpdatePrices();
            var coin = data.coins.SingleOrDefault(c => c.name == coinName); 
            var price = coin.price;
            var value = price * coinQuantity;
            var commission = value * 0.01m; 
            var netValue = value - commission;// valor líquido que o investidor recebe depois de pagar comissão

            if (coin.quantity < coinQuantity) //verificar se o investidor tem coins
            {
                throw new Exception();
            }

            data.euroBalance += netValue;
            coin.quantity -= coinQuantity;
            data.totalCommission += commission;
            Save();
        }

        public decimal GetTotalCommission()
        {
            return data.totalCommission;
        }
        
        /// <summary>
        /// Escrita no ficheiro JSON
        /// </summary>
        public void Save()
        {
            var options = new JsonSerializerOptions(); //objeto options q serve para especificar as opções de serialização
            options.WriteIndented = true; // para dar novas linhas no ficheiro JSON
            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }
        
        /// <summary>
        /// Leitura do ficheiro JSON, feita no arranque do programa
        /// </summary>
        public void Read() 
        {
            var json = File.ReadAllText(filePath);
            data = JsonSerializer.Deserialize<Data>(json);
        }

    }
}
