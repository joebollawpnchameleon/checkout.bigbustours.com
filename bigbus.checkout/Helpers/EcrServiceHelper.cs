using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using bigbus.checkout.data.Model;
using bigbus.checkout.EcrServiceRef;
using Services.Infrastructure;

namespace bigbus.checkout.Helpers
{
    public class EcrServiceHelper
    {
        //private readonly Configs.Environment _environment;
        private readonly string _apiKey;
        private readonly EcrServiceRef.Api _clientApi;
        private const int MaxInfoLen = 100;
        private readonly ITicketService _ticketService;
        private readonly ISiteService _siteService;

        //private readonly bool _isAgentSession;

        /// <summary>
        /// use this constructor for testing (staging and local)
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="ticketService"></param>
        /// <param name="micrositeService"></param>
        public EcrServiceHelper(string apiKey, ITicketService ticketService, ISiteService micrositeService)
        {
            _apiKey = apiKey;
            _clientApi = new EcrServiceRef.Api();
            _ticketService = ticketService;
            _siteService = micrositeService;
        }

        /// <summary>
        /// Use this constructor for live environment calls
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="liveEndPoint"></param>
        /// <param name="ticketService"></param>
        /// <param name="micrositeService"></param>
        public EcrServiceHelper(string apiKey, string liveEndPoint, ITicketService ticketService, ISiteService micrositeService)
        {
            _apiKey = apiKey;
            _clientApi = new EcrServiceRef.Api { Url = liveEndPoint };
            _ticketService = ticketService;
            _siteService = micrositeService;
        }

        public virtual bool IsSupplierCodeValid(string productCode)
        {
            try
            {
                var availability = _clientApi.Availability(_apiKey, productCode, DateTime.Now, true, DateTime.Now, true);
                return (availability.RequestStatus.Status == 0);//means success, 1 means error or ticket not found
            }
            catch
            {
                return false;
            }
        }

        public Tour[] GetTourList()
        {
            try
            {
                return _clientApi.TourList(_apiKey).Tour;
            }
            catch
            {
                return null;
            }
        }

        public BookingResponse SubmitBooking(Order order)
        {
            var sbTem = new StringBuilder();

            try
            {
                var orderlines = order.OrderLines;

                //this assumes that we will only have 1 ticket type either combo or tour but not two at the same time
                var firstLine =
                    orderlines.FirstOrDefault(x => x.IsTour);

                sbTem.Append("Order line count " + orderlines.Count);

                if (firstLine == null)
                    throw new Exception();

                var ticket = _ticketService.GetTicketById(firstLine.TicketId.ToString());
                var microsite = _siteService.GetMicroSiteById(firstLine.MicrositeId);

                firstLine.Ticket = ticket;
                firstLine.MicroSite = microsite;

                var ticketDateValue = firstLine.TicketDate ?? DateTime.Now;

                var availability = _clientApi.Availability(_apiKey, ticket.EcrProductCode, ticketDateValue, true, ticketDateValue, true);

                if (availability == null)
                    throw new Exception();

                sbTem.Append("api url " + _clientApi.Url + " key " + _apiKey);
                sbTem.Append("availability not null ");
                sbTem.Append("availability tours exist " + (availability.TourAvailability != null));
                if (availability.TourAvailability == null)
                    throw new Exception();

                sbTem.Append("availability tours count " + (availability.TourAvailability.Count()));

                var bookingRequest = PrepareBookingRequest(order, availability.TourAvailability[0].AvailabilityHold.Reference, firstLine);

                return _clientApi.Booking(_apiKey, bookingRequest);
            }
            catch
            {
                return new BookingResponse
                {
                    TransactionStatus = new TransactiontStatus { Description = sbTem.ToString(), Status = 1 }
                };
            }
        }


        public Traveller BuildTraveller(string ageBand, string title, string firstName, string surName, decimal price, int identifier)
        {
            return new Traveller
            {
                AgeBand = ageBand,
                GivenName = firstName,
                Surname = surName,
                LeadTraveller = false,
                LeadTravellerSpecified = true,
                Price = price,
                PriceSpecified = true,
                TravellerIdentifier = identifier,
                TravellerIdentifierSpecified = true,
                TravellerTitle = title
            };
        }

