using FollowMeApp.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FollowMeApp.View
{
    public partial class ShareView : PopupPage
    {
        private ShareViewModel _shareVM;

        public ShareView()
        {
            InitializeComponent();
            _shareVM = (ShareViewModel)BindingContext;
        }

        private void OnGenerateUrl(object sender, EventArgs e)
        {
            _shareVM.GenerateUrlCommand.Execute(null);
            _shareVM.PropertyChanged += (s, changedEvent) =>
            {
                DisplayAlert("Test", _shareVM.GroupId, "ok");
            };
        }
        
        #region Popup Animations
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