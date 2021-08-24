<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDebriefingEmail.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreDebriefingEmail" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mail Compose</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:TabStrip ID="MenuMailRead" runat="server" OnTabStripCommand="MenuMailRead_TabStripCommand"></eluc:TabStrip>
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" border="1">
                <tr>
                    <td width="40%">
                        <table width="100%">
                            <tr>
                                <td>To
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtTo" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Cc
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtCc" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Subject
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSubject" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="40%"></td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="2" width="100%">
                        <telerik:RadEditor ID="edtBody" runat="server" Width="100%" Height="320px" CssClass="readonlytextbox">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
