<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Voucher.aspx.cs" Inherits="bigbus.checkout.Voucher" %>

<%@ Import Namespace="BigBus.Helpers" %>
<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<meta name="robots" content="noindex">

<title>Big Bus Tours Voucher Page</title>
<style type="text/css">
body{
    font-family: Arial,Helvetica,sans-serif;
    font-size: 50px;
    margin: 0;}

    p{margin: 0;}

    .page-breaker {
        height:1px;
        display: block;
        page-break-after: always;
    }

    .wrapper{
        border: 0.12em solid #000;
        box-sizing: border-box;
        background-size: cover;
        width: 21.6em;
        height: 9.6em;
        background: url(images/watermark/watermark.jpg) no-repeat;
        background-origin: border-box;
        margin-bottom: 20px;
        position: relative;
    }

        .lhs{/* left hand side */
            margin: 0.380em 0 0 0.64em;
            width: 15.880em;
            /*height: 6.65em;*/
            float: left;
        }

            .branding{/* logo */
                height: 2.040em;
                margin-left: -0.14em;
            }

                .branding img{
                    height: 2.040em;
                    display: block;
                    float: left;
                    margin-right: 15px;
                }

            .specs{
                height: 3.060em;
                /*line-height: 0.75em;*/
                margin-top: 0.40em;
            }

                .specs > div{
                    float: left;
                    width: 100%;
                    margin-bottom: 5px;
                }

                .specs p{
                    width: 19%;
                    /*height: 3.1em;*/
                    font-size: 0.28em;
                    line-height: 1.786em;
                    float:left;
                }

                .specs p + p{
                    width: 80%;
                    font-size: 18px;
                    line-height: 1.190em;
                    font-weight: bold;
                }

            .small-print{/* smallprint */
                left: 32px;
                font-size: 0.30em;
                position: absolute;
                bottom: 5px;
                width: 52.933em;
            }

                .small-print p:first-child{
                    font-weight: bold;
                    margin-bottom: 0.020em;
                }

        .rhs{/* right hand side */
            width: 4.300em;
            height: 6.920em;
            margin: 0.200em 0.440em 0 0 ;
            float: right;
        }

            .summary{
                height: 3.800em;
                /*margin-top: 30px;*/
            }

                .summary div{
                    float: left;
                    clear: left;
                    width: 100%;
                }

                .summary p{
                    float: right;
                    line-height: 43px;
                    font-size: 28px;
                    font-weight: bold;
                    text-align: center;
                    width: 1.75em;
                    /*width: 2.870em;*/
                    line-height: 55px;
                }

                .summary p:first-child{
                    float: left;
                    /*width: 4.286em;*/
                    font-size: 28px;
                    text-align: left;
                    width: auto;
                    font-weight: normal;
                }

            .b-code{
                position: absolute;
                bottom: 0.2em;
                right: 0.440em;
                width: 4.000em;
            }

                .b-code img{
                    width: 200px;
                    /*height: 200px;*/
                }

                .b-code div{
                     font-size:0.5em;
                     text-align: center;
                }
</style>
</head>
<body>
    <asp:Literal ID="LiMerchantReceipt" runat="server"></asp:Literal>
    <asp:Literal ID="LiCustomerReceipt" runat="server"></asp:Literal>
<%
    var counter = 0;

    var lineisbroke = false;

    foreach (var line in MainList)
    {
        counter++;

        if (counter > 1 && line.IsAttraction && !lineisbroke)
        {
%>
    <div class="page-breaker">&nbsp;</div>
<%
        }
%>
    <div class="wrapper">
        <div class="lhs">
            <div class="branding">
                <img src="/images/watermark/logo-bigbus.png"/>
                <%
                    if (!string.IsNullOrWhiteSpace(line.AttractionImageUrl))
                    {
                %>
                        <img alt="<%=line.TicketName%>" src="<%=line.AttractionImageUrl%>"/>
                <%
                    }
                %>
            </div>
            <div class="specs">
                <div>
                    <p>Ticket Type:</p>
                    <p><%=line.TicketName%></p>
                </div>
                <div>
                    <p>Date:</p>
                    <p><%=line.MainDate%></p>
                </div>
<%
        if (!string.IsNullOrWhiteSpace(line.GroupDepartureTime))
        {
%>
                <div>
                    <p>Time:</p>
                    <p><%=line.GroupDepartureTime%></p>
                </div>
<%
        }

        if (!string.IsNullOrWhiteSpace(line.GroupDeparturePoint))
        {
%>
                <div>
                    <p>Location:</p>
                    <p><%=line.GroupDeparturePoint%></p>
                </div>
<%
        }
%>
                <div>
                    <p>Lead name:</p>
                    <p><%=line.LeadName%></p>
                </div>
<%
        if (!IsTradeTicketSale)
        {
%>
                <div>
                    <p><%= line.PaymentType %></p>
                    <p><%= line.CcNumber %></p>
                </div>
<%
        }
%>
                <div>
                    <p>Order number:</p>
                    <p><%=line.OrderNumber%></p>
                </div>
<%
        if (!string.IsNullOrWhiteSpace(line.AgentRef))
        {
%>
                <div>
                    <p>Agent ref:</p>
                    <p><%=line.AgentRef%></p>
                </div>
<%
        }

        if (!IsTradeTicketSale)
        {
%>
                <div>
                    <p>Ticket Price</p>
                    <p><%=line.Price%></p>
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
<%
        if ((line.IsAttraction || counter % 3 == 0 ) && MainList.Count() > counter)
        {
            lineisbroke = true;
%>
<div class="page-breaker">&nbsp;</div>
<%
        }
        else
        {
            lineisbroke = false;
        }
    }
%>
</body>
</html>
