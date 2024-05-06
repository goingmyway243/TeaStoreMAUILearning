
using GoingMyTeaStore.Models;
using GoingMyTeaStore.Services;

namespace GoingMyTeaStore.Pages;

public partial class ProductDetailPage : ContentPage
{
    private int _productId;
    private string _imageUrl;
    private BookmarkItemService _bookmarkItemService = new BookmarkItemService();

    public ProductDetailPage(int productId)
    {
        InitializeComponent();

        _productId = productId;

        GetProductDetail();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateFavoriteButton();
    }

    private async void GetProductDetail()
    {
        var result = await ApiService.GetProductDetail(_productId);
        if (result != null)
        {
            LblProductName.Text = result.Name;
            LblProductDescription.Text = result.Detail;
            LblProductPrice.Text = result.Price.ToString("0:C0");
            LblTotalPrice.Text = result.Price.ToString("0:C0");
            ImgProduct.Source = result.FullPathImageUrl;
            _imageUrl = result.FullPathImageUrl;
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
            CustomerId = Preferences.Get("userid", 0),
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

    private void ImgBtnFavorite_Clicked(object sender, EventArgs e)
    {
        var existingBookmark = _bookmarkItemService.GetById(_productId);
        if(existingBookmark != null)
        {
            _bookmarkItemService.Delete(existingBookmark);
        }
        else
        {
            _bookmarkItemService.Insert(new BookmarkedProduct()
            {
                ProductId = _productId,
                IsBookmarked = true,
                Detail = LblProductDescription.Text,
                Name = LblProductName.Text,
                Price = Convert.ToInt32(LblProductPrice.Text),
                ImageUrl = _imageUrl
            });
        }

        UpdateFavoriteButton();
    }

    private void UpdateFavoriteButton()
    {
        var existingBookmark = _bookmarkItemService.GetById(_productId);
        ImgBtnFavorite.Source = existingBookmark != null ? "heartfill" : "heart";
    }
}