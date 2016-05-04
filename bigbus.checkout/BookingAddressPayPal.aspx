<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingAddressPayPal.aspx.cs" Inherits="bigbus.checkout.BookingAddressPayPal" %>
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
         <asp:Button runat="server" text="Complete Ticket Purchase" OnClick="CompletePaypalCheckout" />
    </div>

</asp:Content>
