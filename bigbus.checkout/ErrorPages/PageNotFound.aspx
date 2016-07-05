<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="PageNotFound.aspx.cs" Inherits="bigbus.checkout.ErrorPages.PageNotFound" %>
<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cplhBody" runat="server">
    <header class="content__header">
        
    </header>
    
    <section class="basket">
        
        <h1><%=GetTranslation("AnErrorOccured")%></h1>

        <p>The page you are looking for could not be found</p>
        
         <a class="form__continue button button_red button_forward">
            <span id="spnContinueText" runat="server" class="right"><%= GetTranslation("agt_ReturnHomepage") %></span>
        </a>

    </section>

</asp:Content>