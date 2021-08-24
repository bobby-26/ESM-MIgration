<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferBrowseFolder.aspx.cs" Inherits="DataTransferBrowseFolder" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Folders</title>
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
                    wnd.setUrl("DataTransferXML.aspx?currentpath="+selectedItem.get_path());
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
                    if (selectedItem.isDirectory() && explorer.get_currentDirectory().toUpperCase().endsWith('IMPORT/')) {
                        if (item.get_value() == "ARCHIVERESTORE"
                            || item.get_value() == "DELETE") {
                            item.set_visible(true);
                        }                        
                    }
                    else {                        
                        if ((explorer.get_currentDirectory().toUpperCase().endsWith('MERGE/')
                            || explorer.get_currentDirectory().toUpperCase().endsWith('MAIL/OUT/')
                            || explorer.get_currentDirectory().toUpperCase().endsWith('MAIL/TRASH/'))) {
                            if (item.get_value() == "MOVETOINBOX"
                                || item.get_value() == "MAILTOVESSEL"
                                || item.get_value() == "MAILTOUSER") {
                                item.set_visible(true);
                            }             
                        }                        
                        if (item.get_value() == "OPENFILE" && (selectedItem.get_name().includes("TBLAUDIT") || selectedItem.get_name().includes("TBLCREWAUDIT"))) {
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
            function onClientItemSelected(fileExplorer, args) {
                if (args.get_item().get_type() == Telerik.Web.UI.FileExplorerItemType.File) {
                    // Cancel the default dialog;
                    args.set_cancel(true);
                    var filepath = args.get_item().get_path();
                    var names = (filepath).split('/');
                    var filename = names[2];
                    var ext = filename.split('.');

                    window.radopen("../common/FileDownload.aspx?filename=" + filename + "&filepath=" + filepath + "&mod=CREW", "Email Attachment");
                }
                else {
                    // if the item is a folder        
                    alert("The selected item is a directory");
                }

            }
        </script>
       
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="DataTransferFolder" runat="server" OnTabStripCommand="DataTransferFolder_TabStripCommand"></eluc:TabStrip>

         <%-- <table width="100%">
                         
                            <tr>
                                <td>
                                    <%--Up One Folder--%>
                                  <%--  <asp:Label id="lblVessel" runat="server" Text="Show folders of"></asp:Label>
                                </td>
                                <td>
                                    <eluc:Vessel runat="server" ID="ucVessel" CssClass="input" AutoPostBack="true" AppendDataBoundItems="true" />
                                   
                                </td>
                            </tr>                            
                        </table>--%>

        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">

            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="FileExplorer1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="FileExplorer1" UpdatePanelHeight="94%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadFileExplorer runat="server" ID="FileExplorer1" Width="99.5%" Height="94%" 
            EnableCreateNewFolder="false" EnableOpenFile="false" DisplayUpFolderItem="false">               
        </telerik:RadFileExplorer>
        <telerik:RadButton ID="BtnClick" runat="server" CssClass="hidden" CommandArgument="" OnClick="BtnClick_Click"></telerik:RadButton>
    </form>    
</body>
</html>
