<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteMaster.Master" CodeBehind="BookingCompleted.aspx.cs" Inherits="bigbus.checkout.BookingCompleted" %>
<%@ Register TagPrefix="ViatorWidget" TagName="Viator" Src="~/controls/ViatorWidget/Desktop.ascx"  %>
<%@ Register Src="~/controls/CommissionJunctionTag.ascx" TagPrefix="CommissionJunction" TagName="CommissionJunctionTag" %>

<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick-theme.css" />
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick.css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cplhBody" Runat="Server">
<div class="quovadis-wrapper">
    <%= GetQuovadisImage() %>
    <h2><%=GetTranslation("Booking_Confirmation_Title")%></h2>

</div>

<CommissionJunction:CommissionJunctionTag runat="server" id="CommissionJunctionTag" />
    <asp:HiddenField runat="server" id="hdnUserEmail"/>
<div class="progress cf">
    <div class="progress-info">
        <p class="information"><%= GetTranslation("Booking_Success_Thanks_Header") %> - <%=GetTranslation("OrderNumber") %> <asp:Literal ID="ltlOrderNumber" runat="server" /></p>
    </div>
</div>
<div class="ticket-confirmation-options">
    <ul>
        <li class="cf">
            <h3 class="print"><%=GetTranslation("Booking_PrintTickets")%></h3>
            <div class="content">
                <p><%= GetTranslation("Booking_Success_PDF_Message") %></p>
            </div>
            <div class="cta-container">
                <asp:HyperLink ID="eVoucherLink" runat="server" CssClass="cta" Target="_blank"><span class="right"><%= GetTranslation("Booking_ViewandPrint") %></span></asp:HyperLink>
            </div>
        </li>
        <asp:PlaceHolder ID="plhShowMobile" runat="server">
            <li class="cf">
                <h3 class="mobile"><%=GetTranslation("Booking_SendToMobile")%></h3>
                <div class="content">
                    <p><%=GetTranslation("Booking_Select_Telephone")%></p>
                    <asp:DropDownList ID="DiallingList" runat="server" AutoPostBack="true" />
                    <input id="CountryCode" runat="server" disabled type="text"/>
                    <input id="Mobile" runat="server" type="tel" class="mobileNumber" title="Mobile. This is a required field eg. 07956007121" />
                </div>
                <div class="cta-container">
                    <asp:Literal ID="LIMobileError" runat="server"></asp:Literal>
                    <asp:LinkButton ID="SendToMobileLink" runat="server" CssClass="cta" OnClick="SendToMobileClick"><span class="right"><%= GetTranslation("Booking_SendMobile")%></span></asp:LinkButton>
                </div>
            </li>
        </asp:PlaceHolder>
        <li class="cf">
        <%
            if(CurrentSite.ViatorWidgetDestinationId != null)
            {
        %>
            <ViatorWidget:Viator runat="server" USCitiesOnly="True" />
        <%
            }
        %>
            <h3><%=this.GetTranslation("StayInTouch") %></h3>
            <div class="content">
                <p><asp:CheckBox ID="chkReceiveNews" ClientIDMode="Static" runat="server" Checked="false" />&nbsp;<%= this.GetTranslation("Pleasecheckhereifyouwishtoreceive")%></p>
                <h3><%= GetTranslation("Booking_Success_Enjoy_your_trip") %></h3>
            </div>
            <div class="cta-container">
                <asp:LinkButton ID="btnFinish" runat="server" CausesValidation="true" CssClass="cta" OnClientClick="TrackSubscription()" OnClick="btnFinish_Click"><span class="right"><%= GetTranslation("Finish") %></span></asp:LinkButton>
            </div>
        </li>
        <asp:PlaceHolder ID="DisplaySurveyLink" runat="server" Visible="false">
            <li class="cf">
                <h3>Take our 5-minute survey:</h3>
                <div class="content">
                    <p>Thank you for buying from Big Bus Tours today. We really value your feedback, so please take a few moments to tell us about your experience so we can improve the website for our customers.</p>
                </div>
                <div class="cta-container">
                    <asp:HyperLink ID="lnkSurveyMonkey" runat="server" CssClass="cta" Target="_blank" NavigateUrl="https://www.surveymonkey.com/s/JY63RH8?browser=true"><span class="right">Take the survey</span></asp:HyperLink>
                </div>
            </li>
        </asp:PlaceHolder>
    </ul>
</div>
<script type="text/javascript">
    $(function () {
        var showmobile = '<%= ShowMobile %>';

        if (showmobile === 'True') {
            var countrycode = '<%= CountryCode.ClientID%>';

            $(".DiallingList").removeAttr('onchange');
            $(".DiallingList").change(function () {
                $("#" + countrycode).val('+' + $(this).val());
            });
        }
    });

    function TrackSubscription() {
        var isUserSubscribed = $('#chkReceiveNews:checked').length > 0;

        if (isUserSubscribed) {
            _itq.push([
                "_trackUserEvent", "subscribe",
                [
                    { "name": "Customer.Email", "value": "<%=UserEmail%>" },
                    { "name": "Customer.Subscribed", "value": "true" }
                ],
                "Subscribe"
            ]);
        }
    }
</script>
<div class="widthConstrained">
    <Google:Analytics runat="server" />
    <asp:Literal ID="marketingscripts" runat="server"></asp:Literal>
    <asp:HiddenField ID="hdnIsMobileAppSession" runat="server" Value="false" />
</div>

    <%= GetQuovadisScripts() %>
</asp:Content>

