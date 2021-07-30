namespace SmartSolucionesCuba.Viderhe.Extend.SpringMotion.Transitioning.Presentation.Factories
{
    public static class RandomTransitionContextModelFactory
    {
        public static Models.Context.FlatBoxMatrixSweptCurtainTransitionContextModel CreateFlatBoxMatrixSweptCurtainTransitionContextModel(Xamarin.Forms.Color color)
        {
            var randomizer = new System.Random();

            var motionTypeCount = System.Enum.GetNames(typeof(Enumerations.SweptTransitionItemMotionType)).Length;
            //var motionModeCount = System.Enum.GetNames(typeof(Enumerations.SweptTransitionItemMotionMode)).Length;
            var orientationCount = System.Enum.GetNames(typeof(Enumerations.SweptTransitionOrientation)).Length;
            var inOutKindCount = System.Enum.GetNames(typeof(Enumerations.SweptTransitionInOutKind)).Length;

            return new Models.Context.FlatBoxMatrixSweptCurtainTransitionContextModel
            {
                Color = color,
                MotionType = (Enumerations.SweptTransitionItemMotionType)randomizer.Next(0, motionTypeCount),
                MotionMode = Enumerations.SweptTransitionItemMotionMode.Quick,//(Enumerations.SweptTransitionItemMotionMode)randomizer.Next(0, motionModeCount),
                Orientation = (Enumerations.SweptTransitionOrientation)randomizer.Next(0, orientationCount),
                InOutKind = (Enumerations.SweptTransitionInOutKind)randomizer.Next(0, inOutKindCount)
            };
        }

        public static Models.Context.BaseFlatSpectrumTransitionContextModel CreateBaseFlatSpectrumTransitionContextModel()
        {
            var randomizer = new System.Random();

            return new Models.Context.BaseFlatSpectrumTransitionContextModel
            {
                UseNeutralFlatSpectrumItems = randomizer.Next(0, 2) == 1,
                UseOpaqueFlatSpectrumItems = randomizer.Next(0, 2) == 1
            };
        }

        public static Models.Context.FlatDualSpectrumHandFanCurtainTransitionContextModel CreateFlatDualSpectrumHandFanCurtainTransitionContextModel()
        {
            var randomizer = new System.Random();

            var modeCount = System.Enum.GetNames(typeof(Enumerations.DualSpectrumHandFanCurtainMode)).Length;

            return new Models.Context.FlatDualSpectrumHandFanCurtainTransitionContextModel
            {
                UseNeutralFlatSpectrumItems = randomizer.Next(0, 2) == 1,
                UseOpaqueFlatSpectrumItems = randomizer.Next(0, 2) == 1,
                Mode = (Enumerations.DualSpectrumHandFanCurtainMode)randomizer.Next(0, modeCount)
            };
        }

        public static Models.Context.FlatSpectrumInverseGothTransitionContextModel CreateFlatSpectrumInverseGothTransitionContextModel()
        {
            var randomizer = new System.Random();

            var easingTypeCount = System.Enum.GetNames(typeof(XFToolKit.Presentation.Transform.Enumerations.EasingType)).Length;

            return new Models.Context.FlatSpectrumInverseGothTransitionContextModel
            {
                UseNeutralFlatSpectrumItems = randomizer.Next(0, 2) == 1,
                UseOpaqueFlatSpectrumItems = randomizer.Next(0, 2) == 1,
                HorizontalTransitionOrientation = (Common.Transitioning.Presentation.Enumerations.HorizontalTransitionOrientation)randomizer.Next(1, 3),
                VerticalTransitionOrientation = (Common.Transitioning.Presentation.Enumerations.VerticalTransitionOrientation)randomizer.Next(0, 3),
                HorizontalTransitionEasingType = (XFToolKit.Presentation.Transform.Enumerations.EasingType)randomizer.Next(0, easingTypeCount),
                VerticalTransitionEasingType = (XFToolKit.Presentation.Transform.Enumerations.EasingType)randomizer.Next(0, easingTypeCount)
            };
        }
    }
}
