using bigbus.checkout.data.Model;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class PageContentService : BaseService, IPageContentService
    {
        public virtual  List<HtmlMetaTag> GetPageMetaTags(string pageType, string pageIdentifier)
        {
            var tagList = HtmlMetaTagRepository.GetList(x => (x.PageType.Equals(pageType, StringComparison.CurrentCultureIgnoreCase)
                    && x.PageIdentifier.Equals(pageIdentifier, StringComparison.CurrentCultureIgnoreCase)));

            return (tagList != null) ? tagList.ToList() : null;
        }
    }
}
