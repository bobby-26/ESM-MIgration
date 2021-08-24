<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentCompanyResponse.aspx.cs" Inherits="InspectionIncidentCompanyResponse" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bug Edit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuBugDiscussion" runat="server" OnTabStripCommand="MenuBugDiscussion_TabStripCommand" Title="Comments">
            </eluc:TabStrip>
            <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                width="99%">
                <tr>
                    <td align="left" colspan="2">
                        <telerik:RadLabel ID="lblPostYourCommentsHere" runat="server" Text="Post your comments here." ForeColor="Blue" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblComment" runat="server" Text="Comment"></telerik:RadLabel>
                    </td>
                    <td align="left" style="vertical-align: top;">
                        <telerik:RadTextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                            Height="49px" TextMode="MultiLine" Width="700px" Resize="Both"></telerik:RadTextBox>
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
                            <td width="10%">
                                <telerik:RadLabel ID="lblPostedBy" runat="server" Text="Posted By"></telerik:RadLabel>
                                <br />
                                <b>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>
                                </b>
                            </td>
                            <td class="input" width="60%">
                                <telerik:RadLabel ID="lblComments" runat="server" Text="Comments "></telerik:RadLabel>-<br />
                                <div style="height: 54px; float: left; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                    <%# Eval("FLDCOMMENTS")%>
                                </div>
                            </td>
                            <td width="12%">
                                <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                                <br />
                                <b>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDBYNAME")%>
                                </b>
                            </td>
                            <td width="15%">
                                <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                                <br />
                                <b>
                                    <%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPOSTEDDATE")) %>
                                </b>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
