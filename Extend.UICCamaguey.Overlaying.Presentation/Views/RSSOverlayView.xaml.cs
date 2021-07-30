using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Presentation.Views
{
    public partial class RSSOverlayView : Common.Overlaying.Presentation.Views.BaseOverlayView
    {
        internal static readonly double MotionTranslation = 108;

        public RSSOverlayView(Models.Context.RSSOverlayContextModel model, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider)
        {
            InitializeComponent();

            BindingContext = new Models.View.RSSOverlayViewModel
            {
                PrimaryColor = accountInformationProvider.PrimaryColor
            };

            if (model.FeedUrl == null)
            {
                (BindingContext as Models.View.RSSOverlayViewModel).Text = "Dirección de fuente de noticias no válida o servicio no disponible ...";

                return;
            }

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                httpClient.GetStringAsync(model.FeedUrl).ContinueWith(task =>
                {
                    if (task.Exception != null || task.IsCanceled)
                    {
                        (BindingContext as Models.View.RSSOverlayViewModel).Text = "Dirección de fuente de noticias no válida o servicio no disponible ...";

                        return;
                    }

                    var parser = new Microsoft.Toolkit.Parsers.Rss.RssParser();

                    var rssSchemas = parser.Parse(task.Result);

                    var textBuilder = new System.Text.StringBuilder();

                    if (!string.IsNullOrEmpty(model.Head) && !string.IsNullOrWhiteSpace(model.Head))
                    {
                        textBuilder.Append(model.Head);

                        textBuilder.Append(" || ");
                    }

                    textBuilder.Append(string.Join(" || ", rssSchemas.Select(item => item.Title)));

                    if (!string.IsNullOrEmpty(model.Tail) && !string.IsNullOrWhiteSpace(model.Tail))
                    {
                        textBuilder.Append(" || ");

                        textBuilder.Append(model.Tail);
                    }

                    (BindingContext as Models.View.RSSOverlayViewModel).Text = textBuilder.ToString();
                });
            }
        }

        public override void ShowIn()
        {
            var animation = new XFToolKit.Presentation.Transform.Animations.TranslateToAnimation
            {
                Target = this,
                EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut,
                TranslateY = 0
            };

            animation.Begin();
        }

        public override void ShowOut()
        {
            var animation = new XFToolKit.Presentation.Transform.Animations.TranslateToAnimation
            {
                Target = this,
                EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicIn,
                TranslateY = MotionTranslation
            };

            animation.Begin();
        }
    }
}