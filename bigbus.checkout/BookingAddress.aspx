<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingAddress.aspx.cs" Inherits="bigbus.checkout.BookingAddress" %>
<%@ Register TagPrefix="NCK" TagName="UserDetails" src="~/Controls/UserDetails.ascx"  %>
<%@ Register TagPrefix="NCK" TagName="BasketDisplay" src="~/Controls/BasketDisplay.ascx"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dvBasketSummary">
        <%= GetTranslation("Total") + ":" + string.Format("<span class=\"red\">{0}</span>", TotalSummary) %>     &nbsp;
        <span class="action"><%= GetTranslation("View_Basket") %></span>
        <hr/>
         <NCK:BasketDisplay id="ucBasketDisplay" runat="server"/>
         <hr/>
    </div>

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
