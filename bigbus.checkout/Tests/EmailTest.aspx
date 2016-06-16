<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="EmailTest.aspx.cs" Inherits="bigbus.checkout.TestingBeforeLive.EmailTestSend" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCmsToolbarHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cplhBody" runat="server">
    
    <p>
        Send Top Email
        <asp:Button runat="server" Text="Send Top Email" OnClick="SendTopEmailTest"/>
        
        <asp:Label runat="server" id="lbdResult"></asp:Label>
    </p>
    
    <p>
        Test Order ConfirmationEmail <asp:Button runat="server" Text="Create email" OnClick="CreateOrderConfirmationEmailTest"/>
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphFooterScriptAndStylesheets" runat="server">
</asp:Content>
