namespace SmartSolucionesCuba.Viderhe.Weather.Extensions
{
    internal static class ApiWeatherResultModelExtensions
    {
        internal static Models.WeatherReportCacheModel ToReportCacheModel(this Models.OWM.ApiWeatherResultModel target)
        {
            var result = new Models.WeatherReportCacheModel
            {
                PorvisionService = Enumerations.PorvisionService.OpenWeatherMap,
                Report = new Api.Common.Models.WeatherReportModel()
            };

            Models.OWM.DailyWeatherModel closerDailyWeather = null;

            foreach (var item in target.Dailys)
            {
                closerDailyWeather = item;

                var dailyMeasuredAt = System.DateTimeOffset.FromUnixTimeSeconds(closerDailyWeather.MeasuredAt).UtcDateTime.ToLocalTime();

                if (target.FetchedAt.Date == dailyMeasuredAt.Date) break;
            }

            var dailyWeatherCurrentCondition = closerDailyWeather.WeatherConditions[0];

            var nowTime = System.DateTime.Now.TimeOfDay;

            result.Report.Overview = new Api.Common.Models.WeatherOverviewModel
            {
                MeasuredAt = System.DateTimeOffset.FromUnixTimeSeconds(closerDailyWeather.MeasuredAt).UtcDateTime.ToLocalTime(),
                SunRiseAt = System.DateTimeOffset.FromUnixTimeSeconds(closerDailyWeather.SunRiseAt).UtcDateTime.ToLocalTime(),
                SunSetAt = System.DateTimeOffset.FromUnixTimeSeconds(closerDailyWeather.SunSetAt).UtcDateTime.ToLocalTime(),
                ConditionId = dailyWeatherCurrentCondition.Id,
                ConditionDetail = dailyWeatherCurrentCondition.Detail,
                CloudDensity = closerDailyWeather.CloudDensity,
                DewPoint = closerDailyWeather.DewPoint,
                Humidity = closerDailyWeather.Humidity,
                Pressure = closerDailyWeather.Pressure,
                UltravioletIndex = closerDailyWeather.UltravioletIndex,
                WindDegrees = closerDailyWeather.WindDegrees,
                WindSpeed = closerDailyWeather.WindSpeed,
                Visibility = target.Current.Visibility,
                MinTemperature = closerDailyWeather.TemperatureSummary.Min,
                MaxTemperature = closerDailyWeather.TemperatureSummary.Max
            };

            if (nowTime.Hours < 11)
            {
                result.Report.Overview.Temperature = closerDailyWeather.TemperatureSummary.Morning;
                result.Report.Overview.TemperatureFeelLike = closerDailyWeather.TemperatureFeelLike.Morning;
            }
            else if (nowTime.Hours < 17)
            {
                result.Report.Overview.Temperature = closerDailyWeather.TemperatureSummary.Day;
                result.Report.Overview.TemperatureFeelLike = closerDailyWeather.TemperatureFeelLike.Day;
            }
            else if (nowTime.Hours < 21)
            {
                result.Report.Overview.Temperature = closerDailyWeather.TemperatureSummary.Evening;
                result.Report.Overview.TemperatureFeelLike = closerDailyWeather.TemperatureFeelLike.Evening;
            }
            else
            {
                result.Report.Overview.Temperature = closerDailyWeather.TemperatureSummary.Night;
                result.Report.Overview.TemperatureFeelLike = closerDailyWeather.TemperatureFeelLike.Night;
            }

            if (target.FetchedAt.Date == result.Report.Overview.MeasuredAt.Date)
            {
                result.Report.Overview.Temperature = target.Current.Temperature;
            }

            return result;
        }
    }
}
