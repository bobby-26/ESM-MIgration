<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenaneManualsExplorer.aspx.cs" Inherits="PlannedMaintenaneManualsExplorer" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manuals</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .leftPane {
            padding-right: 10px;
            margin-right: 20px;
        }

        .leftPane, .rightPane {
            float: left;
        }

        .previmage {
            width: 230px;
            height: 220px;
            vertical-align: middle;
        }

        #pvwImage {
            display: none;
            margin: 10px;
            width: 200px;
            height: 180px;
            vertical-align: middle;
        }

        .size-custom {
            width: 815px;
        }
    </style>
</head>
<body onload="pageLoad()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="Configuratorpanel1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FileExplorer1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadFileExplorer RenderMode="Lightweight" runat="server" ID="FileExplorer1" Width="100%" OnClientFileOpen="OnClientFileOpen" >
        </telerik:RadFileExplorer>
    </form>
    <script type="text/javascript">
        (function (global, undefined) {
            //A function that will return a reference to the parent radWindow in case the page is loaded in a RadWindow object
            function getRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function OnClientFileOpen(sender, args) {// Called when a file is open.
                var item = args.get_item();

                //If file (and not a folder) is selected - call the OnFileSelected method on the parent page
                if (item.get_type() == Telerik.Web.UI.FileExplorerItemType.File) {
                    // Cancel the default dialog;
                    args.set_cancel(true);

                    // get reference to the RadWindow
                    var wnd = getRadWindow();

                    //Get a reference to the opener parent page using RadWndow
                    var openerPage = wnd.BrowserWindow;

                    //if you need the URL for the item, use get_url() instead of get_path()
                    openerPage.OnFileSelected(item.get_path());// Call the method declared on the parent page

                    //Close the window which hosts this page
                    wnd.close();
                }
            }

            global.OnClientFileOpen = OnClientFileOpen;
        })(window);
    </script>
</body>
</html>
