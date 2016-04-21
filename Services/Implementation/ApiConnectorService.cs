using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common.Model;
using Common.Model.Pci;
using Newtonsoft.Json;
using Services.Infrastructure;

namespace Services.Implementation
{
    public class ApiConnectorService : IApiConnectorService
    {
        private readonly string _endPoint;
        private readonly string _apiPath;
        private readonly string _fullUrl;

        public ApiConnectorService(string fullUrl)
        {
            _fullUrl = fullUrl;
        }

        public ApiConnectorService(string endPoint, string apiPath)
        {
            _endPoint = endPoint;
            _apiPath = apiPath;
        }
        
        public BornBasket GetExternalBasketByCookie(string cookieValue)
        {
            BornBasket basket = null;

            try
            {
                var client = new HttpClient();
                var task = client.GetAsync(_fullUrl)
                  .ContinueWith((taskwithresponse) =>
                  {
                      var response = taskwithresponse.Result;
                      var jsonString = response.Content.ReadAsStringAsync();
                      jsonString.Wait();
                      basket = JsonConvert.DeserializeObject<BornBasket>(jsonString.Result);

                  });

                task.Wait();
                
            }
            catch (Exception ex)
            {
                //log exception here.
            }

            return basket;
        }

        
    }
}
