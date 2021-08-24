<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterPOSHProjectCode.aspx.cs" Inherits="Registers_RegisterPOSHProjectCode" %>

<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplit" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Project Code</title>
    <telerik:RadCodeBlock runat="server" ID="DivHeader">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style>
            /*Vertical Splitbars*/
            .rspCollapseBarExpand, .rspCollapseBarExpandOver,
            .rspCollapseBarCollapse, .rspCollapseBarCollapseOver {
                height: 35px !important; /*the height of your button-image */
                line-height: 35px !important; /*the height of your button-image */
                width: 10px !important;
                background-position: 0 !important;
            }

            .RadSplitter .rspCollapseBarExpand:before,
            .RadSplitter .rspCollapseBarCollapse:before {
                font-size: 14px !important;
                width: 10px !important;
            }
        </style>

        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
               && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
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
        <eluc:TabStrip ID="MenuDocumentCategoryMain" runat="server" Title="Project Code" OnTabStripCommand="MenuDocumentCategoryMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" SplitBarsSize="10" runat="server" Height="100%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="30%" Height="100%">
                <eluc:TreeView runat="server" ID="tvwDocumentCategory" RootText="ROOT" OnNodeClickEvent="ucTree_SelectNodeEvent"></eluc:TreeView>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" Height="100%">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server" Width="70%" Height="100%">
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <table>
                        <tr style="position: absolute">
                            <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblProjectId" runat="server"></telerik:RadLabel>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="5">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblProjectSeqno" runat="server" Text="Seq. Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtseqno" runat="server" CssClass="gridinput_mandatory"
                                    onkeypress="return isNumberKey(event)" Width="180px" Height="20px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Project Code
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtProjectCode" Width="180px" Height="20px" ReadOnly="true" Enabled="false" MaxLength="100" runat="server"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Project Title
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtProjectTitle" CssClass="input_mandatory" Width="180px" Height="20px" MaxLength="100" runat="server"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td>Status
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlstatus" runat="server" Width="100px"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Open" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Close" Value="2"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                Sub Accounts
                            </td>
                            <td>
                                <div runat="server" id="dvaccount" class="input" style="overflow: auto; width: 70%; height: 100px">
                                    <asp:CheckBoxList runat="server" ID="chksubaccounts" Height="100%" RepeatColumns="1"
                                        RepeatDirection="Horizontal" AppendDataBoundItems="true"
                                        RepeatLayout="Flow">
                                    </asp:CheckBoxList>
                                </div>
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
