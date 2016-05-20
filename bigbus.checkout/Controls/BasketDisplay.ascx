<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BasketDisplay.ascx.cs" Inherits="bigbus.checkout.Controls.BasketDisplay" %>

<asp:Repeater runat="server" id="rptBasketLines">
    <HeaderTemplate>
        <table>
        <tr>
            <td>&nbsp;</td>
            <td><%= ParentPage.GetTranslation("Date") %></td>
            <td><%= ParentPage.GetTranslation("Title") %></td>
            <td><%= ParentPage.GetTranslation("No_Of_Tickets") %></td>
            <td><%= ParentPage.GetTranslation("Total") %></td>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("TicketName") %></td>
            <td><%# Eval("Date") %></td>
            <td><%# Eval("Title") %></td>
            <td><%# Eval("Quantity") %></td>
            <td><%# Eval("TotalSummary") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        <tr style="<%= DisplayMode %>">
           <td colspan="4">
               <asp:Button runat="server" OnClick="AddMoreTickets" Text="ADD MORE TICKETS"/>
           </td> 
            <td>
                <%= ParentPage.GetTranslation("Total") + ":" + TotalString %>
            </td>
        </tr>
        </table>
    </FooterTemplate>
</asp:Repeater>