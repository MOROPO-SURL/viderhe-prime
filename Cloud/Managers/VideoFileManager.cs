using System.IO;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Managers
{
    public class VideoFileManager : MOROPO.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IVideoFileManagerFacade
    {
        private const string TEMP_DIRECTORY_NAME = "temp";

        private readonly Microsoft.Extensions.Options.IOptions<Options.MediaPathOptions> mediaPathOptions;

        public VideoFileManager(Microsoft.Extensions.Options.IOptions<Options.MediaPathOptions> mediaPathOptions)
        {
            this.mediaPathOptions = mediaPathOptions;
        }

        public void AddVideoFileContentSegment(System.Guid videoId, int segment, byte[] content)
        {
            var targetDirectryInfo = new FileInfo(Path.Combine(mediaPathOptions.Value.VideoMediaPath, TEMP_DIRECTORY_NAME, videoId.ToString()));

            if (!Directory.Exists(targetDirectryInfo.FullName))
            {
                Directory.CreateDirectory(targetDirectryInfo.FullName);
            }

            File.WriteAllBytes(Path.Combine(targetDirectryInfo.FullName, segment.ToString()), content);
        }

        public void AddVideoFileContentSegmentAndClose(System.Guid videoId, byte[] content, string extension)
        {
            var targetSegmentsDirectryInfo = new FileInfo(Path.Combine(mediaPathOptions.Value.VideoMediaPath, TEMP_DIRECTORY_NAME, videoId.ToString()));
            var targetVideoDirectoryInfo = new FileInfo(Path.Combine(mediaPathOptions.Value.VideoMediaPath));

            var segments = Directory.GetFiles(targetSegmentsDirectryInfo.FullName).OrderBy(path => File.GetCreationTime(path)).ToArray();

            using (var targetVideoFileStream = File.Create(Path.Combine(targetVideoDirectoryInfo.FullName, string.Format("{0}{1}", videoId, extension))))
            {
                foreach (var segment in segments)
                {
                    using (var segmentFileStream = File.OpenRead(segment))
                    {
                        segmentFileStream.CopyTo(targetVideoFileStream);

                        segmentFileStream.Close();
                    }
                }

                targetVideoFileStream.Write(content, 0, content.Length);

                targetVideoFileStream.Flush();

                targetVideoFileStream.Close();
            }

            foreach (var segment in segments)
            {
                File.Delete(segment);
            }

            Directory.Delete(targetSegmentsDirectryInfo.FullName);
        }

        public void DeleteVideoFile(System.Guid videoId, string extension)
        {
            File.Delete(Path.Combine(mediaPathOptions.Value.VideoMediaPath, videoId.ToString() + extension));
        }

        public void DeleteVideoFileContentSegment(System.Guid videoId, int segment)
        {
            File.Delete(Path.Combine(mediaPathOptions.Value.VideoMediaPath, TEMP_DIRECTORY_NAME, videoId.ToString(), segment.ToString()));
        }

        public string GetVideoAbsoluteFilePath(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Video video)
        {
            return Path.Combine(mediaPathOptions.Value.VideoMediaPath, video.Id.ToString()) + video.Extension;
        }

        public byte[] GetVideoFileContent(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Video video)
        {
            throw new System.NotImplementedException();
        }

        public string GetVideoRelativeFilePath(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Video video)
        {
            throw new System.InvalidOperationException();
        }

        public void SaveVideoFileContent(System.Guid videoId, byte[] content, string extension)
        {
            throw new System.NotImplementedException();
        }
    }
}
