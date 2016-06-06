<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemarketingScript.ascx.cs" Inherits="bigbus.checkout.Controls.Google.RemarketingScript" %>
<!-- Google Code for Big Bus Home Page Remarketing List -->
<script type='text/javascript'>
    /* <![CDATA[ */
    var google_conversion_id = <%= BasePage.CurrentSite.GoogleRemarketingConversionId ?? string.Empty %>;
    var google_conversion_language = 'en';
    var google_conversion_format = '3';
    var google_conversion_color = '666666';
    var google_conversion_label = '<%= BasePage.CurrentSite.GoogleRemarketingConversionLabel ?? string.Empty %>';
    var google_conversion_value = 0;
    /* ]]> */
</script>
<script type='text/javascript' src='https://www.googleadservices.com/pagead/conversion.js'></script>
<noscript>
    <div style='display:inline;'>
    <img height='1' width='1' style='border-style:none;' alt='' src='https://www.googleadservices.com/pagead/conversion/1040537082/?label=<%= BasePage.CurrentSite.GoogleRemarketingConversionLabel ?? string.Empty %>&amp;guid=ON&amp;script=0'/>
    </div>
</noscript>
<!--- End --->
