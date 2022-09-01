using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(SmartSolucionesCuba.Viderhe.Terminal.Dashboard.iOS.Managers.ApplicationManager))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.iOS.Managers
{
    public class ApplicationManager : Dashboard.Managers.Facade.IApplicationManagerFacade
    {
        public string GetUnhandleExceptionMessage(System.Exception exception)
        {
            return null;
        }

        public void Quit()
        {
            System.Threading.Thread.CurrentThread.Abort();
        }

        public void RequestRequiredPermissions()
        {
            System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(1)).ContinueWith(_ =>
            {
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    UserNotifications.UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
                    {
                        if (settings.AuthorizationStatus == UserNotifications.UNAuthorizationStatus.Authorized) return;

                        if (settings.AuthorizationStatus == UserNotifications.UNAuthorizationStatus.Denied)
                        {
                            UIApplication.SharedApplication.InvokeOnMainThread(ShowNoNotificationAuthorizedAlert);

                            return;
                        }

                        UIApplication.SharedApplication.InvokeOnMainThread(() =>
                        {
                            var notificationAuthorizationRequiredExplanationAlertController = UIAlertController.Create("Aviso", string.Format("Viderhe Control requiere tu autorización para el uso de notificaciones para confirmar el resultado de las acciones solicitadas.{0}{0}A continuación te será solicitado este permiso por parte del sistema.", System.Environment.NewLine), UIAlertControllerStyle.Alert);

                            notificationAuthorizationRequiredExplanationAlertController.AddAction(UIAlertAction.Create("Aceptar", UIAlertActionStyle.Default, (alertAction) =>
                            {
                                UserNotifications.UNUserNotificationCenter.Current.RequestAuthorization(
                                    UserNotifications.UNAuthorizationOptions.Alert |
                                    UserNotifications.UNAuthorizationOptions.Badge |
                                    UserNotifications.UNAuthorizationOptions.Sound,
                                    (granted, error) =>
                                    {
                                        if (granted) return;

                                        UIApplication.SharedApplication.InvokeOnMainThread(ShowNoNotificationAuthorizedAlert);
                                    });
                            }));

                            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentedViewController.PresentViewController(notificationAuthorizationRequiredExplanationAlertController, true, null);
                        });
                    });
                });
            });
        }

        private void ShowNoNotificationAuthorizedAlert()
        {
            var noNotificationAuthorizedAlertController = UIAlertController.Create("Aviso", string.Format("No se podrán mostrar confirmación del resultado de las acciones del Viderhe Control pues no se han permitido el uso de notificaciones.{0}{0}Puede permir las notificaciones en los Ajustes del sistema para la aplicación.", System.Environment.NewLine), UIAlertControllerStyle.Alert);

            noNotificationAuthorizedAlertController.AddAction(UIAlertAction.Create("Ajustes", UIAlertActionStyle.Default, (alertAction) =>
            {
                UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(UIApplication.OpenSettingsUrlString));
            }));
            noNotificationAuthorizedAlertController.AddAction(UIAlertAction.Create("Aceptar", UIAlertActionStyle.Cancel, null));

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentedViewController.PresentViewController(noNotificationAuthorizedAlertController, true, null);
        }
    }
}
