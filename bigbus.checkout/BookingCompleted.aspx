<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteMaster.Master" CodeBehind="BookingCompleted.aspx.cs" Inherits="bigbus.checkout.BookingCompleted" %>
<%@ Register TagPrefix="ViatorWidget" TagName="Viator" Src="~/controls/ViatorWidget/Desktop.ascx"  %>
<%@ Register Src="~/controls/CommissionJunctionTag.ascx" TagPrefix="CommissionJunction" TagName="CommissionJunctionTag" %>

<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick-theme.css" />
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick.css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cplhBody" Runat="Server">
<header class="content__header">
    <h1><%=GetTranslation("Booking_Confirmation_Title")%></h1>
    <h2><%= GetTranslation("Booking_Success_Thanks_Header") %> - <span><%=GetTranslation("OrderNumber") %> <asp:Literal ID="ltlOrderNumber" runat="server" /></span></h2>
</header>

<CommissionJunction:CommissionJunctionTag runat="server" id="CommissionJunctionTag" />

    <asp:HiddenField runat="server" id="hdnUserEmail"/>

<section class="ticket-options">
    <ul class="ticket-options__types">
    
        <li class="cf">
            <h3><%=GetTranslation("Booking_PrintTickets")%></h3>
            <div class="ticket-options__description">
                <p><%= GetTranslation("Booking_Success_PDF_Message") %></p>
            </div>
            
            <asp:HyperLink ID="eVoucherLink" runat="server" CssClass="button button_red button_print ticket-options__link" Target="_blank">
                <span class="right"><%= GetTranslation("Booking_ViewandPrint") %></span>
            </asp:HyperLink>
        </li>

        <asp:PlaceHolder ID="plhShowMobile" runat="server">
            <li class="cf">
                <h3><%=GetTranslation("Booking_SendToMobile")%></h3>
                <div class="ticket-options__description form">
                    <p><%=GetTranslation("Booking_Select_Telephone")%></p>
                    <p>
                        <asp:DropDownList ID="DiallingList" runat="server" AutoPostBack="true" />
                        <input id="CountryCode" runat="server" disabled type="text"/>
                        <input id="Mobile" runat="server" type="tel" class="mobileNumber" title="Mobile. This is a required field eg. 07956007121" />
                    </p>
                    <asp:Literal ID="LIMobileError" runat="server"></asp:Literal>
                </div>
                
                <asp:LinkButton ID="SendToMobileLink" runat="server" CssClass="button button_red button_mobile ticket-options__link" OnClick="SendToMobileClick">
                    <span class="right"><%= GetTranslation("Booking_SendMobile")%></span>
                </asp:LinkButton>
               
            </li>
        </asp:PlaceHolder>

        <li class="cf">
        <%
            if(CurrentSite.ViatorWidgetDestinationId != null)
            {
        %>
            <ViatorWidget:Viator runat="server" USCitiesOnly="True" />
        <%
            }
        %>

            <h3><%=this.GetTranslation("StayInTouch") %></h3>

            <div class="ticket-options_description">
                <p>
                    <asp:CheckBox ID="chkReceiveNews" ClientIDMode="Static" runat="server" Checked="false" />&nbsp;<%= this.GetTranslation("Pleasecheckhereifyouwishtoreceive")%>
                </p>
            </div>
        </li>
        <asp:PlaceHolder ID="DisplaySurveyLink" runat="server" Visible="false">
            <li class="cf">
                <h3>Take our 5-minute survey:</h3>
                <div class="ticket-options__description">
                    <p>Thank you for buying from Big Bus Tours today. We really value your feedback, so please take a few moments to tell us about your experience so we can improve the website for our customers.</p>
                </div>
               <asp:HyperLink ID="lnkSurveyMonkey" runat="server" CssClass="button button_red ticket-options__link" Target="_blank" NavigateUrl="https://www.surveymonkey.com/s/JY63RH8?browser=true"><span class="right">Take the survey</span></asp:HyperLink>
            </li>
        </asp:PlaceHolder>
    </ul>
</section>

    <section class="ticket-information">
    <ul class="ticket-information__steps">
        <li>
            <img src="/Content/images/design/print-ticket.png">
            <h4>1. Print your Ticket</h4>
            <p>Click the link above to view, download and print your ticket. It will feature a code which will be checked by a Big Bus Tours staff member when you’re ready to board the bus.</p>
        </li>
        <li>
            <img src="/Content/images/design/download-app.png">
            <h4>2. Download our Free App</h4>
            <p>View bus stop locations, bus timetables, city maps and great things to do in [city name] with our free app – available now from the <a href="">App Store</a> or <a href="">Google Play</a>.</p>
          </li>
        <li>
            <img src="/Content/images/design/enjoy-tour.png">
            <h4>3. Enjoy the tour</h4>
            <p>Show your printed ticket to a member of staff at one of our stops or aboard the bus. They’ll issue you a receipt that you’ll be able to use to hop on the bus throughout the validity period of your ticket.</p>
        </li>
    </ul>
    <div class="ticket-information__sms">
        <div class="image-container"><img src="/Content/images/design/information.png"></div>
        <div class="copy-container">
            <h4>Did you request your tickets to be sent via SMS?</h4>
            <p>If you requested to have your tickets sent via SMS, remember that there is no need to print your tickets, just bring your phone with you with the ticket page open to show it to one of our on street staff who will issue you your ticket.</p>
        </div>
    </div>
    <div class="download-app">
          <div class="copy-container">
            <h1>Download the Big Bus Tours mobile app and enhance your sightseeing experience.</h1>
            <p>With maps and useful information to discover the city</p><img src="/Content/images/design/app-store.png" alt="App Store"><img src="/Content/images/design/google-play.png" alt="Google Play">
          </div>
          <div class="image-container"><img src="/Content/images/design/iphone.png"></div>
        </div>
</section>

<section class="enjoy-trip">
    <h1><%= GetTranslation("Booking_Success_Enjoy_your_trip") %></h1>
    <asp:LinkButton ID="btnFinish" runat="server" CausesValidation="true" CssClass="button button_forward button_red home-link" OnClientClick="TrackSubscription()" OnClick="btnFinish_Click">
        <span><%= GetTranslation("Finish") %></span>
    </asp:LinkButton>
</section>
    

<script type="text/javascript">
    $(function () {
        var showmobile = '<%= ShowMobile %>';

        if (showmobile === 'True') {
            var countrycode = '<%= CountryCode.ClientID%>';

            $(".DiallingList").removeAttr('onchange');
            $(".DiallingList").change(function () {
                $("#" + countrycode).val('+' + $(this).val());
            });
        }
    });

    function TrackSubscription() {
        var isUserSubscribed = $('#chkReceiveNews:checked').length > 0;

        if (isUserSubscribed) {
            _itq.push([
                "_trackUserEvent", "subscribe",
                [
                    { "name": "Customer.Email", "value": "<%=UserEmail%>" },
                    { "name": "Customer.Subscribed", "value": "true" }
                ],
                "Subscribe"
            ]);
        }
    }
</script>
<div class="widthConstrained">
    <Google:Analytics runat="server" />
    <asp:Literal ID="marketingscripts" runat="server"></asp:Literal>
    <asp:HiddenField ID="hdnIsMobileAppSession" runat="server" Value="false" />
</div>

    <%= GetQuovadisScripts() %>
</asp:Content>

