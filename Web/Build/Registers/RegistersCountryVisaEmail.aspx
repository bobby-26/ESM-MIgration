<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCountryVisaEmail.aspx.cs"
    Inherits="RegistersCountryVisaEmail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<%--<script runat="server">
    [System.Web.Services.WebMethod]
    public static void Message(string sessionid, string filename)
    {
        try
        {
            string destPath = HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/" + sessionid + "/" + filename);
            System.IO.File.Delete(destPath);
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
        }
    }
</script>--%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Country Visa Email</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
     
        <script lang="javascript" type="text/javascript">

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
        <style>
            .RadFileExplorer .rtPlus {
                display: none !important;
            }

            .RadFileExplorer .rtMinus {
                display: none !important;
            }

            .rspFirstItem {
                display: none !important;
            }

            .rfeAddressBox {
                display: none !important;
            }

            .rfePaneGrid {
                width: 697px !important;
                height: 170px !important;
            }

            .rspResizeBar {
                display: none !important;
            }

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
        </style>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCorrespondenceEmail" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="TabStrip1" runat="server" OnTabStripCommand="CorrespondenceEmail_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />       
            <eluc:TabStrip ID="MenuCorrespondenceEmail" runat="server" OnTabStripCommand="CorrespondenceEmail_TabStripCommand"></eluc:TabStrip>

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr id="trFrom" runat="server">
                    <td></td>
                    <td>
                        <telerik:RadTextBox ID="txtFrom" Width="500px" runat="server" CssClass="input" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="trTO" runat="server">
                    <td>To
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTO" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="trCC" runat="server">
                    <td>Cc
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="trBCC" runat="server">
                    <td>Bcc
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>               
                <tr>
                    <td>Subject
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>            
                <tr>
                    <td colspan="2">
                        <telerik:RadFileExplorer RenderMode="Lightweight" runat="server" ID="FileExplorer1" Width="698px" Height="200px" DisplayUpFolderItem="false"
                            EnableCopy="false" EnableCreateNewFolder="false" EnableOpenFile="true" OnClientFileOpen="onClientItemSelected">
                            <Configuration EnableAsyncUpload="true"></Configuration>
                        </telerik:RadFileExplorer>
                    </td>
                  
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblnote" runat="server" Visible="false" Text="Note:Double click on the attachment name to see the details"></telerik:RadLabel>
                    </td>
                </tr>               
            </table>
            <telerik:RadEditor ID="txtBody" runat="server" Width="99%" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            </telerik:RadEditor>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
