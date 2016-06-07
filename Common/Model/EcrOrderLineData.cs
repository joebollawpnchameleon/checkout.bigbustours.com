
using System;

namespace Common.Model
{
    public class EcrOrderLineData
    {
        public string MicrositeId { get; set; }

        public string OrderLineId { get; set; }

        public int NewCheckoutVersionId { get; set; }

        public string NewCheckoutEcrProductCode { get; set; }

        public bool UseQrCode { get; set; }
    }
}
