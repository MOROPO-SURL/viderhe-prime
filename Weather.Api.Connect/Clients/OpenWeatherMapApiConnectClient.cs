namespace SmartSolucionesCuba.Viderhe.Weather.Api.Connect.Clients
{
    public sealed class OpenWeatherMapApiConnectClient
    {
        private const string TARGET_CONTROLLER = "openweathermap";

        private readonly SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper;

        public OpenWeatherMapApiConnectClient(SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper)
        {
            this.httpClientHelper = httpClientHelper;
        }

        public async System.Threading.Tasks.Task<Common.Models.CityModel[]> GetCitiesAsync()
        {
            return await httpClientHelper.GetAsync<Common.Models.CityModel[]>(string.Format("{0}/cities", TARGET_CONTROLLER));
        }

        public async System.Threading.Tasks.Task<Common.Models.WeatherOverviewModel> GetOverviewByCityAsync(int key)
        {
            return await httpClientHelper.GetAsync<Common.Models.WeatherOverviewModel>(string.Format("{0}/overview/city/{1}", TARGET_CONTROLLER, key));
        }

        public async System.Threading.Tasks.Task<Common.Models.WeatherReportModel> GetReportByCityAsync(int key)
        {
            return await httpClientHelper.GetAsync<Common.Models.WeatherReportModel>(string.Format("{0}/report/city/{1}", TARGET_CONTROLLER, key));
        }

        public async System.Threading.Tasks.Task<Common.Models.WeatherOverviewModel> GetOverviewByLocationAsync(double latitude, double longitude)
        {
            return await httpClientHelper.GetAsync<Common.Models.WeatherOverviewModel>(string.Format("{0}/overview/lat/{1}/lon/{2}", TARGET_CONTROLLER, latitude, longitude));
        }

        public async System.Threading.Tasks.Task<Common.Models.WeatherReportModel> GetReportByLocationAsync(double latitude, double longitude)
        {
            return await httpClientHelper.GetAsync<Common.Models.WeatherReportModel>(string.Format("{0}/report/lat/{1}/lon/{2}", TARGET_CONTROLLER, latitude, longitude));
        }
    }
}
