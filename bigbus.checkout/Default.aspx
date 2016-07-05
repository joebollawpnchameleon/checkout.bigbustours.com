<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="bigbus.checkout.Default" %>
<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
    <style type="text/css">
        table{font-size:9pt;}
        table tr:nth-child(odd)		{ background-color:#eee; }
        table tr:nth-child(even)		{ background-color:#fff; }
        table tr td{vertical-align:top;padding:5px 0;}
        table tr:first-of-type{background-color:#dfd;font-size:10pt;padding:10px;}
        b.minus, b.plus{color:red;font-size:12px;font-weight:bold;cursor:pointer;}
        b.plus{color:green;}
    </style>

    <script type="text/javascript" src="https://code.jquery.com/jquery-2.2.4.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cplhBody" runat="server">
     <header class="content__header">
        <h1>Test Page (Not For Public Use)</h1>
    </header>
    
    <section class="basket">
    
        <div runat="server" id="dvError" visible="false">
            <p><asp:Literal ID="ltError" runat="server" /></p>
        </div>
        <div>
            This is a test page for new checkout!<br/>
            <div runat="server" visible="false">
            Default Cookie Value:<asp:TextBox runat="server" id="txtCookieValue"></asp:TextBox>
           <asp:LinkButton  ValidationGroup="CreditCardCheckout"  runat="server" OnClick="PlantCookie" CssClass="form__continue button button_red button_forward">
                <span runat="server" class="right">Set Cookie</span>
            </asp:LinkButton>
                </div>
            <p>
                Please select Products from the list below or just click 'Go to checkout' button to use default test products.
            </p>
           <%-- Click to plant cookie <asp:Button runat="server" Text="Click" OnClick="PlantCookie"/>--%>
            <%--<asp:Button runat="server" Text="Go to checkout" OnClick="GoToCheckout" />--%>
            <%-- <asp:LinkButton ID="LinkButton1" ValidationGroup="CreditCardCheckout"  runat="server" OnClick="GoToCheckout" CssClass="form__continue button button_red button_forward">
                <span id="Span1" runat="server" class="right">Go to checkout</span>
            </asp:LinkButton>--%>
        </div>
        <div style="text-align:left;">
            Select checkout language:  &nbsp;
            <asp:DropDownList id="ddlLanguage" runat="server">
                <asp:ListItem Value="eng" Text="English" />
                <asp:ListItem Value="deu" Text="German" />                
                <asp:ListItem Value="fra" Text="French" />
                <asp:ListItem Value="hun" Text="Hungarian" />
                <asp:ListItem Value="spa" Text="Spanish" />
                <asp:ListItem Value="yeu" Text="Cantonese" />
                <asp:ListItem Value="cmn" Text="Mandarine" />
            </asp:DropDownList>
        </div>
        <div style="text-align:left;">
            Select basket currency:  &nbsp;
            <asp:DropDownList id="ddlCurrency" runat="server" OnSelectedIndexChanged="ChangeCurrency"  AutoPostBack="True" >
                <asp:ListItem Value="GBP" Text="British Pound"  />
                <asp:ListItem Value="EUR" Text="Euro" />                
                <asp:ListItem Value="USD" Text="US Dollar" />
            </asp:DropDownList>
        </div>
        <div>
            <asp:Repeater id="rptItems" runat="server">
                <HeaderTemplate>
                    <table style="border-collapse: collapse; width: 98%; text-align: left; margin: 20px 0;">
                    <tr>
                        <th>&nbsp;</th>
                        <th>
                            Name
                        </th>
                        <th>
                            City
                        </th>
                        <th>
                            ECR Version
                            </th>
                        <th>
                            Product SKU
                        </th>
                        <th>
                            Ticket Type
                        </th>
                        <th>
                            Passenger Type
                        </th>                       
                        <th>
                            Quantity
                        </th>
                       <th>
                           Total
                       </th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <%# Eval("TicketName") %>
                        </td>
                        <td>
                            <%# Eval("MicroSiteId") %>
                            <input type="hidden" id="hdn_City_<%# Eval("EcrProductCode") %>" value="<%# Eval("MicroSiteId") %>" />
                        </td>
                        <td>
                            <%# Eval("MicroSiteEcrVersionId") %>
                        </td>
                        <td>
                            <%# Eval("EcrProductCode") %>
                        </td>
                        <td>
                            <%# Eval("TicketType") %>
                        </td>
                        <td>
                            <%# Eval("PriceOptionHtml") %>
                        </td>                       
                         <td>
                             <%# Eval("QuantityOptionHtml") %>
                        </td> 
                        <td>
                            <%# Eval("TotalOptionHtml") %>
                        </td>                      
                    </tr>
                </ItemTemplate>
                <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>
        </div>
        
        <div>
       

             <asp:LinkButton ID="btnContinue" ValidationGroup="CreditCardCheckout" onclick="PostUserToDetailsPage"  runat="server" CssClass="form__continue button button_red button_forward">
                <span id="spnContinueText" runat="server" class="right">Go to checkout</span>
            </asp:LinkButton>
            <asp:HiddenField runat="server" id="hdnSelections" ClientIDMode="Static"/>
        </div>
    </section>
    
    <script type="text/javascript">
        var arrAllProducts = new Array();

        
        //var allSelections = '';
        //var arrSelection = new Array();

        function ChangeValue(skuId, offset, mainSku) {
            var obj = $('#sp_total_' + skuId);
            var objQuantity = $('#sp_quantity_' + skuId);
            var price = $('#hdn_price_' + skuId).val();
            var currencySymbol = $('#hdn_currency_' + skuId).val();
            var hdnCity = $('#hdn_City_' + mainSku).val();
            var content = objQuantity.html();
            var quantity = 0;

            if (content === '') {
                quantity = 0;
            }
            else {
                quantity = parseInt(content);
            }
            
            quantity += offset;
            price = parseFloat(price);

            if (quantity == 0) {
                objQuantity.html('');
                obj.html('');
                obj.hide();
                objQuantity.hide();

                if (arrAllProducts[mainSku] != null) {
                    arrAllProducts[mainSku][skuId] = null;
                }
            }
            else if (quantity > 0) {
                objQuantity.html(quantity);
                obj.html(currencySymbol + '' + (price * quantity));
                obj.show();
                objQuantity.show();

                if (arrAllProducts[mainSku] == null)
                {
                    arrAllProducts[mainSku] = new Array();
                }

                arrAllProducts[mainSku][skuId] = new Array();
                arrAllProducts[mainSku][skuId][0] = quantity;
                arrAllProducts[mainSku][skuId][1] = price;
                arrAllProducts[mainSku][skuId][2] = hdnCity;
           }
            else {
                quantity = 0;
                obj.hide();
                objQuantity.hide();
                alert(quantity);
            }

            BuildXml();
        }

        function BuildXml() {
            var finalXml = '';

            for (var x in arrAllProducts) {
                var tempArr = arrAllProducts[x];
                if (tempArr == null ||  arrAllProducts[x] == null) continue;

                var temp = '';

                for (var y in tempArr) {
                    if (tempArr[y] == null)
                        continue;
                    if (temp != '')
                        temp += ',';
                    temp += '{"sku":"' + y + '", "quantity":' + tempArr[y][0] + ', "price":' + tempArr[y][1] + ',"city":"' + tempArr[y][2] + '"}'
                }

                temp = '{"productsku":"' + x + '","selections": [' + temp + ']}';

                if (finalXml != '')
                    finalXml += ',';

                finalXml += temp;
                temp = '';
            }

            $('#hdnSelections').val('{"selection": [' + finalXml + ']}');
        }

        //function CheckIndex(obj) {
        //    arrSelection[obj.id] = obj.checked ? obj.value : -1;
        //    //alert(obj.id + ' - ' + obj.checked + ' - ' + obj.value);

        //    var temp = '';

        //    for (var key in arrSelection) {
        //        if (arrSelection.hasOwnProperty(key)) {
        //            if (arrSelection[key] > -1) {
        //                temp += (temp !== '') ? '-' : '';
        //                temp += arrSelection[key];
        //            }
        //        }
        //    }

        //    document.getElementById('hdnSelections').value = temp;

        //    //alert(temp);
        //}


        
    </script>
</asp:Content>
