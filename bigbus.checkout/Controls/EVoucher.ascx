﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EVoucher.ascx.cs" Inherits="bigbus.checkout.Controls.EVoucher" %>

    <div class="wrapper">
        <div class="lhs">
            <div class="branding">
                <img src="/images/watermark/logo-bigbus.png"/>
                <asp:Image ID="imgAttractionImage" runat="server" />                
            </div>
            <div class="specs">
                <div>
                    <p>Ticket Type:</p>
                    <p><%= ValidTicketName %></p>
                </div>
                <div>
                    <p>Date:</p>
                    <p><%= "Open Date Ticket"%></p>
                </div>

                <div>
                    <p>Lead name:</p>
                    <p><%=LeadName%></p>
                </div>
                 <div>
                    <p><%= PaymentType %></p>
                    <p><%= CcNumber %></p>
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
            if (!string.IsNullOrWhiteSpace(OrderTotal))
            {
%>
                <div>
                    <p>Order total:</p>
                    <p><%=OrderTotal%></p>
                </div>
<%
            }
        }
%>
            </div>
            <div class="small-print">
                <p><%=TicketLine1%></p>
                <p>
                    <%=TicketLine2%><br/>
                    <%=TicketLine3%>
                </p>
            </div>
        </div>
        <div class="rhs">
            <div class="summary">
<%
    if (AdultQuantity > 0)
        {
%>
                <div>
                    <p>Adult</p>
                    <p><%=AdultQuantity%></p>
                </div>
<%
        }

    if (ChildQuantity > 0)
        {
%>
                <div>
                    <p>Child</p>
                    <p><%=ChildQuantity%></p>
                </div>
<%
        }

    if (FamilyQuantity > 0)
        {
%>
                <div>
                    <p>Family</p>
                    <p><%=FamilyQuantity%></p>
                </div>
<%
        }
       
%>

         </div>
          <div class="b-code">
              <asp:Image runat="server" id="imgQR"/>
            </div>
        </div>
    </div>