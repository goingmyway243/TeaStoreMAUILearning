
using GoingMyTeaStore.Models;
using GoingMyTeaStore.Services;

namespace GoingMyTeaStore.Pages;

public partial class OrderPage : ContentPage
{
	public OrderPage()
	{
		InitializeComponent();
		GetOrderList();
	}

    private async void GetOrderList()
    {
		var orders = await ApiService.GetOrdersByUser(Preferences.Get("userid", 0));
		CvOrders.ItemsSource = orders;
    }

    private void CvOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		var selectedItem = e.CurrentSelection.FirstOrDefault() as OrderByUser;
		if (selectedItem == null)
		{
			return;
		}

		Navigation.PushAsync(new OrderDetailPage(selectedItem.Id, selectedItem.OrderTotal));
    }
}