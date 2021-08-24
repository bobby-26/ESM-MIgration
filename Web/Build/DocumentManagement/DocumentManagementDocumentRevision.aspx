<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentRevision.aspx.cs" Inherits="DocumentManagementDocumentRevision" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Revisions</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form runat="server" id="form1">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <br />
    <br />
    <br />
    <asp:Repeater ID="repRevision" runat="server" OnPreRender="repRevision_PreRender">
        <HeaderTemplate>
            <table width="85%" align="center" cellpadding="2" cellspacing="0" border="1" bordercolor="black"
                style="border-collapse: collapse; border-style: solid;" mce_style="border-collapse:collapse; border-style:solid;">
                <tr>
                    <th width="15%">
                        Document Revision
                    </th>
                    <th width="55%">
                        Section
                    </th>
                    <th width="15%">
                        Section Revision
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td width="15%" id="tdDocumentRevision" runat="server" valign="top">
                    <b>
                        <%#DataBinder.Eval(Container, "DataItem.FLDDOCUMENTREVISION")%>
                    </b>
                </td>
                <td width="55%" style="word-wrap: break-word; white-space: normal;">
                    <%# Eval("FLDSECTIONNAME")%>
                </td>
                <td width="15%">
                    <%#DataBinder.Eval(Container, "DataItem.FLDSECTIONREVISION")%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <table id="tblFooter" runat="server" width="85%" align="center" cellpadding="2" cellspacing="0"
        visible="false" border="1" bordercolor="black" style="border-collapse: collapse;
        border-style: solid;" mce_style="border-collapse:collapse; border-style:solid;">
        <tr>
            <td colspan="3" align="center">
                NO REVISIONS
            </td>
        </tr>
    </table>
    <br />
    <br />
    </form>
</body>
</html>
