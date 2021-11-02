using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = AttributeName)]
    public class ActiveRoute : TagHelper
    {
        private const string AttributeName = "ws-active-route";

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-all-route-data")]
        public Dictionary<string, string> RouteValues { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsActive())
                MakeActive(output);

            output.Attributes.RemoveAll(AttributeName);
        }

        private bool IsActive()
        {
            var routeValues = ViewContext.RouteData.Values;
            var routeController = routeValues["controller"]?.ToString();
            var routeAction = routeValues["action"]?.ToString();

            if (!string.IsNullOrEmpty(Action) && !string.Equals(Action, routeAction))
                return false;

            if (!string.IsNullOrEmpty(Controller) && !string.Equals(Controller, routeController))
                return false;

            foreach(var (key, value) in RouteValues)
            {
                if(!routeValues.ContainsKey(key) || routeValues[key]?.ToString() != value)
                {
                    return false;
                }
            }

            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            const string activeCssClass = "active";

            var classAttribute = output.Attributes.FirstOrDefault(attr => attr.Name == "class");

            if (classAttribute is null)
            {
                output.Attributes.Add("class", activeCssClass);
            }
            else
            {
                if (classAttribute.Value?.ToString()?.Contains(activeCssClass) ?? false)
                {
                    return;
                }

                output.Attributes.SetAttribute("class", $"{classAttribute.Value} {activeCssClass}");
            }
        }
    }
}
