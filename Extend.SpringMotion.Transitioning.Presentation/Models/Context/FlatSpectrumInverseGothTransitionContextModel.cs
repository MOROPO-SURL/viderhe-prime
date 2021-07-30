using SmartSolucionesCuba.Viderhe.Extend.Common.Transitioning.Presentation.Enumerations;

namespace SmartSolucionesCuba.Viderhe.Extend.SpringMotion.Transitioning.Presentation.Models.Context
{
    public class FlatSpectrumInverseGothTransitionContextModel : BaseFlatSpectrumTransitionContextModel
    {
        public FlatSpectrumInverseGothTransitionContextModel()
        {
            HorizontalTransitionOrientation = HorizontalTransitionOrientation.FromRight;
            HorizontalTransitionEasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut;

            VerticalTransitionOrientation = VerticalTransitionOrientation.FromTop;
            VerticalTransitionEasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.Linear;
        }

        public HorizontalTransitionOrientation HorizontalTransitionOrientation { get; set; }

        public VerticalTransitionOrientation VerticalTransitionOrientation { get; set; }

        public XFToolKit.Presentation.Transform.Enumerations.EasingType HorizontalTransitionEasingType { get; set; }

        public XFToolKit.Presentation.Transform.Enumerations.EasingType VerticalTransitionEasingType { get; set; }
    }
}
