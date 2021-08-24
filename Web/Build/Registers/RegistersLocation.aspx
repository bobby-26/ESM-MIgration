<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersLocation.aspx.cs"
    Inherits="RegistersLocation" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized() {
            var sender = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight - 40);
            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 72);
        }
    </script>

</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersLocation" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwLocation">
                    <UpdatedControls>                        
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="MenuLocation">
                    <UpdatedControls>                   
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuLocation" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="MenuLocation_TabStripCommand"></eluc:TabStrip>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="200px" Width="100%">
                <telerik:RadPane ID="navigationPane" runat="server" Width="200">
                    <eluc:TabStrip ID="MenuExport" runat="server" OnTabStripCommand="MenuExport_TabStripCommand"></eluc:TabStrip>
                    <eluc:TreeView runat="server" ID="tvwLocation" OnNodeClickEvent="ucTree_SelectNodeEvent"></eluc:TreeView>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server">
                    <div id="Details" runat="server">
                        <table cellpadding="5">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblLocationcode" runat="server" Text="Location Code"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblLocationID" runat="server" Text='' Visible="false"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtLocationCode" runat="server" CssClass="input" MaxLength="6"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblLocationname" runat="server" Text="Location Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtLocationName" runat="server" Text=''
                                        CssClass="input_mandatory" MaxLength="100">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblDept" runat="server" Text="Department"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadDropDownList runat="server" ID="ddlDepartment" CssClass="input"></telerik:RadDropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPane>
            </telerik:RadSplitter>
    </form>
</body>
</html>
