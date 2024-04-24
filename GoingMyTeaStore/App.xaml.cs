using GoingMyTeaStore.Pages;

namespace GoingMyTeaStore
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var accessToken = Preferences.Get("accesstoken", string.Empty);
            if (string.IsNullOrEmpty(accessToken))
            {
                MainPage = new NavigationPage(new SignupPage());
            }
            else
            {
                MainPage = new AppShell();
            }
        }
    }
}
