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

            List<Product?> list = ProductServiceProxy.Current.Products;

            char choice;

            do
            {
                Console.WriteLine("I. Manage Inventory");
                Console.WriteLine("C. Manage Cart");
                Console.WriteLine("Q. Quit");

                string? input = Console.ReadLine();
                choice = input[0];
                switch (choice)
                {
                    case 'I':
                    case 'i':
                        ManageInventory();
                        break;

                    case 'C':
                    case 'c':
                        ManageCart();
                        break;
                    case 'Q':
                    case 'q':
                        Checkout();
                        break;
                    default:
                        Console.WriteLine("Error. Unknown Command");
                        break;
                }
            } while (choice != 'Q' && choice != 'q');

            Console.ReadLine();
        }


        static void ManageInventory()
        {
            char option;
            do
            {
                Console.WriteLine("Inventory Managament");
                Console.WriteLine("C. Create Product");
                Console.WriteLine("R. Read Products");
                Console.WriteLine("U. Update Product");
                Console.WriteLine("D. Delete Product");
                Console.WriteLine("B. Back to Menu");

                string? input = Console.ReadLine();
                option = input[0];

                switch (option)
                {
                    case 'C':
                    case 'c':
                        Console.WriteLine("Enter Product Name: ");
                        string name = Console.ReadLine() ?? "Unnamed";
                        Console.WriteLine("Enter Product Price: ");
                        decimal price = decimal.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("Enter Product Quantity: ");
                        int stock = int.Parse(Console.ReadLine() ?? "0");
                        ProductServiceProxy.Current?.AddOrUpdate(new Product(name, price, stock));
                        break;
                    case 'R':
                    case 'r':
                        ProductServiceProxy.Current?.DisplayInventory();

                        break;
                    case 'U':
                    case 'u':
                        Console.WriteLine("Enter Product to update: ");
                        int id = int.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("Enter new name: ");
                        string newName = Console.ReadLine() ?? "Unnamed";
                        Console.WriteLine("Enter new price: ");
                        decimal newPrice = decimal.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("Enter new stock quantity: ");
                        int newStock = int.Parse(Console.ReadLine() ?? "0");
                        ProductServiceProxy.Current?.Update(id, newName, newPrice, newStock); 
                        break;
                    case 'D':
                    case 'd':
                        Console.WriteLine("Enter Product to delete: ");
                        id = int.Parse(Console.ReadLine() ?? "0");
                        ProductServiceProxy.Current?.Delete(id);
                        break;
                }

            } while (option != 'B' && option != 'b');


        }

        static void ManageCart()
        {
            char option;
            do
            {
                Console.WriteLine("Shopping Cart Management");
                Console.WriteLine("A. Add item to Cart");
                Console.WriteLine("R. Read items in cart");
                Console.WriteLine("U. Update items in cart");
                Console.WriteLine("D. Delete items from cart");
                Console.WriteLine("B. Back to main menu");

                string? input = Console.ReadLine();
                option = input[0];

                switch (option)
                {
                    case 'A':
                    case 'a':
                        Console.WriteLine("Enter the product ID to add: ");
                        int id = int.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("Enter quantity: ");
                        int quantity = int.Parse(Console.ReadLine() ?? "0");
                        CartServiceProxy.Current?.AddToCart(id, quantity);
                        break;

                    case 'R':
                    case 'r':
                        CartServiceProxy.Current?.DisplayCart();
                        break;

                    case 'U':
                    case 'u':
                        Console.WriteLine("Enter product ID to update: ");
                        id = int.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("Enter new quantity: ");
                        quantity = int.Parse(Console.ReadLine() ?? "0");
                        CartServiceProxy.Current?.UpdateCart(id, quantity);
                        break;

                    case 'D':
                    case 'd':
                        Console.WriteLine("Enter product ID to delete: ");
                        id = int.Parse(Console.ReadLine() ?? "0");
                        CartServiceProxy.Current?.DeleteFromCart(id);
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again");
                        break;
                }

            } while (option != 'B' && option != 'b');


        }

        static void Checkout()
        {
            CartServiceProxy.Current?.Checkout();
        }
    }
}

