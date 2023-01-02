namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Helpers
{
    public sealed class ConstantProvisioningHelper
    {
        private static ConstantProvisioningHelper instance;

        public static ConstantProvisioningHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConstantProvisioningHelper();
                }

                return instance;
            }
        }

        private ConstantProvisioningHelper() { }

        internal const string ACTIONSHEET_PICKIMAGE_OPTION_CAMARA = "Captura con la Cámara";
        internal const string ACTIONSHEET_PICKIMAGE_OPTION_GALLERY = "Galería de Imágenes";

        public string FallbackUserProfileImageUrl => string.Format("{0}://{1}/img/default-person-picture.jpg", Support.Presentation.Managers.SessionMemoryManager.Instance.UseSSL ? "https" : "http", Support.Presentation.Managers.SessionMemoryManager.Instance.Host);

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex[] Sex_All => (MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex[])System.Enum.GetValues(typeof(MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex));

        //public SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus[] CivilStatus_All => (SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus[])System.Enum.GetValues(typeof(SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus));
    }
}
