using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.InteropServices;
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

        private ServerCommunicator() { }

        public static IServerCommunicator Instance { get; } = new ServerCommunicator();
        #endregion

        private static Device _device = new DeviceService().GetDeviceData();

        #region Properties
        public string GroupID
        {
            get
            {
                return "IfJTQuuskxmJX7Qulx9j";
            }
            set { }
        }
        #endregion

        public async Task SendTokenAsync(String token)
        {
            string url = "http://192.168.4.146:4567/token/?member=" + _device.DeviceID;
            string contentType = "application/json";
            JObject json = new JObject
            {
                { "token", token }
            };
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error sending push token:", response.ReasonPhrase);
                }
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

        /// <summary>
        /// Requested group id is assigned to <b>GroupID</b> property of ServerCommunicator class.
        /// do an assignment after calling this function to get GroupID value.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task RequestGroupIdAsync(Location location)
        {
            string groupId = "";
            string url = "http://192.168.4.146:4567/group/";
            string contentType = "application/json";
            JObject json = new JObject
            {
                { "id", _device.DeviceID },
                { "name", _device.DeviceName },
                { "lat", location.Latitude },
                { "lon", location.Longitude },
                { "speed", location.Speed },
                { "heading", location.Heading }, //TODO: need heading
                { "platform", _device.Platform }
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
            GroupID = groupId;
        }

        public async Task<string> GetLeaderIdAsync()
        {
            string url = "http://192.168.4.146:4567/leader/?group=" + GroupID;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var leaderId = await response.Content.ReadAsStringAsync();
                return leaderId;
            }
            return null;
        }

        /// <summary>
        ///  Device Information(device id, device name, platform) is used as Member Info sent to app server
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task SendMemberInfo()
        {
            string url = "http://192.168.4.146:4567/member/";
            string contentType = "application/json";
            JObject json = new JObject
            {
                { "id", _device.DeviceID },
                { "name", _device.DeviceName },
                { "platform", _device.Platform }
            };
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));
                response.EnsureSuccessStatusCode();
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

        public async Task SendLocationAsync(Location location)
        {
            string url = String.Format("http://192.168.4.146:4567/trip/?group={0}&member={1}", GroupID, _device.DeviceID);
            string contentType = "application/json";
            JObject json = new JObject
            {
                { "lat", location.Latitude },
                { "lon", location.Longitude },
                { "speed", location.Speed },
                { "heading", location.Heading }
            };
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, contentType));
                response.EnsureSuccessStatusCode();
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

        public async Task<Location> GetLocationAsync(string memberId)
        {
            string url = String.Format("http://192.168.4.146:4567/trip/?group={0}&member={1}", GroupID, memberId);

            HttpClient client = new HttpClient();
            try
            {
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                JObject jContent = (JObject)JsonConvert.DeserializeObject(content);

                Location location = new Location
                {
                    Latitude = (double)jContent.GetValue("latitude"),
                    Longitude = (double)jContent.GetValue("longitude"),
                    Heading = (int)jContent.GetValue("heading"),
                    Speed = (int)jContent.GetValue("speed")
                };
                return location;
            }
            catch (COMException ex)
            {
                Console.WriteLine("The request was null. ", ex.Message);
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine("Underlying issue:network connectivity, DNS failure, or timeout.", ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

    }
}
