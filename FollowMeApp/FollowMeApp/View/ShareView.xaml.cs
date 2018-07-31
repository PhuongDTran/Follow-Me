using FollowMeApp.ViewModel;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Text;
using System.Net.Http;
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

        private async void OnGenerateUrl(Object sender, EventArgs e)
        {
            await PostDataAndGetGroupIdAsync();
        }


        /// <see cref="https://forums.xamarin.com/discussion/27186/how-to-post-a-json-in-rest-service"/>
        /// <see cref="https://stackoverflow.com/questions/36458551/send-http-post-request-in-xamarin-forms-c-sharp"/>
        private async Task PostDataAndGetGroupIdAsync()
        {
            String url = "http://192.168.4.146:4567/groupid/";
            String contentType = "application/json";
            JObject json = new JObject();
            json.Add("latitude", _shareVM.UserCurrentPosition.Latitude);
            json.Add("longitude", _shareVM.UserCurrentPosition.Longitude);
            json.Add("device_id", _shareVM.DeviceData.DeviceID);
            json.Add("device_name", _shareVM.DeviceData.DeviceName);
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));
                var groupId = await response.Content.ReadAsStringAsync();

                await DisplayAlert("testing", groupId, "ok");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("The request was null. ", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Already sent by the HttpClient instance.", ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Underlying issue:network connectivity, DNS failure, or timeout.", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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