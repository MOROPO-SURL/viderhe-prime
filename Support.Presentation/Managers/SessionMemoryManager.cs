namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Managers
{
    public class SessionMemoryManager : Providers.Facade.ICloudServerProviderFacade
    {
        private const string LAST_CLOUD_SERVER_KEY = "last-cloud-server";

        private static SessionMemoryManager instance;

        public static SessionMemoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SessionMemoryManager();
                }

                return instance;
            }
        }

        private SessionMemoryManager()
        {
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey(LAST_CLOUD_SERVER_KEY))
            {
                Server = Xamarin.Forms.Application.Current.Properties[LAST_CLOUD_SERVER_KEY] as string;
            }
            else
            {
                Server = "cloud.viderhe.net";
            }
        }

        public string Server { get; set; }

        public void Persist()
        {
            Xamarin.Forms.Application.Current.Properties[LAST_CLOUD_SERVER_KEY] = Server;

            Xamarin.Forms.Application.Current.SavePropertiesAsync();
        }

        #region ICloudServerProviderFacade implementation

        public string Host => Server;

        public bool UseSSL
        {
            get
            {
#if DEBUG
                return true;
#else
                return true;
#endif
            }
        }

        #endregion
    }
}
