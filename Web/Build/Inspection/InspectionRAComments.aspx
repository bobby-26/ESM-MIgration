<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAComments.aspx.cs" Inherits="InspectionRAComments" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>JHA / RA Comments</title>
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
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuRAComments" runat="server" Title="JHA / RA Comments" OnTabStripCommand="MenuRAComments_TabStripCommand"></eluc:TabStrip>
        <table width="99%">
            <tr>
                <td align="left" colspan="4">
                    <font color="blue" size="0">
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Post your comments here." ForeColor="Blue"></telerik:RadLabel>
                    </font>
                </td>
            </tr>
            <tr>
                <td width="10%">
                    <telerik:RadLabel ID="lblComment" runat="server" Text="Comment"></telerik:RadLabel>
                </td>
                <td align="left" style="vertical-align: top;">
                    <telerik:RadTextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                        Height="49px" TextMode="MultiLine" Width="600px" Resize="Both">
                    </telerik:RadTextBox>
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
                        <td width="10%">
                            <telerik:RadLabel ID="lblPosted" runat="server" Text="Posted By"></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblPostedname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>' Font-Bold="true"></telerik:RadLabel>
                        </td>
                        <td class="input" width="60%">
                            <telerik:RadLabel ID="lblcomment" runat="server" Text="Comment - "></telerik:RadLabel>
                            <br />
                            <div style="height: 54px; width: 600px; float: left; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                <telerik:RadLabel ID="lblcommentname" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMMENTS")%>' Font-Bold="true"></telerik:RadLabel>
                            </div>
                        </td>
                        <td width="12%">
                            <telerik:RadLabel ID="lblpostedby" runat="server" Text="Name"></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblpostedbyname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDBYNAME")%>' Font-Bold="true"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lbldate" runat="server" Text="Date"></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblpostedDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDDATE")%>' Font-Bold="true"></telerik:RadLabel>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
</telerik:RadAjaxPanel>
    </form>
</body>
</html>
