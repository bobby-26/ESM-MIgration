<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOwnerBudgetGroup.aspx.cs"
    Inherits="RegistersOwnerBudgetGroup" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Owner Budget Group</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
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
                <telerik:AjaxSetting AjaxControlID="tvwOwnerBGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="90%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuOwnerBGroup">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="90%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>



        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />

        <eluc:TabStrip ID="MenuOwnerBGroup" runat="server" OnTabStripCommand="OwnerBGroup_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                    <div id="divFind">

        <table id="tblConfigure" width="100%">
            <tr>
                <td>
                    <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                </td>
                <td>
                    <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128" OnTextChangedEvent="ucOwner_Changed" Width="27%"
                        AutoPostBack="true" AppendDataBoundItems="true" />
                </td>
            </tr>
        </table>
                                </div>

        <br />

        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="85%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="30%" >
                <eluc:TabStrip ID="MenuOwnerBGroupExport" runat="server" OnTabStripCommand="OwnerBGroupExport_TabStripCommand"></eluc:TabStrip>
                <eluc:TreeView runat="server" ID="tvwOwnerBGroup" OnNodeClickEvent="ucTree_SelectNodeEvent"></eluc:TreeView>
                <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Width="70%" >
             
                   <table width="40%" cellpadding="5">
                        <tr>
                            <td width="50%">
                                <asp:Literal ID="lblBudgetGroupName" runat="server" Text="Budget Group Name"></asp:Literal>
                                <telerik:RadLabel ID="lblParentGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOwnerBGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtOwnerBGroupName" runat="server" CssClass="input_mandatory" MaxLength="200"></telerik:RadTextBox>
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
                    </table>

            </telerik:RadPane>
        </telerik:RadSplitter>
  
    </form>
</body>
</html>
