<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceBasicFilterRWAForPurchase.aspx.cs"
    Inherits="AccountsInvoiceBasicFilterRWAForPurchase" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MCUser" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Register Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtInvoiceNumberSearch" MaxLength="200" CssClass="input"
                            Width="230px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceReference" runat="server" Text="Vendor Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSupplierReferenceSearch" MaxLength="100" CssClass="input"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                     
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrderNumber" runat="server" Text="Order Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtOrderNumber" MaxLength="200" CssClass="input"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                   
                  <td>
                        <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                                Width="180px">
                            </telerik:RadTextBox>
                            <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                    </tr>
                <tr>
                     
                    <td>
                        <telerik:RadLabel ID="lblPurchaseSupdt" runat="server" Text="Technical Superintendent"></telerik:RadLabel>
                    </td>
                    <td>
<%--                        <telerik:RadComboBox ID="ddlSuptList" runat="server" CssClass="input" Width="100px"></telerik:RadComboBox>--%>
                        <eluc:MCUser ID="RadMcUserSup"  Visible="true" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                   <%-- <td>
                        <telerik:RadLabel ID="lblPurchaser" runat="server" Text="Purchaser"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPurchaserList" runat="server" CssClass="input" Width="100px"></telerik:RadComboBox>
                    </td>--%>
                </tr>

               <tr>
                     <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Fleet Manager"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadDropDownList ID="ddlFleetmanager" runat="server" CssClass="input" Width="70%">
                        </telerik:RadDropDownList>--%>
                        <eluc:MCUser ID="RadMcUserFM" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Technical Director"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadDropDownList ID="ddlTechnicaldirector" runat="server" CssClass="input" Width="70%">
                        </telerik:RadDropDownList>--%>
                       <eluc:MCUser ID="RadMcUserTD" runat="server" Width="80%" emailrequired="true" designationrequired="true" />

                    </td>
                    
                </tr>
                </table>
               
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
