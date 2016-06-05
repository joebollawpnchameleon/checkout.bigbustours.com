<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/controls/Google/Analytics.ascx.cs" Inherits="BigBusWebsite.controls.Google.Analytics" %>
<script type="text/javascript">
    var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
    document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
</script>
<script type="text/javascript">
    var pageTracker = _gat._getTracker("UA-1768217-2");
    pageTracker._initData();
    pageTracker._trackPageview();
</script>