<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingAddress.aspx.cs" Inherits="bigbus.checkout.BookingAddress" %>
<%@ Register TagPrefix="NCK" TagName="UserDetails" src="~/Controls/UserDetails.ascx"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dvErrorSummary" runat="server" Visible="False">
        <asp:Literal runat="server" id="ltError"></asp:Literal>
    </div>
   
    <div  runat="server" id="dvAddressDetails" Visible="True" class="customer-details">
        <NCK:UserDetails id="ucUserDetails" runat="server"/>
    </div>
   
    <div id="dvActions" runat="server">
        <asp:Button runat="server" text="Back" OnClick="ContinueShopping" id="btnCancel"/>
        &nbsp;
        <asp:Button runat="server" text="Checkout With Credit Card" OnClientClick="return TrackUserSubscription();" OnClick="CheckoutWithCreditCard" id="btnContinueCheckout"/>
        &nbsp;
         <asp:Button runat="server" text="Checkout With Paypal" OnClick="CheckoutWithPaypal" />
    </div>
    
    <script type="text/javascript">

        function TrackUserSubscription() {

            var email = $('#txtEmail').val();
            var checkbox = $('#ckSubscribe:checked');

            //intilery tracking
            if (email !== '' && checkbox === true) {
                _itq.push([
                    "_trackUserEvent", "sign in", [
                        { "name": "Customer.Email", "value": email }
                    ], "Sign in"
                ]);
            }
        }

    </script>

</asp:Content>
