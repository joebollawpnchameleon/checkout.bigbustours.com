<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingAddressPayPal.aspx.cs" Inherits="bigbus.checkout.BookingAddressPayPal" %>
<%@ Register TagPrefix="NCK" TagName="UserDetails" src="~/Controls/UserDetails.ascx"  %>
<%@ Register TagPrefix="NCK" TagName="BasketDisplay" Src="~/Controls/BasketDisplay.ascx" %>

<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick-theme.css" />
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cplhBody" runat="server">
     <header class="content__header">
        <h1><%= GetTranslation("Booking_Step3") %></h1>
    </header>

    <section class="basket">
        
        <div class="basket__header">
            <p class="basket__total"><span class="title"><%= GetTranslation("Total") + ":" +  TotalSummary %>   </span></p>
            <p><a class="basket__toggle js-toggle-basket" href="#"><%=GetTranslation("ViewBasket")%></a></p>
        </div>
        <NCK:BasketDisplay id="ucBasketDisplay" ShowActionRow="False" runat="server"/>
        
    </section>
    
     <section class="last-step">
        <h2><%=GetTranslation("PaypalLastStepAlmostDone") %></h2>
        <p><%=GetTranslation("PaypalLastStepReviewAlert")%></p>
    </section>

    <section class="contact-details">
        <div class="contact-details__header">
            <p class="contact-details__payment-method">Payment method<img src="/Content/images/Design/paypal-blue.png" alt="PayPal"></p>
         </div>
         
        <div class="form contact-details__form">
         
            <div id="dvErrorSummary" runat="server" Visible="False">
                <asp:Literal runat="server" id="ltError"></asp:Literal>
            </div>

            <NCK:UserDetails id="ucUserDetails" runat="server"/>
            
            <div class="form-footer">
                <p class="form__buttons">
                   <asp:LinkButton ID="btnContinueCheckout"  runat="server" OnClientClick="TrackUserSubscription();"  OnClick="CompletePaypalCheckout" 
                        CssClass="form__continue button button_red button_forward">
                        <span id="spnContinueText" runat="server" class="right"><%= GetTranslation("CompleteMyTicketPurchase") %></span>
                    </asp:LinkButton>
                </p>
                <p class="form__secure"><%=GetTranslation("OurCheckoutIsSecureAndYourDetailsAreProtected")%></p>
                <ul class="form__secure-logos">
                    <li><img src="/Content/images/paymentMethods/worldpay.png" alt="Worldpay" /></li>
                    <li><img src="/Content/images/paymentMethods/safekey.png" alt="SafeKey" /></li>
                    <li><img src="/Content/images/paymentMethods/verified-visa.png" alt="Verified by Visa" /></li>
                    <li><img src="/Content/images/paymentMethods/mastercard-securecode.png" alt="Mastercard Securecode" /></li>
                </ul>
            </div>

        </div>
    </section>
    
    <script type="text/javascript">

        function TrackUserSubscription() {
            var isUserSubscribed = $('#ckSubscribe:checked').length > 0;
            var email = $('#txtEmail').val();

            if (isUserSubscribed) {
                _itq.push([
                    "_trackUserEvent", "subscribe",
                    [
                        { "name": "Customer.Email", "value": email },
                        { "name": "Customer.Subscribed", "value": "true" }
                    ],
                    "Subscribe"
                ]);
            }
        }
      
    </script>
    <script src="/Scripts/local/basket.js"></script>
</asp:Content>
