using System;
using FollowMeApp.Model;
using Xamarin.Essentials;

namespace FollowMeApp.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data

            var item = new DataItem("Welcome to MVVM Light [design]", "Start [design]");
            callback(item, null);
        }

        public void GetLocation(Action<Location, Exception> callback)
        {
            throw new NotImplementedException();
        }
    }
}