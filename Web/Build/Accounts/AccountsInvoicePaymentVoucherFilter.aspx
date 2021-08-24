<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoicePaymentVoucherFilter.aspx.cs"
    Inherits="AccountsInvoicePaymentVoucherFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<!DOCTYPE html >
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
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVoucherNumberSearch" MaxLength="200" CssClass="input"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtMakerCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="80px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtMakerName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="200" Width="120px">
                            </telerik:RadTextBox>
                            <img runat="server" id="ImgShowMaker" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx', true); " />
                            <telerik:RadTextBox ID="txtMakerId" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherFromDate" runat="server" Text="Voucher From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtVoucherFromdateSearch" runat="server" CssClass="input" />
                        <%--<asp:TextBox ID="txtVoucherFromdateSearch" runat="server" Width="90px" CssClass="input"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                            Enabled="True" TargetControlID="txtVoucherFromdateSearch" PopupPosition="TopLeft">
                        </ajaxToolkit:CalendarExtender> --%>
                        
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoucherToDate" runat="server" Text="Voucher To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtVoucherTodateSearch" runat="server" CssClass="input" />

                        <%--<asp:TextBox ID="txtVoucherTodateSearch" runat="server" Width="90px" CssClass="input"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                            Enabled="True" TargetControlID="txtVoucherTodateSearch" PopupPosition="TopLeft">
                        </ajaxToolkit:CalendarExtender>  --%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherCurrency" runat="server" Text="Voucher Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemittancenotGenerated" runat="server" Text="Remittance not Generated"></telerik:RadLabel>
                    </td>
                    <td>
                        <input type="checkbox" id="chkShowRemittancenotGenerated" value="0" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text="Voucher Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlVoucherStatus" runat="server" AppendDataBoundItems="true" CssClass="input"
                            HardTypeCode="15" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="input"
                            MaxLength="200" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurchaseInvoiceVoucherNumber" runat="server" Text="Purchase Invoice Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPurchaseInvoiceVoucherNumber" runat="server" CssClass="input"
                            MaxLength="200" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSource" runat="server" Text="Source"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlSource" runat="server" AppendDataBoundItems="true" CssClass="input"
                            HardTypeCode="221" ShortNameFilter="CTM,APT,INV,DEP,ALT,ICT,IOT,TCL,EAD" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentType" runat="server" Text="Payment Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlType" runat="server" AppendDataBoundItems="true" CssClass="input"
                            HardTypeCode="213" ShortNameFilter="HCY,RME,OWN,AVN,PCD,SPI,MDL,ROL,BNP,QTY,LIC,CAS,GAR,PAR,SAS,VCP,CRW,AGY,OFC,OBV,WKG,LTR,TAD,RMA" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblReportnottaken" runat="server" Text="Report not taken"></telerik:RadLabel>
                    </td>
                    <td>
                        <input type="checkbox" id="chkReportNotTaken" value="0" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAllocationStatus" runat="server" Text="Allocation Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAllocationStatus" runat="server" CssClass="input">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="0" />
                                <telerik:RadComboBoxItem Text="Allocated" Value="1" />
                                <telerik:RadComboBoxItem Text="Not Yet Allocated" Value="2" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Office Staff"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--  <asp:DropDownList ID ="ddlSubaccount" runat="server"  
                                CssClass="input_mandatory">
                            </asp:DropDownList>
                        --%>
                        <span id="spnPickListFleetManager">
                            <telerik:RadTextBox ID="txtMentorName" runat="server" CssClass="input" MaxLength="100"
                                Width="80%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtuserDesignation" runat="server" CssClass="hidden" Enabled="false"
                                MaxLength="30" Width="5px" ReadOnly="true">
                            </telerik:RadTextBox>

                            <asp:ImageButton runat="server" ID="imguser" Style="cursor: pointer; vertical-align: top"
                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListEmployeeAccount.aspx', true); "
                                ToolTip="Select Employee" />
                            <telerik:RadTextBox runat="server" ID="txtuserid" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtuserEmailHidden" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                        </span>


                    </td>
                       <td>
                        <telerik:RadLabel ID="lblInvoiceReference" runat="server" Text="Vendor Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSupplierReferenceSearch" MaxLength="100" CssClass="input"
                            Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>

                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
