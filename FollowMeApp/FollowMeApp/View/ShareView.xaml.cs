using FollowMeApp.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FollowMeApp.View
{
    public partial class ShareView : PopupPage
	{
        private ShareViewModel _shareVM;
		public ShareView ()
		{
			InitializeComponent ();
            _shareVM = (ShareViewModel)BindingContext;
		}

        private async void OnGenerateUrl(Object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://192.168.4.146:4567/groupid/");
            response.EnsureSuccessStatusCode();
            var groupId = await response.Content.ReadAsStringAsync();
            await DisplayAlert("testing", groupId, "ok");
        }

        #region Animations
        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return Content.FadeTo(1);
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return Content.FadeTo(1);
        }
        #endregion
    }
}