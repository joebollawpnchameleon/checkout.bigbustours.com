using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Common.Model
{
    public class BornBasket
    {
        [JsonProperty("subtotal")]
        public decimal SubTotal { get; set; }

        [JsonProperty("discount")]
        public decimal Discount { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("coupon")]
        public string Coupon { get; set; }

        [JsonProperty("currency")]
        public string CurrencyCode { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("store")]
        public string LastMicroSite { get; set; }

        [JsonProperty("items")]
        public List<BornBasketItem> BasketItems { get; set; }

        #region JsonIgnoredProperties
        
        [JsonIgnore]
        public Guid CurrencyId { get; set; }

        [JsonIgnore]
        public bool IsValid { get; set; }

        [JsonIgnore]
        public string ExternalCookieValue { get; set; }

        #endregion

    }


}
