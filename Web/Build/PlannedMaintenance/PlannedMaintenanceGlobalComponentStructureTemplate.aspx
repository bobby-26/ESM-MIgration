<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalComponentStructureTemplate.aspx.cs"
    Inherits="PlannedMaintenanceGlobalComponentStructureTemplate" MaintainScrollPositionOnPostback="true" %>

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
                <telerik:RadAsyncUpload runat="server" ID="btnImport" HideFileInput="true" AllowedFileExtensions=".xlsx, .xls"
                    OnClientFileSelected="importFile" OnClientFileDropped="importFile" Localization-Select="Import From Excel File">
                </telerik:RadAsyncUpload>
                <telerik:RadButton runat="server" ID="btnExport" AutoPostBack="false"
                    OnClientClicked="exportFile" Text="Export to Excel File">
                </telerik:RadButton>
            </div>     
        <telerik:RadSpreadsheet runat="server" ID="RadSpreadsheet1" Height="94%">
            <Toolbar>
                <telerik:SpreadsheetToolbarTab Text="Home">
                    <telerik:SpreadsheetToolbarGroup>
                        <telerik:SpreadsheetTool Name="Save" ShowLabel="true" />
                        <%--<telerik:SpreadsheetTool Name="Open" ShowLabel="true" />
                        <telerik:SpreadsheetTool Name="ExportAs" ShowLabel="true" />--%>
                        <telerik:SpreadsheetTool Name="Sort" ShowLabel="true"/>
                    </telerik:SpreadsheetToolbarGroup>
                </telerik:SpreadsheetToolbarTab>
            </Toolbar>
        </telerik:RadSpreadsheet>
            </telerik:RadAjaxPanel>
       
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
                function pageLoad() {
                    var spreadsheet = $find("<%=RadSpreadsheet1.ClientID%>");
                    var activeSheet = spreadsheet.get_activeSheet();
                    activeSheet.set_frozenRows(1);
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
                //Telerik.Web.UI.RadSpreadsheet.prototype._onCallbackResponse = function (response, context) {
                //    radalert("Saved Successfully", 350, 250, "Result");
                //}
                Telerik.Web.UI.RadSpreadsheet.prototype._onCallbackResponse = function (response, context) {
                    PageMethods.Message("", OnSucceeded, OnFailed);
                }

                function OnSucceeded(data) {
                    radalert(data, 350, 250, "Result");
                    //if (data.toLowerCase().indexOf('saved successfully') > -1) {
                        
                    //}                    
                }

                function OnFailed(error) {
                    radalert(error.get_message());
                }            

            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
