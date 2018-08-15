using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FollowMeApp.Model
{
    public class ServerCommunication : IServerCommunication
    {
        public event EventHandler OnGroupIdAssigned;
        private string _groupId;
        public string GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                _groupId = value;
                OnGroupIdAssigned?.Invoke(this, EventArgs.Empty);
            }
        }
        public async Task<string> RequestGroupIdAsync(Device device, Location location)
        {
            string groupId = "";
            string url = "http://192.168.4.146:4567/groupid/";
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
                var groupId = await response.Content.ReadAsStringAsync();
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
            return null;
        }

        public async Task SendLocationAsync( string memberId, Location location)
        {

        }
    }
}
