using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementation
{
    public class EcrService
    {
        //private readonly Configs.Environment _environment;
        private readonly string _apiKey;
        private readonly EcrWebServiceV2.Api _clientApi;
        private const int MaxInfoLen = 100;

        //private readonly bool _isAgentSession;

        /// <summary>
        /// use this constructor for testing (staging and local)
        /// </summary>
        /// <param name="apiKey"></param>
        public EcrService(string apiKey)
        {
            _apiKey = apiKey;
            _clientApi = new EcrWebServiceV2.Api();
        }

        /// <summary>
        /// Use this constructor for live environment calls
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="liveEndPoint"></param>
        public EcrService(string apiKey, string liveEndPoint)
        {
            _apiKey = apiKey;
            _clientApi = new EcrWebServiceV2.Api { Url = liveEndPoint };
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
                var orderlines = order.GetOrderLines();
                //this assumes that we will only have 1 ticket type either combo or tour but not two at the same time
                var firstTour =
                    orderlines.FirstOrDefault(
                        x => x.TicketTOrA.Equals("Tour", StringComparison.InvariantCultureIgnoreCase) ||
                        x.TicketTOrA.Equals("ComboTicket", StringComparison.InvariantCultureIgnoreCase));

                sbTem.Append("Order line count " + orderlines.Count);

                if (firstTour == null)
                    throw new Exception();

                var availability = _clientApi.Availability(_apiKey, firstTour.Ticket.EcrProductCode, firstTour.TicketDate, true, firstTour.TicketDate, true);

                if (availability == null)
                    throw new Exception();

                sbTem.Append("api url " + _clientApi.Url + " key " + _apiKey);
                sbTem.Append("availability not null ");
                sbTem.Append("availability tours exist " + (availability.TourAvailability != null));
                if (availability.TourAvailability == null)
                    throw new Exception();

                sbTem.Append("availability tours count " + (availability.TourAvailability.Count()));

                var bookingRequest = PrepareBookingRequest(order, availability.TourAvailability[0].AvailabilityHold.Reference, firstTour);

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

        public Traveller[] MakeTravellers(Order order, OrderLines orderLines)
        {
            if (orderLines == null || orderLines.Count < 1)
                return null;

            var travellerIndex = 1;
            var orderLineIndex = 0;
            var allTravellers = new List<Traveller>();

            foreach (OrderLine line in orderLines)
            {
                //this might not be necessary
                var ticketType = line.Ticket.TicketType;
                if (ticketType.Equals("Attraction", StringComparison.InvariantCultureIgnoreCase)
                    || ticketType.Equals("TimedAttraction", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (orderLineIndex == 0)
                {
                    allTravellers.Add(BuildTraveller(
                        line.TicketType.ToUpper(),
                        (order.User == null || string.IsNullOrEmpty(order.User.Title) ? "Title" : order.User.Title),
                         (order.User == null || string.IsNullOrEmpty(order.User.Firstname) ? "First Name " : order.User.Firstname),
                         (order.User == null || string.IsNullOrEmpty(order.User.Lastname) ? "Surname" : order.User.Lastname),
                         line.GrossOrderLineValue,
                         1
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
                        line.GrossOrderLineValue,
                        travellerIndex
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
                                line.GrossOrderLineValue,
                                travellerIndex
                             ));
                        travellerIndex++;
                    }
                }

                orderLineIndex++;
            }

            return allTravellers.ToArray();
        }

        private string TruncateStringToMaxLen(string originalString, int len)
        {
            if (string.IsNullOrEmpty(originalString))
                return string.Empty;

            return originalString.Length <= len ? originalString : originalString.Substring(0, len);
        }

        public BookingRequest PrepareBookingRequest(Order order, string productBookingRef, OrderLine tourLine)
        {
            var orderlines = order.GetOrderLines();

            var contactInfo = new ContactInfo
            {
                ContactName = string.IsNullOrEmpty(order.UserName.Trim()) ? "Lead Traveller" : order.UserName,
                ContactType = "Lead traveller",
                ContactValue = string.IsNullOrEmpty(order.User.FriendlyEmail.Trim()) ? order.eMailAddress : order.User.FriendlyEmail
            };

            var ticket = tourLine.Ticket;
            var tourDescription = ticket.Description.Trim();
            var suppliernote = (ticket.TicketTextTopLine + " " + ticket.TicketTextMiddleLine + " " + ticket.TicketTextBottomLine).Trim();
            var requiredinfo = (ticket.TicketTextLine2 + " " + ticket.TicketTextLine3).Trim();

            var bookingRequest = new BookingRequest
            {
                AgentCode = string.IsNullOrEmpty(order.AgentRef) ? "BigBusToursDotComWebSales" : order.AgentRef,
                Amount = order.Total,
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
                TravelDateSpecified = false,
                BookingReference = productBookingRef,
                ContactInfo = contactInfo,
                Travellers = MakeTravellers(order, orderlines)
            };

            if (!tourLine.FixedDateTicket) return bookingRequest; //review this for opendate tickets. consider using a field that is string for travel date

            bookingRequest.TravelDate = tourLine.TicketDate;
            bookingRequest.TravelDateSpecified = true;

            return bookingRequest;
        }
    }
}
