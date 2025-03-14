
using COP.Servuces;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

[QueryProperty(nameof(ProductId), "productId")]
public partial class Product : ContentPage
{
	public Product()
	{
		InitializeComponent();
		BindingContext = new ProductViewModel();
	}
	public int ProductId { get; set; }	
	private void GoBackClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//InventoryManagement");
	}

	private void OKClicked(object sender, EventArgs e)
	{
		(BindingContext as ProductViewModel).AddOrUpdate();
		var name = (BindingContext as ProductViewModel).Name;
		ProductServiceProxy.Current.AddOrUpdate(new COP.Models.Product { Name = name });
		Shell.Current.GoToAsync("//InventoryManagement");
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
		if (ProductId == 0) 
		{
			BindingContext = new ProductViewModel();
		}
		else
		{
			BindingContext =new ProductViewModel(ProductServiceProxy.Current.GetById(ProductId));
		}
    }
}