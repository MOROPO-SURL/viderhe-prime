namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Descriptors
{
    public class TextOverlayDescriptor : Common.Overlaying.Descriptors.IOverlayDescriptor
    {
        public static System.Guid ID = System.Guid.Parse("CF38160A-1B63-4B53-960C-2C2FE1A816E5");

        public System.Guid Id => ID;

        public string Name => "Cinta Informativa con Texto";

        // TODO Define upperviewing description
        public string Description => "Cinta Informativa para mostrar un párrafo texto que contenga de manera sencilla información relevante para la Unión de Informáticos de Cuba en Camagüey";
    }
}