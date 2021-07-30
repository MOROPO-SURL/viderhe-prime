using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenWeatherMapController : ControllerBase
    {
        private readonly Providers.OpenWeatherMapCacheProvider provider;

        public OpenWeatherMapController(Providers.OpenWeatherMapCacheProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet("cities")]
        public ActionResult<Api.Common.Models.CityModel[]> GetCities()
        {
            return provider.OpenWeatherMapOptions.Cities.Select(item => new Api.Common.Models.CityModel
            {
                Id = item.Id.ToString(),
                Name = item.Name,
                ISOCountryCode = item.Country,
                Location = new Api.Common.Models.LocationModel(item.Coord.Lat, item.Coord.Lon)
            }).ToArray();
        }

        [HttpGet("overview/lat/{latitude}/lon/{longitude}")]
        public async System.Threading.Tasks.Task<ActionResult<Api.Common.Models.WeatherOverviewModel>> GetOverviewByLocationAsync(double latitude, double longitude)
        {
            try
            {
                var cached = await provider.GetWeatherAsync(new Api.Common.Models.LocationModel(latitude, longitude));

                return cached.Report.Overview;
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpGet("overview/city/{key}")]
        public async System.Threading.Tasks.Task<ActionResult<Api.Common.Models.WeatherOverviewModel>> GetOverviewByCityIdAsync(int key)
        {
            try
            {
                var cached = await provider.GetWeatherAsync(key);

                return cached.Report.Overview;
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpGet("report/lat/{latitude}/lon/{longitude}")]
        public async System.Threading.Tasks.Task<ActionResult<Api.Common.Models.WeatherReportModel>> GetReportByLocationAsync(double latitude, double longitude)
        {
            try
            {
                var cached = await provider.GetWeatherAsync(new Api.Common.Models.LocationModel(latitude, longitude));

                return cached.Report;
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpGet("report/city/{key}")]
        public async System.Threading.Tasks.Task<ActionResult<Api.Common.Models.WeatherReportModel>> GetReportByCityIdAsync(int key)
        {
            try
            {
                var cached = await provider.GetWeatherAsync(key);

                return cached.Report;
            }
            catch
            {
                return NoContent();
            }
        }
    }
}
