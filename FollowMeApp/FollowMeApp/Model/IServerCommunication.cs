using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FollowMeApp.Model
{
    public interface IServerCommunication
    {
        Task<String> RequestGroupId(Device device, Location location);

    }
}
