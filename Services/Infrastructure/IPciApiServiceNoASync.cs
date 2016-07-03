
using Common.Model.Pci;

namespace Services.Infrastructure
{
    public interface IPciApiServiceNoASync : ICommonPciService
    {
        T SendGetRequest<T>(string language, string microSiteId, string basketId);

        string SendDeleteRequest(string language, string microSiteId, string basketId);

        string SendPostRequest(string language, string microSiteId, Basket basket);

        bool SendPciBasket(Basket pciBasket, string languageId, string siteId);
    }
}
