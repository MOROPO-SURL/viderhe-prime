namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Dashboard.Presentation.Factories
{
    public static class RSSOverlayConfigurationPageModelFactory
    {
        public static Models.Page.RSSOverlayConfigurationPageModel Build(string serializedContext)
        {
            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Overlaying.Presentation.Models.Context.RSSOverlayContextModel>(serializedContext);

            return new Models.Page.RSSOverlayConfigurationPageModel
            {
                Head = context.Head,
                Tail = context.Tail,
                FeedUrl = context.FeedUrl?.ToString()
            };
        }
    }
}
