
using Common.Model;

namespace Services.Infrastructure
{
    public interface ICommonPciService
    {
        ReturnStructure GetBasketPciStatus(string basketId, string currentLanguageId, string subSite);

        ReturnStructure DeletePciBasket(string basketId, string currentLanguageId, string subSite);
    }
}
