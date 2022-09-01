namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Models.Page
{
    public class UserAuthenticationPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private const string DEFAULT_PROTOCOL = "https";

        private string targetCloudHostName;

        private int targetCloudProtocolIndex;

        private string userName;

        private string password;

        public UserAuthenticationPageModel()
        {
            AvailableProtocols = new string[] { DEFAULT_PROTOCOL, "http" };
            TargetCloudProtocolIndex = 0;
        }

        public string TargetCloudHostName
        {
            get => targetCloudHostName;
            set => SetProperty(ref targetCloudHostName, value);
        }

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public int TargetCloudProtocolIndex { get => targetCloudProtocolIndex; set => SetProperty(ref targetCloudProtocolIndex, value); }

        public string[] AvailableProtocols { get; private set; }
    }
}
