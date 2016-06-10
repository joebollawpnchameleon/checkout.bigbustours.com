using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;

namespace bigbus.checkout.Controllers
{
    public class MagentoTestController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(string id)
        {
            var jsonString =
//                @"{
//                ""items"":[{""name"":""London 24 hour"",""sku"":""01011"",""ProductDimensionUID"":""c4bf36f4-acf4-4da8-b8ca-f4f9ce9345a0"",""qty"":2,""price"":29.99,""discount"":0,""total"":59.98,""city"":""London"",""type"":""adult""}
//                ,{""name"":""London 24 hour"",""sku"":""01011"",""ProductDimensionUID"":""c4bf36f4-acf4-4da8-b8ca-f4f9ce9345a0"",""qty"":1,""price"":25.99,""discount"":5,""total"":20.99,""city"":""London"",""type"":""child""}],
//                ""subtotal"":85.97,""discount"":5,""total"":80.97,""coupon"":""TEST-COUPON1"",""currency"":""GBP"",""language"":""eng""
//                }";
//            @"{
//            ""items"":[
//             {""name"":""London 24 hour"",""sku"":""01011"",""ProductDimensionUID"":""c4bf36f4-acf4-4da8-b8ca-f4f9ce9345a0"",""qty"":2,""price"":29.99,""discount"":0,""total"":59.98,""city"":""London"",""type"":""adult""}
//            ,{""name"":""London 48 hour"",""sku"":""32033"",""ProductDimensionUID"":""E5CD49D7-3575-464F-AE3F-A093C83261BB"",""qty"":1,""price"":25.99,""discount"":5,""total"":20.99,""city"":""London"",""type"":""child""}],
//            ""subtotal"":85.97,""discount"":5,""total"":80.97,""coupon"":""TEST-COUPON1"",""currency"":""GBP"",""language"":""eng""
//            }";

//            @"{
//            ""items"":[
//             {""name"":""London 24 hour"",""sku"":""1348"",""ProductDimensionUID"":""c4bf36f4-acf4-4da8-b8ca-f4f9ce9345a0"",""qty"":2,""price"":29.99,""discount"":0,""total"":59.98,""city"":""London"",""type"":""adult""},            
//            {""name"":""Madame Tusseau"",""sku"":""32033"",""ProductDimensionUID"":""E5CD49D7-3575-464F-AE3F-A093C83261BB"",""qty"":1,""price"":23.59,""discount"":3.5,""total"":20.09,""city"":""London"",""type"":""adult""}],
//            ""subtotal"":83.57,""discount"":3.5,""total"":80.07,""coupon"":""TEST-COUPON1"",""currency"":""GBP"",""language"":""eng""
//            }";

            @"{
            ""items"":[
             {""name"":""London 24 hour"",""sku"":""1348"",""ProductDimensionUID"":""c4bf36f4-acf4-4da8-b8ca-f4f9ce9345a0"",""qty"":2,""price"":29.99,""discount"":0,""total"":59.98,""city"":""London"",""type"":""adult""},
            {""name"":""24 Hour Day Tour"",""sku"":""1503"",""ProductDimensionUID"":""2f83ee23-b357-4f2d-b52c-bf8e32904381"",""qty"":1,""price"":21.00,""discount"":1,""total"":20.00,""city"":""abudhabi"",""type"":""Child""},            
            {""name"":""Madame Tusseau"",""sku"":""32033"",""ProductDimensionUID"":""E5CD49D7-3575-464F-AE3F-A093C83261BB"",""qty"":1,""price"":23.59,""discount"":3.5,""total"":20.09,""city"":""London"",""type"":""adult""}],
            ""subtotal"":104.57,""discount"":4.5,""total"":100.07,""coupon"":""TEST-COUPON1"",""currency"":""GBP"",""language"":""eng""
            }";

            JToken json = JObject.Parse(jsonString);

            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


        public class JsonContent : HttpContent
        {
            private readonly JToken _value;

            public JsonContent(JToken value)
            {
                _value = value;
                Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            protected override Task SerializeToStreamAsync(Stream stream,
                TransportContext context)
            {
                var jw = new JsonTextWriter(new StreamWriter(stream))
                {
                    Formatting = Formatting.Indented
                };
                _value.WriteTo(jw);
                jw.Flush();
                return Task.FromResult<object>(null);
            }

            protected override bool TryComputeLength(out long length)
            {
                length = -1;
                return false;
            }
        }
    }
}
