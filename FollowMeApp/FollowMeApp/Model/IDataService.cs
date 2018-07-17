using System;
using Xamarin.Essentials;
namespace FollowMeApp.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
        void GetUserLocation(Action<Location, Exception> callback);
    }
}