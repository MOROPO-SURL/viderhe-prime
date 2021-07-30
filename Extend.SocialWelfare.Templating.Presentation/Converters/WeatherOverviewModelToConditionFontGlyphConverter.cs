namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Presentation.Converters
{
    public class WeatherOverviewModelToConditionFontGlyphConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var target = value as Weather.Api.Common.Models.WeatherOverviewModel;

            switch (target.ConditionId)
            {
                case 200:
                case 201:
                case 202:
                case 230:
                case 231:
                case 232:
                    return "\uf010";

                case 210:
                case 211:
                case 212:
                case 221:
                    return "\uf005";

                case 300:
                case 301:
                case 321:
                case 500:
                    return "\uf00b";

                case 302:
                case 310:
                case 311:
                case 312:
                case 313:
                case 314:
                case 501:
                case 502:
                case 503:
                    return "\uf008";

                case 511:
                case 611:
                case 612:
                case 615:
                case 616:
                case 620:
                    return "\uf006";

                case 520:
                case 521:
                case 522:
                case 710:
                    return "\uf009";

                case 531:
                case 910:
                    return "\uf00e";

                case 600:
                case 602:
                case 621:
                case 622:
                    return "\uf00a";

                case 601:
                    return "\uf0b2";

                case 711:
                    return "\uf062";

                case 721:
                    return "\uf0b6";

                case 731:
                case 761:
                case 762:
                    return "\uf063";

                case 741:
                    return "\uf003";

                case 781:
                case 900:
                    return "\uf056";

                case 800:
                    return "\uf00d";

                case 801:
                case 802:
                case 803:
                    return "\uf000";

                case 804:
                    return "\uf002";

                case 902:
                    return "\uf073";

                case 903:
                    return "\uf076";

                case 904:
                    return "\uf072";

                case 905:
                    return "\uf085";

                case 906:
                    return "\uf004";

                case 957:
                    return "\uf050";

                default:
                    return "\uf03e";
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
