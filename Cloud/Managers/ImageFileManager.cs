using MOROPO.SAPRESSC.Middleware.Media.Web.Common.Extensions;

namespace SmartSolucionesCuba.Viderhe.Cloud.Managers
{
    public class ImageFileManager : MOROPO.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade
    {
        private readonly Microsoft.Extensions.Options.IOptions<Options.MediaPathOptions> mediaPathOptions;

        public ImageFileManager(Microsoft.Extensions.Options.IOptions<Options.MediaPathOptions> mediaPathOptions)
        {
            this.mediaPathOptions = mediaPathOptions;
        }

        public void DeleteImageFile(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image image)
        {
            System.IO.File.Delete(GetImageAbsoluteFilePath(image));
        }

        public string GetImageAbsoluteFilePath(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image image)
        {
            return System.IO.Path.Combine(mediaPathOptions.Value.ImageMediaPath, string.Format("{0}.{1}", image.Id, image.ContentType.ContentTypeToExtension()));
        }

        public byte[] GetImageFileContent(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image image)
        {
            byte[] result;

            using (var file = System.IO.File.OpenRead(GetImageAbsoluteFilePath(image)))
            {
                result = new byte[file.Length];

                file.Read(result, 0, (int)file.Length);

                file.Close();
            }

            return result;
        }

        public string GetImageRelativeFilePath(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image image)
        {
            return null;
        }

        public void SaveImageFileContent(System.Guid imageId, byte[] content, string contentType)
        {
            System.IO.File.WriteAllBytes(GetImageAbsoluteFilePath(new MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image { Id = imageId, ContentType = contentType }), content);
        }
    }
}
