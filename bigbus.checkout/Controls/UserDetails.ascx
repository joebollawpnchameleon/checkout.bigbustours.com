﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.ascx.cs" Inherits="bigbus.checkout.Controls.WebUserControl1" %>
    
    <%--<p class="form__mandatory">* <%= ParentPage.GetTranslation("Youmustfillinthefieldsmarkedwithx") %></p>--%>
    
    <div class="form__errors" id="dvErrorSummary" runat="server" Visible="False">
        <div>
            <asp:Literal ID="TsAndCsStarLit" runat="server"></asp:Literal><asp:Literal ID="TsAndCsLit" runat="server"></asp:Literal>
        </div>
        <div>
            <asp:ValidationSummary ID="ValidationErrorSummary" ValidationGroup="CreditCardCheckout"  CssClass="ValidationErrorSummaryContent" runat="server" />
        </div>
    </div>
       
    <fieldset class="cf">
            <h2><%= ParentPage.GetTranslation("YourDetails") %></h2>
            <p>
                <label for="<%= TitleList.ClientID%>">
                    <%=ParentPage.GetTranslation("Title")%>
                </label>
                <asp:DropDownList ID="TitleList" runat="server" CssClass="form__input form__input-wide" />
            </p>
            <p>
                <label for="<%= txtFirstName.ClientID%>">
                    <%= ParentPage.GetTranslation("FirstName")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtFirstName" ClientIDMode="Static" runat="server" CssClass="form__input form__input-wide" MaxLength="100" />
                <asp:RequiredFieldValidator ValidationGroup="CreditCardCheckout"  CssClass="error" id="rqVFirstName" Display="Dynamic"  runat="server"  ControlToValidate="txtFirstName"/>
            </p>
            <p>
                <label for="<%= txtLastName.ClientID%>">
                    <%= ParentPage.GetTranslation("LastName")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtLastName" runat="server" MaxLength="100"  CssClass="form__input form__input-wide"/>
                 <asp:RequiredFieldValidator CssClass="error" ValidationGroup="CreditCardCheckout"  Display="Dynamic" id="rqVFirstLastName" runat="server"  ControlToValidate="txtLastName"/>
            </p>
            <p>
                <label for="<%= txtEmail.ClientID%>">
                    <%= ParentPage.GetTranslation("Email")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="500" CssClass="form__input form__input-wide" />
                <asp:RequiredFieldValidator CssClass="error" ValidationGroup="CreditCardCheckout"  Display="Dynamic"  id="rqVFirstEmail" runat="server"   ControlToValidate="txtEmail"/>
                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationGroup="CreditCardCheckout" CssClass="error"  ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail"/>
                <span class="form__information"><%= ParentPage.GetTranslation("WeWillSendYouAConfirmationEmail")%></span>
            </p>
             <p  class="form__checkbox">
                 <asp:CheckBox ID="ckSubscribe" runat="server" ClientIDMode="Static" />
                <label>
                   <%= ParentPage.GetTranslation("PleaseIncludeOnMarketingEmails") %>
                </label>
               
            </p>
             <div class="form-row">
                <label for="<%= txtAddress2.ClientID%>"><%= ParentPage.GetTranslation("ExpectedTourDate") %></label>
                <asp:TextBox CssClass="form__input form__input-wide form__input-date hasDatepicker" ID="txtExpectedTourDate" placeholder="dd/mm/yy" ClientIDMode="Static" runat="server" MaxLength="12" />
            </div>
        </fieldset>

    <fieldset class="cf">
            <h2><%= ParentPage.GetTranslation("YourAddress") %></h2>
            <p><%= ParentPage.GetTranslation("EnterCardRegisteredAddress")%></p>
            <p>
                <label for="<%= txtAddress1.ClientID%>">
                    <%= ParentPage.GetTranslation("AddressLine1")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtAddress1" runat="server" MaxLength="255" CssClass="form__input form__input-wide" />
                <asp:RequiredFieldValidator ValidationGroup="CreditCardCheckout" CssClass="error" Display="Dynamic" id="rqVAddress1" runat="server"  ControlToValidate="txtAddress1"/>
            </p>
            <p>
                <label for="<%= txtAddress2.ClientID%>"><%= ParentPage.GetTranslation("AddressLine2") %></label>
                <asp:TextBox ID="txtAddress2" runat="server" MaxLength="255"  CssClass="form__input form__input-wide" />
            </p>
            <p>
                <label for="<%= txtTown.ClientID%>">
                    <%= ParentPage.GetTranslation("Town/City")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtTown" runat="server" MaxLength="100"  CssClass="form__input form__input-wide"/>
                <asp:RequiredFieldValidator CssClass="error" ValidationGroup="CreditCardCheckout"  Display="Dynamic" id="rqVTown" runat="server"  ControlToValidate="txtTown"/>
            </p>
            <p>
                <label for="<%= txtPostCode.ClientID%>">
                    <%= ParentPage.GetTranslation("PostZipCode")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtPostCode" runat="server" MaxLength="100" CssClass="form__input form__input-wide" />
                <asp:RequiredFieldValidator CssClass="error"  ValidationGroup="CreditCardCheckout" Display="Dynamic" id="rqVPostCode" runat="server"  ControlToValidate="txtPostCode"/>
            </p>
            <p>
                <label for="<%= txtState.ClientID%>">
                    <%=ParentPage.GetTranslation("State")%>
                </label>
                <asp:TextBox ID="txtState" runat="server" MaxLength="100"  CssClass="form__input form__input-wide"/>
            </p>
            <p>
                <label for="<%= ddlCountryList.ClientID%>">
                    <%=ParentPage.GetTranslation("Country")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:DropDownList Font-Size="12px" ID="ddlCountryList" runat="server" />
                <asp:CustomValidator id="cstVCountry" ValidationGroup="CreditCardCheckout"  runat="server"  OnServerValidate="ValidateCountry" />
            </p>
        </fieldset>

    <fieldset class="form__terms">
            <p class="form__checkbox">
                <asp:CheckBox id="ckTermsAndConditions" CssClass="required" runat="server" />
                <label for="<%=ckTermsAndConditions.ClientID %>">
                    <%= TermsAndPrivacyLinks %><abbr title="required"> *</abbr>
                </label>
                
                <asp:CustomValidator id="cstVTerms" ValidationGroup="CreditCardCheckout" runat="server"  OnServerValidate="ValidateTermsAndConditions" />

            </p>
        </fieldset>


