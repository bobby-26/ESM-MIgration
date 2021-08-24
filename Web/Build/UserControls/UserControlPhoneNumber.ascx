<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPhoneNumber.ascx.cs"
    Inherits="UserControlPhoneNumber" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<eluc:Error runat="server" Visible="false" ID="ucError" />
<telerik:RadMaskedTextBox ID="txtAreaCode" Width="50px" runat="server" CssClass="input" Mask="#####"></telerik:RadMaskedTextBox>
<telerik:RadMaskedTextBox ID="txtPhoneNumber" Width="120px" runat="server" CssClass="input" Mask="##########"
    SelectionOnFocus="CaretToBeginning" DisplayFormatPosition="Right" ></telerik:RadMaskedTextBox>

