<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSFileNoAttachmentUpload.aspx.cs"
    Inherits="DocumentManagementFMSFileNoAttachmentUpload" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FMS File No</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
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
                    <telerik:RadLabel ID="lblvessellist" runat="server" Text="Vessel">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ddlvessel" runat="server" CssClass="input" AppendDataBoundItems="true"
                       SyncActiveVesselsOnly="true" AutoPostBack="true" Width="270px" />
                </td>
            </tr>
            <tr>
                <td width="11.23%">
                    <telerik:RadLabel ID="lblFilenos" runat="server" Text="File No">
                    </telerik:RadLabel>
                </td>
                <td width="63%">
                    <telerik:RadComboBox ID="ddlFileNo" Width="270px" runat="server" Filter="Contains"
                        EmptyMessage="Type to Select" CssClass="input_mandatory">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblfileUpload" runat="server" Text="Upload File">
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
