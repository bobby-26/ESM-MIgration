<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceHistoryExcelTemplate.aspx.cs"
    Inherits="PlannedMaintenanceHistoryExcelTemplate" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Excel Template</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    </telerik:RadCodeBlock>
    <style>
        .buttons {
            padding-bottom: 15px;
        }

        .RadAsyncUpload {
            float: left;
            padding-right: 40px;
        }
        .RadSpreadsheet .k-spreadsheet-clipboard-paste,
        .RadSpreadsheet .k-spreadsheet-clipboard {
            position: fixed;
        }

        .k-state-disabled a {
            pointer-events: auto !important;
        }
    </style>
</head>
<body>
    <form id="formPMHistoryTemplate" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadSpreadsheet1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadSpreadsheet1" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="83%" />                        
                        <telerik:AjaxUpdatedControl ControlID="ucError" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="txtReportDateCell">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="txtReportDateCell" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="83%" /> 
                        <telerik:AjaxUpdatedControl ControlID="ucError" LoadingPanelID="RadAjaxLoadingPanel1" />                       
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>        
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="buttons">
            <telerik:RadAsyncUpload runat="server" ID="btnImport" HideFileInput="true" AllowedFileExtensions=".xlsx, .xls"
                OnClientFileSelected="importFile" OnClientFileDropped="importFile" Localization-Select="Import From Excel File">
            </telerik:RadAsyncUpload>
            <telerik:RadButton runat="server" ID="btnExport" AutoPostBack="false"
                OnClientClicked="exportFile" Text="Export to Excel File">
            </telerik:RadButton>
        </div>        
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Report Date Cell Ref"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtReportDateCell" runat="server" MaxLength="10" AutoPostBack="true" OnTextChanged="txtReportDateCell_TextChanged"></telerik:RadTextBox>
                </td>
            </tr>
        </table>    
        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet1" Height="100%">
            <ContextMenus>
                <CellContextMenu OnClientItemClicked="CellContextMenuItemClicked">
                    <Items>
                       <%-- <telerik:RadMenuItem Text="Cut" Value="CommandCut" SpriteCssClass="t-efi t-efi-cut"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Copy" Value="CommandCopy" SpriteCssClass="t-efi t-efi-copy"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Paste" Value="CommandPaste" SpriteCssClass="t-efi t-efi-paste"></telerik:RadMenuItem>--%>
                        <telerik:RadMenuItem Text="Enable/Disable" Value="EnableDisable" SpriteCssClass="t-efi t-efi-track-changes-enable"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Vessel" Value="Vessel" SpriteCssClass="fas fa-ship"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Component Name" Value="ComponentName" SpriteCssClass="fas fa-sitemap"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Component Number" Value="ComponentNumber" SpriteCssClass="fas fa-sitemap"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Make" Value="Make" SpriteCssClass="fab fa-monero"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Type" Value="Type" SpriteCssClass="fas fa-industry"></telerik:RadMenuItem>
                    </Items>
                </CellContextMenu>
            </ContextMenus>
        </telerik:RadSpreadsheet>

        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                var $ = $ || $telerik.$;
                function f() {
                    var spread = $find("<%= RadSpreadsheet1.ClientID%>");
                    var homeToolbar = $(spread.get_element()).find(".RadToolBar")[0].control;
                    // or
                    // var homeToolbar = $(spread.get_element()).find(".rtbButton[title=Save]").closest('.RadToolBar')[0].control;
                    homeToolbar.findItemByText("Save").hide();
                    // Sys.Application.remove_load(f);
                }
                function exportFile() {
                    var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                    spreadsheet.saveAsExcel();
                }
                function importFile(sender, args) {
                    var file = null;
                    if (args.get_file) {
                         file = args.get_file();
                    }
                    else if (!(Telerik.Web.Browser.ie && Telerik.Web.Browser.version == "9")) {
                         file = args.get_fileInputField().files[0];
                    }
                    if (file) {
                        var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                        spreadsheet.fromFile(file);
                    }
                    $telerik.$(args.get_row()).remove();
                }
                window.CellContextMenuItemClicked = function (sender, eventArgs) {

                    var selectedItemValue = eventArgs.get_item().get_value();

                    if (selectedItemValue === "EnableDisable") {
                        var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                        var range = spreadsheet.get_activeSheet().get_selection();
                        var isEnabled = range.get_enabled();
                        range.set_enabled(!isEnabled);
                    } else if (selectedItemValue === "Vessel") {
                        var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                        var range = spreadsheet.get_activeSheet().get_selection();
                        range.set_value('{#VESSEL#}');
                    } else if (selectedItemValue === "ComponentName") {
                        var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                        var range = spreadsheet.get_activeSheet().get_selection();
                        range.set_value('{#COMPONENTNAME#}');
                    } else if (selectedItemValue === "ComponentNumber") {
                        var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                        var range = spreadsheet.get_activeSheet().get_selection();
                        range.set_value('{#COMPONENTNUMBER#}');
                    } else if (selectedItemValue === "Make") {
                        var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                        var range = spreadsheet.get_activeSheet().get_selection();
                        range.set_value('{#MAKE#}');
                    } else if (selectedItemValue === "Type") {
                        var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                        var range = spreadsheet.get_activeSheet().get_selection();
                        range.set_value('{#TYPE#}');
                    }
                }
                Telerik.Web.UI.RadSpreadsheet.prototype._onCallbackResponse = function (response, context) {
                    radalert("Saved Successfully", 350, 250, "Result");
                }
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
