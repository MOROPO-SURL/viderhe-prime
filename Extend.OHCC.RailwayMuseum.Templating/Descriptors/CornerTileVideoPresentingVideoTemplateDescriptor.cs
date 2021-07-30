namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Descriptors
{
    public class CornerTileVideoPresentingVideoTemplateDescriptor : Common.Templating.Descriptors.ITemplateDescriptor
    {
        public static System.Guid ID = new System.Guid("CD1BAC6A-5C97-4BE6-B146-B7393D39FD01");

        public System.Guid Id => ID;

        public Common.Templating.Enumerations.PlayerOrientationSupportKind PlayerOrientationSupport => Common.Templating.Enumerations.PlayerOrientationSupportKind.Any;

        public string Name => "Video presentado por Video Miniatura";

        public string Description => "Muestra un video pequeño en una de las esquinas de la pantalla presentando a otro video.";
    }
}
