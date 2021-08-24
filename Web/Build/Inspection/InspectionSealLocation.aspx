<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealLocation.aspx.cs" Inherits="InspectionSealLocation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Location</title>
    <telerik:RadCodeBlock ID="rad1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
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
    <form id="frmInspectionLocation" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwLocation">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuLocation">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuLocation" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuInspectionLocation" runat="server" OnTabStripCommand="MenuInspectionLocation_TabStripCommand"></eluc:TabStrip>

            <eluc:Status ID="ucStatus" runat="server" />

            <eluc:TabStrip ID="MenuExport" runat="server" OnTabStripCommand="MenuExport_TabStripCommand"></eluc:TabStrip>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="200px" Width="100%">
                <telerik:RadPane ID="navigationPane" runat="server" Width="200">
                    <eluc:TreeView runat="server" ID="tvwLocation" OnNodeClickEvent="tvwLocation_NodeClickEvent"></eluc:TreeView>
                    <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
                </telerik:RadPane>

                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server">



                    <div id="Details" runat="server">
                        <table width="100%" cellpadding="5">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblLocationName" runat="server" Text="Location Name"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblLocationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblParentLocationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTLOCATIONID") %>'></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtLocationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONCODE") %>'
                                        CssClass="input_mandatory" Width="300px">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblLocationNo" runat="server" Text="Location Number"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtLocationNo" runat="server" CssClass="input" Width="100px" onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblSealPointYN" runat="server" Text="Seal Point Y/N"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkSealPoint" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </div>
        <eluc:Confirm ID="ucConfirmComplete" runat="server" OnConfirmMesage="btnComplete_Click"
            OKText="Yes" CancelText="No" />
    </form>
</body>

</html>
