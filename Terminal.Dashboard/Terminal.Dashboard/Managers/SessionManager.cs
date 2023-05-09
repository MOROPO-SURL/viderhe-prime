using SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Extensions;
using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using System.Linq;
using Xamarin.Forms;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Managers
{
    public sealed class SessionManager : MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
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

        public bool HasAuthenticatedIdentity => Moderator != null;

        public MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper => new MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}/api/", Support.Presentation.Managers.SessionMemoryManager.Instance.UseSSL ? "https" : "http", Support.Presentation.Managers.SessionMemoryManager.Instance.Server), GetAuthorizeHeader, null);

        public string AuthenticationType { get; private set; }
        public string AuthorizationToken { get; private set; }
        public Cloud.Api.Common.Models.ModeratorDisplayModel Moderator { get; private set; }
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

        private System.Collections.Generic.KeyValuePair<string, string> GetAuthorizeHeader()
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

        public void SyncronizeRequiredAccountInformation(string name, Color primaryColor, Color secoundaryColor, System.Guid logoImageId, System.Guid horizontalSplashImageId, System.Guid verticalSplashImageId, Extend.Common.Overlaying.Descriptors.IOverlayDescriptor defaultOverlay, string serializedOverlayContext)
        {
            AccountName = name;
            AccountPrimaryColor = primaryColor;
            AccountSecoundaryColor = secoundaryColor;
            AccountLogoImageSource = ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(logoImageId));
            AccountHorizontalSplashImageSource = ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(horizontalSplashImageId));
            AccountVerticalSplashImageSource = ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(verticalSplashImageId));
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

        public async System.Threading.Tasks.Task SignInAsync(string server, bool useSSL, string login, string password, System.Action<string> callbackAction)
        {
            Support.Presentation.Managers.SessionMemoryManager.Instance.Server = server;
            Support.Presentation.Managers.SessionMemoryManager.Instance.UseSSL = useSSL;

            var userApiClient = new Cloud.Api.Connect.Clients.UserApiConnectClient(HttpClientHelper);

            var result = await userApiClient.AuthenticateAsync(new Cloud.Api.Common.Models.AuthenticationIntentModel
            {
                User = login,
                Password = password
            });

            Support.Presentation.Managers.SessionMemoryManager.Instance.Persist();

            AuthenticationType = result.Type;
            AuthorizationToken = result.Token;
            Moderator = result.Moderator;

            try
            {
                var accountApiClient = new Cloud.Api.Connect.Clients.AccountApiConnectClient(this);

                var model = await accountApiClient.GetCurrentAsync();

                AccountExtensions = model.Extensions.ToArray();

                // MotoTouch does not support dynamic assembly loading
                if (Device.RuntimePlatform != Device.iOS)
                {
                    var dynamicLoadExtensions = AccountExtensions.Where(item => item.Id != Extend.Essential.Descriptors.EssentialExtensionDescriptor.ID).ToArray();

                    var missingAccountExtensions = new System.Collections.Generic.List<Cloud.Api.Common.Models.ExtensionDisplayModel>();

                    foreach (var accountExtension in dynamicLoadExtensions)
                    {
                        if (System.IO.Directory.Exists(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Extensions", server, accountExtension.Id.ToString(), accountExtension.Version))) continue;
                        
                        missingAccountExtensions.Add(accountExtension);
                    }

                    var counter = 1;
                    var missingAccountExtensionsCount = missingAccountExtensions.Count;

                    foreach (var missingAccountExtension in missingAccountExtensions)
                    {
                        callbackAction.Invoke(string.Format("Sincronizando extensiones {0}/{1}", counter++, missingAccountExtensionsCount));

                        var extensionDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Extensions", server, missingAccountExtension.Id.ToString(), missingAccountExtension.Version));

                        var tempExtensionDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Extensions", server, missingAccountExtension.Id.ToString(), string.Format("_{0}", missingAccountExtension.Version)));

                        await DownloadAccountExtensionBundleAsync(missingAccountExtension.Id, tempExtensionDirectoryInfo);

                        System.IO.Directory.Move(tempExtensionDirectoryInfo.FullName, extensionDirectoryInfo.FullName);
                    }

                    foreach (var accountExtension in dynamicLoadExtensions)
                    {
                        var extensionDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Extensions", server, accountExtension.Id.ToString(), accountExtension.Version));

                        var extensionLibraryFileInfos = extensionDirectoryInfo.GetFiles();

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

                AccountId = model.Id;

                var overalyDescriptor = model.DefaultOverlayId == null ? null : Support.Extend.Overlaying.Providers.OverlayDescriptorProvider.Instance.GetDescriptorById(model.DefaultOverlayId.Value);

                var ffImageLoadingHttpClient = new System.Net.Http.HttpClient();

                ffImageLoadingHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(AuthenticationType, AuthorizationToken);

                FFImageLoading.ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration
                {
                    HttpClient = ffImageLoadingHttpClient
                });

                SyncronizeRequiredAccountInformation(model.Name, Color.FromHex(model.PrimaryColor), Color.FromHex(model.SecoundaryColor), model.LogoImageId, model.HorizontalSplashImageId, model.VerticalSplashImageId, overalyDescriptor, model.SerializedOverlayContext);
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
            Moderator = null;

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
            if (!extensionDirectoryInfo.Exists)
            {
                extensionDirectoryInfo.Create();
            }

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
