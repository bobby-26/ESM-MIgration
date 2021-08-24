<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceHistoryTemplateTemp.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceHistoryTemplateTemp" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    </telerik:RadCodeBlock>
    <style>
        .buttons {
            padding-bottom: 10px;
        }
        .RadSpreadsheet .rssSheetsbar a.t-spreadsheet-sheets-bar-add {
            display: none;
        }
        .RadSpreadsheet .rssSheetsbar span.t-spreadsheet-sheets-remove {
            display: none;
        }
    </style>
</head>
<body>
    <form id="formPMHistoryTemplate" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnablePageMethods="true" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />        
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>  
            <div class="buttons">
                <telerik:RadAsyncUpload runat="server" ID="btnImport" HideFileInput="true" AllowedFileExtensions="xlsx"
                    OnClientFileSelected="importFile" OnClientFileDropped="importFile" Localization-Select="Import From Excel File">
                </telerik:RadAsyncUpload>               
            </div>
            <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet1" Height="90%" CssClass="hidden">
                <ContextMenus>
                    <CellContextMenu>
                        <Items>
                            <telerik:RadMenuItem Text="Copy" Value="CommandCopy" Visible="false"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="Paste" Value="CommandPaste" Visible="false"></telerik:RadMenuItem>
                        </Items>
                    </CellContextMenu>
                    <RowHeaderContextMenu>
                        <Items>
                            <telerik:RadMenuItem Text="HideRow" Value="CommandHideRow" Visible="false"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="DeleteRow" Value="CommandDeleteRow"  Visible="false"></telerik:RadMenuItem>
                        </Items>
                    </RowHeaderContextMenu>
                    <ColumnHeaderContextMenu>
                        <Items>
                            <telerik:RadMenuItem Text="HideColumn" Value="CommandHideColumn" Visible="false"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="DeleteColumn" Value="CommandDeleteColumn" Visible="false"></telerik:RadMenuItem>
                        </Items>
                    </ColumnHeaderContextMenu>
                </ContextMenus>
                <Toolbar>
                    <telerik:SpreadsheetToolbarTab Text="Home">
                        <telerik:SpreadsheetToolbarGroup>                           
                            <telerik:SpreadsheetTool Name="ExportAs" ShowLabel="false" />
                        </telerik:SpreadsheetToolbarGroup>
                        <telerik:SpreadsheetToolbarGroup>
                            <telerik:SpreadsheetTool Name="Save" ShowLabel="false" />
                        </telerik:SpreadsheetToolbarGroup>
                        <telerik:SpreadsheetToolbarGroup>
                            <telerik:SpreadsheetTool Name="Undo" ShowLabel="false" />
                            <telerik:SpreadsheetTool Name="Redo" ShowLabel="false" />
                        </telerik:SpreadsheetToolbarGroup>
                        <telerik:SpreadsheetToolbarGroup>
                            <telerik:SpreadsheetTool Name="Bold" ShowLabel="false" />
                            <telerik:SpreadsheetTool Name="Italic" ShowLabel="false" />
                            <telerik:SpreadsheetTool Name="Underline" ShowLabel="false" />
                        </telerik:SpreadsheetToolbarGroup>
                        <telerik:SpreadsheetToolbarGroup>
                            <telerik:SpreadsheetTool Name="InsertComment" ShowLabel="false" />
                        </telerik:SpreadsheetToolbarGroup>
                        <telerik:SpreadsheetToolbarGroup>
                            <telerik:SpreadsheetTool Name="InsertImage" ShowLabel="false" />
                        </telerik:SpreadsheetToolbarGroup>                        
                    </telerik:SpreadsheetToolbarTab>
                </Toolbar>
            </telerik:RadSpreadsheet>
        </telerik:RadAjaxPanel>        
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                var $ = $ || $telerik.$;  
                function importFile(sender, args) {
                    var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
                    if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
                        if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
                            parent.radalert("This file type is not supported. supported file types are ." + sender.get_allowedFileExtensions());
                            $telerik.$(args.get_row()).remove();
                            return;
                        }
                    }
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
                        setTimeout(function () { parent.validateImport(file); }, 500);
                    }
                    $telerik.$(args.get_row()).remove();
                }
            </script>
        </telerik:RadCodeBlock>        
    </form>
</body>
</html>
