using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Dashboard.Presentation.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Dashboard.Presentation.Pages
{
    public partial class PictureSlideSecuencePlacardConfigurationPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.PictureSlideSecuencePlacardConfigurationPageModel>, Support.Presentation.Dashboard.Pages.Contract.IValidableFormPage
    {
        private readonly System.Collections.Generic.List<Support.Presentation.Dashboard.Behaviors.Contract.IFormFieldValidationBehavior> validationBehaviors;

        private Support.Presentation.Models.View.PlacardViewModel placardViewModel;
        private System.Action successfulCompleteCallbackAction;

        public PictureSlideSecuencePlacardConfigurationPage(System.Action successfulCompleteCallbackAction)
            : this(null, successfulCompleteCallbackAction)
        {
        }

        public PictureSlideSecuencePlacardConfigurationPage(Support.Presentation.Models.View.PlacardViewModel placardViewModel, System.Action successfulCompleteCallbackAction)
            : base(Factories.PictureSlideSecuencePlacardConfigurationPageModelFactory.Build(placardViewModel))
        {
            validationBehaviors = new System.Collections.Generic.List<Support.Presentation.Dashboard.Behaviors.Contract.IFormFieldValidationBehavior>();

            InitializeComponent();

            this.placardViewModel = placardViewModel;
            this.successfulCompleteCallbackAction = successfulCompleteCallbackAction;

            if (placardViewModel == null)
            {
                var templateDescriptor = new Descriptors.PictureSlideSecuenceTemplateDescriptor();

                Title = string.Format("Nuevo Marco ({0})", templateDescriptor.Name);

                return;
            }

            Title = string.Format("Modificando Marco ({0})", placardViewModel.Template.Name);
        }

        private void IncludeImagesButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new Support.Presentation.Dashboard.Pages.ImagesPickPage(BindingContextPageModel.Slides.Select(item => item.Image).ToArray(), ImagesSelected_Callback));
        }

        private void ImageDeleteButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Está seguro que desea eliminar esta imagen de la secuencia?", "Eliminar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    var slide = (sender as Xamarin.Forms.BindableObject).BindingContext as Models.View.PictureSlideViewModel;

                    BindingContextPageModel.Slides.Remove(slide);
                });
            });
        }

        private void ApplyToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            BindingContextPageModel.ResetValidationState();

            foreach (var validationBehavior in validationBehaviors)
            {
                var result = validationBehavior.Evaluate();

                if (!BindingContextPageModel.IsValid) continue;

                if (!result) BindingContextPageModel.IsValid = false;
            }

            if (!BindingContextPageModel.IsValid) return;

            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Api.Connect.Clients.PlacardsApiConnectClient(Xamarin.Forms.DependencyService.Get<MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade>());

                if (placardViewModel == null)
                {
                    var model = new Cloud.Api.Common.Models.PlacardInputModel
                    {
                        Label = BindingContextPageModel.Label,
                        TemplateId = Descriptors.PictureSlideSecuenceTemplateDescriptor.ID,
                        PlayerOrientationSupportKind = BindingContextPageModel.PlayerOrientationSupportKind.FromConfiguration(),
                        SerializedContext = Newtonsoft.Json.JsonConvert.SerializeObject(new Templating.Models.Context.PictureSlideSecuencePlacardContextModel
                        {
                            RequireUnobstructedPresetation = BindingContextPageModel.RequireUnobstructedPresetation,
                            PicturePresentationSecounds = BindingContextPageModel.PicturePresentationSecounds,
                            PictureTransitionId = Transitioning.Transitions.ScalingGrowFadeOutInTransitionDescriptor.ID,
                            Slides = BindingContextPageModel.Slides.Select(item => new Templating.Models.Context.PictureSlideContextModel
                            {
                                Image = new Common.Templating.Models.Context.ImageContextModel { Id = item.Image.Id, ThumbnailId = item.Image.ThumbnailId },
                                LayoutMode = item.LayoutMode
                            }).ToArray()
                        })
                    };

                    if (BindingContextPageModel.DefineBeginAt)
                    {
                        model.BeginAt = BindingContextPageModel.BeginAt;
                    }

                    if (BindingContextPageModel.DefineEndAt)
                    {
                        model.EndAt = BindingContextPageModel.EndAt;
                    }

                    await client.CreateAsync(model);
                }
                else
                {
                    var model = new Cloud.Api.Common.Models.PlacardInputModel
                    {
                        Id = placardViewModel.Id,
                        Label = BindingContextPageModel.Label,
                        TemplateId = placardViewModel.Template.Id,
                        PlayerOrientationSupportKind = BindingContextPageModel.PlayerOrientationSupportKind.FromConfiguration(),
                        SerializedContext = Newtonsoft.Json.JsonConvert.SerializeObject(new Templating.Models.Context.PictureSlideSecuencePlacardContextModel
                        {
                            RequireUnobstructedPresetation = BindingContextPageModel.RequireUnobstructedPresetation,
                            PicturePresentationSecounds = BindingContextPageModel.PicturePresentationSecounds,
                            PictureTransitionId = Transitioning.Transitions.ScalingGrowFadeOutInTransitionDescriptor.ID,
                            Slides = BindingContextPageModel.Slides.Select(item => new Templating.Models.Context.PictureSlideContextModel
                            {
                                Image = new Common.Templating.Models.Context.ImageContextModel { Id = item.Image.Id, ThumbnailId = item.Image.ThumbnailId },
                                LayoutMode = item.LayoutMode
                            }).ToArray()
                        })
                    };

                    if (BindingContextPageModel.DefineBeginAt)
                    {
                        model.BeginAt = BindingContextPageModel.BeginAt;
                    }

                    if (BindingContextPageModel.DefineEndAt)
                    {
                        model.EndAt = BindingContextPageModel.EndAt;
                    }

                    await client.EditAsync(model);
                }
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    if (task.Exception != null || task.IsCanceled)
                    {
                        Xamarin.Forms.DependencyService.Get<Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade>().HandleException(task.Exception);
                    }
                    else
                    {
                        Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Información", "Cambios aplicados con éxito.");

                        this.NavigationPopPageAsync();

                        successfulCompleteCallbackAction.Invoke();
                    }
                });
            });
        }

        private void CancelToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPopPageAsync();
        }

        private void ImagesSelected_Callback(Support.Presentation.Models.View.ImageViewModel[] selection)
        {
            foreach (var item in selection)
            {
                BindingContextPageModel.Slides.Add(new Models.View.PictureSlideViewModel { Image = item });
            }
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            placardViewModel = null;
            successfulCompleteCallbackAction = null;
        }

        #region IValidableFormPage implementation

        public void AddValidationBehavior(Support.Presentation.Dashboard.Behaviors.Contract.IFormFieldValidationBehavior validationBehavior)
        {
            validationBehaviors.Add(validationBehavior);
        }

        public void RemoveValidationBehavior(Support.Presentation.Dashboard.Behaviors.Contract.IFormFieldValidationBehavior validationBehavior)
        {
            validationBehaviors.Remove(validationBehavior);
        }

        public void AddValidationMessage(string label, string message)
        {
            BindingContextPageModel.ValidationMessages.Add(new Support.Presentation.Dashboard.Models.View.ValidationMessageViewModel
            {
                Caption = label,
                Message = message
            });
        }

        #endregion
    }
}