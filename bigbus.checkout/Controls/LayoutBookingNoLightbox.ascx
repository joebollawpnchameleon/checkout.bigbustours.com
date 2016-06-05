<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LayoutBookingNoLightbox.ascx.cs" Inherits="BigBusWebsite.controls.PageHeaders.LayoutBookingNoLightbox" %>
<%@ Import Namespace="Common.Helpers" %>

<asp:placeholder id="customResourceLinks" visible="true" runat="server">
    <link media="screen" href="<%= Fingerprint.Tag("/styles/default/layout_international.css") %>" rel="stylesheet" />
    <script type="text/javascript" src="<%= Fingerprint.Tag("/javascript/jquery/jquery-1.4.2.min.js") %>"></script>
    <script type="text/javascript" src="<%= Fingerprint.Tag("/javascript/jquery/homepage_animation.js") %>"></script>
    <script type="text/javascript" src="<%= Fingerprint.Tag("/javascript/swfobject.js") %>"></script>
</asp:placeholder>
<link rel="stylesheet" type="text/css" media="all" href="<%= Fingerprint.Tag("/javascript/which-city/which-city-Styles.css") %>" />
<link rel="stylesheet" type="text/css" media="all" href="<%= Fingerprint.Tag("/javascript/which-city/jScrollPane.css") %>" />
<link rel="stylesheet" type="text/css" href="<%= Fingerprint.Tag("/styles/default/booking.css") %>" />
<link rel="stylesheet" type="text/css" href="<%= Fingerprint.Tag("/styles/default/booking_skins/tango/skin.css") %>"  />
<script type="text/javascript" src="<%= Fingerprint.Tag("/javascript/which-city/jquery.mousewheel.js") %>"></script>
<script type="text/javascript" src="<%= Fingerprint.Tag("/javascript/which-city/jScrollPane.js") %>"></script>
<script type="text/javascript" src="<%= Fingerprint.Tag("/javascript/jquery/jquery.jcarousel.js") %>"></script>
<script type="text/javascript" src="<%= Fingerprint.Tag("/javascript/ticker/jquery.tickertype.js") %>"></script>
<link rel="stylesheet" href="<%= Fingerprint.Tag("/styles/css/new.css") %>" type="text/css" />