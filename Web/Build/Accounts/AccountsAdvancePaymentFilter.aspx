<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdvancePaymentFilter.aspx.cs"
    Inherits="AccountsAdvancePaymentFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Advance Payment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" Title="Advance Payment" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtPaymentNumber" MaxLength="200" CssClass="input"
                        Width="150px"></telerik:RadTextBox>
                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPaymentReference" runat="server" Text="PO Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtSupplierReferenceSearch" MaxLength="100" CssClass="input"
                        Width="150px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>

                <td>
                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text="Supplier Name"></telerik:RadLabel></td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="60px" CssClass="input" ReadOnly="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" Width="200px"
                            CssClass="input" ReadOnly="false"></telerik:RadTextBox>
                        <img runat="server" id="ImgSupplierPickList" style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="40px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <div id="dvVessel" runat="server" class="input" style="overflow: auto; width: 40%; height: 80px">
                        <telerik:RadCheckBoxList ID="chkVesselList" runat="server" Height="100%" OnSelectedIndexChanged ="chkVesselList_Changed"
                            RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        </telerik:RadCheckBoxList>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
