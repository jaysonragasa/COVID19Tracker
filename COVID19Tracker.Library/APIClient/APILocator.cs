// Data sources

//using COVID19Tracker.Library.APIClient.DataSources.Demo;
using COVID19Tracker.Library.APIClient.DataSources.CoronaTracker;
//using COVID19Tracker.Library.APIClient.DataSources.MyGitRepo;

namespace covid19phlib.APIClient
{
    public class APILocator : APILocatorBase
    {
        public APILocator()
        {
            base.Initialize();
        }
    }
}
