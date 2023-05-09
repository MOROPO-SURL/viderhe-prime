using SmartSolucionesCuba.Viderhe.Extend.Common.Presentation.Views;
using SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Views
{
    public partial class PictureSlideSecuencePlacardView : Common.Templating.Presentation.Views.BaseTemplatedPlacardPresentationView
    {
        private Templating.Models.Context.PictureSlideSecuencePlacardContextModel context;

        private Common.Templating.Presentation.Presenters.ScreenplayPresenter screenplayPresenter;

        public PictureSlideSecuencePlacardView(Templating.Models.Context.PictureSlideSecuencePlacardContextModel context)
        {
            InitializeComponent();

            this.context = context;

            BindingContext = new Models.View.PictureSlideSecuencePlacardViewModel
            {
                RequireUnobstructedPresetation = context.RequireUnobstructedPresetation
            };
        }

        public override Xamarin.Forms.Layout<Xamarin.Forms.View> WrapLayout => Wrapper;

        public override Xamarin.Forms.View PlacardView => FramesContainerWrapper;

        public override void ConfigureLayout(Xamarin.Forms.View view)
        {
            Xamarin.Forms.Grid.SetColumn(view, 0);
            Xamarin.Forms.Grid.SetRow(view, 0);
        }

        public override void PerformPlacardStarup()
        {
            base.PerformPlacardStarup();

            var placards = new System.Collections.Generic.List<Support.Presentation.Models.View.PlacardViewModel>();

            var targetTemplateDescriptor = new Descriptors.SimplePictureTemplateDescriptor();

            var targetTemplateViewModel = new Support.Presentation.Models.View.TemplateViewModel
            {
                Id = targetTemplateDescriptor.Id,
                PlayerOrientationSupport = Common.Templating.Enumerations.PlayerOrientationSupportKind.Any
            };

            foreach (var item in context.Slides)
            {
                placards.Add(new Support.Presentation.Models.View.PlacardViewModel
                {
                    Id = System.Guid.NewGuid(),
                    Template = targetTemplateViewModel,
                    SerializedContext = Newtonsoft.Json.JsonConvert.SerializeObject(new Templating.Models.Context.SimplePicturePlacardContextModel
                    {
                        Image = item.Image,
                        LayoutMode = item.LayoutMode,
                        ScreenPresentationTime = System.TimeSpan.FromSeconds(context.PicturePresentationSecounds),
                        IsZoomingEnabled = true
                    })
                });
            }

            screenplayPresenter = new Common.Templating.Presentation.Presenters.ScreenplayPresenter(FramesContainer, PlacardViewModelToPresentationView, placards.ToArray(), new Providers.PictureSlideSecuenceTransitionProvider(), EndPresentation, false, null);

            screenplayPresenter.BeginPresentation();
        }

        private BasePlacardPresentationView PlacardViewModelToPresentationView(Support.Presentation.Models.View.PlacardViewModel model, System.Func<Xamarin.Essentials.DisplayOrientation> getOrientationFunction)
        {
            if (model.Template.Id != Descriptors.SimplePictureTemplateDescriptor.ID) throw new System.InvalidOperationException();

            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Templating.Models.Context.SimplePicturePlacardContextModel>(model.SerializedContext);

            if (context.LayoutMode == Enumerations.PictureLayoutMode.AspectFitWithOpaqueAspectFilledBySelf)
            {
                return new SelfOpaqueFillSimplePicturePlacardView(new Models.View.SelfOpaqueFillSimplePicturePlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    ScreenPresentationTime = context.ScreenPresentationTime,
                    Source = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.Image.Id)),
                    ThumnailSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ThumbnailUriFactory.BuildThumbnailUri(context.Image.ThumbnailId)),
                    Aspect = context.LayoutMode.ToXamarinFormsAspect(),
                    ZoomingEnabled = context.IsZoomingEnabled
                });
            }
            else
            {
                return new SimplePicturePlacardView(new Models.View.SimplePicturePlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    ScreenPresentationTime = context.ScreenPresentationTime,
                    Source = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(context.Image.Id)),
                    Aspect = context.LayoutMode.ToXamarinFormsAspect(),
                    ZoomingEnabled = context.IsZoomingEnabled
                });
            }
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            context = null;

            screenplayPresenter.Dispose();

            screenplayPresenter = null;
        }
    }
}