using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugaExchange
{
    class Investor
    {
        readonly Exchange exchange;

        public Investor(Exchange exchange)
        {
            this.exchange = exchange;
        }
        public void InvestorMenu()
        {
            while (true)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Investor Menu:");
                Console.WriteLine("1) Deposit");
                Console.WriteLine("2) Buy Coin");
                Console.WriteLine("3) Sell Coin");
                Console.WriteLine("4) Show Portfolio");
                Console.WriteLine("5) Show Prices");
                Console.WriteLine("6) Exit");
                Console.Write("->");
                var option = Console.ReadLine();
                Console.WriteLine("--------------------------------------------------");

                if (option == "1")
                {
                    Deposit();
                }
                else if (option == "2")
                {
                    BuyCoin();
                }
                else if (option == "3")
                {
                    SellCoin();
                }
                else if (option == "4")
                {
                    ShowPortfolio();
                }
                else if (option == "5")
                {
                    ShowPrices();
                }
                else if (option == "6")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid option");
                }
            }
            
        }

        private void Deposit()
        {
            Console.Write("How much do you want to deposit?\n->");
            try
            {
                decimal amount = Convert.ToDecimal(Console.ReadLine()); // try catch para salvaguardar as excepções do ToDecimal - Format (ex:"ab10") & Overflow (ex:"99999...9999999")
                if (amount < 0) 
                {
                    throw new Exception();
                }
                exchange.Deposit(amount);
                Console.WriteLine($"{amount} EUR was deposited successfully.");
            } catch (Exception)
            {
                Console.WriteLine("Invalid amount");
            }
            
        }

        private void BuyCoin()
        {
            Console.Write("What coin do you want to buy?\n->");
            var coinName = Console.ReadLine();
            var coins = exchange.GetPrices();
            var coinToBuy = coins.SingleOrDefault(c => c.name == coinName);

            if (coinToBuy == null)
            {
                Console.WriteLine("Coin not found");
                return;
            }

            Console.Write($"How many {coinName} do you want to buy?\n->");
            try
            {
                decimal quantity = Convert.ToDecimal(Console.ReadLine()); // try catch para salvaguardar as excepções do ToDecimal - Format (ex:"ab10") & Overflow (ex:"99999...9999999")
                if (quantity < 0)
                {
                    throw new Exception();
                }
                exchange.BuyCoin(coinName, quantity);
                Console.WriteLine($"{quantity} {coinName} was bought successfully.");
            }
            catch (Exception) // 3 casos: ToDecimal, negativos, falta de fundos
            {
                Console.WriteLine("Invalid quantity");
            }
        }

        private void SellCoin()
        {
            Console.Write("What coin do you want to sell?\n->");
            var coinName = Console.ReadLine();
            var coins = exchange.GetPrices();
            var coinToSell = coins.SingleOrDefault(c => c.name == coinName);

            if (coinToSell == null)
            {
                Console.WriteLine("Coin not found");
                return;
            }

            Console.Write($"How many {coinName} do you want to sell?\n->");
            try
            {
                decimal quantity = Convert.ToDecimal(Console.ReadLine());
                if (quantity < 0)
                {
                    throw new Exception();
                }
                exchange.SellCoin(coinName, quantity);
                Console.WriteLine($"{quantity} {coinName} was sold successfully.");
            }
            catch (Exception) 
            {
                Console.WriteLine("Invalid quantity");
            }

        }

        private void ShowPortfolio()
        {
            var coins = exchange.GetPrices();
            var euroBalance = exchange.GetEuroBalance();
            Console.WriteLine($"{euroBalance:0.00} EUR @ 1.00 | {euroBalance:0.00} EUR");
            foreach (var coin in coins)
            {
                var value = coin.quantity * coin.price;
                Console.WriteLine($"{coin.quantity:0.00} {coin.name} @ {coin.price:0.00} | {value:0.00} EUR ");
            }
        }

        /// <summary>
        /// Mostra o câmbio : preços atuais das moedas
        /// </summary>
        private void ShowPrices()
        {
            var coins = exchange.GetPrices();
            foreach (var coin in coins)
            {
                Console.WriteLine($"{coin.name}     {coin.price:0.00}");
            }
        }
    }
}
