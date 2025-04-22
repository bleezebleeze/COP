using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using COP.Models;
using COP.Servuces;

namespace Maui.eCommerce.ViewModels
{
    public class ItemViewModel
    {
        public Item Model { get; set; }

        public int QuantityToAdd { get; set; } = 1;


        public ICommand? AddCommand { get; set; }
        private void DoAdd()
        {

            for (int i = 0; i < QuantityToAdd; i++)
            {
                CartServiceProxy.Current.AddOrUpdate(Model);
            }
            
        }

        void SetupCommand()
        {
            AddCommand = new Command(DoAdd);
        }
        public ItemViewModel()
        {
            Model = new Item();
            QuantityToAdd = 1;
            SetupCommand();
        }

        public ItemViewModel(Item model)
        {
            Model = model;
            QuantityToAdd = 1;
            SetupCommand();
        }

    }
}
