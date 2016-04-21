
using System;
using bigbus.checkout.data.Model;
using bigbus.checkout.data.Repositories.Infrastructure;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class SiteService :  ISiteService
    {
        private readonly IGenericDataRepository<MicroSite> _micrositeRepository;

        public SiteService(IGenericDataRepository<MicroSite> micrositeRepository)
        {
            _micrositeRepository = micrositeRepository;
        }

        public MicroSite GetMicroSiteById(string id)
        {
            return _micrositeRepository.GetSingle(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
