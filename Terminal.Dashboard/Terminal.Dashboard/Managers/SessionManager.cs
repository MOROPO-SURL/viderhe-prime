using SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Extensions;
using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using System.Linq;
using Xamarin.Forms;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Managers
{
    public sealed class SessionManager : SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
        private const string LAST_CLOUD_SERVER_KEY = "last-cloud-server";

        private static SessionManager instance;

        public static SessionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SessionManager();
                }

                return instance;
            }
        }

        private SessionManager()
        {
            if (Application.Current.Properties.ContainsKey(LAST_CLOUD_SERVER_KEY))
            {
                Server = Application.Current.Properties[LAST_CLOUD_SERVER_KEY] as string;
            }
        }

        private System.Collections.Generic.KeyValuePair<string, string> AuthorizeHeader
        {
            get
            {
                if (!HasAuthenticatedIdentity)
                {
                    return new System.Collections.Generic.KeyValuePair<string, string>();
                }
                else
                {
                    var type = AuthenticationType;
                    var token = AuthorizationToken;

                    return new System.Collections.Generic.KeyValuePair<string, string>(type, token);
                }
            }
        }

        public bool HasAuthenticatedIdentity => ModeratorLogin != null;

        public SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper => new SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("http://{0}/api/", Server), () => AuthorizeHeader, null);

        public string Server { get; private set; }
        public string AuthenticationType { get; private set; }
        public string AuthorizationToken { get; private set; }
        public string ModeratorLogin { get; private set; }
        public string ModeratorName { get; private set; }
        public ImageSource ModeratorPictureImageSource { get; private set; }
        public System.Guid AccountId { get; private set; }
        public string AccountName { get; private set; }
        public Color AccountPrimaryColor { get; private set; }
        public Color AccountSecoundaryColor { get; private set; }
        public ImageSource AccountLogoImageSource { get; private set; }
        public ImageSource AccountHorizontalSplashImageSource { get; private set; }
        public ImageSource AccountVerticalSplashImageSource { get; private set; }
        public System.Guid? AccountDefaultOverlayId { get; private set; }
        public string AccountSerializedOverlayContext { get; private set; }
        public Cloud.Api.Common.Models.ExtensionDisplayModel[] AccountExtensions { get; private set; }

        public void SyncronizeRequiredAccountInformation(string name, Color primaryColor, Color secoundaryColor, System.Guid logoImageId, System.Guid horizontalSplashImageId, System.Guid verticalSplashImageId, Extend.Common.Overlaying.Descriptors.IOverlayDescriptor defaultOverlay, string serializedOverlayContext)
        {
            AccountName = name;
            AccountPrimaryColor = primaryColor;
            AccountSecoundaryColor = secoundaryColor;
            AccountLogoImageSource = ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(Providers.CloudServerProvider.Instance, logoImageId));
            AccountHorizontalSplashImageSource = ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(Providers.CloudServerProvider.Instance, horizontalSplashImageId));
            AccountVerticalSplashImageSource = ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(Providers.CloudServerProvider.Instance, verticalSplashImageId));
            AccountDefaultOverlayId = defaultOverlay?.Id ?? null;
            AccountSerializedOverlayContext = serializedOverlayContext;
        }

        public async System.Threading.Tasks.Task UpdateAccountInformationAsync(string name, Color primaryColor, Color secoundaryColor, Plugin.FilePicker.Abstractions.FileData logoFileData, Plugin.FilePicker.Abstractions.FileData horizontalSplashFileData, Plugin.FilePicker.Abstractions.FileData verticalSplashFileData, Extend.Common.Overlaying.Descriptors.IOverlayDescriptor defaultOverlay, string serializedOverlayContext)
        {
            var client = new Cloud.Api.Connect.Clients.AccountApiConnectClient(this);

            var input = new Cloud.Api.Common.Models.AccountInputModel
            {
                Id = AccountId,
                Name = name,
                PrimaryColor = primaryColor.ToSolidHex(),
                SecoundaryColor = secoundaryColor.ToSolidHex(),
                DefaultOverlayId = defaultOverlay?.Id ?? null,
                SerializedOverlayContext = serializedOverlayContext
            };

            if (logoFileData != null)
            {
                var imageFormat = System.IO.Path.GetExtension(logoFileData.FilePath).Substring(1);

                input.LogoImageContentType = string.Format("images/{0}", imageFormat == "jpg" ? "jpeg" : imageFormat);

                using (var imageFileStream = logoFileData.GetStream())
                {
                    imageFileStream.Position = 0;

                    input.LogoImageData = new byte[imageFileStream.Length];

                    imageFileStream.Read(input.LogoImageData, 0, input.LogoImageData.Length);
                }
            }

            if (horizontalSplashFileData != null)
            {
                var imageFormat = System.IO.Path.GetExtension(horizontalSplashFileData.FilePath).Substring(1);

                input.HorizontalSplashImageContentType = string.Format("images/{0}", imageFormat == "jpg" ? "jpeg" : imageFormat);

                using (var imageFileStream = horizontalSplashFileData.GetStream())
                {
                    imageFileStream.Position = 0;

                    input.HorizontalSplashImageData = new byte[imageFileStream.Length];

                    imageFileStream.Read(input.HorizontalSplashImageData, 0, input.HorizontalSplashImageData.Length);
                }
            }

            if (verticalSplashFileData != null)
            {
                var imageFormat = System.IO.Path.GetExtension(verticalSplashFileData.FilePath).Substring(1);

                input.VerticalSplashImageContentType = string.Format("images/{0}", imageFormat == "jpg" ? "jpeg" : imageFormat);

                using (var imageFileStream = verticalSplashFileData.GetStream())
                {
                    imageFileStream.Position = 0;

                    input.VerticalSplashImageData = new byte[imageFileStream.Length];

                    imageFileStream.Read(input.VerticalSplashImageData, 0, input.VerticalSplashImageData.Length);
                }
            }

            await client.EditAsync(input);

            var model = await client.GetCurrentAsync();

            SyncronizeRequiredAccountInformation(model.Name, Color.FromHex(model.PrimaryColor), Color.FromHex(model.SecoundaryColor), model.LogoImageId, model.HorizontalSplashImageId, model.VerticalSplashImageId, defaultOverlay, model.SerializedOverlayContext);
        }

        public async System.Threading.Tasks.Task SignInAsync(string server, string login, string password)
        {
            Server = server;

            var userApiClient = new Cloud.Api.Connect.Clients.UserApiConnectClient(HttpClientHelper);

            var result = await userApiClient.AuthenticateAsync(new Cloud.Api.Common.Models.AuthenticationIntentModel
            {
                User = login,
                Password = password
            });

            Application.Current.Properties[LAST_CLOUD_SERVER_KEY] = server;

            await Application.Current.SavePropertiesAsync();

            AuthenticationType = result.Type;
            AuthorizationToken = result.Token;
            ModeratorLogin = login;
            ModeratorName = result.Identity.LastName == null ? result.Identity.Name : string.Format("{0} {1}", result.Identity.Name, result.Identity.LastName);
            // TODO Handle no result.Identity.Image null !!
            ModeratorPictureImageSource = ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(Providers.CloudServerProvider.Instance, result.Identity.Image.Id));

            try
            {
                var accountApiClient = new Cloud.Api.Connect.Clients.AccountApiConnectClient(this);

                var model = await accountApiClient.GetCurrentAsync();

                AccountId = model.Id;

                var overalyDescriptor = model.DefaultOverlayId == null ? null : Support.Extend.Overlaying.Providers.OverlayDescriptorProvider.Instance.GetDescriptorById(model.DefaultOverlayId.Value);

                SyncronizeRequiredAccountInformation(model.Name, Color.FromHex(model.PrimaryColor), Color.FromHex(model.SecoundaryColor), model.LogoImageId, model.HorizontalSplashImageId, model.VerticalSplashImageId, overalyDescriptor, model.SerializedOverlayContext);

                AccountExtensions = model.Extensions.ToArray();

                var dynamicLoadExtensions = AccountExtensions.Where(item => 
                    item.Id != Extend.Essential.Descriptors.EssentialExtensionDescriptor.ID &&
                    item.Id != Extend.SpringMotion.Descriptors.SpringMotionExtensionDescriptor.ID &&
                    item.Id != Extend.SocialWelfare.Descriptors.SocialWelfareExtensionDescriptor.ID &&
                    item.Id != Extend.UICCamaguey.Descriptors.UICCamagueyExtensionDescriptor.ID &&
                    item.Id != Extend.OHCC.RailwayMuseum.Descriptors.RailwayMuseumOHCCExtensionDescriptor.ID)
                    .ToArray();

                foreach (var accountExtension in dynamicLoadExtensions)
                {
                    var extensionDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Extensions", accountExtension.Id.ToString(), accountExtension.Version));

                    if (!extensionDirectoryInfo.Exists)
                    {
                        extensionDirectoryInfo.Create();

                        await DownloadAccountExtensionBundleAsync(accountExtension.Id, extensionDirectoryInfo);
                    }

                    var extensionLibraryFileInfos = extensionDirectoryInfo.GetFiles();

                    if (extensionLibraryFileInfos.Length <= 0)
                    {
                        await DownloadAccountExtensionBundleAsync(accountExtension.Id, extensionDirectoryInfo);
                    }

                    foreach (var extensionLibraryFileInfo in extensionLibraryFileInfos)
                    {
                        var assembly = System.Reflection.Assembly.LoadFrom(extensionLibraryFileInfo.FullName);

                        System.AppDomain.CurrentDomain.Load(assembly.GetName());

                        var attributes = assembly.GetCustomAttributesSafe(typeof(ExportFontAttribute));

                        if (attributes == null || attributes.Length == 0) continue;

                        var length = attributes.Length;

                        for (var i = 0; i < length; i++)
                        {
                            var a = attributes[i];

                            var attribute = a as HandlerAttribute;

                            if (attribute == null && (a is ExportFontAttribute fa))
                            {
                                Xamarin.Forms.Internals.FontRegistrar.Register(fa, assembly);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                SignOut();

                throw e;
            }
        }

        public void SignOut()
        {
            AuthenticationType = null;
            AuthorizationToken = null;
            ModeratorLogin = null;
            ModeratorName = null;
            ModeratorPictureImageSource = null;

            AccountId = System.Guid.Empty;
            AccountName = null;
            AccountPrimaryColor = Color.Default;
            AccountSecoundaryColor = Color.Default;
            AccountLogoImageSource = null;
            AccountHorizontalSplashImageSource = null;
            AccountVerticalSplashImageSource = null;
            AccountDefaultOverlayId = null;
            AccountSerializedOverlayContext = null;
            AccountExtensions = null;
        }

        public async System.Threading.Tasks.Task ChangePasswordAsync(string oldPassword, string newPassword)
        {
            var client = new Cloud.Api.Connect.Clients.UserApiConnectClient(HttpClientHelper);

            await client.ChangePasswordAsync(new Cloud.Api.Common.Models.UserPasswordChangeRequestModel
            {
                OldPassword = oldPassword,
                NewPassword = newPassword
            });
        }

        private async System.Threading.Tasks.Task DownloadAccountExtensionBundleAsync(System.Guid extensionId, System.IO.DirectoryInfo extensionDirectoryInfo)
        {
            var extensionsApiConnectClient = new Cloud.Api.Connect.Clients.ExtensionsApiConnectClient(HttpClientHelper);

            var extensionBundleModules = await extensionsApiConnectClient.GetDashboardBundleModulesAsync(extensionId);

            foreach (var extensionBundleModule in extensionBundleModules)
            {
                using (var extensionLibraryFileStream = System.IO.File.Create(System.IO.Path.Combine(extensionDirectoryInfo.FullName, extensionBundleModule.FileName)))
                {
                    extensionLibraryFileStream.Write(extensionBundleModule.FileContent, 0, extensionBundleModule.FileContent.Length);
                }
            }
        }
    }
}
