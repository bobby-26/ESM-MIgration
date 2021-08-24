<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCDISIREMatrixComments.aspx.cs" Inherits="Inspection_InspectionCDISIREMatrixComments" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CDISIRE Matrix Comments</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function resizediv() {
                var tbl = document.getElementById("tblComments");
                if (tbl != null) {
                    for (var i = 0; i < tbl.rows.length; i++) {
                        tbl.rows[i].cells[2].getElementsByTagName("div")[0].style.width = tbl.rows[i].cells[2].offsetWidth + "px";
                    }
                }
            } //script added for fixing Div width for the comments table
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--<eluc:Title runat="server" ID="ucTitle" Text="Comments" ShowMenu="false"></eluc:Title>--%>
        <eluc:TabStrip ID="MenuBugDiscussion" runat="server" Title="Comments" OnTabStripCommand="MenuBugDiscussion_TabStripCommand"></eluc:TabStrip>
        <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
            width="99%">
            <tr>
                <td width="10%">Comment
                </td>
                <td align="left" style="vertical-align: top;">
                    <telerik:RadTextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                        Height="49px" TextMode="MultiLine" Width="600px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
            width="99%" id="tblComments" name="tblComments">
            <asp:Repeater ID="repDiscussion" runat="server">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td width="10%">Posted By
                            <br />
                            <b>
                                <%#DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>
                            </b>
                        </td>
                        <td class="input" width="60%">Comments -<br />
                            <div style="height: 54px; width: 600px; float: left; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                <%# Eval("FLDCOMMENTS")%>
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
    </form>
</body>
</html>
