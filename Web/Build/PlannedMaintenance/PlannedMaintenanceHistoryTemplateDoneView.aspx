<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceHistoryTemplateDoneView.aspx.cs"
    Inherits="PlannedMaintenanceHistoryTemplateDoneView" MaintainScrollPositionOnPostback="true" %>

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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
           
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <div class="buttons">                
                <telerik:RadButton runat="server" ID="btnExport" AutoPostBack="false"
                    OnClientClicked="exportFile" Text="Export to Excel File">
                </telerik:RadButton>
            </div>
            <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet1" Height="90%">
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
                            <telerik:SpreadsheetTool Name="Undo" ShowLabel="false" />
                            <telerik:SpreadsheetTool Name="Redo" ShowLabel="false" />
                        </telerik:SpreadsheetToolbarGroup>
                        <telerik:SpreadsheetToolbarGroup>
                            <telerik:SpreadsheetTool Name="Save" ShowLabel="false" />
                        </telerik:SpreadsheetToolbarGroup>
                    </telerik:SpreadsheetToolbarTab>
                </Toolbar>
            </telerik:RadSpreadsheet>
        </telerik:RadAjaxPanel>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                var $ = $ || $telerik.$;                
                function exportFile() {
                    var spreadsheet = $find("<%= RadSpreadsheet1.ClientID %>");
                    spreadsheet.saveAsExcel();
                }
                Telerik.Web.UI.RadSpreadsheet.prototype._onCallbackResponse = function (response, context) {
                    PageMethods.Message("", OnSucceeded, OnFailed);
                }

                function OnSucceeded(data) {
                    var frm = "<%= ViewState["FRM"] %>";
                    radalert(data, 350, 250, "Result");
                    if (data.toLowerCase().indexOf('saved successfully') > -1) {
                        if (frm == null || frm == '')
                            fnReloadList('ExcelTemplate', 'ifMoreInfo');
                        else if (frm == "grpwo") {
                            closeTelerikWindow('ExcelTemplate','template',null);
                        }
                    }          
                }

                function OnFailed(error) {
                    radalert(error.get_message());
                }                
            </script>
        </telerik:RadCodeBlock>        
    </form>
</body>
</html>
