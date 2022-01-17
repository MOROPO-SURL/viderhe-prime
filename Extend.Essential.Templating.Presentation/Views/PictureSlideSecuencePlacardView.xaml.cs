using SmartSolucionesCuba.Viderhe.Extend.Common.Presentation.Views;
using SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Views
{
    public partial class PictureSlideSecuencePlacardView : Common.Templating.Presentation.Views.BaseTemplatedPlacardPresentationView
    {
        private Models.Context.PictureSlideSecuencePlacardContextModel context;
        private Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider;
        private Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider;

        private Common.Templating.Presentation.Presenters.ScreenplayPresenter screenplayPresenter;

        public PictureSlideSecuencePlacardView(Models.Context.PictureSlideSecuencePlacardContextModel context, Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider)
        {
            InitializeComponent();

            this.context = context;

            this.cloudServerProvider = cloudServerProvider;
            this.accountInformationProvider = accountInformationProvider;

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
                    SerializedContext = Newtonsoft.Json.JsonConvert.SerializeObject(new Models.Context.SimplePicturePlacardContextModel
                    {
                        Image = item.Image,
                        LayoutMode = item.LayoutMode,
                        ScreenPresentationTime = System.TimeSpan.FromSeconds(context.PicturePresentationSecounds),
                        IsZoomingEnabled = true
                    })
                });
            }

            screenplayPresenter = new Common.Templating.Presentation.Presenters.ScreenplayPresenter(FramesContainer, PlacardViewModelToPresentationView, placards.ToArray(), accountInformationProvider, new Providers.PictureSlideSecuenceTransitionProvider(), EndPresentation, false, null);

            screenplayPresenter.BeginPresentation();
        }

        private BasePlacardPresentationView PlacardViewModelToPresentationView(Support.Presentation.Models.View.PlacardViewModel model)
        {
            if (model.Template.Id != Descriptors.SimplePictureTemplateDescriptor.ID) throw new System.InvalidOperationException();

            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Context.SimplePicturePlacardContextModel>(model.SerializedContext);

            if (context.LayoutMode == Enumerations.PictureLayoutMode.AspectFitWithOpaqueAspectFilledBySelf)
            {
                return new SelfOpaqueFillSimplePicturePlacardView(new Models.View.SelfOpaqueFillSimplePicturePlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    ScreenPresentationTime = context.ScreenPresentationTime,
                    Source = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(cloudServerProvider, context.Image.Id)),
                    ThumnailSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ThumbnailUriFactory.BuildThumbnailUri(cloudServerProvider, context.Image.ThumbnailId)),
                    Aspect = context.LayoutMode.ToXamarinFormsAspect()
                }, accountInformationProvider);
            }
            else
            {
                return new SimplePicturePlacardView(new Models.View.SimplePicturePlacardViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    ScreenPresentationTime = context.ScreenPresentationTime,
                    Source = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(cloudServerProvider, context.Image.Id)),
                    Aspect = context.LayoutMode.ToXamarinFormsAspect()
                });
            }
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            context = null;
            cloudServerProvider = null;
            accountInformationProvider = null;

            screenplayPresenter.Dispose();

            screenplayPresenter = null;
        }
    }
}