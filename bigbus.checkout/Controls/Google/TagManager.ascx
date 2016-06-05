<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/controls/Google/TagManager.ascx.cs" Inherits="bigbus.checkout.Controls.Google.TagManager" %>
<%--https://support.google.com/tagmanager/answer/3002596?hl=en-GB--%>
<asp:placeholder runat="server" ID="ecommerceTracking">
<script type="text/javascript">
    dataLayer = [
        {
            'transactionId': '<%=TransactionId%>',
            'transactionAffiliation': '<%=HttpUtility.HtmlEncode(TransactionAffiliation)%>',
            'transactionTotal': '<%=TransactionTotal%>',
            'transactionTax': '<%=TransactionTax%>',
            'transactionShipping': '<%=TransactionShipping%>',
            'currencyCode': '<%=TransactionCurrency%>',
            <asp:repeater ID="products" runat="server">
            <HeaderTemplate>'transactionProducts':
            [</HeaderTemplate><ItemTemplate>
                {
                    'sku': '<%# HttpUtility.HtmlEncode(Eval("Ticket.Name")) %>',
                    'name': '<%# HttpUtility.HtmlEncode(Eval("TicketType")) %> - <%# HttpUtility.HtmlEncode(Eval("Ticket.Name")) %>',
                    'category': '<%# Eval("TicketTOrA") %>',
                    'price': '<%# Eval("TicketCost") %>',
                    'quantity': '<%# Eval("TicketQuantity") %>'
                }</ItemTemplate><SeparatorTemplate>,
                </SeparatorTemplate><FooterTemplate>

            ]</FooterTemplate></asp:repeater>
        }
            ];

    var currencyCode = '<%=TransactionCurrency%>';
</script>
</asp:placeholder><!-- Google Tag Manager -->
<noscript>
    <iframe src="//www.googletagmanager.com/ns.html?id=GTM-KNQPVV" height="0" width="0" style="display: none; visibility: hidden"></iframe>
</noscript>
<script>
    (function (w, d, s, l, i) { w[l] = w[l] || []; w[l].push({ 'gtm.start': new Date().getTime(), event: 'gtm.js' }); var f = d.getElementsByTagName(s)[0], j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src = '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f); })(window, document, 'script', 'dataLayer', 'GTM-KNQPVV');
</script>
<!-- End Google Tag Manager -->
