namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class Extension : MOROPO.SAPRESSC.Core.Persistence.Entities.IHabilitableEntity<System.Guid>
    {
        public Extension()
        {
            Enabled = true;
            Templates = new System.Collections.Generic.List<Template>();
            Transitions = new System.Collections.Generic.List<Transition>();
            Overlays = new System.Collections.Generic.List<Overlay>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Version { get; set; }

        public bool Enabled { get; set; }

        public System.Collections.Generic.List<Template> Templates { get; set; }

        public System.Collections.Generic.List<Transition> Transitions { get; set; }

        public System.Collections.Generic.List<Overlay> Overlays { get; set; }
    }
}
