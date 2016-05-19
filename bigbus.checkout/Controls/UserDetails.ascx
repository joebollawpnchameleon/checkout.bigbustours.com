<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.ascx.cs" Inherits="bigbus.checkout.Controls.WebUserControl1" %>

    <div class="formErrors">
        <div>
            <asp:Literal ID="TsAndCsStarLit" runat="server"></asp:Literal><asp:Literal ID="TsAndCsLit" runat="server"></asp:Literal>
        </div>
        <div>
            <asp:ValidationSummary ID="ValidationErrorSummary" CssClass="ValidationErrorSummaryContent" runat="server" />
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
                <asp:CustomValidator CssClass="error" Display="Dynamic" ID="FirstnameValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
            <div class="form-row">
                <label for="<%= txtLastName.ClientID%>">
                    <%= ParentPage.GetTranslation("LastName")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtLastName" runat="server" MaxLength="100" />
                <asp:CustomValidator CssClass="error" ID="LastnameValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
            <div class="form-row">
                <label for="<%= txtEmail.ClientID%>">
                    <%= ParentPage.GetTranslation("Email")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="500" />
                <asp:CustomValidator CssClass="error" ID="EmailValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
                <p><%= ParentPage.GetTranslation("WeWillSendYouAConfirmationEmail")%></p>
            </div>
             <div class="form-row">
                <label>
                    &nbsp;
                </label>
                <asp:CheckBox ID="ckSubscribe" runat="server" ClientIDMode="Static" />
                
                <p>
                    <%= "We will send you a confirmation email <br/> Please include me in marketing emails" %><!-- translation needed -->
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
                <asp:CustomValidator CssClass="error" ID="AddressValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
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
                <asp:CustomValidator CssClass="error" ID="TownValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
            <div class="form-row">
                <label for="<%= txtPostCode.ClientID%>">
                    <%= ParentPage.GetTranslation("PostZipCode")%>
                    <abbr title="required">*</abbr>
                </label>
                <asp:TextBox ID="txtPostCode" runat="server" MaxLength="100" />
                <asp:CustomValidator CssClass="error" ID="PostcodeValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
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
                <asp:CustomValidator CssClass="error" ID="CountryValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
        </fieldset>
        <fieldset class="cf terms">
            <div class="form-row">
                <asp:CheckBox id="ckTermsAndConditions" CssClass="required" runat="server" />
                <label for="<%=ckTermsAndConditions.ClientID %>">
                    <%= ParentPage.GetTranslation("IhavereadandagreetotheTermsandConditions")%><abbr title="required">*</abbr>
                </label>
                <p><a href="terms-and-conditions.html?browser=true" target="_blank" ><%=ParentPage.GetTranslation("TermsAndConditions")%> ></a></p>
                <asp:CustomValidator CssClass="error" ID="TAndCValidator" runat="server" OnServerValidate="ValidateRegistration" ValidateEmptyText="True" />
            </div>
        </fieldset>


