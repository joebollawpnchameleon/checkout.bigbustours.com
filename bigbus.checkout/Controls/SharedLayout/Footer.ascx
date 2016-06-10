<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="bigbus.checkout.Controls.SharedLayout.Footer" %>
<div class="site-footer" role="contentinfo">
    <asp:Repeater ID="rptFooterNavigation" runat="server">
        <HeaderTemplate>
            <ul class="site-footer__links">
        </HeaderTemplate>
        <ItemTemplate>
            <li><a href="<%# Eval("URL") %>" target="_blank"><%# Eval("Text") %></a></li>
        </ItemTemplate>
        <FooterTemplate>
            <li>
                &copy; Big Bus Tours Ltd
            </li>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    <ul class="site-footer__payment-methods">
        <li class="payment-method payment-method-visa"></li>
        <li class="payment-method payment-method-mastercard"></li>
        <li class="payment-method payment-method-american-express"></li>
        <li class="payment-method payment-method-paypal"></li>
        <li class="payment-method payment-method-quo-vadis"></li>
    </ul>
</div>
