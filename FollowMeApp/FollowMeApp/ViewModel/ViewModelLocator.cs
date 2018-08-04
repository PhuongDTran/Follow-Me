using CommonServiceLocator;
using FollowMeApp.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Diagnostics.CodeAnalysis;

namespace FollowMeApp.ViewModel
{
    /// <summary>
    /// This class contains static references to the most relevant view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// The key used by the NavigationService to go to the second page.
        /// </summary>
        public const string SecondPageKey = "SecondPage";

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public ShareViewModel ShareVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShareViewModel>();
            }
        }

        /// <summary>
        /// This property can be used to force the application to run with design time data.
        /// </summary>
        public static bool UseDesignTimeData
        {
            get
            {
                return false;
            }
        }

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (!ViewModelBase.IsInDesignModeStatic
                && !UseDesignTimeData)
            {
                // Use this service in production.
                //SimpleIoc.Default.Register<IDataService, DataService>();
            }
            else
            {
                // Use this service in Blend or when forcing the use of design time data.
                
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ShareViewModel>();
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
       
    }
}