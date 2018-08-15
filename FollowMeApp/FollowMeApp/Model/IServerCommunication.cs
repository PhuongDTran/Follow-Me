using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FollowMeApp.Model
{
    public interface IServerCommunication
    {
        event EventHandler OnGroupIdAssigned;
        String GroupId { get; set; }   
        Task<String> RequestGroupIdAsync(Device device, Location location);
        Task<Location> SendMemberInfo(Device device, Location location);
        Task SendLocationAsync(string memberId, Location location);
    }
}
