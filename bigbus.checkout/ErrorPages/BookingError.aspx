<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingError.aspx.cs" Inherits="bigbus.checkout.ErrorPages.BookingError" %>
<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cplhBody" runat="server">
    <header class="content__header">
        <h1><%=GetTranslation("AnErrorOccured")%></h1>
    </header>
    
    <section class="basket">
        <p><%= GetTranslation("CouldNotPlaceOrderPaymentError") %></p>
        
         <a class="form__continue button button_red button_forward">
            <span id="spnContinueText" runat="server" class="right"><%= GetTranslation("Button_ReturnToBasket") %></span>
        </a>
    </section>

</asp:Content>
