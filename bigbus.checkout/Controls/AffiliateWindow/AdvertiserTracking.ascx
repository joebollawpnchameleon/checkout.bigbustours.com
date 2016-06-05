<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/controls/AffiliateWindow/AdvertiserTracking.ascx.cs" Inherits="bigbus.checkout.Controls.AdvertiserTracking" %>
<!--AffiliateWindow start-->
<asp:placeholder runat="server" ID="ecommerceTracking">
<!-- Image Pixel Tracking - Mandatory -->
<img src="https://www.awin1.com/sread.img?tt=ns&tv=2&merchant=<%=MerchantId%>&amount=<%=OrderSubtotal%>&cr=<%=CurrencyCode%>&ref=<%=OrderRef%>&parts=<%=PartsString%>&vc=<%=VoucherCode%>&ch=aw&testmode=<%=IsTest%>" border="0" width="0" height="0">
<!-- Product Level Tracking - Optional -->
 <form style="display:none;" name="aw_basket_form">
 <textarea wrap="physical" id="aw_basket" style="display:none;">
<asp:repeater ID="basketRows" runat="server" ItemType="bigbus.checkout.data.Model.OrderLine"><ItemTemplate>

AW:P|<%=MerchantId%>|<%#: HttpUtility.HtmlEncode(Item.Order.OrderNumber)%>|<%#: HttpUtility.HtmlEncode(Item.Ticket.Id)%>|<%#: HttpUtility.HtmlEncode(Item.Ticket.Name)%> - <%#: HttpUtility.HtmlEncode(Item.TicketType)%>|<%#: Item.TicketCost %>|<%#: Item.TicketQuantity %>||<%#: GetAttractionsOrTour(Item)%>|Advertisers</ItemTemplate>
</asp:repeater>
</textarea>
</form>
<!-- Javascript Tracking - Mandatory -->
<script type="text/javascript">
 //<![CDATA[
    /*** Do not change ***/
    var AWIN = {};
    AWIN.Tracking = {};
    AWIN.Tracking.Sale = {};
    /*** transaction parameters ***/
    AWIN.Tracking.Sale.amount = '<%=OrderSubtotal%>';
    AWIN.Tracking.Sale.orderRef = '<%=OrderRef%>';
    AWIN.Tracking.Sale.parts = '<%=PartsString%>';
    AWIN.Tracking.Sale.voucher = '<%=VoucherCode%>';
    AWIN.Tracking.Sale.currency = '<%=CurrencyCode%>';
    AWIN.Tracking.Sale.test = '<%=IsTest%>';
    AWIN.Tracking.Sale.channel = '<%=Channel%>';
 //]]>
</script>
</asp:placeholder>
<script src="https://www.dwin1.com/<%=MerchantId%>.js" type="text/javascript" defer="defer"></script>
<!--AffiliateWindow end-->
