<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSeaProcConfigureFolder.aspx.cs" Inherits="PurchaseSeaProcConfigureFolder" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SeaProcFolder</title>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script lang="javascript" type="text/javascript">
            function gridContextMenuClicked(toolbar, args) {
                var explorer = $find("<%=FileExplorer1.ClientID%>");
                var btn = $find("<%=BtnClick.ClientID%>");
                var selectedItem = explorer.get_selectedItem();
                var buttonValue = args.get_item().get_value();
                btn.set_commandName(buttonValue);
                btn.set_commandArgument(selectedItem.get_path() + "," + selectedItem.get_name());
                if (buttonValue == "OPENFILE") {
                    var wnd = explorer.get_windowManager();
                    wnd.set_title(selectedItem.get_name());
                    //wnd.setUrl("DataTransferXML.aspx?currentpath="+selectedItem.get_path());
                    wnd.show();
                    wnd.maximize();
                }
                else {
                    btn.click();
                }
            }
            function gridContextMenuOpening(sender, args) {
                var explorer = $find("<%=FileExplorer1.ClientID%>");
                var selectedItem = explorer.get_selectedItem();                
                var menu = sender.get_allItems();                
                for (i = 0; i < menu.length; i++) {
                    var item = menu[i];
                    if (selectedItem.isDirectory() && item.get_value() == "Open") continue;                   
                    item.set_visible(false)                    
                    if (selectedItem.isDirectory() && explorer.get_currentDirectory().toUpperCase().endsWith('INBOX/')) {
                        if (item.get_value() == "IMPORT") {
                            item.set_visible(true);
                        }                        
                    }
                }
            }
            function OnGridRowDataBound(oGrid, args) {
                var dataItem = args.get_dataItem();
                if (dataItem.Length != null) {
                    dataItem.Length = formatSizeValue(dataItem.Length);
                }
            }

            function formatSizeValue(originalSizeValue) {
                if (originalSizeValue > 2000) {
                    var valueInKB = originalSizeValue / 1024;// Convert to MB
                    var newValue = Math.round(valueInKB * 100) / 100
                    return String.format("{0} KB", newValue);
                }
                return String.format("{0}", originalSizeValue);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="pageLoad()">
    <form id="form1" runat="server">
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="FileExplorer1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FileExplorer1" UpdatePanelHeight="100%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadFileExplorer RenderMode="Lightweight" runat="server" ID="FileExplorer1" style="width:99.6%; height: 98.4%" EnableCreateNewFolder="false" DisplayUpFolderItem="false">
        </telerik:RadFileExplorer>
        <telerik:RadButton ID="BtnClick" runat="server" CssClass="hidden" CommandArgument="" OnClick="BtnClick_Click"></telerik:RadButton>
    </form>
</body>
</html>
