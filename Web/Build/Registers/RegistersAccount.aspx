<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAccount.aspx.cs"
    Inherits="RegistersAccount" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chart Of Accounts</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized() {
            var sender = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight - 40);
            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 65);
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmAccounts" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwAccounts">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tvwAccounts" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuAccount" runat="server" OnTabStripCommand="Account_TabStripCommand"></eluc:TabStrip>
        </div>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="100%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="30%" Height="100%">
                <eluc:TabStrip ID="MenuRegisterAccount" runat="server" OnTabStripCommand="MenuRegisterAccount_TabStripCommand"></eluc:TabStrip>
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblShowAll" runat="server" Text="Show All"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkShowAllAccount" AutoPostBack="true" runat="server" OnCheckedChanged="chkShowAllAccount_CheckedChanged">
                            </telerik:RadCheckBox>
                        </td>
                    </tr>
                </table>
                <eluc:TreeView runat="server" ID="tvwAccounts" OnNodeClickEvent="ucTree_SelectNodeEvent" Height="100%" EmptyMessage="Type to search accounts"></eluc:TreeView>
                <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" Height="100%">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="RadPane1" runat="server" Width="70%" Height="100%">
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" />
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAccountCode" runat="server" MaxLength="50" CssClass="readonlytextbox"
                                    ReadOnly="true" Width="200px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDescription" runat="server" MaxLength="200" CssClass="input_mandatory"
                                    TextMode="MultiLine" Width="200px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDropDownList ID="ddlAccountType" runat="server" CssClass="input_mandatory" Width="200px">
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblUsage" runat="server" Text="Usage"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDropDownList ID="ddlAccountUsage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BudgetSearch" 
                                    CssClass="input_mandatory" Width="200px">
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSource" runat="server" Text="Source"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDropDownList ID="ddlAccountSource" runat="server" AutoPostBack="true" OnTextChanged="BudgetSearch"
                                    CssClass="input_mandatory" Width="200px">
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSecurityLevel" runat="server" Text="Security"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ucSecurityLevel" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    HardTypeCode="251" HardList='<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 251) %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblBankCurrency" Text="Currency"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Currency runat="server" ID="ucBankCurrency" CssClass="dropdown_mandatory" AppendDataBoundItems="true"></eluc:Currency>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblVPCurrencyCode" Text="Voucher Prefix Currency Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtVPCurrencyCode" runat="server" MaxLength="1" CssClass="input" Width="20px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Principal"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListMaker">
                                    <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                                        Width="30%">
                                    </telerik:RadTextBox>
                                    <telerik:RadImageButton ID="ImgSupplierPickList" runat="server" Image-Url="<%$ PhoenixTheme:images/picklist.png %>"
                                        Style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" Width="20px" Height="20px" />
                                    <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                                        Width="50%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblActive" runat="server" Text="Active"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkActive" runat="server"></telerik:RadCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblMonetaryItems" Text="Monetary Items for Forex Revaluation at Year End"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkMonetaryItems" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblRestrictToOpenProject" Text="Restrict to Open Projects Only"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkOpenProjecct" runat="server" />
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
