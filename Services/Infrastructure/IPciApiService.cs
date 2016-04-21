using System.Threading.Tasks;
using Common.Model;
using Common.Model.Pci;

namespace Services.Infrastructure
{
    public interface IPciApiService : ICommonPciService
    {
        Task<T> SendGetRequest<T>(string language, string microSiteId, string basketId);

        Task<string> SendDeleteRequest(string language, string microSiteId, string basketId);

        Task<string> SendPostRequest(string language, string microSiteId, Basket basket);
        
    }
}
