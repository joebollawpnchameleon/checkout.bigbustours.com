<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="EcrProductImport.aspx.cs" Inherits="bigbus.checkout.Admin.EcrProductImport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IePlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeaderScriptAndStylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCmsToolbarHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cplhBody" runat="server">
    <section>
        <div>
            <h4>ECR Product Import</h4>
            <div>
                <asp:FileUpload ID="FileUploader" runat="server" />
                <br />
                <br />
                <asp:Button ID="UploadButton" runat="server" Text="Upload File" OnClick="UploadButton_Click" /><br />
                <br />
                <asp:Label ID="lbResult" runat="server"></asp:Label>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphFooterScriptAndStylesheets" runat="server">
</asp:Content>
