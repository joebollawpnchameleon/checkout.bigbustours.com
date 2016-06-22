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
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Common.Enums;
using Common.Model;
using MVC = System.Web.Mvc;
using Http = System.Web.Http;

namespace bigbus.checkout.Controllers
{
    public class MagentoTestController : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        
        public static List<BornBasketItem> TestBasketItems = new List<BornBasketItem>
        {
            new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "A8A795B7-5F8F-42A8-990C-27630F12AF78", Quantity = 2, Sku = "15721", TicketType = TicketVariation.Adult, UnitCost = (decimal) 24.50, Total = 49 },
            new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            new BornBasketItem{ProductName = "Deep Sea Fishing (Exclusive) Per Hour", Discount = 0, Microsite = "dubai", ProductDimensionUid = "D5A4BFF0-09DB-44DC-9DF1-C026347F8CA9", Quantity = 2, Sku = "11173", TicketType = TicketVariation.Adult, UnitCost = (decimal)37.65, Total = (decimal)75.3 },
            new BornBasketItem{ProductName = "Dim Sum Lunch (For Two)", Discount =(decimal) 4.5, Microsite = "hongkong", ProductDimensionUid = "07436026-C5A0-4B38-BAAE-7A35844EECA8", Quantity = 1, Sku = "13198", TicketType = TicketVariation.Adult, UnitCost = (decimal)23.40, Total = (decimal)46.80 }//,
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
            //new BornBasketItem{ProductName = "360 Chicago General Admission", Discount = 0, Microsite = "chicago", ProductDimensionUid = "4223FD0E-7EA6-4239-BEF7-B8E24D903757", Quantity = 1, Sku = "15721", TicketType = TicketVariation.Child, UnitCost = 17, Total = 34 },
        };

        [Http.HttpGet]
        [Http.Route("Api/MagentoTest/ProductList/")]
        public HttpResponseMessage ProductList()
        {
            return Request.CreateResponse(HttpStatusCode.OK, TestBasketItems);
        }

        [Http.HttpGet]
        [Http.Route("Api/MagentoTest/SelectProducts/")]
        public HttpResponseMessage SelectProducts(string id)
        {
            var indexArray = id.Split('-');
            var lst = indexArray.Select(index => TestBasketItems[Convert.ToInt32(index)]).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, lst);
        }

        // GET api/<controller>/5
        [Http.HttpGet]
        [Http.Route("Api/MagentoTest/DumCart/")]
        public HttpResponseMessage DumCart(string id)
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

            @"{
            ""items"":[
             {""name"":""360 Chicago General Admission"",""sku"":""15721"",""ProductDimensionUID"":""4ABCEC20-DF14-4942-BD74-820271AEDDA9"",""qty"":2,""price"":29.99,""discount"":0,""total"":59.98,""city"":""chicago"",""type"":""adult""},            
            {""name"":""Art Institute General Admission"",""sku"":""15722"",""ProductDimensionUID"":""76EB57C6-DE0D-455F-9FBE-342D87ADEF6D"",""qty"":1,""price"":23.59,""discount"":3.5,""total"":20.09,""city"":""chicago"",""type"":""adult""}],
            ""subtotal"":83.57,""discount"":3.5,""total"":80.07,""coupon"":""TEST-COUPON1"",""currency"":""EUR"",""language"":""eng""
            }";

            //            @"{
            //            ""items"":[
            //             {""name"":""London 24 hour"",""sku"":""1348"",""ProductDimensionUID"":""c4bf36f4-acf4-4da8-b8ca-f4f9ce9345a0"",""qty"":2,""price"":29.99,""discount"":0,""total"":59.98,""city"":""London"",""type"":""adult""},
            //            {""name"":""24 Hour Day Tour"",""sku"":""1503"",""ProductDimensionUID"":""2f83ee23-b357-4f2d-b52c-bf8e32904381"",""qty"":1,""price"":21.00,""discount"":1,""total"":20.00,""city"":""abudhabi"",""type"":""Child""},            
            //            {""name"":""Madame Tusseau"",""sku"":""32033"",""ProductDimensionUID"":""E5CD49D7-3575-464F-AE3F-A093C83261BB"",""qty"":1,""price"":23.59,""discount"":3.5,""total"":20.09,""city"":""London"",""type"":""adult""}],
            //            ""subtotal"":104.57,""discount"":4.5,""total"":100.07,""coupon"":""TEST-COUPON1"",""currency"":""GBP"",""language"":""eng""
            //            }";

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
