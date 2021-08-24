<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceBasicFilter.aspx.cs"
    Inherits="AccountsInvoiceBasicFilter" %>

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
                            Width="230px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReceivedFromDate" runat="server" Text="Received From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtReceivedFromdateSearch" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReceivedToDate" runat="server" Text="Received To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtReceivedTodateSearch" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceType" runat="server" Text="Invoice Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlInvoiceType" runat="server" AppendDataBoundItems="true" AutoPostBack="TRUE"
                            CssClass="input" HardTypeCode="59" Width="230px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucInvoiceStatus" AppendDataBoundItems="true" CssClass="input" runat="server" Width="230px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrderNumber" runat="server" Text="Order Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtOrderNumber" MaxLength="200" CssClass="input"
                            Width="230px">
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
                                Width="160px">
                            </telerik:RadTextBox>
                            <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceCurrency" runat="server" Text="Invoice Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            runat="server" AppendDataBoundItems="true" Width="230px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="4">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" MaxLength="200" Width="230px" Rows="3" TextMode="MultiLine"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPurchaser" runat="server" Text="Purchaser"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlPurchaserList" runat="server" CssClass="input" Width="230px">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 230px;">
                        <telerik:RadLabel ID="lblAdvancedYN" runat="server" Text="Advanced Filter Y/N"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <asp:CheckBox ID="ckAdvancedYN" runat="server" OnCheckedChanged="chkAdvancedYN_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                FadeTransitions="false" FramesPerSecond="40" TransitionDuration="50" AutoSize="None"
                RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                <Panes>
                    <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                        <Header>
                            <a href="" class="accordionLink">Advanced</a>
                        </Header>
                        <Content>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblInvoiceFromDate" runat="server" Text="Invoice From Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:UserControlDate ID="txtInvoiceFromdateSearch" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblInvoiceToDate" runat="server" Text="Invoice To Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:UserControlDate ID="txtInvoiceTodateSearch" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <div id="divFleet" runat="server" class="input" style="overflow: auto; width: 70%; height: 80px">
                                            <asp:CheckBoxList ID="chkFleetList" runat="server" AutoPostBack="true" Height="100%"
                                                OnSelectedIndexChanged="chkFleetList_Changed" RepeatColumns="1" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <div id="dvVessel" runat="server" class="input" style="overflow: auto; width: 70%; height: 80px">
                                            <asp:CheckBoxList ID="chkVesselList" runat="server" Height="100%" OnSelectedIndexChanged="chkVesselList_Changed"
                                                RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <div runat="server" id="dvVesselType" class="input" style="overflow: auto; width: 70%; height: 100px"
                                            visible="False">
                                            <asp:CheckBoxList ID="chkCompanyList" runat="server" Height="100%" RepeatColumns="1"
                                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblPriorityInvoice" runat="server" Text="Priority Invoice"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <input type="checkbox" id="chkPriorityInv" value="0" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPIC" runat="server" Text="PIC"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <%--<eluc:UserName ID="ucPIC" runat="server" AppendDataBoundItems="true" CssClass="input"
                                            UserNameList="<%# PhoenixUser.UserList()%>" Width="70%"/>--%>
                                        <span id="spnPIC">
                                            <telerik:RadTextBox ID="txtUserName" runat="server" CssClass="input" ReadOnly="false"
                                                Width="70%">
                                            </telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtUserDesignation" runat="server" CssClass="input" ReadOnly="false"
                                                Width="0px">
                                            </telerik:RadTextBox>
                                            <img id="imgPIC" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                                            <telerik:RadTextBox ID="txtUserId" runat="server" Width="0px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtEmail" runat="server" Width="0px"></telerik:RadTextBox>
                                        </span>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblInvoicePending" runat="server" Text="Invoice Pending"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox runat="server" ID="ddlInvPending" CssClass="input" Width="70%">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                                <telerik:RadComboBoxItem Text="> 30" Value="30" />
                                                <telerik:RadComboBoxItem Text="> 60" Value="60" />
                                                <telerik:RadComboBoxItem Text="> 90" Value="90" />
                                                <telerik:RadComboBoxItem Text="> 120" Value="120" />
                                                <telerik:RadComboBoxItem Text="> 150" Value="150" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPurchaseSupdt" runat="server" Text="Purchase Supdt"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadDropDownList ID="ddlSuptList" runat="server" CssClass="input" Width="70%">
                                        </telerik:RadDropDownList>
                                    </td>
                                    <%--<td>
                                        <telerik:RadLabel ID="lblPortList" runat="server" Text="Port"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <div id="div1" runat="server" class="input" style="overflow: auto; width: 70%; height: 80px">
                                            <asp:CheckBoxList ID="chkPortList" runat="server" AutoPostBack="true" Height="100%"
                                                RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>--%>
                                </tr>
                            </table>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                </Panes>
            </ajaxToolkit:Accordion>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
