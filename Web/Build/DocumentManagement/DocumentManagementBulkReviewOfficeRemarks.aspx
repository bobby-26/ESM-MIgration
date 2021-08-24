<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementBulkReviewOfficeRemarks.aspx.cs" Inherits="DocumentManagement_DocumentManagementBulkReviewOfficeRemarks" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bulk Office Comments</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDirectorComment" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>      
        <eluc:TabStrip ID="MenuOfficeRemarks" runat="server" OnTabStripCommand="MenuOfficeRemarks_TabStripCommand"></eluc:TabStrip>
        <br />
        <table cellpadding="1" cellspacing="1" width="25%">
            <tr style="height: 30px">
                <td width="35%">Accepted</td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblbtn1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px">
                        <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <%--            <td style="width: 35%">
                Not Accepted
            </td>
            <td style="width: 20%">
                <asp:RadioButton ID="rblbtn2" runat="server" />                      
            </td> --%>
                <%--            <td style="width: 35%">
                <asp:CheckBox ID="chkAccepted" runat="server" AutoPostBack="true" />
            </td> --%>
            </tr>
            <tr></tr>
            <tr style="height: 35px">
                <td style="width: 30%">Due
                </td>
                <td>
                    <eluc:Date ID="ucCommentsDueDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td width="35%">
                    <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text="Office Comments"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtOfficeRemarks" runat="server" CssClass="input_mandatory" Height="200px" Rows="15"
                        TextMode="MultiLine" Width="300%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Width="150%"></telerik:RadLabel>
                </td>
            </tr>
            <eluc:Status runat="server" ID="ucStatus" />
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
