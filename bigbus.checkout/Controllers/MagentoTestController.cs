using System;
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

namespace bigbus.checkout.Controllers
{
    public class MagentoTestController : ApiController
    {
        public HttpResponseMessage Get(string id)
        {
            var jsonString = @"{
            ""items"":[{""name"":""London 24 hour"",""sku"":""01011"",""qty"":2,""price"":29.99,""discount"":0,""total"":59.98,""city"":""London"",""type"":""adult""}
            ,{""name"":""London 24 hour"",""sku"":""01011"",""qty"":1,""price"":25.99,""discount"":5,""total"":20.99,""city"":""London"",""type"":""child""}],
            ""subtotal"":85.97,""discount"":5,""total"":80.97,""coupon"":""TEST-COUPON1"",""currency"":""GBP"",""language"":""eng""
            }";
            //

            JToken json = JObject.Parse(jsonString);

            return new HttpResponseMessage()
            {
                Content = new JsonContent(json)
            };
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
