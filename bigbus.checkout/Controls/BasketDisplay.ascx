<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BasketDisplay.ascx.cs" Inherits="bigbus.checkout.Controls.BasketDisplay" %>

 <div class="basket__container">

    <asp:Repeater runat="server" id="rptBasketLines">
        <HeaderTemplate>
        
            <table class="basket__table basket__table_four">
                        <thead>
                            <tr>
                                <td>
                                    <table class="basket__table-header">
                                        <colgroup class="basket__table-columns">
                                            <col class="basket__table-column column__type">
                                            <col class="basket__table-column">
                                            <col class="basket__table-column">
                                            <col class="basket__table-column">
                                        </colgroup>
                                        <tr>
                                            <th class="ticket__type"></th>
                                            <th><%=ParentPage.GetTranslation("Title")%></th>
                                            <th><%=ParentPage.GetTranslation("Booking_NoOfTickets")%></th>
                                            <th><%=ParentPage.GetTranslation("Total")%></th>
                                            <th></th>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </thead>
                        <tbody>
        
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <table class="ticket">
                        <colgroup class="basket__table-columns">
                            <col class="basket__table-column">
                            <col class="basket__table-column">
                            <col class="basket__table-column">
                            <col class="basket__table-column">
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="ticket__type"><%# Eval("TicketName") %></th>
                                <td><%# Eval("Title") %></td>
                                <td><%# Eval("Quantity")%></td>
                                <td><%# Eval("TotalSummary") %></td>
                            </tr>
                            <tr class="ticket__mobile-info">
                                <td colspan="4">
                                    <%# Eval("TicketName") %><br />
                                    <%# Eval("Date")%><%--<%# Eval("DepartureDetails") %>--%>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
             </tbody>
            </table>
            <div class="basket__footer">
                <p class="basket__link" style="<%= DisplayMode %>">
                    <asp:LinkButton CssClass="button button_white button_back" runat="server" OnClick="AddMoreTickets" >
                        <span><%=ParentPage.GetTranslation("Booking_AddTourTickets")%></span>
                    </asp:LinkButton>
                </p>

                <p class="basket__total">
                    <span><%=ParentPage.GetTranslation("Total")%>:</span> <%= TotalString %>
                </p>
            </div>
        </FooterTemplate>
    </asp:Repeater>

 </div>