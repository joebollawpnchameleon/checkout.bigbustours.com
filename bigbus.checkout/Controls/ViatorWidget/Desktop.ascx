<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/controls/ViatorWidget/Desktop.ascx.cs" Inherits="bigbus.checkout.Controls.ViatorWidget.Desktop" %>
<!-- Begin Viator Partner Desktop Widget -->
<style type="text/css">
@import "https://www.partner.viator.com/modules/widgets/css/niftycorners.css";
@import "https://www.partner.viator.com/modules/widgets/css/widgets.css";
#viatorWidgetDiv_c1f61e3d #viatorWidget { color: #000000; font-family: 'trebuchet ms'; font-weight: normal;  font-size: 11px;}
#viatorWidgetDiv_c1f61e3d .widget_header { background-color: #a39161; color: #fff0d8; font-family: 'trebuchet ms'; font-weight: normal; font-size: 18px; }
#viatorWidgetDiv_c1f61e3d .widget_body { background-color: #fff0d8; }
#viatorWidgetDiv_c1f61e3d .widget_horizontal .widget_entry { background-color: #fff0d8; width: 196px; float: none; display: inline-block; vertical-align: top;}
#viatorWidgetDiv_c1f61e3d .widget_product_title { color:#ab0634; }
#viatorWidgetDiv_c1f61e3d .widget_more_info { color:#ab0634; }
#viatorWidgetDiv_c1f61e3d .widget_more_tours { color:#ab0634; }
#viatorWidgetDiv_c1f61e3d .widget_entry ul li { padding: 0;border: none; }
</style>
<div id="viatorWidgetDiv_c1f61e3d" style="display: none;padding-top:10px;">
<div id="viatorWidgetDiv_c1f61e3d_action">https://www.partner.viator.com/widgets/hotsellers.jspa</div>
<div id="viatorWidgetDiv_c1f61e3d_PUID">15152</div>
<div id="viatorWidgetDiv_c1f61e3d_setLocale">en</div>
<div id="viatorWidgetDiv_c1f61e3d_destinationID"><%= BasePage.CurrentSite.ViatorWidgetDestinationId ?? 0 %></div>
<div id="viatorWidgetDiv_c1f61e3d_numProducts">4</div>
<div id="viatorWidgetDiv_c1f61e3d_title">Complete your city experience with one of our add-ons</div>
<div id="viatorWidgetDiv_c1f61e3d_width">810</div>
<div id="viatorWidgetDiv_c1f61e3d_horizontal">true</div>
<div id="viatorWidgetDiv_c1f61e3d_showThumbs">true</div>
<div id="viatorWidgetDiv_c1f61e3d_widgetAction">hotsellers</div>
<div id="viatorWidgetDiv_c1f61e3d_SUBPUID"></div>
<div id="viatorWidgetDiv_c1f61e3d_linkNewWindow">true</div>
</div>
<script type="text/javascript" src="//partner.vtrcdn.com/modules/widgets/js/initWidget.js?v=releases_CVV_TSM-1914-20150119.1.39b4fa27" ></script>
<script type="text/javascript">
    $(function() {
        initViatorWidgetDiv('c1f61e3d');
    });
</script>
<!-- End Viator Partner Widget -->
