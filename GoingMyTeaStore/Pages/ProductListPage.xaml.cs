
using GoingMyTeaStore.Models;
using GoingMyTeaStore.Services;

namespace GoingMyTeaStore.Pages;

public partial class ProductListPage : ContentPage
{
    public ProductListPage(int categoryId)
    {
        InitializeComponent();

        GetProducts(categoryId);
    }

    private async void GetProducts(int categoryId)
    {
        var result = await ApiService.GetProducts("category", categoryId.ToString());
        CvProducts.ItemsSource = result;
    }

    private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelect = e.CurrentSelection.FirstOrDefault() as Product;
        if (currentSelect != null)
        {
            Navigation.PushAsync(new ProductDetailPage(currentSelect.Id));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}