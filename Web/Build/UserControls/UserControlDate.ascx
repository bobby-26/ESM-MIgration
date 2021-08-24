<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDate.ascx.cs" Inherits="UserControlDate" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<link href="../css/Theme1/PhoenixNew/DataPager.PhoenixNew.css" rel="stylesheet" />--%>
<telerik:RadDateTimePicker RenderMode="Lightweight" runat="server" ID="txtDate" MaxLength="12"  OnSelectedDateChanged="OnTextChange" 
    TimePopupButton-Visible="false">
</telerik:RadDateTimePicker>
