using bigbus.checkout.data.Model;
using Common.Helpers;
using System.Collections.Generic;

namespace Services.Infrastructure
{
    public interface INavigationService
    {
        Navigation GetNavigationBySiteAndSection(string site, string section);

        TranslatedNavigationItem[] GetTranslatedItems(ICollection<NavigationItem> items, string languageId);
    }
}
