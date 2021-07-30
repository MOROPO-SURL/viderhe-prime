namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Presentation.Views
{
    public partial class InmersiveCityWeatherOverviewView : Common.Templating.Presentation.Views.BaseTemplatedPlacardPresentationView
    {
        public InmersiveCityWeatherOverviewView(Models.View.InmersiveCityWeatherOverviewViewModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }

        public override Xamarin.Forms.Layout<Xamarin.Forms.View> WrapLayout => Wrapper;

        public override Xamarin.Forms.View PlacardView => PlacardLayout;

        public override void ConfigureLayout(Xamarin.Forms.View view)
        {
            Xamarin.Forms.Grid.SetColumn(view, 0);
            Xamarin.Forms.Grid.SetRow(view, 0);
        }

        public override async void PerformPlacardStarup()
        {
            base.PerformPlacardStarup();

            var concreteBindingContextModel = BindingContext as Models.View.InmersiveCityWeatherOverviewViewModel;

            var weatherApiConnectClient = new Weather.Api.Connect.Clients.OpenWeatherMapApiConnectClient(new SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(concreteBindingContextModel.ServiceBaseUri.ToString()));

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            weatherApiConnectClient.GetOverviewByCityAsync(int.Parse(concreteBindingContextModel.City.Id)).ContinueWith(task =>
            {
                if (task.IsCanceled || task.Exception != null)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        EndPresentation();
                    });

                    return;
                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    concreteBindingContextModel.WeatherOverview = task.Result;

                    var weatherInfoShowInStoryboard = new XFToolKit.Presentation.Transform.Animations.Storyboard
                    {
                        IsParallel = true
                    };

                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
                    {
                        Target = ConditionLabel,
                        Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
                    {
                        Target = ConditionDetailLabel,
                        Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up,
                        Delay = "200"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
                    {
                        Target = TemperatureLabel,
                        Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up,
                        Delay = "400"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
                    {
                        Target = TemperatureFeelLikeLayout,
                        Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up,
                        Delay = "600"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
                    {
                        Target = SunBehaviorInfoLayout,
                        Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up,
                        Delay = "700"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
                    {
                        Target = TemperatureLimitsLayout,
                        Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up,
                        Delay = "800"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
                    {
                        Target = HumidityInfoLayout,
                        Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up,
                        Delay = "1000"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
                    {
                        Target = PressureInfoLayout,
                        Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up,
                        Delay = "1125"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
                    {
                        Target = WindInfoLayout,
                        Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up,
                        Delay = "1250"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeToAnimation
                    {
                        Target = HumidityInfoLayoutEmphasisBoxView,
                        Opacity = 1,
                        Delay = "1500"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeToAnimation
                    {
                        Target = PressureInfoLayoutEmphasisBoxView,
                        Opacity = 1,
                        Delay = "1500"
                    });
                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeToAnimation
                    {
                        Target = WindInfoLayoutEmphasisBoxView,
                        Opacity = 1,
                        Delay = "1500"
                    });

                    weatherInfoShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeToAnimation
                    {
                        Target = SourceInfoLayout,
                        Opacity = 1,
                        Delay = "1500"
                    });

                    concreteBindingContextModel.IsBusy = false;

                    weatherInfoShowInStoryboard.Begin();
                });
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            var globalShowInStoryboard = new XFToolKit.Presentation.Transform.Animations.Storyboard
            { 
                IsParallel = false,
                Delay = "1000"
            };

            globalShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeToAnimation
            {
                Target = WeatherInfoLayout,
                Opacity = 1
            });
            globalShowInStoryboard.Animations.Add(new XFToolKit.Presentation.Transform.Animations.FadeInAnimation
            {
                Target = WeatherContextLayout,
                Delay = "1000",
                Direction = XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up
            });

            await globalShowInStoryboard.Begin();

            if (concreteBindingContextModel.WeatherOverview != null) return;

            concreteBindingContextModel.IsBusy = true;
        }
    }
}