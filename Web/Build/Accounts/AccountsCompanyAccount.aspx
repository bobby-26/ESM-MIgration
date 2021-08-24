<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCompanyAccount.aspx.cs"
    Inherits="AccountsCompanyAccount" MaintainScrollPositionOnPostback="true" %>

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
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
<%--<body onresize="resizeFrame(document.getElementById('divAccounts'));" onload="resizeFrame(document.getElementById('divAccounts'));">--%>
    <body onresize="PaneResized()" onload="PaneResized()" >
    <form id="frmAccounts" runat="server">
    <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server"  >
    </telerik:RadScriptManager>
           <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwAccounts">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tvwAccounts" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="txtAccountCode" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="txtCompanyAccountId" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="txtDescription" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="rblAccountType" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="rblAccountSource" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="rblAccountUsage" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucBankCurrency" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="chkActive" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
                    <eluc:TabStrip ID="MenuAccount" runat="server" OnTabStripCommand="Account_TabStripCommand">
                    </eluc:TabStrip>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="100%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="35%" Height="100%">
                    <eluc:TabStrip ID="MenuAccountsCompanyAccount" runat="server" OnTabStripCommand="MenuAccountsCompanyAccount_TabStripCommand">
                    </eluc:TabStrip>     
                                <table style="float: left; width: 100%;">
                                    <tr>
                                      <%--  <td style="white-space:nowrap">
                                            <telerik:RadLabel ID="lblAccountCodeCaption" runat="server" Text="Account Code"></telerik:RadLabel>&nbsp;
                                            <telerik:RadTextBox runat="server" ID="txtAccountSearch" CssClass="input"
                                                MaxLength="10" Width="150px"></telerik:RadTextBox>&nbsp;<asp:ImageButton runat="server"
                                                    ImageUrl="<%$ PhoenixTheme:images/search.png %>" ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click"
                                                    ToolTip="Search" />
                                        </td>
                                      --%>  <td style="white-space:nowrap">
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
            <telerik:RadSplitBar ID="RadSplitbar2" runat="server" CollapseMode="Forward" Height="100%">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="RadPane1" runat="server" Width="70%" Height="100%">
                    <telerik:RadAjaxPanel runat="server" ID="pnlAccount">
                                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>                             
                            <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtAccountCode" runat="server" MaxLength="50" CssClass="readonlytextbox"
                                                ReadOnly="true" Width="300px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtCompanyAccountId" runat="server" Visible="false" Width="300px"></telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtDescription" runat="server" MaxLength="250" CssClass="readonlytextbox"
                                                ReadOnly="true" Width="300px"></telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadRadioButtonList ID="rblAccountType" runat="server" Direction="Horizontal"
                                                Width="200px" RepeatLayout="Table" CssClass="readonlytextbox" Enabled="false">

                                            </telerik:RadRadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblUsage" runat="server" Text="Usage"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadRadioButtonList ID="rblAccountUsage" runat="server" Direction="Horizontal"
                                                Width="390px" TextAlign="Right" RepeatLayout="Table" CssClass="readonlytextbox"
                                                Enabled="false">
                                            </telerik:RadRadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblSource" runat="server" Text="Source"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadRadioButtonList ID="rblAccountSource" runat="server" Direction="Horizontal"
                                                Width="300px" RepeatLayout="Table" CssClass="readonlytextbox" Enabled="false">
                                            </telerik:RadRadioButtonList>
                                        </td>
                                    </tr>
<%--                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblGroup" runat="server" Text="Group"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Hard ID="ucHard" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                                Enabled="false" />
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel runat="server" ID="lblBankCurrency" Text="Bank Currency"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Currency runat="server" ID="ucBankCurrency" CssClass="readonlytextbox" Enabled="false"
                                                AppendDataBoundItems="true"></eluc:Currency>
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
                                </table>                           
                     <%--   </td>
                    </tr>
                </table>
                <eluc:VerticalSplit runat="server" ID="ucVerticalSplit" TargetControlID="divAccounts"
                    Visible="false" />
                <br clear="right" />--%>
    </telerik:RadAjaxPanel>
              </telerik:RadPane>
                </telerik:RadSplitter>

    </form>
</body>
</html>
