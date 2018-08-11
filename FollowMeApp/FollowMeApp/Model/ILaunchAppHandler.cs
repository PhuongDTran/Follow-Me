using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FollowMeApp.Model
{
    public interface ILaunchAppHandler
    {
        Task<bool> LaunchApp(String uri);
    }
}
