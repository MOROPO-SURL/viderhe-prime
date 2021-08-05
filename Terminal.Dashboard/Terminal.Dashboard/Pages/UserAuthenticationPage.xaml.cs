using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Pages
{
    public partial class UserAuthenticationPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.UserAuthenticationPageModel>
    {
        public UserAuthenticationPage() : base(new Models.Page.UserAuthenticationPageModel { TargetCloudHostName = Managers.SessionManager.Instance.Server })
        {
            InitializeComponent();

            PlatformIdentityImageView.Source = FFImageLoading.Svg.Forms.SvgImageSource.FromResource("SmartSolucionesCuba.Viderhe.Support.Presentation.Resources.Images.viderhe-identity-main.svg", typeof(Support.Presentation.Providers.TemplateViewModelProvider).Assembly);
        }

        protected override bool OnBackButtonPressed()
        {
            DisplayAlert("Aviso", "Está seguro que desea salir de la aplicación?", "Salir", "Cancelar").ContinueWith(task =>
            {
                if (task.Result)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        Xamarin.Forms.DependencyService.Get<Managers.Facade.IApplicationManagerFacade>().Quit();
                    });
                }
            });

            return true;
        }

        private void SignButton_Clicked(object sender, System.EventArgs e)
        {
            BindingContextPageModel.IsBusy = true;

            Managers.SessionManager.Instance.SignInAsync(BindingContextPageModel.TargetCloudHostName, BindingContextPageModel.UserName, BindingContextPageModel.Password).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.IsCanceled || task.Exception != null)
                    {
                        var message = "Problemas no esperados en el sistema.";

                        if (task.Exception == null)
                        {
                            message = "El sistema ha tardado demasiado tiempo en responder.";
                        }
                        else
                        {
                            System.Exception targetException = task.Exception;

                            while (targetException is System.AggregateException)
                            {
                                targetException = targetException.InnerException;
                            }

                            if (targetException is System.Net.Http.HttpRequestException)
                            {
                                message = "No se puede acceder al servidor. Revise su conexión de red y/o la disponibilidad del servicio.";
                            }
                            else if (targetException is SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException)
                            {
                                var unspectedHttpStatus = (targetException as SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException).HttpStatusCode;

                                if (unspectedHttpStatus == System.Net.HttpStatusCode.BadRequest)
                                {
                                    message = "Datos incorrectos.";
                                }
                                else if (unspectedHttpStatus == System.Net.HttpStatusCode.Forbidden)
                                {
                                    message = "Usuario no autorizado.";
                                }
                                else if (unspectedHttpStatus == System.Net.HttpStatusCode.Unauthorized)
                                {
                                    message = "Usuario y/o contraseña incorrectos.";
                                }
                            }
                        }

                        BindingContextPageModel.IsBusy = false;

                        DisplayAlert("Error", message, "Aceptar");

                        return;
                    }

                    BindingContextPageModel.IsBusy = false;

                    this.NavigationPopModalPageAsync();
                });
            });
        }
    }
}