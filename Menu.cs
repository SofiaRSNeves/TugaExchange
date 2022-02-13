using System;
using System.IO;
using System.Text.Json;

namespace TugaExchange
{
    class Menu
    {
        static void Main(string[] args)
        {
            var exchange = new Exchange();
            exchange.Read();

            var investor = new Investor(exchange);
            var admin = new Admin(exchange);
            
            Console.WriteLine("Please select an option:");
            Console.WriteLine("a) I am an Investor");
            Console.WriteLine("b) I am an Admin");
            Console.Write("->");
            var option = Console.ReadLine();

            if (option == "a")
            {
                investor.InvestorMenu();
            }
            else if (option == "b")
            {
                admin.AdminMenu();
            }
            else
            {
                Console.WriteLine("Invalid option");
            }
            
        }
    }
}
