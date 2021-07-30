namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Descriptors
{
    public class InfinityVideoTemplateDescriptor : Common.Templating.Descriptors.ITemplateDescriptor
    {
        public static System.Guid ID = new System.Guid("D49EAB3A-F407-4E0E-835B-F15BFD917269");

        public System.Guid Id => ID;

        public Common.Templating.Enumerations.PlayerOrientationSupportKind PlayerOrientationSupport => Common.Templating.Enumerations.PlayerOrientationSupportKind.Any;

        public string Name => "Video Infinito";

        public string Description => "Muestra un video que se reproduce infinitamente reiniciando la reproducción al llegar al final del mismo.";
    }
}
