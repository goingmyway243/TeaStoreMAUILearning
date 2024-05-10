using GoingMyTeaStore.Services;

namespace GoingMyTeaStore.Pages;

public partial class ProfilePage : ContentPage
{
    private byte[] _imageArray;

    public ProfilePage()
    {
        InitializeComponent();
        LblUserName.Text = Preferences.Get("username", string.Empty);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var response = await ApiService.GetUserProfileImage();
        if (response != null)
        {
            ImgUserProfileBtn.Source = response.FullPathImageUrl;
        }
    }
    private async void ImgUserProfileBtn_Clicked(object sender, EventArgs e)
    {
        var file = await MediaPicker.PickPhotoAsync();
        if (file != null)
        {
            using (var stream = await file.OpenReadAsync())
            {
                using (var memory = new MemoryStream())
                {
                    await stream.CopyToAsync(memory);
                    _imageArray = memory.ToArray();
                    ImgUserProfileBtn.Source = ImageSource.FromStream(() => new MemoryStream(_imageArray));
                }
            }

            var response = await ApiService.UploadUserImage(_imageArray);
            if (response)
            {
                await DisplayAlert("", "Image uploaded successfully", "Alright");
            }
            else
            {
                await DisplayAlert("", "Error occured", "Cancel");
            }
        }
    }

    private void TapOrders_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new OrderPage());
    }

    private void BtnLogout_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("accesstoken", string.Empty);
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}