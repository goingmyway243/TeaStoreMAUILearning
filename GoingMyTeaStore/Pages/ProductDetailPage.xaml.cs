
using GoingMyTeaStore.Models;
using GoingMyTeaStore.Services;

namespace GoingMyTeaStore.Pages;

public partial class ProductDetailPage : ContentPage
{
    private int _productId;

    public ProductDetailPage(int productId)
    {
        InitializeComponent();

        _productId = productId;

        GetProductDetail();
    }

    private async void GetProductDetail()
    {
        var result = await ApiService.GetProductDetail(_productId);
        if (result != null)
        {
            LblProductName.Text = result.Name;
            LblProductDescription.Text = result.Detail;
            LblProductPrice.Text = result.Price.ToString();
            LblTotalPrice.Text = result.Price.ToString();
        }
    }

    private void BtnAdd_Clicked(object sender, EventArgs e)
    {
        var quantity = Convert.ToInt32(LblQty.Text);
        quantity++;
        LblQty.Text = quantity.ToString();

        var totalPrice = quantity * Convert.ToInt32(LblProductPrice.Text);
        LblTotalPrice.Text = totalPrice.ToString();
    }

    private void BtnRemove_Clicked(object sender, EventArgs e)
    {
        var quantity = Convert.ToInt32(LblQty.Text);
        quantity--;
        if (quantity > 0)
        {
            LblQty.Text = quantity.ToString();

            var totalPrice = quantity * Convert.ToInt32(LblProductPrice.Text);
            LblTotalPrice.Text = totalPrice.ToString();
        }
    }

    private async void BtnAddToCart_Clicked(object sender, EventArgs e)
    {
        var shoppingCart = new ShoppingCart()
        {
            CustomerId = Convert.ToInt32(Preferences.Get("userid", string.Empty)),
            ProductId = _productId,
            TotalAmount = Convert.ToInt32(LblTotalPrice.Text),
            Qty = Convert.ToInt32(LblQty.Text),
            Price = Convert.ToInt32(LblProductPrice.Text),
        };

        var isSuccess = await ApiService.AddItemsInCart(shoppingCart);
        if (isSuccess)
        {
            await DisplayAlert(string.Empty, "Added to cart sucessfully!", "Alright");
        }
        else
        {
            await DisplayAlert(string.Empty, "Oops, something when wrong...", "Try again");
        }
    }
}