
using GoingMyTeaStore.Models;
using GoingMyTeaStore.Services;

namespace GoingMyTeaStore.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();

        LblUserName.Text = "Hi" + Preferences.Get("username", string.Empty);

        GetCategories();
        GetBestSelling();
        GetTrending();
    }

    private async void GetTrending()
    {
        var result = await ApiService.GetProducts("trending", string.Empty);
        CvCategories.ItemsSource = result;
    }

    private async void GetBestSelling()
    {
        var result = await ApiService.GetProducts("bestselling", string.Empty);
        CvBestSelling.ItemsSource = result;
    }

    private async void GetCategories()
    {
        var result = await ApiService.GetCatergories();
        CvCategories.ItemsSource = result;
    }

    private void CvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelect = e.CurrentSelection.FirstOrDefault() as Category;
        if (currentSelect != null)
        {
            Navigation.PushAsync(new ProductListPage(currentSelect.Id));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}