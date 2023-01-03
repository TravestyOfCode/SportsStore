using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Infrastructure;

[HtmlTargetElement("div", Attributes = "page-model")]
public class PageLinkTagHelper : TagHelper
{
    private IUrlHelperFactory urlHelperFactory;

    public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        this.urlHelperFactory = urlHelperFactory;
    }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    public PagingInfo PageModel { get; set; }

    public string PageAction { get; set; }

    public bool PageClassesEnabled { get; set; } = false;

    public string PageClass { get; set; } = string.Empty;

    public string PageClassNormal { get; set; } = string.Empty;

    public string PageClassSelected { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if(ViewContext != null && PageModel != null)
        {
            // UrlHelper generates links based on our controllers/actions
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

            // The DIV tag where our page links go
            TagBuilder result = new TagBuilder("div");

            // Loop through each of the pages and generate a page link
            for(int i = 1; i<= PageModel.TotalPages; i++)
            {
                // Create the link to the page
                TagBuilder tag = new TagBuilder("a");
                tag.Attributes["href"] = urlHelper.Action(PageAction, new { productPage = i });

                // Add CSS if enabled
                if(PageClassesEnabled)
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                }

                // Add Page number text
                tag.InnerHtml.Append(i.ToString());

                // Append generated link to div
                result.InnerHtml.AppendHtml(tag);
            }

            // Add our pages DIV to the HTML content
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
