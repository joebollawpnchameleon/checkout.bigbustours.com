<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="BookingAddress.aspx.cs" Inherits="bigbus.checkout.BookingAddress" %>
<%@ Register TagPrefix="NCK" TagName="UserDetails" src="~/Controls/UserDetails.ascx"  %>
<%@ Register TagPrefix="NCK" TagName="BasketDisplay" src="~/Controls/BasketDisplay.ascx"  %>

<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick-theme.css" />
    <link rel="stylesheet" href="/Scripts/vendor/slick/slick.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cplhBody" runat="server">
    <header class="content__header">
        <h1><%= GetTranslation("YourDetails") %></h1>
    </header>

    <section class="basket">
       
        <NCK:BasketDisplay id="ucBasketDisplay" ShowActionRow="true" runat="server"/>   
    </section>
    
    <section class="contact-details">
        <div class="contact-details__header">
            <p class="contact-details__payment-options" runat="server" id="pPaypal">
                <asp:LinkButton runat="server" CssClass="contact-details__paypal-link" OnClick="CheckoutWithPaypal">
                    <img src="/Content/images/Design/paypal.png" alt="PayPal">
                </asp:LinkButton>
                <%= GetTranslation("PaymentOrFillYourDetails") %>
            </p>
        </div>

        <div class="form contact-details__form">

            <div id="dvErrorSummary" class="form__error" runat="server" Visible="False">
                <p>
                    <asp:Literal runat="server" id="ltError"></asp:Literal>
                </p>
            </div>

            <NCK:UserDetails id="ucUserDetails" runat="server"/>
            
            <div class="form-footer">
                <p class="form__buttons">
                    <asp:LinkButton runat="server" CssClass="form__back button button_grey button_back" OnClick="ContinueShopping" id="btnCancel">
                        <span><%=GetTranslation("Back")%></span>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnContinueCheckout" ValidationGroup="CreditCardCheckout"  runat="server" OnClientClick="TrackUserSubscription();" OnClick="CheckoutWithCreditCard" CssClass="form__continue button button_red button_forward">
                        <span id="spnContinueText" runat="server" class="right"><%= GetTranslation("ContinueToSecurePayment") %></span>
                    </asp:LinkButton>
                </p>
                <p class="form__secure"><%=GetTranslation("OurCheckoutIsSecureAndYourDetailsAreProtected")%></p>
                <ul class="form__secure-logos">
                    <li><img src="/Content/images/paymentMethods/worldpay.png" alt="Worldpay" /></li>
                    <li><img src="/Content/images/paymentMethods/safekey.png" alt="SafeKey" /></li>
                    <li><img src="/Content/images/paymentMethods/verified-visa.png" alt="Verified by Visa" /></li>
                    <li><img src="/Content/images/paymentMethods/mastercard-securecode.png" alt="Mastercard Securecode" /></li>
                </ul>
            </div>
            

    </div>
</section>

    <script type="text/javascript">

        function TrackUserSubscription() {
            var isUserSubscribed = $('#ckSubscribe:checked').length > 0;
            var email = $('#txtEmail').val();

            if (isUserSubscribed) {
                _itq.push([
                    "_trackUserEvent", "subscribe",
                    [
                        { "name": "Customer.Email", "value": email },
                        { "name": "Customer.Subscribed", "value": "true" }
                    ],
                    "Subscribe"
                ]);
            }
        }
      
        $(function () {
            $('.btnSubmit_Order').click(function () {
                ShowPopUp();
                StartTimeout();
            });

            function StartTimeout() {
                setTimeout("ResetForm()", 45000);
            }
        });

        function ShowPopUp() {
            $('#loadingModalBox').show();
            $('#loadingModalContent').show();
            $('.btnSubmit_Order').hide();
            $('#loadingHeader').blink();
            return false;
        }

        function HidePopup() {
            $('#loadingModalBox').hide();
            $('#loadingModalContent').hide();
            $('.btnSubmit_Order').show();
            return false;
        }

        function ResetForm() {
            HidePopup();
        }

        (function ($) {
            $.fn.blink = function (options) {
                var defaults = { delay: 800, altText: '' };
                var options = $.extend(defaults, options);
                return this.each(function () {
                    var obj = $(this);
                    var originalText = $(this).html();
                    // If we dont have alt Text, Blink
                    if (options.altText === '') setInterval(function () { if ($(obj).css("visibility") === "visible") { $(obj).css('visibility', 'hidden'); } else { $(obj).css('visibility', 'visible'); } }, options.delay);
                        // Else swap text on delay
                    else setInterval(function () { if ($(obj).html() === originalText) { $(obj).html(options.altText); } else { $(obj).html(originalText); } }, options.delay);
                });
            }
        }(jQuery));

    </script>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphFooterScriptAndStylesheets" runat="server">
    <script src="/Scripts/vendor/jquery/dist/jquery.js" type="text/javascript"></script>
    <script src="/Scripts/vendor/jquery-ui/jquery-ui.min.js"></script>
    <script src="/Scripts/vendor/jquery-ui/ui/core.min.js"></script>
    <script src="/Scripts/vendor/jquery-ui/ui/datepicker.min.js"></script>
    <script src="/Scripts/local/basket.js"></script>
    <%= GetQuovadisScripts() %>
</asp:Content>
