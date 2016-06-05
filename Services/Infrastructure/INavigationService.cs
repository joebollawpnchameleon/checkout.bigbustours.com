using bigbus.checkout.data.Model;
using Common.Helpers;
using System.Collections.Generic;
using Common.Model;

namespace Services.Infrastructure
{
    public interface INavigationService
    {
        Navigation GetNavigationBySiteAndSection(string site, string section);

        List<FrontEndNavigationItem> GetNavigationBySiteAndSection(string site, string section, string languageid);

    }
}
