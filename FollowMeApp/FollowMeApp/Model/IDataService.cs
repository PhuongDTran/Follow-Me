using System;
using Xamarin.Essentials;
namespace FollowMeApp.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
        void GetLocation(Action<Location, Exception> callback);
    }
}