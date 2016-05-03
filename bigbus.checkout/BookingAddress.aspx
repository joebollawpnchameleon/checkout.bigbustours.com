<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingAddress.aspx.cs" Inherits="bigbus.checkout.BookingAddress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="dvErrorSummary" runat="server" Visible="False">
        <asp:Literal runat="server" id="ltError"></asp:Literal>
    </div>
   
    <div  runat="server" id="dvAddressDetails" Visible="True" class="customer-details">
        
        <fieldset class="cf">
            <legend><%= GetTranslation("YourDetails") %></legend>
            <div class="form-row">
                <label for="<%= TitleList.ClientID%>">
                    <%=GetTranslation("Title")%>
                </label>
                <asp:DropDownList ID="TitleList" runat="server"  />
            </div>
            <div class="form-row">
                <label for="<%= RegisterFirstNameTextbox.ClientID%>">
                    <%= GetTranslation("FirstName")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="RegisterFirstNameTextbox" ClientIDMode="Static" runat="server" MaxLength="100" />
                <asp:CustomValidator CssClass="error" Display="Dynamic" ID="FirstnameValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
            <div class="form-row">
                <label for="<%= RegisterLastnameTextbox.ClientID%>">
                    <%= GetTranslation("LastName")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="RegisterLastnameTextbox" runat="server" MaxLength="100" />
                <asp:CustomValidator CssClass="error" ID="LastnameValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
            <div class="form-row">
                <label for="<%= RegisterEmailTextbox.ClientID%>">
                    <%= GetTranslation("Email")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="RegisterEmailTextbox" runat="server" MaxLength="500" />
                <asp:CustomValidator CssClass="error" ID="EmailValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
                <p><%= GetTranslation("WeWillSendYouAConfirmationEmail")%></p>
            </div>
        </fieldset>
        <fieldset class="cf">
            <legend><%= GetTranslation("YourAddress") %></legend>
            <p><%= GetTranslation("EnterCardRegisteredAddress")%></p>
            <div class="form-row">
                <label for="<%= AddressLine1Textbox.ClientID%>">
                    <%= GetTranslation("AddressLine1")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="AddressLine1Textbox" runat="server" MaxLength="255" />
                <asp:CustomValidator CssClass="error" ID="AddressValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
            <div class="form-row">
                <label for="<%= AddressLine2Textbox.ClientID%>"><%= GetTranslation("AddressLine2") %></label>
                <asp:TextBox ID="AddressLine2Textbox" runat="server" MaxLength="255" />
            </div>
            <div class="form-row">
                <label for="<%= TownTextbox.ClientID%>">
                    <%= GetTranslation("Town/City")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="TownTextbox" runat="server" MaxLength="100" />
                <asp:CustomValidator CssClass="error" ID="TownValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
            <div class="form-row">
                <label for="<%= PostCodeTextbox.ClientID%>">
                    <%= GetTranslation("PostZipCode")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="PostCodeTextbox" runat="server" MaxLength="100" />
                <asp:CustomValidator CssClass="error" ID="PostcodeValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
            <div class="form-row">
                <label for="<%= StateTextbox.ClientID%>">
                    <%=GetTranslation("State")%>
                </label>
                <asp:TextBox ID="StateTextbox" runat="server" MaxLength="100" />
            </div>
            <div class="form-row">
                <label for="<%= CountryList.ClientID%>">
                    <%=GetTranslation("Country")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:DropDownList Font-Size="12px" ID="CountryList" runat="server" />
                <asp:CustomValidator CssClass="error" ID="CountryValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
        </fieldset>
        <fieldset class="cf terms">
            <div class="form-row">
                <asp:CheckBox id="AgreeTermsAndConditions" CssClass="required" runat="server" />
                <label for="<%=AgreeTermsAndConditions.ClientID %>">
                    <%= GetTranslation("IhavereadandagreetotheTermsandConditions")%><abbr title="required">*</abbr>
                </label>
                <p><a href="terms-and-conditions.html?browser=true" target="_blank" ><%=GetTranslation("TermsAndConditions")%> ></a></p>
                <asp:CustomValidator CssClass="error" ID="TAndCValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
        </fieldset>
    </div>
    <div class="formErrors">
        <div>
            <asp:Literal ID="TsAndCsStarLit" runat="server"></asp:Literal><asp:Literal ID="TsAndCsLit" runat="server"></asp:Literal>
        </div>
        <div>
            <asp:ValidationSummary ID="ValidationErrorSummary" CssClass="ValidationErrorSummaryContent" runat="server" />
        </div>
    </div>
    <div id="dvActions" runat="server">
        <asp:Button runat="server" text="Back" OnClick="ContinueShopping" id="btnCancel"/>
        &nbsp;
        <asp:Button runat="server" text="Checkout With Credit Card" OnClick="CheckoutWithCreditCard" id="btnContinueCheckout"/>
        &nbsp;
         <asp:Button runat="server" text="Checkout With Paypal" OnClick="CheckoutWithPaypal" />
    </div>
</asp:Content>
