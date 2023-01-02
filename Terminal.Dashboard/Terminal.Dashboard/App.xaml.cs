using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard
{
    public partial class App : Xamarin.Forms.Application, Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade
    {
        private const string SUPPORT_ORIENTATION_MESSAGE = "Por favor contacte con el servicio de asistencia técnica para notificar esta situación.";

        public App()
        {
            InitializeComponent();

            var sessionManager = Managers.SessionManager.Instance;

            Xamarin.Forms.DependencyService.Register<Extend.Common.Presentation.Providers.Facade.IAccountInformationProviderFacade, Providers.AccountInformationProvider>();
            Xamarin.Forms.DependencyService.Register<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade, Presenters.NotificationPresenter>();
            Xamarin.Forms.DependencyService.RegisterSingleton<Support.Presentation.Providers.Facade.ICloudServerProviderFacade>(Support.Presentation.Managers.SessionMemoryManager.Instance);
            Xamarin.Forms.DependencyService.RegisterSingleton<MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade>(sessionManager);
            Xamarin.Forms.DependencyService.RegisterSingleton<Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade>(this);

            Support.Presentation.Helpers.ImageUriFactoryHelper.Instance.Initialize();

            On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

            Xamarin.Forms.DependencyService.Get<Managers.Facade.IApplicationManagerFacade>().RequestRequiredPermissions();

            PresentForUserAuthentication();
        }

        public void PresentForUserAuthentication()
        {
            MainPage = new Pages.UserAuthenticationPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public void HandleException(System.Exception exception, System.Action continuationCallback = null)
        {
            if (exception == null)
            {
                MainPage.DisplayAlert("Aviso", string.Format("El sistema ha tardado demasiado tiempo en responder.{1}{1}{0}", SUPPORT_ORIENTATION_MESSAGE, System.Environment.NewLine), "Aceptar").ContinueWith(_ => { continuationCallback?.Invoke(); });

                return;
            }

            var title = "Aviso";

            System.Exception targetException = exception;

            while (targetException is System.AggregateException)
            {
                targetException = targetException.InnerException;
            }

            var message = "Problemas no esperados en el sistema.";

            if (targetException is System.Net.Http.HttpRequestException)
            {
                message = "No se puede acceder al servidor. Revise su conexión de red y/o la disponibilidad del servicio.";
            }
            else if (targetException is MOROPO.SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException)
            {
                var unspectedHttpStatus = (targetException as MOROPO.SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException).HttpStatusCode;

                if (unspectedHttpStatus == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Datos del envío incorrectos.";
                }
                else if (unspectedHttpStatus == System.Net.HttpStatusCode.Unauthorized)
                {
                    message = "Acceso denegado.";

                    continuationCallback = () =>
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            Managers.SessionManager.Instance.SignOut();

                            PresentForUserAuthentication();
                        });
                    };
                }
                else if (unspectedHttpStatus == System.Net.HttpStatusCode.PreconditionFailed)
                {
                    message = "Es obligatorio que cambie su contraseña para continuar.";

                    continuationCallback = () =>
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(PresentForUserAuthentication);
                    };
                }
                else if (unspectedHttpStatus == System.Net.HttpStatusCode.Forbidden)
                {
                    title = "No Autorizado";
                    message = "Usted no cuenta con los permisos necesarios para realizar esta operación.";

                    // TODO Remove if no required!!
                    //continuationCallback = () =>
                    //{
                    //    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    //    {
                    //        Managers.SessionManager.Instance.SignOut();

                    //        (MainPage as Xamarin.Forms.NavigationPage).CurrentPage.IsEnabled = false;

                    //        PresentForUserAuthentication();
                    //    });
                    //};
                }
                else if (unspectedHttpStatus == System.Net.HttpStatusCode.Conflict)
                {
                    message = string.Format("Esta operación no puede realizarse pues compromete la integridad del sistema.{0}{0}Compruebe que el elemento a modificar o eliminar no sea necesario en otras secciones.", System.Environment.NewLine);
                }
            }

            MainPage.DisplayAlert(title, message, "Aceptar").ContinueWith(task => { continuationCallback?.Invoke(); });
        }
    }
}
