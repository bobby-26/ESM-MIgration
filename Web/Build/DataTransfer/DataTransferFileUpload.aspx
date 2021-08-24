<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferFileUpload.aspx.cs" Inherits="DataTransferFileUpload" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align:top; width:100%; position:absolute; z-index:+1">
        <div class="subHeader" style="position: relative">                    
            <div id="divHeading">
                <eluc:Title runat="server" ID="ucTitle" Text="Data Synchronizer - Utilities" ShowMenu="false" />                                 
            </div>
        </div>              
        <div style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuUtilities" runat="server" OnTabStripCommand="MenuUtilities_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <p>
            <b>Upload the zip file here to import. The file that you upload will be placed
                in the INBOX folder.</b>
        </p>
        <table width="100%">
            <tr>
                <td>
                    Upload the Zip file here
                </td>
                <td>
                    <asp:FileUpload ID="filUpload" runat="server" /><br />
                    <asp:Label ID="lblFileUploadStatus" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Password (To force import the uploaded file)
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtForceImportPassword" TextMode="Password"></asp:TextBox>
                </td>
            </tr>           
        </table>
        
        
    </div>
    </form>
</body>
</html>
