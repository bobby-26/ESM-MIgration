<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementOfficeRemarks.aspx.cs"
    Inherits="DocumentManagementOfficeRemarks" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Remarks</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmDirectorComment" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="divHeading">
            <eluc:Title runat="server" ID="ucTitle" Text="Review" ShowMenu="false"></eluc:Title>
        </div>
    </div>
    <br />
    <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px;
        border-style: solid; border-width: 1px;" width="99%">
        <%--            <tr>
                <td align="left" colspan="2">
                    <font color="blue" size="0"><b>Details of change</b></font>
                </td>
            </tr>--%>
        <tr>
            <td colspan="2" style="width: 35%">
                <asp:RadioButtonList ID="rblbtn1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"
                    Width="200px">
                    <asp:ListItem Text="Accepted" Value="1" Selected="False"></asp:ListItem>
                    <asp:ListItem Text="Not Accepted" Value="0" Selected="False"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td style="width: 50%">
            </td>
        </tr>
        <tr></tr>
        <tr style = "height: 30px">
            <td style = "width: 30%">
                Due
            </td>
            <td style="width: 35%">
                <eluc:Date ID="ucCommentsDueDate" runat="server" CssClass="input" DatePicker="true" />
            </td>
        </tr>
        <tr>
            <td width="10%">
                Remarks
            </td>
            <td align="left" style="vertical-align: top;">
                <asp:TextBox ID="txtOfficeRemarks" runat="server" CssClass="gridinput" Height="49px"
                    TextMode="MultiLine" Width="600px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid;
        border-width: 1px;" width="99%" id="tblComments" name="tblComments">
        <asp:Repeater ID="repDiscussion" runat="server">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td width="10%">
                        Posted By
                        <br />
                        <b>
                            <%#DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>
                        </b>
                    </td>
                    <td class="input" width="60%">
                        Comments -<br />
                        <div style="height: 54px; width: 600px; float: left; border-width: 1px; overflow-y: auto;
                            white-space: normal; word-wrap: break-word; font-weight: bold">
                            <%# Eval("FLDCOMMENTS")%>
                        </div>
                    </td>
                    <td width="12%">
                        Name
                        <br />
                        <b>
                            <%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDBYNAME")%>
                        </b>
                    </td>
                    <td width="15%">
                        Date
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
        <eluc:Status runat="server" ID="ucStatus" />
    </table>
    <div>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"
                    Width="150%"></asp:Label>
            </td>
        </tr>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuOfficeRemarks" runat="server" OnTabStripCommand="MenuOfficeRemarks_TabStripCommand">
        </eluc:TabStrip>
    </div>
    </form>
</body>
</html>
