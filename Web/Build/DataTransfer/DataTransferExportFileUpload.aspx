<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferExportFileUpload.aspx.cs" Inherits="DataTransferExportFileUpload" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Synchronizer Import</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuUtilities" runat="server" OnTabStripCommand="MenuUtilities_TabStripCommand"></eluc:TabStrip>
        <p>
            <b>Upload the Zip file here to Import. <i> The Uploaded Zip file will be placed in the INBOX folder.</i></b>
        </p>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblzipfile" runat="server" Text="Upload the Zip file here"></asp:Label>
                </td>
                <td>
                    <asp:FileUpload ID="filUpload" runat="server" /><br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFilename" runat="server" Text="File Name : " Visible="false"></asp:Label>
                </td>
                <td>

                    <asp:Label ID="lblFileUploadStatus" runat="server"></asp:Label></td>
            </tr>
        </table>

    </form>
</body>
</html>
