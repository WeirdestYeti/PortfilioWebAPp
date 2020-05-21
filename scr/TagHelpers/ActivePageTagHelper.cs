using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApp.TagHelpers
{
    [HtmlTargetElement(Attributes = "is-active-page")]
    public class ActivePageTagHelper : TagHelper
    {
        /// <summary>The name of the action method.</summary>
        /// <remarks>Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-<c>null</c>.</remarks>
        [HtmlAttributeName("asp-page")]
        public string Page { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.Rendering.ViewContext" /> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            string activeWhen = null;

            // Check if link has the attribute
            if (output.Attributes.ContainsName("active-when"))
            {
                // Get the value of the attribute and remove it from the page.
                activeWhen = output.Attributes.Single(x => x.Name.Equals("active-when")).Value.ToString();
                output.Attributes.RemoveAll("active-when");
            }

            if (ShouldBeActive(activeWhen))
            {
                MakeActive(output);
            }

            output.Attributes.RemoveAll("is-active-page");
        }

        private bool ShouldBeActive(string activeWhen = null)
        {
            string currentPage = ViewContext.RouteData.Values["Page"].ToString();

            // If the string is set check if the current page contains the set attribute.
            // !!! Subject to change, could return false positives. We should only check the part of the string from the first "/" to the next "/". Should be more accurate !!!
            if(activeWhen != null)
            {
                if (currentPage.Contains(activeWhen))
                {
                    return true;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Page) && Page.ToLower() != currentPage.ToLower())
                {
                    return false;
                }

                return true;
            } 

            return false;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "active");
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null || classAttr.Value.ToString().IndexOf("active") < 0)
            {
                output.Attributes.SetAttribute("class", classAttr.Value == null
                    ? "active"
                    : classAttr.Value.ToString() + " active");
            }
        }
    }
}
