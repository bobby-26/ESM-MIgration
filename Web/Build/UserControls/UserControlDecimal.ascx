<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDecimal.ascx.cs" Inherits="UserControlDecimal" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%--<asp:TextBox ID="txtDecimal" runat="server" MaxLength="22" Width="120px" CssClass="input" style="text-align:right;" ></asp:TextBox>
<ajaxToolkit:MaskedEditExtender ID="txtDecimalMask" runat="server" TargetControlID="txtDecimal"
            Mask="999,999,999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
</ajaxToolkit:MaskedEditExtender>--%>
<telerik:RadNumericTextBox ID="txtDecimal" RenderMode="Lightweight" runat="server" MaxLength="22" Width="120px" style="text-align:right;" 
    Type="Number">
    <NumberFormat AllowRounding="false" DecimalSeparator="." DecimalDigits="2" />
</telerik:RadNumericTextBox> 
<%--<telerik:RadMaskedTextBox ID="txtDecimal" runat="server" Mask="999,999,999,999,999.99" Width="120px" CssClass="input" style="text-align:right;"
    DisplayFormatPosition="Right"></telerik:RadMaskedTextBox>--%>
