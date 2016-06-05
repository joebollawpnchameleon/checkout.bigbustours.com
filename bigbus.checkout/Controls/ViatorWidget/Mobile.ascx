<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/controls/ViatorWidget/Mobile.ascx.cs" Inherits="bigbus.checkout.Controls.ViatorWidget.Mobile" %>
<!-- Begin Viator Partner Mobile Widget -->
<style type="text/css">
   @import "https://www.partner.viator.com/modules/widgets/css/niftycorners.css";
   @import "https://www.partner.viator.com/modules/widgets/css/widgets.css";
    #viatorWidgetDiv_1e4ff5fc { color: #000000; font-family: 'trebuchet ms'; font-weight: normal; font-size: 11px; }
    #viatorWidgetDiv_1e4ff5fc .widget_header { background-color: #a39161; color: #fff0d8; font-family: 'trebuchet ms'; font-weight: normal; font-size: 18px; }
    #viatorWidgetDiv_1e4ff5fc .widget_body { background-color: #fff0d8; }
    #viatorWidgetDiv_1e4ff5fc .widget_horizontal .widget_entry { background-color: #fff0d8; width: 66px; }
    #viatorWidgetDiv_1e4ff5fc .widget_product_title{ color:#ab0634; }
    #viatorWidgetDiv_1e4ff5fc .widget_more_info{ color:#ab0634; }
    #viatorWidgetDiv_1e4ff5fc .widget_more_tours{ color:#ab0634; }
    #viatorWidgetDiv_c1f61e3d .widget_entry ul li { padding: 0;border: none; }
</style>
<div id="viatorWidgetDiv_1e4ff5fc" style="display: none ;padding-top:10px;">
    <div id="viatorWidgetDiv_1e4ff5fc_action">https://www.partner.viator.com/widgets/hotsellers.jspa</div>
    <div id="viatorWidgetDiv_1e4ff5fc_PUID">15152</div>
    <div id="viatorWidgetDiv_1e4ff5fc_setLocale">en</div>
    <div id="viatorWidgetDiv_1e4ff5fc_destinationID"><%= BasePage.CurrentSite.ViatorWidgetDestinationId %></div>
    <div id="viatorWidgetDiv_1e4ff5fc_numProducts">3</div>
    <div id="viatorWidgetDiv_1e4ff5fc_title">Complete your city experience with one of our add-ons</div>
    <div id="viatorWidgetDiv_1e4ff5fc_width">300</div>
    <div id="viatorWidgetDiv_1e4ff5fc_horizontal">false</div>
    <div id="viatorWidgetDiv_1e4ff5fc_showThumbs">true</div>
    <div id="viatorWidgetDiv_1e4ff5fc_widgetAction">hotsellers</div>
    <div id="viatorWidgetDiv_1e4ff5fc_SUBPUID"></div>
    <div id="viatorWidgetDiv_1e4ff5fc_linkNewWindow">true</div>
</div>
<script type="text/javascript" src="//partner.vtrcdn.com/modules/widgets/js/initWidget.js?v=releases_TSMII_2.0.5-20150401.5.f2267fba" ></script>
<script type="text/javascript" src="https://www.partner.viator.com/modules/widgets/js/initWidget.js?v=releases_TSMII_2.0.5-20150401.5.f2267fba"></script>
<script type="text/javascript">
    $(function() {
        initViatorWidgetDiv('1e4ff5fc');
    });
</script>
<!-- End Viator Partner Widget -->
