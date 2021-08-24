<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonRemarks.aspx.cs"
    Inherits="CommonRemarks" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="../UserControls/UserControlUserName.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Licence Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
<body onload="resizediv();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <eluc:TabStrip ID="Remarks" runat="server" OnTabStripCommand="Remarks_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <br />
        <div>
            <table>
                <tr>
                    <td align="left" colspan="2">
                        <telerik:RadLabel ID="lblPostYourCommentsHere" runat="server" Text="Post Your Comments Here"></telerik:RadLabel>
                    </td>
                    <td align="left" style="vertical-align: top;" colspan="2">
                        <telerik:RadTextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory" Resize="Both"
                            Height="49px" TextMode="MultiLine" Width="692px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSearchBy" runat="server" Text="Search By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUser" runat="server" Text="User"></telerik:RadLabel>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <eluc:User ID="ucUser" runat="server" AppendDataBoundItems="true" Width="220px"
                            ActiveYN="172" />
                        <asp:ImageButton ID="ImgSearch" runat="server" ImageAlign="AbsBottom" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                            ToolTip="Search" OnClick="ImgBtnSearch_Click" />
                    </td>
                </tr>
            </table>
            <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                width="99%" id="tblComments" name="tblComments">
                <asp:Repeater ID="repDiscussion" runat="server" OnItemDataBound="repDiscussion_OnItemDataBound">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td width="70px" style="border-bottom: 1px solid">
                                <telerik:RadLabel ID="lblPostedBy" runat="server" Text="Posted By"></telerik:RadLabel>
                            </td>
                            <td width="70px" style="border-bottom: 1px solid">
                                <b>
                                    <%#DataBinder.Eval(Container, "DataItem.NAME")%>
                                </b>
                            </td>
                            <td align="left" style="border-bottom: 1px solid; border-left: 1px solid;">
                                <telerik:RadLabel ID="lblComments" runat="server" Text="Comments"></telerik:RadLabel>
                                -<%--<br />--%>
                                <div style="height: 34px; float: left; width: 200px; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                    <telerik:RadLabel ID="lblDescription" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.DESCRIPTION")%>'></telerik:RadLabel>
                                </div>
                            </td>
                            <td width="30px" align="left" valign="top" style="border-left: 1px solid; border-bottom: 1px solid">
                                <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                            </td>
                            <td width="50px" align="left" valign="top" style="border-bottom: 1px solid">
                                <b>
                                    <%#DataBinder.Eval(Container, "DataItem.POSTEDDATE")%>
                                </b>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
