// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COP.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name {  get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CartQuantity { get; set; }




        public string? Display
        {
            get
            {
                return $"{Id}. {Name}  ${Price} {StockQuantity} in stock";
            }
        }

        public string? CartDisplay
        {
            get
            {
                return $"{Id}. {Name}  ${Price}  {CartQuantity} in cart";
            }
        }

        public Product()
        {
            Name = "Unnamed";
            Price = 0;
            StockQuantity = 0;
            CartQuantity = 0;
        }
        public Product(string name, decimal price, int stockQuantity)
        {
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
            CartQuantity = 0;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}