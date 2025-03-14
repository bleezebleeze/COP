using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COP.Models;

namespace COP.Servuces
{
    public class ProductServiceProxy
    {

        private static ProductServiceProxy? instance;
        private static object instanceLock = new object();

        private ProductServiceProxy()
        {
            Products = new List<Product?>
            {
                new Product{Id = 1, Name ="Product 1"},
                new Product{Id = 2, Name ="Product 2"},
                new Product{Id = 3, Name = "Product 3" }
            };
        }
            
        public static ProductServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {

                    if (instance == null)
                    {
                        instance = new ProductServiceProxy();
                    }
                }
                return instance;
            }
        }


        public List<Product?> Products { get; private set; }



        public Product AddOrUpdate(Product product)
        {
            var existing = Products.FirstOrDefault(p => p.Id == product.Id);
            if (existing == null)
            {
                product.Id = Products.Count + 1;
                Products.Add(product);
            }
            else
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.StockQuantity = product.StockQuantity;
            }

            return product;

        }

        public void DisplayInventory()
        {
            Console.WriteLine("Inventory: ");
            Products.ForEach(Console.WriteLine);
        }

        public void Update(int id, string newName, decimal newPrice, int newStock)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.Name = newName;
                product.Price = newPrice;
                product.StockQuantity = newStock;
                Console.WriteLine("Item Updated.");


            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        public Product? Delete(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Products.Remove(product);
            }
            return product;
        }

        public Product? GetById(int id)
        {
            return Products.FirstOrDefault(p => p.Id == id);
        }

    }
}
