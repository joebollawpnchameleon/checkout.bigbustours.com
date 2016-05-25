<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.ascx.cs" Inherits="bigbus.checkout.Controls.WebUserControl1" %>
    <p>
        * These fields are mandatory
    </p>

    <div class="formErrors">
        <div>
            <asp:Literal ID="TsAndCsStarLit" runat="server"></asp:Literal><asp:Literal ID="TsAndCsLit" runat="server"></asp:Literal>
        </div>
        <div>
            <asp:ValidationSummary ID="ValidationErrorSummary" ValidationGroup="CreditCardCheckout"  CssClass="ValidationErrorSummaryContent" runat="server" />
        </div>
    </div>
       
        <fieldset class="cf">
            <legend><%= ParentPage.GetTranslation("YourDetails") %></legend>
            <div class="form-row">
                <label for="<%= TitleList.ClientID%>">
                    <%=ParentPage.GetTranslation("Title")%>
                </label>
                <asp:DropDownList ID="TitleList" runat="server"  />
            </div>
            <div class="form-row">
                <label for="<%= txtFirstName.ClientID%>">
                    <%= ParentPage.GetTranslation("FirstName")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtFirstName" ClientIDMode="Static" runat="server" MaxLength="100" />
                <asp:RequiredFieldValidator ValidationGroup="CreditCardCheckout"  CssClass="error" id="rqVFirstName" Display="Dynamic"  runat="server"  ControlToValidate="txtFirstName"/>
            </div>
            <div class="form-row">
                <label for="<%= txtLastName.ClientID%>">
                    <%= ParentPage.GetTranslation("LastName")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtLastName" runat="server" MaxLength="100" />
                 <asp:RequiredFieldValidator CssClass="error" ValidationGroup="CreditCardCheckout"  Display="Dynamic" id="rqVFirstLastName" runat="server"  ControlToValidate="txtLastName"/>
            </div>
            <div class="form-row">
                <label for="<%= txtEmail.ClientID%>">
                    <%= ParentPage.GetTranslation("Email")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="500" />
                <asp:RequiredFieldValidator CssClass="error" ValidationGroup="CreditCardCheckout"  Display="Dynamic"  id="rqVFirstEmail" runat="server"   ControlToValidate="txtEmail"/>
                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationGroup="CreditCardCheckout"  ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail"/>
                <p><%= ParentPage.GetTranslation("WeWillSendYouAConfirmationEmail")%></p>
            </div>
             <div class="form-row">
                <label>
                    &nbsp;
                </label>
                <asp:CheckBox ID="ckSubscribe" runat="server" ClientIDMode="Static" />
                
                <p>
                    <%= "We will send you a confirmation email Please include me in marketing emails" %><!-- translation needed -->
                </p>
            </div>
        </fieldset>
        <fieldset class="cf">
            <legend><%= ParentPage.GetTranslation("YourAddress") %></legend>
            <p><%= ParentPage.GetTranslation("EnterCardRegisteredAddress")%></p>
            <div class="form-row">
                <label for="<%= txtAddress1.ClientID%>">
                    <%= ParentPage.GetTranslation("AddressLine1")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtAddress1" runat="server" MaxLength="255" />
                <asp:RequiredFieldValidator ValidationGroup="CreditCardCheckout" CssClass="error" Display="Dynamic" id="rqVAddress1" runat="server"  ControlToValidate="txtAddress1"/>
            </div>
            <div class="form-row">
                <label for="<%= txtAddress2.ClientID%>"><%= ParentPage.GetTranslation("AddressLine2") %></label>
                <asp:TextBox ID="txtAddress2" runat="server" MaxLength="255" />
            </div>
            <div class="form-row">
                <label for="<%= txtTown.ClientID%>">
                    <%= ParentPage.GetTranslation("Town/City")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtTown" runat="server" MaxLength="100" />
                <asp:RequiredFieldValidator CssClass="error" ValidationGroup="CreditCardCheckout"  Display="Dynamic" id="rqVTown" runat="server"  ControlToValidate="txtTown"/>
            </div>
            <div class="form-row">
                <label for="<%= txtPostCode.ClientID%>">
                    <%= ParentPage.GetTranslation("PostZipCode")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtPostCode" runat="server" MaxLength="100" />
                <asp:RequiredFieldValidator CssClass="error"  ValidationGroup="CreditCardCheckout" Display="Dynamic" id="rqVPostCode" runat="server"  ControlToValidate="txtPostCode"/>
            </div>
            <div class="form-row">
                <label for="<%= txtState.ClientID%>">
                    <%=ParentPage.GetTranslation("State")%>
                </label>
                <asp:TextBox ID="txtState" runat="server" MaxLength="100" />
            </div>
            <div class="form-row">
                <label for="<%= ddlCountryList.ClientID%>">
                    <%=ParentPage.GetTranslation("Country")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:DropDownList Font-Size="12px" ID="ddlCountryList" runat="server" />
                <asp:CustomValidator id="cstVCountry" ValidationGroup="CreditCardCheckout"  runat="server"  OnServerValidate="ValidateCountry" />
            </div>
        </fieldset>
        <fieldset class="cf terms">
            <div class="form-row">
                <asp:CheckBox id="ckTermsAndConditions" CssClass="required" runat="server" />
                <label for="<%=ckTermsAndConditions.ClientID %>">
                    <%= ParentPage.GetTranslation("IhavereadandagreetotheTermsandConditions")%><abbr title="required">*</abbr>
                </label>
                <p>
                    <a href="<%= ConfigurationManager.AppSettings["BaseUrl"] %>terms-and-conditions.html?browser=true" target="_blank" ><%=ParentPage.GetTranslation("TermsAndConditions")%> </a>&nbsp;
                    <a href="<%= ConfigurationManager.AppSettings["BaseUrl"] %>privacy-policy.html?browser=true" target="_blank" ><%=ParentPage.GetTranslation("PrivacyPolicy")%> </a>
                </p>
                
                <asp:CustomValidator id="cstVTerms" ValidationGroup="CreditCardCheckout" runat="server"  OnServerValidate="ValidateTermsAndConditions" />

            </div>
        </fieldset>


