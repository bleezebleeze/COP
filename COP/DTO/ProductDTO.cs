using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COP.Models;

namespace COP.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public string? Display
        {
            get
            {
                return $"{Id}. {Name} - ${Price:F2}";
            }
        }

        public ProductDTO()
        {
            Name = string.Empty;
            Price = 0;
        }

        public ProductDTO(Product p)
        {
            Name = p.Name;
            Id = p.Id;
            Price = p.Price;
        }
        public ProductDTO(ProductDTO p)
        {
            Name = p.Name;
            Id = p.Id;
            Price = p.Price;
        }
        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}
