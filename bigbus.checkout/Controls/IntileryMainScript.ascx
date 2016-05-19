<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntileryMainScript.ascx.cs" Inherits="bigbus.checkout.Controls.IntileryMainScript" %>
<script type="text/javascript">
	window.INTILERY = {};
       	 var _itq = _itq || [];
         _itq.push(["_setAccount", "<%= AccountId %>"]);
         (function (t) {
            var it = document.createElement(t), s = document.getElementsByTagName(t)[0];
            it.type = 'text/javascript'; it.async = true;
            it.src = '//www.intilery-analytics.com/rest/md/<%= AccountId %>';
            s.parentNode.insertBefore(it, s);
         })('script');
</script>