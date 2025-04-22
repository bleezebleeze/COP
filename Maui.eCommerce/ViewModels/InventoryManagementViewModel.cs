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
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        public Item? SelectedProduct { get; set; }
        public string? Query { get; set; }
        private ProductServiceProxy _svc = ProductServiceProxy.Current;
        public enum SortOption
        {
            None,
            NameAscending,
            NameDescending,
            PriceAscending,
            PriceDescending
        }

        private SortOption _currentSortOption = SortOption.None;
        public SortOption CurrentSortOption
        {
            get => _currentSortOption;
            set
            {
                if (_currentSortOption != value)
                {
                    _currentSortOption = value;
                    NotifyPropertyChanged(nameof(Products));
                }
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

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
        }

        public ObservableCollection<Item?> Products
        {
            get
            {
                var filteredList = _svc.Products.Where(p => p?.Product?.Name?.ToLower().Contains(Query?.ToLower() ?? string.Empty) ?? false);
                switch (CurrentSortOption)
                {
                    case SortOption.NameAscending:
                        filteredList = filteredList.OrderBy(p => p?.Product?.Name);
                        break;
                    case SortOption.NameDescending:
                        filteredList = filteredList.OrderByDescending(p => p?.Product?.Name);
                        break;
                    case SortOption.PriceAscending:
                        filteredList = filteredList.OrderBy(p => p?.Price);
                        break;
                    case SortOption.PriceDescending:
                        filteredList = filteredList.OrderByDescending(p => p?.Price);
                        break;
                    default:
                        break;
                }

                return new ObservableCollection<Item?>(filteredList);
            }
        }

        public Item? Delete()
        {
            var item = _svc.Delete(SelectedProduct?.Id ?? 0);
            NotifyPropertyChanged("Products");
            return item;
        }

        public void SortByNameAscending()
        {
            CurrentSortOption = SortOption.NameAscending;
        }

        public void SortByNameDescending()
        {
            CurrentSortOption = SortOption.NameDescending;
        }

        public void SortByPriceAscending()
        {
            CurrentSortOption = SortOption.PriceAscending;
        }

        public void SortByPriceDescending()
        {
            CurrentSortOption = SortOption.PriceDescending;
        }
    }
}
