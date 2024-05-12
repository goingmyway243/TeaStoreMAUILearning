
using GoingMyTeaStore.Models;
using GoingMyTeaStore.Services;
using System.Collections.ObjectModel;

namespace GoingMyTeaStore.Pages;

public partial class CartPage : ContentPage
{
    private ObservableCollection<ShoppingCartItem> ShoppingCartItems = new ObservableCollection<ShoppingCartItem>();

    public CartPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetShoppingCartItem();

        bool hasAddress = Preferences.ContainsKey("address");
        if (hasAddress)
        {
            LblAddress.Text = Preferences.Get("address", string.Empty);
        }
        else
        {
            LblAddress.Text = "Provide Your Address";
        }
    }

    private async void GetShoppingCartItem()
    {
        ShoppingCartItems.Clear();
        var cartItems = await ApiService.GetShoppingCartItems(Preferences.Get("userid", 0));
        foreach (var cartItem in cartItems)
        {
            ShoppingCartItems.Add(cartItem);
        }

        CvCart.ItemsSource = ShoppingCartItems;

        UpdateTotalPrice();
    }

    private void UpdateTotalPrice()
    {
        var totalPrice = ShoppingCartItems.Sum(p => p.Price * p.Qty);
        LblTotalPrice.Text = totalPrice.ToString();
    }

    private async void UpdateCartQuantity(int productId, string action)
    {
        var response = await ApiService.UpdateCartQuantity(productId, action);
        if (!response)
        {
            await DisplayAlert("Oops", "Something went wrong", "Cancel");
        }
    }

    private void BtnIncrease_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ShoppingCartItem cartItem)
        {
            cartItem.Qty++;
            UpdateTotalPrice();
            UpdateCartQuantity(cartItem.ProductId, "increase");
        }
    }

    private void BtnDecrease_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ShoppingCartItem cartItem)
        {
            if (cartItem.Qty == 1)
            {
                return;
            }

            cartItem.Qty--;
            UpdateTotalPrice();
            UpdateCartQuantity(cartItem.ProductId, "decrease");
        }
    }

    private void BtnDelete_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.BindingContext is ShoppingCartItem cartItem)
        {
            ShoppingCartItems.Remove(cartItem);
            UpdateTotalPrice();
            UpdateCartQuantity(cartItem.ProductId, "delete");
        }
    }

    private void BtnEditAddress_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddressPage());
    }

    private async void TapPlaceOrder_Tapped(object sender, TappedEventArgs e)
    {
        var order = new Order()
        {
            Address = LblAddress.Text,
            UserId = Preferences.Get("userid", 0),
            OrderTotal = Convert.ToInt32(LblTotalPrice.Text)
        };

        var response = await ApiService.PlaceOrder(order);
        if (response)
        {
            await DisplayAlert("", "Your order has been placed", "Alright");
            ShoppingCartItems.Clear();
        }
        else
        {
            await DisplayAlert("Oops", "Something went wrong", "Cancel");
        }
    }
}