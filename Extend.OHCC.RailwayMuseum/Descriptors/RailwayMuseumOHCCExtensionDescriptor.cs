namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Descriptors
{
    public class RailwayMuseumOHCCExtensionDescriptor : Common.Descriptors.IExtensionDescriptor
    {
        public static System.Guid ID = System.Guid.Parse("3C20AF34-28A3-4D05-8C56-724F5446A8E0");

        public System.Guid Id => ID;

        public string Name => "Museo Ferroviario OHCC";

        public string Description => "Elementos de la personalización de la plataforma para satisfacer las necesidades específicas del Museo Ferroviario de la Oficina del Historiador de la Ciudad de Camagüey (OHCC).";
    }
}
