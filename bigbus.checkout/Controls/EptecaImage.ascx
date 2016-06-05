<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EptecaImage.ascx.cs" Inherits="BigBusWebsite.controls.EptecaImage" %>
 <% if (isEptec)
                   { %>
                <img src="<%=EptecString %>" style="display:none;" />
                <% } %>
