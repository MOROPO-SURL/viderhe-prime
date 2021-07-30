namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Dashboard.Presentation.Factories
{
    public static class TextOverlayConfigurationPageModelFactory
    {
        public static Models.Page.TextOverlayConfigurationPageModel Build(string serializedContext)
        {
            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Overlaying.Presentation.Models.Context.TextOverlayContextModel>(serializedContext);

            return new Models.Page.TextOverlayConfigurationPageModel
            {
                Head = context.Head,
                Tail = context.Tail,
                Text = context.Text
            };
        }
    }
}
