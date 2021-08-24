<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMobileNumber.ascx.cs" Inherits="UserControlMobileNumber" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>

<%--<asp:TextBox ID="txtMobileNumber" runat="server" MaxLength="22" Width="120px" CssClass="input" style="text-align:right;" ></asp:TextBox>
<ajaxToolkit:MaskedEditExtender ID="txtMobileNumberMask" runat="server" TargetControlID="txtMobileNumber"
            Mask="+99-9999999999" MaskType="None" ClearMaskOnLostFocus="false" InputDirection="LeftToRight" >
</ajaxToolkit:MaskedEditExtender>--%>

<telerik:RadMaskedTextBox ID="txtMobileNumber" runat="server" MaxLength="22" Width="120px" CssClass="input" style="text-align:right;"
    SelectionOnFocus="CaretToBeginning" Mask="############" DisplayFormatPosition="Right">

</telerik:RadMaskedTextBox>
