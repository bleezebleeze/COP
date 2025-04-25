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
        private string _newCartName = string.Empty;
        private string? _selectedCartName;

        public enum SortOption
        {
            None,
            NameAscending,
            NameDescending,
            PriceAscending,
            PriceDescending
        }
        private SortOption _inventorySortOption = SortOption.None;
        public SortOption InventorySortOption
        {
            get => _inventorySortOption;
            set
            {
                if (_inventorySortOption != value)
                {
                    _inventorySortOption = value;
                    NotifyPropertyChanged(nameof(Inventory));
                }
            }
        }

        private SortOption _cartSortOption = SortOption.None;
        public SortOption CartSortOption
        {
            get => _cartSortOption;
            set
            {
                if (_cartSortOption != value)
                {
                    _cartSortOption = value;
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
            }
        }

        public ShoppingManagementViewModel()
        {
            _selectedCartName = _cartSvc.CurrentCartName;
        }

        public string NewCartName
        {
            get => _newCartName;
            set
            {
                if (_newCartName != value)
                {
                    _newCartName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string SelectedCartName
        {
            get => _selectedCartName;
            set
            {
                if (_selectedCartName != value)
                {
                    _selectedCartName = value;
                    _cartSvc.CurrentCartName = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ShoppingCart));
                    NotifyPropertyChanged(nameof(CurrentCartDisplayName));
                }
            }
        }

        public string CurrentCartDisplayName => $"Current Cart: {SelectedCartName}";

        public ObservableCollection<string> AvailableCarts
        {
            get => new ObservableCollection<string>(_cartSvc.AvailableCartNames);
        }
        public decimal CurrentTaxRate
        {
            get
            {
                return ConfigurationViewModel.TaxRate * 100;
            }
        }

        public ObservableCollection<ItemViewModel?> Inventory
        {
            get
            {
                var items = _invSvc.Products.Where(i => i?.Quantity > 0);
                switch (InventorySortOption)
                {
                    case SortOption.NameAscending:
                        items = items.OrderBy(p => p?.Product?.Name);
                        break;
                    case SortOption.NameDescending:
                        items = items.OrderByDescending(p => p?.Product?.Name);
                        break;
                    case SortOption.PriceAscending:
                        items = items.OrderBy(p => p?.Price);
                        break;
                    case SortOption.PriceDescending:
                        items = items.OrderByDescending(p => p?.Price);
                        break;
                    default:
                        break;
                }

                return new ObservableCollection<ItemViewModel?>(items.Select(m => new ItemViewModel(m)));
            }
        }

        public ObservableCollection<ItemViewModel?> ShoppingCart
        {
            get
            {
                var items = _cartSvc.CartItems.Where(i => i?.Quantity > 0);
                switch (CartSortOption)
                {
                    case SortOption.NameAscending:
                        items = items.OrderBy(p => p?.Product?.Name);
                        break;
                    case SortOption.NameDescending:
                        items = items.OrderByDescending(p => p?.Product?.Name);
                        break;
                    case SortOption.PriceAscending:
                        items = items.OrderBy(p => p?.Price);
                        break;
                    case SortOption.PriceDescending:
                        items = items.OrderByDescending(p => p?.Price);
                        break;
                    default:
                        break;
                }

                return new ObservableCollection<ItemViewModel?>(items.Select(m => new ItemViewModel(m)));
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
            NotifyPropertyChanged(nameof(CurrentTaxRate));
            NotifyPropertyChanged(nameof(AvailableCarts));
            NotifyPropertyChanged(nameof(CurrentCartDisplayName));
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

        public bool CreateNewCart()
        {
            if (string.IsNullOrWhiteSpace(NewCartName))
                return false;

            bool result = _cartSvc.CreateNewCart(NewCartName);
            if (result)
            {
                SelectedCartName = NewCartName;
                NewCartName = string.Empty;
                NotifyPropertyChanged(nameof(AvailableCarts));
            }
            return result;
        }

        public bool DeleteCurrentCart()
        {
            string cartToDelete = SelectedCartName;
            bool result = _cartSvc.DeleteCart(cartToDelete);
            if (result)
            {
                NotifyPropertyChanged(nameof(AvailableCarts));
                NotifyPropertyChanged(nameof(ShoppingCart));
                NotifyPropertyChanged(nameof(SelectedCartName));
                NotifyPropertyChanged(nameof(CurrentCartDisplayName));
            }
            return result;
        }
        public decimal CalculateTotal()
        {
            return _cartSvc.CartItems.Sum(i => (i.Price * i.Quantity) ?? 0);
        }

        public decimal CalculateTax()
        {
            decimal subtotal = CalculateTotal();
            return subtotal * ConfigurationViewModel.TaxRate;
        }

        public decimal CalculateTotalWithTax()
        {
            decimal subtotal = CalculateTotal();
            decimal tax = subtotal * ConfigurationViewModel.TaxRate;
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
            receipt.AppendLine($"Tax ({ConfigurationViewModel.TaxRate * 100:F2}%) ${tax:F2}");
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

        public void SortInventoryByNameAscending()
        {
            InventorySortOption = SortOption.NameAscending;
        }

        public void SortInventoryByNameDescending()
        {
            InventorySortOption = SortOption.NameDescending;
        }

        public void SortInventoryByPriceAscending()
        {
            InventorySortOption = SortOption.PriceAscending;
        }

        public void SortInventoryByPriceDescending()
        {
            InventorySortOption = SortOption.PriceDescending;
        }

        public void SortCartByNameAscending()
        {
            CartSortOption = SortOption.NameAscending;
        }

        public void SortCartByNameDescending()
        {
            CartSortOption = SortOption.NameDescending;
        }

        public void SortCartByPriceAscending()
        {
            CartSortOption = SortOption.PriceAscending;
        }

        public void SortCartByPriceDescending()
        {
            CartSortOption = SortOption.PriceDescending;
        }
    }

}
