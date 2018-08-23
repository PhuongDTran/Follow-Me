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
            //if CrossPermissions.Current.CheckPermissionStatus(Permission.Location) {
            MainPage = new MainView();
            //} else
            //{
            //    CrossPermissions.Current.RequestPermission(Permission.Location)
            //    MainPage = new SplashView();
            //}
        }

        public delegate void AppEventDelegate();
        public event AppEventDelegate AppStart;
        public event AppEventDelegate AppResume;
        public event AppEventDelegate AppSleep;

        protected override void OnStart ()
		{
            // Handle when your app starts

           //fired this event if there is a subscriber
            AppStart?.Invoke();
        }

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            AppSleep?.Invoke();
		}

		protected  override void OnResume ()
		{
            // Handle when your app resumes
            AppResume.Invoke();
        }
	}
}
