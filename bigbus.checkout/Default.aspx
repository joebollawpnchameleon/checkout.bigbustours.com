<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="bigbus.checkout.Default" %>
<asp:Content ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cplhBody" runat="server">
     <header class="content__header">
        <h1>Test Page (Not For Public Use)</h1>
    </header>
    
    <section class="basket">
    
        <div>
            This is a test page for new checkout!<br/>
            Default Cookie Value:<asp:TextBox runat="server" id="txtCookieValue"></asp:TextBox>
           <asp:LinkButton  ValidationGroup="CreditCardCheckout"  runat="server" OnClick="PlantCookie" CssClass="form__continue button button_red button_forward">
                <span runat="server" class="right">Set Cookie</span>
            </asp:LinkButton>
            <p>
                Please select Products from the list below or just click 'Go to checkout' button to use default test products.
            </p>
           <%-- Click to plant cookie <asp:Button runat="server" Text="Click" OnClick="PlantCookie"/>--%>
            <%--<asp:Button runat="server" Text="Go to checkout" OnClick="GoToCheckout" />--%>
            <%-- <asp:LinkButton ID="LinkButton1" ValidationGroup="CreditCardCheckout"  runat="server" OnClick="GoToCheckout" CssClass="form__continue button button_red button_forward">
                <span id="Span1" runat="server" class="right">Go to checkout</span>
            </asp:LinkButton>--%>
        </div>
        
        <div>
            <asp:Repeater id="rptItems" runat="server" Visible="False">
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
                            Product SKU
                        </th>
                        <th>
                            Ticket Type
                        </th>
                        <th>
                            Unit cost
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
                            <input data-index="<%# ItemIndex++ %>" type="checkbox" id="ck_<%# ItemIndex %>" value="<%# ItemIndex %>" onclick="CheckIndex(this);"/>
                        </td>
                        <td>
                            <%# Eval("ProductName") %>
                        </td>
                        <td>
                            <%# Eval("Microsite") %>
                        </td>
                        <td>
                            <%# Eval("sku") %>
                        </td>
                        <td>
                            <%# Eval("TicketType") %>
                        </td>
                        <td>
                            <%# Eval("UnitCost") %>
                        </td>
                         <td>
                            <%# Eval("Quantity") %>
                        </td>
                        <td>
                            <%# Eval("Total") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>
        </div>
        
        <div>
             <asp:LinkButton ID="btnContinue" ValidationGroup="CreditCardCheckout"  runat="server" OnClick="PostUserToDetailsPage" CssClass="form__continue button button_red button_forward">
                <span id="spnContinueText" runat="server" class="right">Go to checkout</span>
            </asp:LinkButton>
            <asp:HiddenField runat="server" id="hdnSelections" ClientIDMode="Static"/>
        </div>
    </section>
    
    <script type="text/javascript">
        var allSelections = '';
        var arrSelection = new Array();

        function CheckIndex(obj) {
            arrSelection[obj.id] = obj.checked ? obj.value : -1;
            //alert(obj.id + ' - ' + obj.checked + ' - ' + obj.value);

            var temp = '';

            for (var key in arrSelection) {
                if (arrSelection.hasOwnProperty(key)) {
                    if (arrSelection[key] > -1) {
                        temp += (temp !== '') ? '-' : '';
                        temp += arrSelection[key];
                    }
                }
            }

            document.getElementById('hdnSelections').value = temp;

            //alert(temp);
        }
    </script>
</asp:Content>
