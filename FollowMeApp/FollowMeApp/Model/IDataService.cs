using System;

namespace FollowMeApp.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}