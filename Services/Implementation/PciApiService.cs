using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common.Enums;
using Common.Model;
using Common.Model.Pci;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class PciApiService : BaseService, IPciApiService
    {
        private readonly string _pciEndPoint;

        public PciApiService(string pciEndPoint)
        {
            _pciEndPoint = pciEndPoint;
        }
        
        public async Task<T> SendGetRequest<T>(string language, string microSiteId, string basketId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format(_pciEndPoint, language, microSiteId));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response = await client.GetAsync(string.Format("Basket/{0}", basketId));
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsAsync<T>();
                return result;
            }
        }

        public async Task<string> SendDeleteRequest(string language, string microSiteId, string basketId)
        {
            string callResult;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format(_pciEndPoint, language, microSiteId));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response = await client.DeleteAsync(string.Format("Basket/{0}", basketId));
                response.EnsureSuccessStatusCode();

                callResult = await response.Content.ReadAsStringAsync();
            }

            return callResult;
        }

        public async Task<string> SendPostRequest(string language, string microSiteId, Basket basket)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format(_pciEndPoint, language, microSiteId));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response = await client.PostAsJsonAsync("Basket/", basket);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }
        
        #region Methods called on webforms

        public ReturnStructure GetBasketPciStatus(string basketId, string currentLanguageId, string subSite)
        {
            try
            {
                Log("PCI web request returned to Success page for basket id " + basketId);
                var task = SendGetRequest<BasketStatus>(currentLanguageId, subSite, basketId);
                task.Wait();

                var basketStatusResponse = task.Result;
                Log(string.Format("Payment Returned from PCI with status message:{0} BasketId: {1}", task.Result.status.text, basketId));
                return new ReturnStructure
                {
                    Status = ReturnStatus.Success,
                    ReturnObject = basketStatusResponse
                };
            }
            catch (Exception exception)
            {
                Log("PCI web request error: Process failure request " + DateTime.Now + " - Exception.Message: " + exception.Message);

                if (exception.InnerException != null && !string.IsNullOrWhiteSpace(exception.InnerException.Message))
                {
                    Log("PCI web request error: " + DateTime.Now + " - InnerException.Message: " + exception.InnerException.Message);
                }

                return new ReturnStructure
                {
                    Status = ReturnStatus.Failure,
                    ErrorMessage = "Error getting basket status from PCI"
                };
            }

        }

        public ReturnStructure DeletePciBasket(string basketId, string currentLanguageId, string subSite)
        {
            try
            {
                Log("Entering basket DeletePciBasket with basketid: " + basketId);
                //send request to flush
                var task = SendDeleteRequest(currentLanguageId, subSite, basketId);
                task.Wait();
                Log("Payment Returned from PCI after deleting basket with ID" + basketId + task.Result);
                return new ReturnStructure {Status = ReturnStatus.Success};
            }
            catch (Exception exception)
            {
                Log("PCI web request error: Delete basket request - " + DateTime.Now + " - Exception.Message: " + exception.Message);

                if (exception.InnerException != null && !string.IsNullOrWhiteSpace(exception.InnerException.Message))
                {
                    Log("PCI web request error: Delete basket request - " + DateTime.Now + " - InnerException.Message: " + exception.InnerException.Message);
                }

                return new ReturnStructure
                {
                    Status = ReturnStatus.Failure,
                    ErrorMessage = "Error deleting PCI basket."
                };
            }
        }

        #endregion

    }
}
