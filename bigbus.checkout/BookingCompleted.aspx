<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SiteMaster.Master" CodeBehind="BookingCompleted.aspx.cs" Inherits="bigbus.checkout.BookingCompleted" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lbError" runat="server" Visible="false" />

    
<p>
    Dear user,<br/>
    Thank you for buying from us.
    <em style="border: 1px solid red; padding: 5px;">
        <a href="ViewVoucher.aspx?oid=<%= Request.QueryString["oid"] %>">View your voucher</a>
    </em>
</p>
<p>
    <input type="checkbox" id="chkReceiveNews" />
    </p>

<script type="text/javascript">
    function TrackSubscription() {
        var isUserSubscribed = $('#chkReceiveNews:checked').length > 0;
        var email = '<%= UserEmail %>';

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

    <%= IntileryTagScript %>
</script>

</asp:Content>

