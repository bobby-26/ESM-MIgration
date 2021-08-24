<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseContractGeneral.aspx.cs"
    Inherits="PurchaseContractGeneral" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Contract</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseContractGeneral" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuFormGeneral">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuFormGeneral" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <eluc:TabStrip ID="MenuFormGeneral" Title="General" runat="server" OnTabStripCommand="MenuFormGeneral_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <telerik:RadAjaxPanel runat="server" ID="pnlFormGeneral">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContractNumber" runat="server" Width="60px" MaxLength="50" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContractDate" runat="server" Text="Contract Date"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtContractDate" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="60px" CssClass="input_mandatory"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" Width="240px" CssClass="input_mandatory"
                                ReadOnly="True">
                            </telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?framename=ifMoreInfo', true);"
                                Text=".." />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContractExpiryDate" runat="server" Text="Contract Expiry Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtContractExpireDate" CssClass="input" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContractDescription" runat="server" Width="325px" MaxLength="200" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentTerms" runat="server" Text="Payment Terms"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlQuick ID="UCPaymentTerms" AppendDataBoundItems="true" CssClass="input"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComments" runat="server" Text="Comments"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtConractComments" runat="server" CssClass="input" Width="325px" MaxLength="800"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDeliveryTerms" runat="server" Text="Delivery Terms"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlQuick ID="UCDeliveryTerms" AppendDataBoundItems="true" CssClass="input"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNotes" runat="server" Text="Notes"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContractNotes" runat="server" CssClass="input" Width="325px" MaxLength="800"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPackagingapplicable" runat="server" Text="Packaging applicable"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkPackage" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
