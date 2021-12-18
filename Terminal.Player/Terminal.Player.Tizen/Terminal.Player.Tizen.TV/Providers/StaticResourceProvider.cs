[assembly: Xamarin.Forms.Dependency(typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.Tizen.TV.Providers.StaticResourceProvider))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Tizen.TV.Providers
{
    public sealed class StaticResourceProvider : Support.Presentation.Providers.Facade.IStaticResourceProviderFacade
    {
        public double TinyBodyTextFontSize => 10;

        public double SmallBodyTextFontSize => 12;

        public double NormalBodyTextFontSize => 14;

        public double MiddleBodyTextFontSize => 16;

        public double LargeBodyTextFontSize => 18;

        public double TinyHeaderTextFontSize => 19;

        public double SmallHeaderTextFontSize => 21;

        public double NormalHeaderTextFontSize => 23;

        public double MiddleHeaderTextFontSize => 25;

        public double LargeHeaderTextFontSize => 27;

        public double TinyTitleTextFontSize => 28;

        public double SmallTitleTextFontSize => 30;

        public double NormalTitleTextFontSize => 36;

        public double MiddleTitleTextFontSize => 40;

        public double LargeTitleTextFontSize => 48;

        public double TinyHeroTextFontSize => 50;

        public double SmallHeroTextFontSize => 56;

        public double NormalHeroTextFontSize => 64;

        public double MiddleHeroTextFontSize => 74;

        public double LargeHeroTextFontSize => 96;
    }
}
