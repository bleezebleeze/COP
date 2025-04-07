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

        public ICommand? AddCommand { get; set; }


        public override string ToString()
        {
            return $"{ Product.ToString()} Quantity: {Quantity}";
        }
        public string Display 
        {  
            get
            {
                return $"{Product?.Display ?? string.Empty} {Quantity}";
            } 
        }

        public Item()
        {
            Product = new ProductDTO();
            Quantity = 0;

            AddCommand = new Command(DoAdd);
        }

        private void DoAdd()
        {
            CartServiceProxy.Current.AddOrUpdate(this);
        }
        public Item(Item i)
        {
            Product = new ProductDTO(i.Product);
            Quantity = i.Quantity;
            Id = i.Id;

           AddCommand = new Command(DoAdd);
        }
    }
}
