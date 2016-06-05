<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/controls/Misc/CloudFrontScript.ascx.cs" Inherits="BigBusWebsite.controls.Misc.CloudFrontScript" %>
<script type="text/javascript">
    setTimeout(function () {
        var a = document.createElement("script");
        var b = document.getElementsByTagName('script')[0];
        a.src = document.location.protocol + "//dnn506yrbagrg.cloudfront.net/pages/scripts/0006/6426.js?" + Math.floor(new Date().getTime() / 3600000);
        a.async = true; a.type = "text/javascript"; b.parentNode.insertBefore(a, b);
    }, 1);
</script>