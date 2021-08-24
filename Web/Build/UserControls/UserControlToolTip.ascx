<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlToolTip.ascx.cs"
    Inherits="UserControlToolTip" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<div id="divToolTip" class="tooltiptext" runat="server" style="margin-top:10px; white-space:normal;word-wrap:break-word; width:auto" onmouseover="this.style.visibility='hidden'">
    <telerik:Radlabel  ID="lblToolTip" runat="server" ></telerik:Radlabel>
</div>--%>
<telerik:RadToolTip ID="RadToolTip1" runat="server" RenderMode="Lightweight" RenderInPageRoot="false" IgnoreAltAttribute="true" IsClientID="true"
    ShowEvent="OnMouseOver" HideEvent="LeaveTargetAndToolTip" RelativeTo="Element" style="margin-top:10px; white-space:normal;word-wrap:break-word; width:auto">
</telerik:RadToolTip>

