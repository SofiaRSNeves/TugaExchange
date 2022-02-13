using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugaExchange
{
    /// <summary>
    /// FRONT-END PARA O MENU DE ADMIN
    /// </summary>
    class Admin
    {
        readonly Exchange exchange;

        public Admin (Exchange exchange) 
        {
            this.exchange = exchange;
        }

        public void AdminMenu()
        {
            while (true) // para continuar a mostrar o menu
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Admin Menu:");
                Console.WriteLine("1) Add Coin");
                Console.WriteLine("2) Remove Coin");
                Console.WriteLine("3) Show Commission Report");
                Console.WriteLine("4) Exit");
                Console.Write("->");
                var option = Console.ReadLine();
                Console.WriteLine("--------------------------------------------------");

                if (option == "1")
                {
                    AddCoin();
                }
                else if (option == "2")
                {
                    RemoveCoin();
                }
                else if (option == "3")
                {
                    ShowCommissionReport();
                }
                else if (option == "4")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid option");
                }
            }
            
        }

        private void AddCoin()
        {
            Console.Write("Please type the name of the coin you want to add:\n->");
            var coinName = Console.ReadLine();
       
            try
            {
                exchange.AddCoin(coinName);
                Console.WriteLine($"Coin {coinName} was added to the system successfully.");
            }
            catch (Exception)
            {
                Console.WriteLine($"Coin {coinName} already exists.");
            }
        }

        private void RemoveCoin()
        {
            Console.Write("Please type the name of the coin you want to remove:\n->");
            var coinName = Console.ReadLine();

            try
            {
                exchange.RemoveCoin(coinName);
                Console.WriteLine($"Coin {coinName} was removed from the system successfully.");
            }
            catch( Exception) // 2 excepções: coin não existe/coin existe no portfolio do investidor
            {
                Console.WriteLine($"Coin {coinName} cannot be removed.");
            }
        }

        private void ShowCommissionReport()
        {
            var commission = exchange.GetTotalCommission();
            Console.WriteLine($"Total Commission is: {commission:0.00} euros");
        }
    }
}
