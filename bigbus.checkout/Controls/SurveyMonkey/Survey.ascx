<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/controls/SurveyMonkey/Survey.ascx.cs" Inherits="bigbus.checkout.Controls.SurveyMonkey.Survey" %>
<!--SurveyMonkey start-->
<script type="text/javascript">
    function createCookie(name, value, expires, path, domain) {
        var cookie = name + "=" + escape(value) + ";";
        if (expires) {
            // If it's a date
            if (expires instanceof Date) {
                // If it isn't a valid date
                if (isNaN(expires.getTime()))
                    expires = new Date();
            }
            else
                expires = new Date(new Date().getTime() + parseInt(expires) * 1000 * 60 * 60 * 24);

            cookie += "expires=" + expires.toGMTString() + ";";
        }

        if (path) cookie += "path=" + path + ";";
        if (domain) cookie += "domain=" + domain + ";";
        document.cookie = cookie;
    }

    var regexp = new RegExp("(?:^" + "surveyCookie" + "|;\s*" + "surveyCookie" + ")=(.*?)(?:;|$)", "g");
    var result = regexp.exec(document.cookie);
    if (result == null) {
        setTimeout(function () {
            new $.Zebra_Dialog('<strong>We are currently conducting a short customer survey.</strong><br /><br />' +
                'We value your feedback, and would really like to know about your experience on our website.<br />' +
                'The survey takes no more than 5 minutes to complete.<br /><br />' +
                'Thank you!<br />', {
                    'buttons': [
                        { caption: 'Take survey', callback: function () { window.open('<%= SurveyUrl %>'); } },
                        { caption: 'No thanks!' }],
                    'modal': false,
                    'position': ['right - 20', 'bottom - 20'],
                    'onClose': function (caption) {
                        createCookie("surveyCookie", "hidden", null, null, null);
                    }
                })
        }, 2000);
    }
</script>
<!--SurveyMonkey end-->