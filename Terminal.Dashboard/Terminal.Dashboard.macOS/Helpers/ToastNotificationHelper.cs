using Plugin.Toasts;
using Xamarin.Forms.Platform.MacOS;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.macOS.Helpers
{
	internal class ToastNotificationHelper : IToastNotificator
    {
        private readonly Managers.UNNotificationManager notificationManager;

        private static bool toastNotificationAvailable = false;

        public static void Init()
        {
            UserNotifications.UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
            {
                if (settings.AuthorizationStatus == UserNotifications.UNAuthorizationStatus.NotDetermined)
                {
                    UserNotifications.UNUserNotificationCenter.Current.RequestAuthorization(UserNotifications.UNAuthorizationOptions.Alert, (granted, error) =>
                    {
                        toastNotificationAvailable = granted;
                    });
                }
                else
                {
                    toastNotificationAvailable = settings.AuthorizationStatus != UserNotifications.UNAuthorizationStatus.Denied;
                }
            });
        }

        public ToastNotificationHelper()
        {
            notificationManager = new Managers.UNNotificationManager();
        }

        public void CancelAllDelivered()
        {
            UserNotifications.UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.IList<INotification>> GetDeliveredNotifications()
        {
            var list = new System.Collections.Generic.List<INotification>();

            if (toastNotificationAvailable)
            {
                var deliveredNotifications = await UserNotifications.UNUserNotificationCenter.Current.GetDeliveredNotificationsAsync();

                foreach (var notification in deliveredNotifications)
                {
                    var content = notification.Request.Content;

                    list.Add(new Notification()
                    {
                        Id = notification.Request.Identifier,
                        Title = content.Title,
                        Description = content.Body,
                        Delivered = notification.Date.ToDateTime()
                    });
                }
            }

            return list;
        }

        public async System.Threading.Tasks.Task<INotificationResult> Notify(INotificationOptions options)
        {
            if (toastNotificationAvailable)
            {
                return await System.Threading.Tasks.Task.Run(() =>
                {
                    return notificationManager.Notify(options);
                });
            }
            else
            {
                var alert = new AppKit.NSAlert
                {
                    MessageText = options.Title,
                    InformativeText = options.Description,
                    AlertStyle = AppKit.NSAlertStyle.Informational,

                };

                alert.AddButton("Aceptar");

                alert.RunModal();

                return new NotificationResult() { Action = NotificationAction.Clicked };
            }
        }

        public void Notify(System.Action<INotificationResult> callback, INotificationOptions options)
        {
            System.Threading.Tasks.Task.Run(async () => callback.Invoke(await Notify(options)));
        }

        public void SystemEvent(object args)
        {
            throw new System.NotSupportedException();
        }
    }
}