        public Traveller[] MakeTravellers(Order order)
        {
            var orderLines = order.OrderLines;

            if (orderLines == null || orderLines.Count < 1)
                return null;

            var travellerIndex = 1;
            var orderLineIndex = 0;
            var allTravellers = new List<Traveller>();

            foreach (var line in orderLines)
            { 
                var grossLineValue = line.GrossOrderLineValue ?? (decimal) 0.0;

                if (orderLineIndex == 0)
                {
                    allTravellers.Add(BuildTraveller(
                        line.TicketType.ToUpper(),
                        (order.User == null || string.IsNullOrEmpty(order.User.Title) ? "Title" : order.User.Title),
                         (order.User == null || string.IsNullOrEmpty(order.User.Firstname) ? "First Name " : order.User.Firstname),
                         (order.User == null || string.IsNullOrEmpty(order.User.Lastname) ? "Surname" : order.User.Lastname),
                         grossLineValue, 1
                      ));
                    travellerIndex++;
                }

                if (line.TicketQuantity == 1 && orderLineIndex > 0)
                {
                    allTravellers.Add(BuildTraveller(
                        line.TicketType.ToUpper(),
                        string.Format("Traveller #{0} Title", travellerIndex),
                        string.Format("Traveller #{0} Firstname", travellerIndex),
                        string.Format("Traveller #{0} Surname", travellerIndex),
                        grossLineValue, travellerIndex
                     ));
                    travellerIndex++;
                }
                else if (line.TicketQuantity > 1)
                {
                    var lineQuantity = orderLineIndex == 0 ? line.TicketQuantity - 1 : line.TicketQuantity;

                    for (var x = 0; x < lineQuantity; x++)
                    {
                        allTravellers.Add(
                            BuildTraveller(
                                line.TicketType.ToUpper(),
                                string.Format("Traveller #{0} Title", travellerIndex),
                                string.Format("Traveller #{0} Firstname", travellerIndex),
                                string.Format("Traveller #{0} Surname", travellerIndex),
                                grossLineValue, travellerIndex
                             ));
                        travellerIndex++;
                    }
                }

                orderLineIndex++;
            }

            return allTravellers.ToArray();
        }

        private static string TruncateStringToMaxLen(string originalString, int len)
        {
            if (string.IsNullOrEmpty(originalString))
                return string.Empty;

            return originalString.Length <= len ? originalString : originalString.Substring(0, len);
        }

        public BookingRequest PrepareBookingRequest(Order order, string productBookingRef, OrderLine tourLine)
        {
            var orderlines = order.OrderLines;

            var contactInfo = new ContactInfo
            {
                ContactName = string.IsNullOrEmpty(order.UserName.Trim()) ? "Lead Traveller" : order.UserName,
                ContactType = "Lead traveller",
                ContactValue = string.IsNullOrEmpty(order.User.FriendlyEmail.Trim()) ? order.EmailAddress : order.User.FriendlyEmail
            };

            var ticket = tourLine.Ticket;
            var tourDescription = ticket.Description.Trim();
            var suppliernote = (ticket.TicketTextTopLine + " " + ticket.TicketTextMiddleLine + " " + ticket.TicketTextBottomLine).Trim();
            var requiredinfo = (ticket.TicketTextLine2 + " " + ticket.TicketTextLine3).Trim();
            var orderTotal = order.Total ?? (decimal) 0.0;

            var bookingRequest = new BookingRequest
            {
                AgentCode = string.IsNullOrEmpty(order.AgentRef) ? "BigBusToursDotComWebSales" : order.AgentRef,
                Amount = orderTotal,
                AmountSpecified = true,
                CurrencyCode = order.Currency.ISOCode,
                Location = tourLine.MicroSite.Name,
                PickupPoint = tourLine.MicroSite.Name,
                SpecialRequirement = "None.",
                SupplierNote = string.IsNullOrEmpty(suppliernote) ? "None provided" : TruncateStringToMaxLen(suppliernote, MaxInfoLen),
                TourDescription = string.IsNullOrEmpty(tourDescription) ? "None provided" : TruncateStringToMaxLen(tourDescription, MaxInfoLen),
                SupplierProductCode = tourLine.Ticket.EcrProductCode,
                RequiredInfo = string.IsNullOrEmpty(requiredinfo) ? "None provided " : TruncateStringToMaxLen(requiredinfo, MaxInfoLen),
                TourDuration = tourLine.Ticket.EcrProductDuration,
                TravelDateSpecified = true,
                BookingReference = productBookingRef,
                ContactInfo = contactInfo,
                Travellers = MakeTravellers(order),
                TravelDate = tourLine.TicketDate ?? DateTime.Now
            };
            
            return bookingRequest;
        }
    }
}