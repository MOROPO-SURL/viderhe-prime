namespace SmartSolucionesCuba.Viderhe.Cloud.Extensions
{
    public static class ManagementDashboardSectionExtensions
    {
        public static Microsoft.AspNetCore.Html.HtmlString ToIconographyHtmlString(this MOROPO.SAPRESSC.Core.Web.Backend.Models.View.ManagementDashboardSectionViewModel self)
        {
            if (self.Controller == "users") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-account\"></span>");

            if (self.Controller == "groups") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-account-group\"></span>");

            if (self.Controller == "traces") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-foot-print\"></span>");

            if (self.Controller == "accounts") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-briefcase\"></span>");

            if (self.Controller == "templates") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-picture-in-picture-bottom-right\"></span>");

            if (self.Controller == "transitions") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-transition\"></span>");

            if (self.Controller == "images") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-image-size-select-actual\"></span>");

            if (self.Controller == "videos") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-filmstrip\"></span>");

            if (self.Controller == "players") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-youtube-tv\"></span>");

            return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-toy-brick-outline\"></span>");
        }
    }
}
