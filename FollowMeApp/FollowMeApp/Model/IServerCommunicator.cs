using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FollowMeApp.Model
{
    public interface IServerCommunicator : INotifyPropertyChanged
    {
        string GroupID { get; set; }
        string LeaderId { get; set; }
        Task<string> RequestGroupIdAsync(Location location);
        Task<string> GetLeaderIdAsync();
        Task SendMemberInfo();
        Task<string> GetMemberIdAsync(string groupId);
        Task SendLocationAsync(Location location);
        Task<Location> GetLocationAsync(string memberId);
    }
}
