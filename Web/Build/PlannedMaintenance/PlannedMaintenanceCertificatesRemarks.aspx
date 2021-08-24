<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceCertificatesRemarks.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceCertificatesRemarks" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="../UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Discussion forum</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>

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

</telerik:RadCodeBlock></head>
<body onload="resizediv();">
    <form id="frmCertificatesRemarks" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDiscussion">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Discussion forum" ShowMenu="false">
                        </eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuDiscussion" runat="server" OnTabStripCommand="MenuDiscussion_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px;
                    border-style: solid; border-width: 1px;" width="99%">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Literal ID="lblPostYourCommentsHere" runat="server" Text="Post Your Comments Here"></asp:Literal>
                        </td>
                        <td align="left" style="vertical-align: top;" colspan="2">
                            <asp:TextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                                Height="49px" TextMode="MultiLine" Width="692px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSearchBy" runat="server" Text="Search By"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblUser" runat="server" Text="User"></asp:Literal>
                        </td>
                        <td>
                            <eluc:User ID="ucUser" runat="server" AppendDataBoundItems="true" CssClass="input"
                                ActiveYN="172"/>
                            <asp:ImageButton ID="ImgSearch" runat="server" ImageAlign="AbsBottom" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                                ToolTip="Search" OnClick="ImgBtnSearch_Click" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="1" cellspacing="1" style="padding: 1px; margin: 1px; border-style: solid;
                    border-width: 1px;" width="99%" id="tblComments" name="tblComments">
                    <asp:Repeater ID="repDiscussion" runat="server">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td width="70px" style="border-bottom: 1px solid">
                                    <asp:Literal ID="lblPostedBy" runat="server" Text="Posted By"></asp:Literal>
                                </td>
                                <td width="70px" style="border-bottom: 1px solid">
                                    <b>
                                        <%#DataBinder.Eval(Container, "DataItem.NAME")%>
                                    </b>
                                </td>
                                <td align="left" style="border-bottom: 1px solid; border-left: 1px solid;">
                                    <asp:Literal ID="lblComments" runat="server" Text="Comments -"></asp:Literal><br />
                                    <div style="height: 34px; float: left; width: 200px; border-width: 1px; overflow-y: auto;
                                        white-space: normal; word-wrap: break-word; font-weight: bold">
                                        <%# Eval("DESCRIPTION")%>
                                    </div>
                                </td>
                                <td width="30px" align="left" valign="top" style="border-left: 1px solid; border-bottom: 1px solid">
                                    <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
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
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
