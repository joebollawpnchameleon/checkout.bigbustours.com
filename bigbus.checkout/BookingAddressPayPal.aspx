<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingAddressPayPal.aspx.cs" Inherits="bigbus.checkout.BookingAddressPayPal" %>
<%@ Register TagPrefix="NCK" TagName="UserDetails" src="~/Controls/UserDetails.ascx"  %>
<%@ Register TagPrefix="NCK" TagName="BasketDisplay" Src="~/Controls/BasketDisplay.ascx" %>

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
     <div id="dvErrorSummary" runat="server" Visible="False">
        <asp:Literal runat="server" id="ltError"></asp:Literal>
    </div>
     <div  runat="server" id="dvAddressDetails" Visible="True" class="customer-details">
        <NCK:UserDetails id="ucUserDetails" runat="server"/>
    </div>

     <div id="dvActions" runat="server">
         <asp:Button runat="server" text="Complete Ticket Purchase" OnClientClick="TrackUserSubscription();" OnClick="CompletePaypalCheckout" />
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
