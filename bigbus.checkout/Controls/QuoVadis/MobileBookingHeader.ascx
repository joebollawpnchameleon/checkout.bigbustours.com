<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileBookingHeader.ascx.cs" Inherits="BigBusWebsite.controls.QuoVadis.MobileBookingHeader" %>
<div class="quovadis">
    <script type="text/javascript" src="https://siteseal.quovadisglobal.com/scripts/script.js"></script>
    <script type="text/javascript">
        function popUp() {
            window.open('https://siteseal.quovadisglobal.com/default.aspx?cn=*.bigbustours.com', 'popUp', 'toolbar=no,resizable=yes,scrollbars=yes,location=yes,dependent=yes,status=0,alwaysRaised=yes,left=300,top=200,width=655,height=610');
            return false;
        }

        try {
            document.write('<a href=""><img src="https://siteseal.quovadisglobal.com/images/quovadis_110x46.gif" border="0" alt="QuoVadis Secured Site - Click for details" onclick="return popUp();"></a>');
        }
        catch (e) { }
    </script>
</div>
