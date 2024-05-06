
using GoingMyTeaStore.Models;
using GoingMyTeaStore.Services;

namespace GoingMyTeaStore.Pages;

public partial class FavoritePage : ContentPage
{
    private BookmarkItemService _bookmarkItemService;
    public FavoritePage()
    {
        InitializeComponent();

        _bookmarkItemService = new BookmarkItemService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetFavoriteProducts();
    }

    private void GetFavoriteProducts()
    {
        CvFavoriteProducts.ItemsSource = _bookmarkItemService.GetAll();
    }

    private void CvFavoriteProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as BookmarkedProduct;
        if (currentSelection != null)
        {
            Navigation.PushAsync(new ProductDetailPage(currentSelection.ProductId));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}