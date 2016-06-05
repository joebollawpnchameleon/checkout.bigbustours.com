<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingAddress.aspx.cs" Inherits="bigbus.checkout.BookingAddress" %>
<%@ Register TagPrefix="NCK" TagName="UserDetails" src="~/Controls/UserDetails.ascx"  %>
<%@ Register TagPrefix="NCK" TagName="BasketDisplay" src="~/Controls/BasketDisplay.ascx"  %>

<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick-theme.css" />
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cplhBody" runat="server">
    <header class="content__header">
        <h1><%= GetTranslation("Booking_Step3") %></h1>
    </header>

    <section class="basket">
        <div id="basket__header">
            <p><%= GetTranslation("Total") + ":" + string.Format("<span class=\"red\">{0}</span>", TotalSummary) %>   </p>
            <p><a class="basket__toggle js-toggle-basket" href="#"><%=GetTranslation("ViewBasket")%></a></p>
        </div>
        <NCK:BasketDisplay id="ucBasketDisplay" runat="server"/>   
    </section>

    <div id="dvPayPalAction">
        <asp:Button runat="server" text="Checkout With Paypal" OnClick="CheckoutWithPaypal" /> Or fill in your details below to pay by credit/debit card
    </div>

    <div id="dvErrorSummary" runat="server" Visible="False">
        <asp:Literal runat="server" id="ltError"></asp:Literal>
    </div>
   
    <div  runat="server" id="dvAddressDetails" Visible="True" class="customer-details">
        <NCK:UserDetails id="ucUserDetails" runat="server"/>
    </div>
   
    <div id="dvActions" runat="server">
        <asp:Button runat="server" text="Back" OnClick="ContinueShopping" id="btnCancel"/>
        &nbsp;
        <asp:Button runat="server" ValidationGroup="CreditCardCheckout" text="Checkout With Credit Card" OnClientClick="TrackUserSubscription();" OnClick="CheckoutWithCreditCard" id="btnContinueCheckout"/>
        &nbsp;
         
    </div>
    
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

</asp:Content>
