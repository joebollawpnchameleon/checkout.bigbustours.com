<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EVoucher.ascx.cs" Inherits="bigbus.checkout.Controls.EVoucher" %>

    <div class="wrapper">
        <div class="lhs">
            <div class="branding">
                <img src="/images/watermark/logo-bigbus.png"/>
                <asp:Image ID="imgAttractionImage" runat="server" />                
            </div>
            <div class="specs">
                <div>
                    <p>Ticket Type:</p>
                    <p><%= TicketName %></p>
                </div>
                <div>
                    <p>Date:</p>
                    <p><%= "Open Date"%></p>
                </div>

                <div>
                    <p>Lead name:</p>
                    <p><%=LeadName%></p>
                </div>

                <div>
                    <p>Order number:</p>
                    <p><%=OrderNumber%></p>
                </div>

<%      

        if (!IsTradeTicketSale)
        {
%>
                <div>
                    <p>Ticket Price</p>
                    <p><%=VoucherPrice%></p>
                </div>
<%
            if (!string.IsNullOrWhiteSpace(line.OrderTotal))
            {
%>
                <div>
                    <p>Order total:</p>
                    <p><%=line.OrderTotal%></p>
                </div>
<%
            }
        }
%>
            </div>
            <div class="small-print">
                <p><%=line.TicketLine1%></p>
                <p>
                    <%=line.TicketLine2%><br/>
                    <%=line.TicketLine3%>
                </p>
            </div>
        </div>
        <div class="rhs">
            <div class="summary">
<%
        if (line.AdultQty > 0)
        {
%>
                <div>
                    <p>Adult</p>
                    <p><%=line.AdultQty%></p>
                </div>
<%
        }

        if (line.ChildQty > 0)
        {
%>
                <div>
                    <p>Child</p>
                    <p><%=line.ChildQty%></p>
                </div>
<%
        }

        if (line.FamilyQty > 0)
        {
%>
                <div>
                    <p>Family</p>
                    <p><%=line.FamilyQty%></p>
                </div>
<%
        }

        if (line.ConcessionQty > 0)
        {
%>
                <div>
                    <p>Concession</p>
                    <p><%=line.ConcessionQty%></p>
                </div>
<%
        }

        if (line.GroupQty > 0)
        {
%>
                <div>
                    <p>Group</p>
                    <p><%=line.GroupQty%></p>
                </div>
<%
        }

        if (line.GroupAQty > 0)
        {
%>
                <div>
                    <p>Adult(Group)</p>
                    <p><%=line.GroupAQty%></p>
                </div>
<%
        }

        if (line.GroupCQty > 0)
        {
%>
                <div>
                    <p>Child(Group)</p>
                    <p><%=line.GroupCQty%></p>
                </div>
<%
        }
%>
            </div>
            <div class="b-code">
                <img alt="codeimage" src="<%=line.CodeImageUrl%>"/>
<%
        if (!string.IsNullOrWhiteSpace(line.Barcode))
        {
%>
                <div><%=line.Barcode%></div>
<%
        }
%>
            </div>
        </div>
    </div>