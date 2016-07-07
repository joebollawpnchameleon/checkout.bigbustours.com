using bigbus.checkout.data.Model;
using System.Collections.Generic;

namespace Services.Infrastructure
{
    public interface IPageContentService
    {
        List<HtmlMetaTag> GetPageMetaTags(string pageType, string pageIdentifier);
    }
}
