<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingFailure.aspx.cs" Inherits="bigbus.checkout.ErrorPages.BookingFailure" %>
<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cplhBody" runat="server">
    <header class="content__header">
        
    </header>
    
    <section class="basket">
        
        <h1><%=GetTranslation("AnErrorOccured")%></h1>

        <p><%= GetTranslation("CouldNotPlaceOrderPaymentError") %></p>
        
         <a class="form__continue button button_red button_forward">
            <span id="spnContinueText" runat="server" class="right"><%= GetTranslation("Button_ReturnToBasket") %></span>
        </a>

    </section>

</asp:Content>
