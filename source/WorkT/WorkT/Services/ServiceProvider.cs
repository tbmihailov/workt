using WorkT.DesignServices;

namespace WorkT.Services
{
    public abstract class ServiceProvider : ServiceProviderBase
    {
        public const string DATABASE_FILE_NAME = "/workt.db";

        public ServiceProvider()
        {
            GroupsDataService = new GroupsDataService(DATABASE_FILE_NAME);
        }

        private static ServiceProviderBase _instance;
        public static ServiceProviderBase Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
                    {
                        _instance = new DesignServiceProvider();
                    }
                    else
                    {
                        _instance = new ServiceProvider();
                    }
                }

                return _instance;
            }
        }

    }
}
