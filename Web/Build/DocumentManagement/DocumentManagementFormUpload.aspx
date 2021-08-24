<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFormUpload.aspx.cs" Inherits="DocumentManagementFormUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Form Upload</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <br />
        <table id="tblFiles">
            <tr>
                <td>Choose a file
                </td>
                <td colspan="2">   
                    <asp:FileUpload ID="txtFileUpload" runat="server" CssClass="input" />
                </td>
                <td>
                    <asp:ImageButton runat="server" AlternateText="UPLOAD" ImageUrl="<%$ PhoenixTheme:images/upload.png %>"
                        CommandName="UPLOAD" ID="cmdUpload" ToolTip="Upload Form" OnClick="cmdUpload_Click"></asp:ImageButton>
                    <img id="Img5" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                        width="3" />
                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                        CommandName="DELETE" ID="cmdDelete" ToolTip="Delete" OnClick="cmdDelete_Click"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2" align="right">
                    <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View Uploaded Form" runat="server"
                        Height="14px" ToolTip="Download Form">
                    </asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></telerik:RadLabel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
