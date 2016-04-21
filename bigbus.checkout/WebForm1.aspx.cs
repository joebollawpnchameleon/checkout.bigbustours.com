using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;
using Common.Model.Pci;
using Services.Infrastructure;
using Basket = bigbus.checkout.data.Model.Basket;

namespace bigbus.checkout
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public IAuthenticationService AuthenticationService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var status = SendGetRequest("eng", "london", "ED8E0866-69BC-4D60-9569-5D078546695B");
        }

        private void Log(string message)
        {
            
        }

        //public async static Task<string> SendPostRequest()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(string.Format(@"http://eng.secure.localdev.com:62176/api/london/", "eng", microSiteId));
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        // HTTP GET
        //        HttpResponseMessage response = await client.PostAsJsonAsync("Basket/", basket);
        //        response.EnsureSuccessStatusCode();

        //        var result = await response.Content.ReadAsStringAsync();
        //        return result;
        //    }
        //}

        private BasketStatus CheckBasketStatusPci()
        {
            try
            {
                var task = SendGetRequest<BasketStatus>("eng", "london", "ED8E0866-69BC-4D60-9569-5D078546695B");
                task.Wait();

                Log("Payment Returned from PCI with status message:" + task.Result.status.text + "UserEmail");
                return task.Result;
            }
            catch (Exception exception)
            {
                Log("PCI web request error: Process failure request " + DateTime.Now + " - Exception.Message: " + exception.Message);

                if (exception.InnerException != null && !string.IsNullOrWhiteSpace(exception.InnerException.Message))
                {
                    Log("PCI web request error: " + DateTime.Now + " - InnerException.Message: " + exception.InnerException.Message);
                }
               
                return null;
            }
        }

        public async static Task<T> SendGetRequest<T>(string language, string microSiteId, string basketId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format(ConfigurationManager.AppSettings["PciWebsite.ApiDomain"], language, microSiteId));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync(string.Format("Basket/{0}", basketId));
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsAsync<T>();
                return result;
            }
        }

        public static string SendGetRequest(string language, string microSiteId, string basketId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format(ConfigurationManager.AppSettings["PciWebsite.ApiDomain"], language, microSiteId));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response =  client.GetAsync(string.Format("Basket/{0}", basketId)).Result;
                
                return  response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}