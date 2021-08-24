<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentSectionContentEditReasonNew.aspx.cs" Inherits="DocumentManagement_DocumentManagementDocumentSectionContentEditReasonNew" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Remarks</title>
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
    <form id="frmOfficeComment" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>        
        <eluc:TabStrip ID="MenuOfficeComment" runat="server" Title="Enter Details Of Change" OnTabStripCommand="MenuOfficeComment_TabStripCommand"></eluc:TabStrip>

        <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
            width="99%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblGrammatical" runat="server" Text="Grammatical correction Y/N"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="cbGrammaticalyn" runat="server" OnCheckedChanged="cbGrammaticalyn_CheckedChanged"></telerik:RadCheckBox>
                </td>
            </tr>
            <tr>
                <td width="15%">Details of Change
                </td>
                <td align="left" style="vertical-align: top;">
                    <telerik:RadTextBox ID="txtOfficeComments" runat="server" CssClass="gridinput"
                        Height="49px" TextMode="MultiLine" Width="600px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
            width="99%" id="tblComments">
            <asp:Repeater ID="repDiscussion" runat="server">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td width="10%">Reviewed By
                            <br />
                            <b>
                                <%#DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>
                            </b>
                        </td>
                        <td class="input" width="60%">Office Remarks -<br />
                            <div style="height: 54px; width: 600px; float: left; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                <%# Eval("FLDOFFICEREMARKS")%>
                            </div>
                        </td>
                        <td width="12%">Name
                            <br />
                            <b>
                                <%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDBYNAME")%>
                            </b>
                        </td>
                        <td width="15%">Date
                            <br />
                            <b>
                                <%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDDATE")%>
                            </b>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
        </table>



        <%--    <table cellpadding="1" cellspacing="1" width="25%">
        <tr>
            <td colspan="4">
                <b><asp:Literal ID="lblOfficeComment" runat="server" Text="Enter Details of Change"></asp:Literal></b>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:TextBox ID="txtOfficeComments" runat="server" CssClass="input_mandatory" Height="100px" Rows="4"
                    TextMode="MultiLine" Width="250%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Width="150%"></asp:Label>
            </td>
        </tr>
    </table>
        --%>

        <div>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Width="150%"></asp:Label>
                </td>
            </tr>
        </div>
    </form>
</body>
</html>
