namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Descriptors
{
    public class UICCamagueyExtensionDescriptor : Common.Descriptors.IExtensionDescriptor
    {
        public static System.Guid ID = System.Guid.Parse("5E6010DA-0860-4BDF-86D5-7533B7CACE12");

        public System.Guid Id => ID;

        public string Name => "UIC Camagüey";

        // TODO Define upperviewing description
        public string Description => "Extensión para la plataforma Viderhe con elementos a medida para la Unión de Informáticos de Cuba en Camagüey";
    }
}
