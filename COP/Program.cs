// See https://aka.ms/new-console-template for more information


using System;
using System.Xml.Serialization;
using COP.Models;
using COP.Servuces;
namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Amazon!");

            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("U. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");
            Console.WriteLine("Q. Quit");

            List<Item?> list = ProductServiceProxy.Current.Products;

            char choice;
            do
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch (choice)
                {
                    case 'C':
                    case 'c':
                        ProductServiceProxy.Current.AddOrUpdate(new Item
                        {
                            Name = Console.ReadLine()
                        });
                        break;
                    case 'R':
                    case 'r':

                        list.ForEach(Console.WriteLine);
                        break;
                    case 'U':
                    case 'u':
                        //select one of the products
                        Console.WriteLine("Which product would you like to update?");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = list.FirstOrDefault(p => p.Id == selection);

                        if (selectedProd != null)
                        {
                            selectedProd.Product.Name = Console.ReadLine() ?? "ERROR";
                            ProductServiceProxy.Current.AddOrUpdate(selectedProd);
                        }
                        break;
                    case 'D':
                    case 'd':
                        //select one of the products
                        //throw it away
                        Console.WriteLine("Which product would you like to update?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        ProductServiceProxy.Current.Delete(selection);
                        break;
                    case 'Q':
                    case 'q':
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                }
            } while (choice != 'Q' && choice != 'q');

            Console.ReadLine();
        }
    }


}
