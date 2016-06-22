

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common.Enums;
using Common.Model;
using Common.Model.Pci;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class PciApiServiceNoASync : BaseService, IPciApiServiceNoASync
    {
         private readonly string _pciEndPoint;

         public PciApiServiceNoASync(string pciEndPoint)
        {
            _pciEndPoint = pciEndPoint;
        }

         public T SendGetRequest<T>(string language, string microSiteId, string basketId)
         {
             using (var client = new HttpClient())
             {
                 client.BaseAddress = new Uri(string.Format(_pciEndPoint, microSiteId));
                 client.DefaultRequestHeaders.Accept.Clear();
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                 // HTTP GET
                 var response = client.GetAsync(string.Format("Basket/{0}", basketId)).Result;
                 return response.Content.ReadAsAsync<T>().Result;
             }
         }

        public string SendDeleteRequest(string language, string microSiteId, string basketId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format(_pciEndPoint, microSiteId));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response = client.DeleteAsync(string.Format("Basket/{0}", basketId)).Result;
                
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string SendPostRequest(string language, string microSiteId, Basket basket)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format(_pciEndPoint, microSiteId));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response =  client.PostAsJsonAsync("Basket/", basket).Result;
                return  response.Content.ReadAsStringAsync().Result;
            }
        }
        
        #region Methods called on webforms

        public ReturnStructure GetBasketPciStatus(string basketId, string currentLanguageId, string subSite)
        {
            try
            {
                Log("PCI web request returned to Success page for basket id " + basketId);
                var basketStatus = SendGetRequest<BasketStatus>(currentLanguageId, subSite, basketId);

                Log(string.Format("Payment Returned from PCI with status message:{0} BasketId: {1}", basketStatus.status.text, basketId));
                return new ReturnStructure
                {
                    Status = ReturnStatus.Success,
                    ReturnObject = basketStatus
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
                var result = SendDeleteRequest(currentLanguageId, subSite, basketId);

                Log("Payment Returned from PCI after deleting basket with ID" + basketId + " result " + result);
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
