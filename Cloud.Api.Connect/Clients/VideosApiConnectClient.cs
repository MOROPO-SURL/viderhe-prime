namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class VideosApiConnectClient
    {
        private readonly MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper;

        public VideosApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper)
        {
            this.httpClientHelper = httpClientHelper;
        }

        public async System.Threading.Tasks.Task UploadSegmentAsync(System.Guid videoId, string about, byte[] videoSegmentData, string contentType, string fieldName, string fileName, int segment = 0)
        {
            var urlOrResource = string.Format("account/videos/{0}/segment/last", videoId);

            if (segment > 0)
            {
                urlOrResource = string.Format("account/videos/{0}/segment/{1}", videoId, segment);
            }

            var formContent = new System.Net.Http.StringContent(about);

            var fileContent = new System.Net.Http.ByteArrayContent(videoSegmentData);

            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = fieldName,
                FileName = fileName
            };

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            await httpClientHelper.PutFormAsync(urlOrResource, new System.Net.Http.MultipartFormDataContent { formContent, fileContent });
        }

        public async System.Threading.Tasks.Task<MOROPO.SAPRESSC.Core.Common.Models.PaginationModel<Common.Models.VideoDisplayModel>> GetPageAsync(int page = 1)
        {
            return await httpClientHelper.GetAsync<MOROPO.SAPRESSC.Core.Common.Models.PaginationModel<Common.Models.VideoDisplayModel>>(string.Format("account/videos/page/{0}", page));
        }

        public virtual async System.Threading.Tasks.Task EditAsync(Common.Models.VideoInputModel inputModel)
        {
            await httpClientHelper.PutAsync("account/videos", inputModel);
        }

        public async System.Threading.Tasks.Task DeleteAsync(System.Guid videoId)
        {
            await httpClientHelper.DeleteAsync(string.Format("account/videos/{0}", videoId));
        }
    }
}
