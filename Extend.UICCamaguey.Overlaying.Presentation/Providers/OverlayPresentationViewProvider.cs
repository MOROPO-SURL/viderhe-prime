namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Presentation.Providers
{
    public class OverlayPresentationViewProvider : Common.Overlaying.Presentation.Providers.Facade.IOverlayPresentationViewProviderFacade
    {
        public Common.Overlaying.Presentation.Views.BaseOverlayView GetFor(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider)
        {
            if (accountInformationProvider.AccountDefaultOverlayId.Value == Descriptors.TextOverlayDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Context.TextOverlayContextModel>(accountInformationProvider.AccountSerializedOverlayContext);

                var textBuilder = new System.Text.StringBuilder();

                if (!string.IsNullOrEmpty(context.Head) && !string.IsNullOrWhiteSpace(context.Head))
                {
                    textBuilder.Append(context.Head);

                    textBuilder.Append(" || ");
                }

                textBuilder.Append(context.Text);

                if (!string.IsNullOrEmpty(context.Tail) && !string.IsNullOrWhiteSpace(context.Tail))
                {
                    textBuilder.Append(" || ");

                    textBuilder.Append(context.Tail);
                }

                return new Views.TextOverlayView(new Models.View.TextOverlayViewModel
                {
                    Text = textBuilder.ToString(),
                    PrimaryColor = accountInformationProvider.PrimaryColor
                });
            }

            if (accountInformationProvider.AccountDefaultOverlayId.Value == Descriptors.RSSOverlayDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Context.RSSOverlayContextModel>(accountInformationProvider.AccountSerializedOverlayContext);

                return new Views.RSSOverlayView(context, accountInformationProvider);
            }

            return null;
        }
    }
}
