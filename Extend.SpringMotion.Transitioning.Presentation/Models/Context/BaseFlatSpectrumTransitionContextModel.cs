namespace SmartSolucionesCuba.Viderhe.Extend.SpringMotion.Transitioning.Presentation.Models.Context
{
    public class BaseFlatSpectrumTransitionContextModel
    {
        public BaseFlatSpectrumTransitionContextModel()
        {
            UseNeutralFlatSpectrumItems = true;
            UseOpaqueFlatSpectrumItems = true;
        }

        public bool UseNeutralFlatSpectrumItems { get; set; }

        public bool UseOpaqueFlatSpectrumItems { get; set; }
    }
}
