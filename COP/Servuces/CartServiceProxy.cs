using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COP.Models;

namespace COP.Servuces
{
    public class CartServiceProxy
    {

        private static CartServiceProxy? instance;
        private static object instanceLock = new object();

        private CartServiceProxy()
        {
            Cart = new List<Product>();
        }

        public static CartServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new CartServiceProxy();
                        }
                    }
                }
                return instance;
            }

        }

        public List<Product?> Cart { get; set; }
        public void AddToCart(int id, int quantity)
        {
            var product = ProductServiceProxy.Current?.Products.FirstOrDefault(p => p.Id == id);
            if (product != null && product.StockQuantity >= quantity)
            {
                product.StockQuantity -= quantity;
                var cartProduct = new Product(product.Name, product.Price, product.StockQuantity)
                {
                    Id = product.Id,
                    CartQuantity = quantity
                };
                Cart.Add(cartProduct);
            }
            else
            {
                Console.WriteLine("Product is not stocked enough.");
            }
        }

        public void DisplayCart()
        {
            Console.WriteLine("Shopping Cart:");
            Cart.ForEach(item => Console.WriteLine(item?.CartDisplay));
        }


        public void UpdateCart(int id, int newQuantity)
        {
            var item = Cart.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                var inventoryItem = ProductServiceProxy.Current?.Products.FirstOrDefault(p => p.Name == item.Name);
                if (inventoryItem != null && inventoryItem.StockQuantity + item.CartQuantity >= newQuantity)
                {
                    int quantityChange = newQuantity - item.CartQuantity;
                    inventoryItem.StockQuantity -= quantityChange;
                    item.CartQuantity = newQuantity;
                    Console.WriteLine("Cart item successfully updated");
                }
                else
                {
                    Console.WriteLine("Insufficient stock");
                }

            }
            else
            {
                Console.WriteLine("Cart item not found.");
            }
        }
        public void DeleteFromCart(int id)
        {
            var item = Cart.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                Cart.Remove(item);
                var product = ProductServiceProxy.Current?.Products.FirstOrDefault(p => p.Name == item.Name);
                if (product != null)
                {
                    product.StockQuantity += item.CartQuantity;
                }
            }
        }

        public void Checkout()
        {
            decimal total = Cart.Sum(p => p.Price * p.CartQuantity);
            decimal tax = total * 0.07m;
            decimal finalTotal = total + tax;

            Console.WriteLine("Reciept: ");
            Cart.ForEach(item => Console.WriteLine(item.CartDisplay));
            Console.WriteLine($"Subtotal: ${total:F2}");
            Console.WriteLine($"Tax: ${tax:F2}");
            Console.WriteLine($"Total: ${finalTotal:F2}");

            Cart.Clear();
        }
    }
}
