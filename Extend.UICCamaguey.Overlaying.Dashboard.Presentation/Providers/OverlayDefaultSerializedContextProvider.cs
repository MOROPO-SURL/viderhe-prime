namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Dashboard.Presentation.Providers
{
    public sealed class OverlayDefaultSerializedContextProvider : Common.Overlaying.Dashboard.Presentation.Providers.Facade.IOverlayDefaultSerializedContextProviderFacade
    {
        public string GetOverlayDefaultSerializedContext(Common.Overlaying.Descriptors.IOverlayDescriptor descriptor, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider)
        {
            if (descriptor.Id == Descriptors.TextOverlayDescriptor.ID)
            {
                var model = new Overlaying.Presentation.Models.Context.TextOverlayContextModel
                {
                    Text = "Si información que mostrar por el momento..."
                };

                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }

            if (descriptor.Id == Descriptors.RSSOverlayDescriptor.ID)
            {
                var model = new Overlaying.Presentation.Models.Context.RSSOverlayContextModel();

                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }

            return null;
        }
    }
}
