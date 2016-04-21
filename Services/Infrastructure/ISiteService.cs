
using bigbus.checkout.data.Model;

namespace Services.Infrastructure
{
    public interface ISiteService
    {
        MicroSite GetMicroSiteById(string id);
    }
}
