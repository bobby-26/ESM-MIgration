<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSShipboardFormsAttachmentUpload.aspx.cs"
    Inherits="DocumentManagementFMSShipboardFormsAttachmentUpload" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseUrlModelWindow() {
                if (typeof parent.CloseUrlModelWindow === "function") { parent.CloseUrlModelWindow(); };
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <eluc:TabStrip ID="MenuFMSFileNo" runat="server" OnTabStripCommand="FMSFileNo_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No" Visible="false">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFileNumber" runat="server" Width="400px" CssClass="readonlytextbox" ReadOnly="true" Visible="false"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblfileUpload" runat="server" Text="Upload File" >
                    </telerik:RadLabel>
                </td>
                <td>
                    <asp:FileUpload ID="txtFileUpload" runat="server" CssClass="input" />
                    <%--   <asp:ImageButton runat="server" AlternateText="UPLOAD" ImageUrl="<%$ PhoenixTheme:images/upload.png %>"
                        CommandName="UPLOAD" ID="cmdUpload" ToolTip="Upload Form" OnClick="cmdUpload_Click"></asp:ImageButton>--%>
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
