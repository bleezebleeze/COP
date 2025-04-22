using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ConfigurationView : ContentPage
{
    public ConfigurationView()
    {
        InitializeComponent();
        BindingContext = new ConfigurationViewModel();
    }

    private async void SaveClicked(object sender, EventArgs e)
    {
        var viewModel = (ConfigurationViewModel)BindingContext;
        viewModel.SaveConfiguration();

        await DisplayAlert("Configuration", "Settings saved successfully", "OK");
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void CancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}