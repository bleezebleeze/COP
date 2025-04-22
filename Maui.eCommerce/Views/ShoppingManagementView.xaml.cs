using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ShoppingManagementView : ContentPage
{
	public ShoppingManagementView()
	{
		InitializeComponent();
		BindingContext = new ShoppingManagementViewModel();
	}

    private void AddToCartClicked(object sender, EventArgs e)
    {
		(BindingContext as ShoppingManagementViewModel).PurchaseItem();
    }

    private void RemoveFromCartClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel).ReturnItem();
    }

    private void InlineAddClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel).RefreshUx();
    }

    private void CheckoutClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.Checkout();
    }
    private void SortInventoryByNameAscClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.SortInventoryByNameAscending();
    }

    private void SortInventoryByNameDescClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.SortInventoryByNameDescending();
    }

    private void SortInventoryByPriceAscClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.SortInventoryByPriceAscending();
    }

    private void SortInventoryByPriceDescClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.SortInventoryByPriceDescending();
    }
    private void SortCartByNameAscClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.SortCartByNameAscending();
    }

    private void SortCartByNameDescClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.SortCartByNameDescending();
    }

    private void SortCartByPriceAscClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.SortCartByPriceAscending();
    }

    private void SortCartByPriceDescClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.SortCartByPriceDescending();
    }
}