using FollowMeApp.View;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FollowMeApp.Model;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace FollowMeApp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            MainPage = new MainView();
		}

		protected async override void OnStart ()
		{
            // Handle when your app starts
            await GeolocationManager.instance.StartUpdatingLocationAsync();

        }

		protected async override void OnSleep ()
		{
            // Handle when your app sleeps
            await GeolocationManager.instance.StopUpdatingLocationAsync();
		}

		protected async override void OnResume ()
		{
            // Handle when your app resumes
            await GeolocationManager.instance.StartUpdatingLocationAsync();
        }
	}
}
