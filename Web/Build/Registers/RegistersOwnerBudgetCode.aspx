<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOwnerBudgetCode.aspx.cs"
    Inherits="RegistersOwnerBudgetCode" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OwnerBudgetCode</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
      <script type="text/javascript">
        function PaneResized() {
            //var sender = $find('RadSplitter1');
            //var browserHeight = $telerik.$(window).height();
            //sender.set_height(browserHeight - 40);
            //$telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 72);
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmOwnerBudgetCode" runat="server">
 
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
      <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwOwnerBudgetCode">
                    <UpdatedControls>                        
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuOwnerBudgetCodeMain">
                    <UpdatedControls>                   
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                   </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
      <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>   
      
            <eluc:TabStrip ID="MenuOwnerBudgetCodeMain" runat="server" OnTabStripCommand="MenuOwnerBudgetCodeMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>

        <div id="divFind">
            <table id="tblConfigure" width="100%">
                <tr>
                    <td style="width: 10%">
                        <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                    </td>
                    <td style="width: 80%">
                        <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128"
                            OnTextChangedEvent="ucOwner_Onchange" Width="27%" AutoPostBack="true" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>
        </div>
        <br />

                   <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="85%" Width="100%">
                           <telerik:RadPane ID="navigationPane" runat="server" Width="30%" >
                            <eluc:TabStrip ID="MenuOwnerBudgetCodeExport" runat="server" OnTabStripCommand="MenuOwnerBudgetCodeExport_TabStripCommand"></eluc:TabStrip>
                          <eluc:TreeView runat="server" ID="tvwOwnerBudgetCode"  OnNodeClickEvent="ucTree_SelectNodeEvent"></eluc:TreeView>
                           <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
                            </telerik:RadPane>
                           <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" >
                           </telerik:RadSplitBar>
                              <telerik:RadPane ID="contentPane" runat="server" Width="70%" >


                        <table width="70%" cellpadding="5">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></asp:Literal>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtOwnerBudgetcode" CssClass="input_mandatory" Width="300" runat="server"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblOwnerBudgetCodeDescription" runat="server" Text="Owner Budget Code Description"></asp:Literal>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtbudgetdesc" CssClass="input_mandatory" Width="300" runat="server"
                                            Height="60" TextMode="MultiLine"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblSortingOrder" runat="server" Text="Sorting Order"></asp:Literal>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSortingOrder" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblActive" runat="server" Text="Active"></asp:Literal>
                                    </td>
                                    <td>
                                        <telerik:RadCheckBox ID="chkActiveyn" runat="server" />
                                    </td>
                                </tr>
                            </table>
          </telerik:RadPane>
            </telerik:RadSplitter>
    </form>
</body>
</html>
