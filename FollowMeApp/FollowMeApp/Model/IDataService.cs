using System;
using Xamarin.Essentials;
namespace FollowMeApp.Model
{
    public interface IDataService
    {
        void GetUserLocation(Action<Location, Exception> callback);
    }
}