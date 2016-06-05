<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="bigbus.checkout.Default" %>
<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cplhBody" runat="server">
    <div>
        This is a test page for new checkout!<br/>
        Cookie Value:<asp:TextBox runat="server" id="txtCookieValue"></asp:TextBox>
        Click to plant cookie <asp:Button runat="server" Text="Click" OnClick="PlantCookie"/>
        <asp:Button runat="server" Text="Go to checkout" OnClick="GoToCheckout" />
    </div>
</asp:Content>
