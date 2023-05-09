using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using SmartSolucionesCuba.Viderhe.Support.Presentation.Models.View;

namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions
{
    public static class CloudApiModelExtensions
    {
        public static ImageViewModel ToViewModel(this ImageDisplayModel source)
        {
            return new ImageViewModel
            {
                Id = source.Id,
                ThumbnailId = source.ThumbnailId,
                CreateAt = source.CreateAt,
                Format = source.ContentType.Split('/')[1].ToUpperInvariant(),
                Resolution = string.Format("{0}x{1}", source.Width, source.Height),
                Size = source.Length,
                About = source.About
            };
        }

        public static VideoViewModel ToViewModel(this VideoDisplayModel source)
        {
            return new VideoViewModel
            {
                Id = source.Id,
                ThumbnailId = source.ThumbnailId,
                CreateAt = source.CreateAt,
                Extension = source.Extension.Substring(1).ToUpperInvariant(),
                Duration = source.Duration.ToString(),
                FrameSize = source.FrameSize,
                Fps = source.Fps,
                Size = source.Length,
                About = source.About
            };
        }

        public static PlacardViewModel ToViewModel(this PlacardDisplayModel target)
        {
            var templateDescriptor = Extend.Templating.Providers.TemplateDescriptorProvider.Instance.GetDescriptorById(target.Template.Id);

            var result = new PlacardViewModel
            {
                Id = target.Id,
                Label = target.Label,
                PlayerOrientationSupportKind = target.PlayerOrientationSupportKind,
                BeginAt = target.BeginAt,
                EndAt = target.EndAt,
                SerializedContext = target.SerializedContext,
                Template = templateDescriptor.ToViewModel()
            };

            return result;
        }
    }
}
