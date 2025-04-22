using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        private static decimal _taxRate = 0.07m;
        private string? _taxRateInput;

        public event PropertyChangedEventHandler? PropertyChanged;

        public static decimal TaxRate
        {
            get => _taxRate;
            private set => _taxRate = value;
        }

        public string? TaxRateInput
        {
            get => _taxRateInput;
            set
            {
                if (_taxRateInput != value)
                {
                    _taxRateInput = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ConfigurationViewModel()
        {
            _taxRateInput = (TaxRate * 100).ToString("F2");
        }

        public void SaveConfiguration()
        {
            if (decimal.TryParse(_taxRateInput, out decimal inputRate))
            {
                TaxRate = inputRate / 100m;
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
