namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Descriptors
{
    public class RSSOverlayDescriptor : Common.Overlaying.Descriptors.IOverlayDescriptor
    {
        public static System.Guid ID = System.Guid.Parse("7534F4C5-D9E6-4011-9ECE-3B554236632D");

        public System.Guid Id => ID;

        public string Name => "Cinta Informativa con Fuente RSS";

        // TODO Define upperviewing description
        public string Description => "Cinta Informativa para mostrar el contenido difundido mediante una fuente RSS seleccionada por la Unión de Informáticos de Cuba en Camagüey";
    }
}
