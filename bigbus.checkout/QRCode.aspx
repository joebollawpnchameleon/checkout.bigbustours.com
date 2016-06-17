<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRCode.aspx.cs"  MasterPageFile="~/BlankTemplate.Master" Inherits="bigbus.checkout.QrCodeTestWebform" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
 <style type="text/css">
    #wrapperdiv {
        margin: 10px;
        background-color: #fff;
        width: auto;
    }

        #wrapperdiv > div {
            padding: 5px 5px 0 10px;
            background-color: #fff;
            display: block;
            font-size: 40px;
            color: black;
            font-weight: bold;
        }

    #ctl00_main_body_QRImage, .qr-code {
        width: 80%;
        height: auto;
        border-width: 0;
        margin: 0 auto;
    }

    noscript div {
         display: inline;
    }

        noscript div img {
            height: 1px;
            width: 1px;
            border-style:none;
        }

    @media (max-width: 768px) {
        #wrapperdiv > div {
            font-size: 27px
        }
    }

    @media (max-width: 571px) {
         #wrapperdiv > div {
              font-size: 22px;
         }
    }

    @media (max-width: 380px) {
        #wrapperdiv > div {
            font-size: 14px;
        }
    }
</style>

<%-- <div id="wrapperdiv">
       <asp:Literal ID="ltDetails" runat="server"></asp:Literal>
        <asp:Image ID="QrImage"  runat="server" />
 </div>--%>
    
<div id="wrapperdiv">
    <div>
    
        <%= IntroDetails %>

        <asp:Repeater ID="rpProducts" runat="server">
            <ItemTemplate>
                <%# Eval("Details") %> 
                <%# Eval("ImageHtml") %>
            </ItemTemplate>
        </asp:Repeater>
    
        <%= BottomDetails %>

    </div>
</div>

   
</asp:Content>
