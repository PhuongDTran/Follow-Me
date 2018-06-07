using System;

namespace FollowMeApp.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Follow Me", "Start a Trip");
            callback(item, null);
        }
    }
}