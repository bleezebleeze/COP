using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using COP.Models;
using COP.Servuces;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel : INotifyPropertyChanged
    {
        private ProductServiceProxy _invSvc = ProductServiceProxy.Current;
        private CartServiceProxy _cartSvc = CartServiceProxy.Current;
        public ItemViewModel? SelectedItem { get; set; }
        public ItemViewModel? SelectedCartItem { get; set; }
        public ObservableCollection<ItemViewModel?> Inventory
        {
            get
            {
                return new ObservableCollection<ItemViewModel?>(_invSvc.Products
                    .Where(i => i?.Quantity > 0).Select(m => new ItemViewModel(m))
                    );
            }
        }

        public ObservableCollection<ItemViewModel?> ShoppingCart
        {
            get
            {
                return new ObservableCollection<ItemViewModel?>(_cartSvc.CartItems
                    .Where(i => i?.Quantity > 0).Select(m => new ItemViewModel(m))
                    );
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshUx()
        {
            NotifyPropertyChanged(nameof(Inventory));
            NotifyPropertyChanged(nameof(ShoppingCart));
        }

        public void PurchaseItem()
        {
            if (SelectedItem != null)
            {
                var shouldRefresh = SelectedItem.Model.Quantity >= 1;
                var updatedItem = _cartSvc.AddOrUpdate(SelectedItem.Model);

                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }

            }


        }
        public void ReturnItem()
        {
            if (SelectedCartItem != null)
            {
                var shouldRefresh = SelectedCartItem.Model.Quantity >= 1;
                var updatedItem = _cartSvc.ReturnItem(SelectedCartItem.Model);

                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
            }
        }

        public decimal CalculateTotal()
        {
            return _cartSvc.CartItems.Sum(i => (i.Price * i.Quantity) ?? 0);
        }

        public decimal CalculateTax()
        {
            decimal subtotal = CalculateTotal();
            return subtotal * 0.07m;
        }

        public decimal CalculateTotalWithTax()
        {
            decimal subtotal = CalculateTotal();
            decimal tax = subtotal * 0.07m;
            return subtotal + tax;
        }

        public async void Checkout()
        {
            if(_cartSvc.CartItems.Count == 0 || _cartSvc.CartItems.All(i => i.Price <= 0))
            {
                await Application.Current.MainPage.DisplayAlert("Checkout", "Your Cart is Empty", "OK");
                return;

            }

            decimal subtotal = CalculateTotal();
            decimal tax = CalculateTax();
            decimal total = CalculateTotalWithTax();

            StringBuilder receipt = new StringBuilder();
            receipt.AppendLine("Receipt:");
            receipt.AppendLine("-------");

            foreach (var item in _cartSvc.CartItems.Where(i => i.Quantity > 0))
            {
                receipt.AppendLine($"+{item.Product.Name} x{item.Quantity} - ${item.Price * item.Quantity:F2}");
            }

            receipt.AppendLine("-------");
            receipt.AppendLine($"Subtotal: ${subtotal:F2}");
            receipt.AppendLine($"Tax (7%): ${tax:F2}");
            receipt.AppendLine($"Total: ${total:F2}");

            await Application.Current.MainPage.DisplayAlert("Checkout Complete", receipt.ToString(), "OK");

            foreach (var item in _cartSvc.CartItems.ToList())
            {
                while (item.Quantity > 0)
                {
                    _cartSvc.ReturnItem(item);
                }
                
            }

            RefreshUx();
        }

        public void AddItemsToCart(ItemViewModel itemViewModel)
        {
            if (itemViewModel != null && itemViewModel != null)
            {
                _cartSvc.AddOrUpdate(itemViewModel.Model);
            }

            itemViewModel.QuantityToAdd = 1;
            RefreshUx();
        }
    }

}
