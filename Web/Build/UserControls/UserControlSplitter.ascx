<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSplitter.ascx.cs" Inherits="UserControlSplitter" %>
<div style="position:fixed; height:2px; bottom:0px; margin:0px; display:compact; background-color:white; width:100%; z-index:99; border-color:black; border-style:solid; border-width:1px;  cursor:n-resize" id="divIframe" onclick="release();" onmouseup="release();" runat="server">
<span id="spnTargetControlID" runat="server" title="" style="visibility:hidden"></span>
</div>            
