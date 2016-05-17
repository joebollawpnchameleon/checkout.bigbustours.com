using System;
using Common.Enums;
using Newtonsoft.Json;

namespace Common.Model
{
    public class BornBasketItem
    {
        [JsonProperty("name")]
        public string ProductName { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("ProductDimensionUID")]
        public string ProductDimensionUid { get; set; }

        [JsonProperty("qty")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public decimal UnitCost { get; set; }

        [JsonProperty("discount")]
        public decimal Discount { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("city")]
        public string Microsite { get; set; }

        [JsonProperty("type")]
        public TicketVariation TicketType { get; set; }

        [JsonIgnore]
        public Guid TicketId { get; set; }
    }
}
