using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using COP.DTO;
using COP.Servuces;

namespace COP.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public ProductDTO Product { get; set; }
        public int? Quantity { get; set; }

        public decimal Price { get; set; }

        


        public override string ToString()
        {
            return $"{ Product.ToString()} Quantity: {Quantity} Price: {Price:F2}";
        }
        public string Display 
        {  
            get
            {
                return $"{Product?.Display ?? string.Empty} {Quantity} {Price:F2}";
            } 
        }

        public Item()
        {
            Product = new ProductDTO();
            Quantity = 0;
            Price = 0;

             }

        
        public Item(Item i)
        {
            Product = new ProductDTO(i.Product);
            Quantity = i.Quantity;
            Price = i.Price;
            Id = i.Id;
        }
    }
}
