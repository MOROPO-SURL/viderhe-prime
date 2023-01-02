using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend
{
    [Route("[controller]")]
    public class ExtensionsController : MOROPO.SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public ExtensionsController(Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.Extensions.Localization.IStringLocalizer<ExtensionsController> localizer, ILogger<ExtensionsController> logger) : base(localizer, logger)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("[action]/{key}")]
        public IActionResult DashboardBundle(System.Guid key)
        {
            var resultMemoryStream = new System.IO.MemoryStream();

            using (var archive = new System.IO.Compression.ZipArchive(resultMemoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                var extensionRootDirectoryPath = configuration["Viderhe:ExtensionRootDirectoryPath"];

                var extensionRootDirectoryInfo = new System.IO.DirectoryInfo(extensionRootDirectoryPath);

                if (!extensionRootDirectoryInfo.Exists)
                {
                    return NoContent();
                }

                var extensionLibraryFileInfos = Providers.ViderheExtensionLibraryFileInfosProvider.GetRelevantToDashboard(extensionRootDirectoryInfo.GetDirectories().First(d => d.Name == key.ToString()));

                foreach (var extensionLibraryFileInfo in extensionLibraryFileInfos)
                {
                    var zipArchiveEntry = archive.CreateEntry(extensionLibraryFileInfo.Name, System.IO.Compression.CompressionLevel.NoCompression);

                    using (var extensionLibraryMemoryStream = new System.IO.MemoryStream((int)extensionLibraryFileInfo.Length))
                    {
                        using (var extensionLibraryFileStrean = extensionLibraryFileInfo.OpenRead())
                        {
                            var extensionLibraryFileContent = new byte[extensionLibraryFileInfo.Length];

                            extensionLibraryFileStrean.CopyTo(extensionLibraryMemoryStream);

                            using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(extensionLibraryMemoryStream.ToArray(), 0, (int)extensionLibraryFileInfo.Length);
                        }
                    }
                }
            }

            return File(resultMemoryStream.ToArray(), "application/zip", string.Format("{0}.zip", key));
        }

        [HttpGet]
        [Route("[action]/{key}")]
        public IActionResult PlayerBundle(System.Guid key)
        {
            var resultMemoryStream = new System.IO.MemoryStream();

            using (var archive = new System.IO.Compression.ZipArchive(resultMemoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                var extensionRootDirectoryPath = configuration["Viderhe:ExtensionRootDirectoryPath"];

                var extensionRootDirectoryInfo = new System.IO.DirectoryInfo(extensionRootDirectoryPath);

                if (!extensionRootDirectoryInfo.Exists)
                {
                    return NoContent();
                }

                var extensionLibraryFileInfos = Providers.ViderheExtensionLibraryFileInfosProvider.GetRelevantToPlayer(extensionRootDirectoryInfo.GetDirectories().First(d => d.Name == key.ToString()));

                foreach (var extensionLibraryFileInfo in extensionLibraryFileInfos)
                {
                    var zipArchiveEntry = archive.CreateEntry(extensionLibraryFileInfo.Name, System.IO.Compression.CompressionLevel.NoCompression);

                    using (var extensionLibraryMemoryStream = new System.IO.MemoryStream((int)extensionLibraryFileInfo.Length))
                    {
                        using (var extensionLibraryFileStrean = extensionLibraryFileInfo.OpenRead())
                        {
                            var extensionLibraryFileContent = new byte[extensionLibraryFileInfo.Length];

                            extensionLibraryFileStrean.CopyTo(extensionLibraryMemoryStream);

                            using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(extensionLibraryMemoryStream.ToArray(), 0, (int)extensionLibraryFileInfo.Length);
                        }
                    }
                }
            }

            return File(resultMemoryStream.ToArray(), "application/zip", string.Format("{0}.zip", key));
        }
    }
}