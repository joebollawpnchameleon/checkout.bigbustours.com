using System.Collections.Generic;
using bigbus.checkout.EcrWServiceRefV3;
using Common.Model.Interfaces;
using System;

namespace bigbus.checkout.Helpers
{
    public class EcrService : IEcrService
    {
        private readonly string _apiKey;
        private readonly string _agentCode;
        private readonly string _agentUiId;
        private readonly Api _clientApi;
        private const int MaxInfoLen = 100;
        public const string ProductListCacheName = "Ecr_Product_List";

        /// <summary>
        /// use this constructor for testing (staging and local)
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="agentCode"></param>
        /// <param name="agentUiId"></param>
        public EcrService(string apiKey, string agentCode, string agentUiId)
        {
            _apiKey = apiKey;
            _agentCode = agentCode;
            _agentUiId = agentUiId;
            _clientApi = new Api();
        }

        /// <summary>
        /// Use this constructor for live environment calls
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="liveEndPoint"></param>
        /// <param name="agentCode"></param>
        /// <param name="agentUiId"></param>
        public EcrService(string apiKey, string liveEndPoint, string agentCode, string agentUiId)
        {
            _apiKey = apiKey;
            _agentCode = agentCode;
            _agentUiId = agentUiId;
            _clientApi = new Api { Url = liveEndPoint };           
        }

        public Product[] GetProductList()
        {            
            var productListRequest = new ProductListRequest {
                AgentCode = _agentCode,
                AgentUID = _agentUiId,
                ApiKey = _apiKey
            };
            var tourlist = _clientApi.ProductList(productListRequest);
            return tourlist.Products;
        }

        public Product[] GetProductList(ICacheProvider cacheProvider)
        {

            var tourList = cacheProvider.GetFromCache<Product[]>(ProductListCacheName);

            if (tourList != null)
                return tourList;

            var productListRequest = new ProductListRequest
            {
                AgentCode = _agentCode,
                AgentUID = _agentUiId,
                ApiKey = _apiKey
            };

            var tourlist = _clientApi.ProductList(productListRequest);

            cacheProvider.AddToCache(ProductListCacheName, tourlist.Products, DateTime.Now.AddMinutes(20));

            return tourlist.Products;
        }

        public AvailabilityResponse GetAvailability(List<AvailabilityTransactionDetail> availabilityTransactionDetails)
        {
            var availabilityRequest = new AvailabilityRequest
            {
                AgentCode = _agentCode,
                AgentUID = _agentUiId,
                ApiKey = _apiKey,
                TransactionDetails = availabilityTransactionDetails.ToArray()
            };

            return _clientApi.AvailabilityCheck(availabilityRequest);
        }

        public BookingResponse SubmitBooking(int ordernumber, AvailabilityResponse availability, List<BookingTransactionDetail> bookingDetails)
        {
            var bookingRequest = new BookingRequest
            {
                AgentUID = _agentUiId,
                AgentCode = _agentCode,
                ApiKey = _apiKey,
                SupplierTranReference = ordernumber.ToString(),
                TransactionDetails = bookingDetails.ToArray(),
                TransactionReference = availability.TransactionReference
            };

            var bookingResponse = _clientApi.Booking(bookingRequest);

            return bookingResponse;
        }
    }
}