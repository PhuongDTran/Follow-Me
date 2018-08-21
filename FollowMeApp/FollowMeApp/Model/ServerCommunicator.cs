using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FollowMeApp.Model
{
    public sealed class ServerCommunicator : IServerCommunicator
    {
        
        #region singleton pattern
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ServerCommunicator()
        {
        }

        private ServerCommunicator(){ }

        public static IServerCommunicator Instance { get; } = new ServerCommunicator();
        #endregion

        private string _groupId;

        #region  PropertyChange Event
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(
           [System.Runtime.CompilerServices.CallerMemberName] string propertyName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion

        #region Properties
        public string GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                _groupId = value;
                NotifyPropertyChanged();
            }
        }

        public string LeaderId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion


        public async Task<string> RequestGroupIdAsync(Device device, Location location)
        {
            string groupId = "";
            string url = "http://192.168.4.146:4567/newgroup/";
            string contentType = "application/json";
            JObject json = new JObject
            {
                { "id", device.DeviceID },
                { "name", device.DeviceName },
                { "lat", location.Latitude },
                { "lon", location.Longitude },
                { "speed", location.Speed },
                { "heading", location.Heading }, //TODO: need heading
                { "platform", device.Platform }
            };
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));
                groupId = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Responsed Group Id:" + groupId);
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
            return groupId;
        }

        public async Task<Location> SendMemberInfo( Device device, Location location)
        {
            string url = "http://192.168.4.146:4567/trip/?groupid=" + GroupId;
            string contentType = "application/json";
            JObject json = new JObject
            {
                { "id", device.DeviceID },
                { "name", device.DeviceName },
                { "lat", location.Latitude },
                { "lon", location.Longitude },
                { "speed", location.Speed },
                { "heading", location.Heading }, //TODO: need heading
                { "platform", device.Platform }
            };
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));
                var content = await response.Content.ReadAsStringAsync();
                JObject jContent = (JObject)JsonConvert.DeserializeObject(content);

                //TODO: what to do with id sent from server???
                Location leaderLocation = new Location
                {
                    Latitude = (double)jContent.GetValue("latitude"),
                    Longitude = (double)jContent.GetValue("longitude"),
                    Heading = (int)jContent.GetValue("heading"),
                    Speed = (int)jContent.GetValue("speed")
                };
                return leaderLocation;
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
            return null;
        }

        public async Task<string> GetMemberIdAsync(string groupId)
        {
            string url = "http://192.168.4.146:4567/member/?groupid=" + groupId;

            HttpClient client = new HttpClient();
            try
            {
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                return content;
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
            return null;
        }

            public Task SendLocationAsync(string memberId, Location location)
        {
            throw new NotImplementedException();
        }

        public Task GetLocationAsync(string memberId)
        {
            throw new NotImplementedException();
        }
    }
}
