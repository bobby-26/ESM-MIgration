<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementCategory.aspx.cs" Inherits="DocumentManagementCategory" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Document Category</title>
    <telerik:RadCodeBlock runat="server" ID="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
    <script language="Javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
           && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmDocumentCategory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxPanel1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwDocumentCategory">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tvwDocumentCategory" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuDocumentCategoryMain" runat="server" OnTabStripCommand="MenuDocumentCategoryMain_TabStripCommand"></eluc:TabStrip>
        </div>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="93%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="30%" Height="100%">
                <eluc:TreeView runat="server" ID="tvwDocumentCategory" Height="100%" OnNodeClickEvent="ucTree_SelectNodeEvent"></eluc:TreeView>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" Height="100%">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Width="70%" Height="100%">
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="Status1" />
                    <table>
                        <tr style="position: absolute">
                            <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCategoryId" runat="server"></telerik:RadLabel>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>Category Name
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDocumentCategory" CssClass="input_mandatory" Width="180px" MaxLength="100" runat="server"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Category Number
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCategoryNumber" runat="server" CssClass="input_mandatory"
                                    onkeypress="return isNumberKey(event)" Width="180px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Operational Document</td>
                            <td>
                                <telerik:RadComboBox ID="ddlType" runat="server" Width="200px" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Company</td>
                            <td>
                                <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="180px" />
                            </td>
                        </tr>
                        <tr>
                            <td>Active
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkActiveyn" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%" valign="center">
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkCheckAll" Font-Bold="true" runat="server" Text="Check All" AutoPostBack="true"
                                OnCheckedChanged="chkCheckAll_CheckedChanged" />                                
                            </td>
                        </tr>
                        <tr>
                            <td>                                
                            </td>
                            <td>
                                <telerik:RadCheckBoxList ID="chkVesselType" runat="server" AutoPostBack="true" Style="overflow-y: auto; overflow-x: auto; width: 420px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd" Direction="Vertical" Columns="2">
                                </telerik:RadCheckBoxList>
                            </td>
                        </tr>
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </telerik:RadAjaxPanel>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
