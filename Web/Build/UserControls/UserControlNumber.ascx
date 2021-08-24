<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlNumber.ascx.cs" Inherits="UserControlNumber" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtNumber" Width="190px" MaxLength="10" CssClass="input"
    MinValue="0">
    <NumberFormat AllowRounding="false" DecimalDigits="0" />
    <EnabledStyle HorizontalAlign="Right" />
</telerik:RadNumericTextBox>