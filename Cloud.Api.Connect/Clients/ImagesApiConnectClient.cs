namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Connect.Clients
{
    public class ImagesApiConnectClient
    {
        private readonly MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper;

        public ImagesApiConnectClient(MOROPO.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper)
        {
            this.httpClientHelper = httpClientHelper;
        }

        public async System.Threading.Tasks.Task UploadAsync(string about, byte[] imageData, string contentType, string fieldName, string fileName)
        {
            var formContent = new System.Net.Http.StringContent(about);

            var fileContent = new System.Net.Http.ByteArrayContent(imageData);

            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = fieldName,
                FileName = fileName
            };

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            await httpClientHelper.PostFormAsync("account/images", new System.Net.Http.MultipartFormDataContent { formContent, fileContent });
        }

        public async System.Threading.Tasks.Task<MOROPO.SAPRESSC.Core.Common.Models.PaginationModel<Common.Models.ImageDisplayModel>> GetPageAsync(int page = 1)
        {
            return await httpClientHelper.GetAsync<MOROPO.SAPRESSC.Core.Common.Models.PaginationModel<Common.Models.ImageDisplayModel>>(string.Format("account/images/page/{0}", page));
        }

        public virtual async System.Threading.Tasks.Task EditAsync(Common.Models.ImageInputModel inputModel)
        {
            await httpClientHelper.PutAsync("account/images", inputModel);
        }

        public async System.Threading.Tasks.Task DeleteAsync(System.Guid imageId)
        {
            await httpClientHelper.DeleteAsync(string.Format("account/images/{0}", imageId));
        }
    }
}
