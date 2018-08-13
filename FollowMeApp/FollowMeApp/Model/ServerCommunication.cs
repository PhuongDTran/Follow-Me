using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FollowMeApp.Model
{
    public class ServerComminication : IServerCommunication
    {

        public async Task<string> RequestGroupId(Device device, Location location)
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
    }
}
