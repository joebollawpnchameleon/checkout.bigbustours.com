using bigbus.checkout.data.Model;
using Common.Helpers;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class NavigationService : BaseService, INavigationService
    {
        public Navigation GetNavigationBySiteAndSection(string site, string section)
        {
            return NavigationRepository.GetSingle(x => x.MicroSiteId.Equals(site, StringComparison.CurrentCultureIgnoreCase)
                    && x.Section.Equals(section, StringComparison.CurrentCultureIgnoreCase));
        }

        public TranslatedNavigationItem[] GetTranslatedItems(ICollection<NavigationItem> items, string languageId)
        {
            TranslatedNavigationItem[] arr = new TranslatedNavigationItem[items.Count];

            //int c = 0;

            //foreach (var item in items)
            //{
                //string text = null;

                //var main = NavigationItemLanguageRepository.GetSingle(x => x.)
                //    TranslationService.TranslateTerm(item.Id, languageId);

                //if (main != null) { text = main.Text; }

                //if (string.IsNullOrWhiteSpace(text))
                //{
                //    NavigationItem_Language fallback = item.GetTranslation(FallBackLanguage);

                //    if (fallback != null)
                //    {
                //        text = fallback.Text;
                //    }

                //    #region Temp. auto-translate back from pre-nav existing data
                //    if (string.IsNullOrWhiteSpace(text))
                //    {
                //        Phrase_Language pl = null;

                //        pl =
                //            GetObjectFactory().GetByOQL<Phrase_Language>(
                //                "Language(Id=$p0$)<-Phrase_Language(Translation=$p2$)->Phrase<-*Phrase_Language->Language(Id=$p1$)",
                //                FallBackLanguage,
                //                CurrentLanguage,
                //                text);

                //        if (pl != null)
                //        {
                //            text = pl.Translation;

                //            if (string.IsNullOrWhiteSpace(text))
                //            {
                //                if (main == null)
                //                {
                //                    main = GetObjectFactory().GetBlankNew<NavigationItem_Language>();
                //                    main.Language_Id = CurrentLanguage;
                //                    main.NavigationItem = item;
                //                    main.Text = text;
                //                    main.PersistDataAsNew();
                //                }
                //                else
                //                {
                //                    main.Text = text;
                //                    main.PersistData();
                //                }
                //            }
                //        }

                //    }
                  //  #endregion
                //}

            //    if (text == null)
            //    {
            //        text = "Section";
            //    }

            //    string newUrl = string.Empty;

            //    if (item.URL.IndexOf("/") == 0)
            //    {
            //        newUrl = string.Concat("http://", Request.Url.Host, "/", CurrentLanguage, item.URL);
            //    }
            //    else
            //    {
            //        newUrl = item.URL;
            //    }

            //    arr[c] = new TranslatedNavigationItem(item.Id, text, newUrl);
            //    c++;
            //}

            //return arr;
            return arr;
        }
    }
}
