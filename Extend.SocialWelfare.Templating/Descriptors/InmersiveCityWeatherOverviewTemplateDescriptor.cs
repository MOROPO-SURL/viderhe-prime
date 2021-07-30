namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Descriptors
{
    public class InmersiveCityWeatherOverviewTemplateDescriptor : Common.Templating.Descriptors.ITemplateDescriptor
    {
        public static System.Guid ID = System.Guid.Parse("2E4818BE-1E31-4CA7-B1F1-408A66B72D5D");

        public System.Guid Id => ID;

        public Common.Templating.Enumerations.PlayerOrientationSupportKind PlayerOrientationSupport => Common.Templating.Enumerations.PlayerOrientationSupportKind.Any;

        public string Name => "Sumario de Estado del Tiempo Inmersivo";

        public string Description => "Muestra un vistaso general del estado del tiempo para una ciudad con una imagen de fondo inmersiva preferentemente relacionada con dicha ciudad.";
    }
}
