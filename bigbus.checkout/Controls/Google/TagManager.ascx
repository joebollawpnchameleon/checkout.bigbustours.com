<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/controls/Google/TagManager.ascx.cs" Inherits="bigbus.checkout.Controls.Google.TagManager" %>
<%--https://support.google.com/tagmanager/answer/3002596?hl=en-GB--%>
<asp:placeholder runat="server" ID="ecommerceTracking">
<script type="text/javascript">
    <%= GetTagManagerScript() %>

    var currencyCode = '<%=BaseCurrencyCode%>';
</script>

</asp:placeholder><!-- Google Tag Manager -->
<noscript>
    <iframe src="//www.googletagmanager.com/ns.html?id=<%= GtmCode %>" height="0" width="0" style="display: none; visibility: hidden"></iframe>
</noscript>
<script>
    (function (w, d, s, l, i) { w[l] = w[l] || []; w[l].push({ 'gtm.start': new Date().getTime(), event: 'gtm.js' }); var f = d.getElementsByTagName(s)[0], j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src = '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f); })(window, document, 'script', 'dataLayer', '<%= GtmCode %>');
</script>
<!-- End Google Tag Manager -->
