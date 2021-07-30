using Microsoft.Extensions.Caching.Memory;
using SmartSolucionesCuba.Viderhe.Weather.Extensions;
using System.Linq;
using System.Net;

namespace SmartSolucionesCuba.Viderhe.Weather.Providers
{
    public sealed class OpenWeatherMapCacheProvider
    {
        private readonly Microsoft.Extensions.Options.IOptions<Options.OpenWeatherMapOptions> openWeatherMapOptions;
        private readonly Microsoft.Extensions.Options.IOptions<Options.NetworkProxyOptions> networkProxyOptions;

        private readonly MemoryCache memoryCache;
        private readonly Helpers.AsyncDuplicateLockHelper asyncDuplicateLocker;

        /// <summary>
        /// Initializes a new instance of <see cref="OpenWeatherMapCacheProvider"/>.
        /// </summary>
        public OpenWeatherMapCacheProvider(Microsoft.Extensions.Options.IOptions<Options.NetworkProxyOptions> networkProxyOptions, Microsoft.Extensions.Options.IOptions<Options.OpenWeatherMapOptions> openWeatherMapOptions)
        {
            this.openWeatherMapOptions = openWeatherMapOptions;
            this.networkProxyOptions = networkProxyOptions;

            memoryCache = new MemoryCache(new MemoryCacheOptions());
            asyncDuplicateLocker = new Helpers.AsyncDuplicateLockHelper();
        }

        public Options.OpenWeatherMapOptions OpenWeatherMapOptions => openWeatherMapOptions.Value;

        private async System.Threading.Tasks.Task<Models.OWM.ApiWeatherResultModel> GetApiWeatherResultFromUri(System.Uri uri)
        {
            var request = WebRequest.CreateHttp(uri);

            request.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
            request.AllowAutoRedirect = true;
            request.CookieContainer = new CookieContainer();
            request.AllowReadStreamBuffering = true;
            request.Date = System.DateTime.UtcNow;
            request.IfModifiedSince = System.DateTime.MinValue;
            request.Accept = "application/json";
            request.KeepAlive = false;
            request.ProtocolVersion = new System.Version(1, 1);
            request.UserAgent = null;
            request.Method = "GET";
            request.Timeout = openWeatherMapOptions.Value.Timeout;
            request.ReadWriteTimeout = openWeatherMapOptions.Value.Timeout;

            // TODO Support full proxy variant!

            if (networkProxyOptions.Value.Address != null)
            {
                request.Proxy = new WebProxy
                {
                    Address = networkProxyOptions.Value.Address,
                    Credentials = networkProxyOptions.Value.Credentials
                };
            }

            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    var responseContent = await reader.ReadToEndAsync();

                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.OWM.ApiWeatherResultModel>(responseContent);                        
                }                        
            }
        }

        public async System.Threading.Tasks.Task<Models.WeatherReportCacheModel> GetWeatherAsync(int cityId)
        {
            try
            {
                var city = openWeatherMapOptions.Value.Cities.First(item => item.Id == cityId);

                return await GetWeatherAsync(new Api.Common.Models.LocationModel(city.Coord.Lat, city.Coord.Lon));
            }
            catch
            {
                var defaultSerialization = openWeatherMapOptions.Value.Defaults[cityId.ToString()];

                var @default = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.OWM.ApiWeatherResultModel>(defaultSerialization);

                return @default.ToReportCacheModel();
            }
        }

        public async System.Threading.Tasks.Task<Models.WeatherReportCacheModel> GetWeatherAsync(Api.Common.Models.LocationModel location)
        {
            var lockObj = await asyncDuplicateLocker.LockAsync(location);

            var dateTime = System.DateTime.Now;

            var found = memoryCache.TryGetValue(location, out Models.OWM.ApiWeatherResultModel cachedModel);

            if (found)
            {
                var timeElapsed = dateTime.Subtract(cachedModel.FetchedAt).TotalMilliseconds;
                
                if (timeElapsed <= openWeatherMapOptions.Value.ApiCachePeriod)
                {
                    lockObj.Dispose();

                    return cachedModel.ToReportCacheModel();
                }
            }

            try
            {
                var apiUri = new System.Uri(string.Format("https://api.openweathermap.org/data/2.5/onecall?lat={0}&lon={1}&units=metric&lang=es&appid={2}&cache={3}", location.Latitude, location.Longitude, openWeatherMapOptions.Value.ApiKey, System.Guid.NewGuid()));

                var newModel = await GetApiWeatherResultFromUri(apiUri);

                newModel.FetchedAt = System.DateTime.Now;

                if (!found || openWeatherMapOptions.Value.FetchMode == Enumerations.FetchMode.AlwaysUseLastFetchedValue || (newModel.Current.MeasuredAt >= cachedModel.Current.MeasuredAt))
                {
                    memoryCache.Set(location, newModel, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = newModel.FetchedAt.AddMilliseconds(openWeatherMapOptions.Value.ResiliencyPeriod)
                    });

                    lockObj.Dispose();

                    return newModel.ToReportCacheModel();
                }
                else
                {
                    if (openWeatherMapOptions.Value.FetchMode == Enumerations.FetchMode.AlwaysUseLastMeasuredButExtendCache)
                    {
                        cachedModel.FetchedAt = newModel.FetchedAt;

                        memoryCache.Set(location, cachedModel, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = cachedModel.FetchedAt.AddMilliseconds(openWeatherMapOptions.Value.ResiliencyPeriod)
                        });
                    }

                    lockObj.Dispose();

                    return cachedModel.ToReportCacheModel();
                }
            }
            catch (System.Exception exception)
            {
                lockObj.Dispose();

                if (found && dateTime.Subtract(cachedModel.FetchedAt).TotalMilliseconds <= openWeatherMapOptions.Value.ResiliencyPeriod)
                {
                    return cachedModel.ToReportCacheModel();
                }

                throw exception;
            }
        }
    }
}
