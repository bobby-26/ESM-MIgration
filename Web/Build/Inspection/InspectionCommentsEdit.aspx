<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCommentsEdit.aspx.cs" Inherits="Inspection_InspectionCommentsEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Comments Edit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDirectorComment" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <eluc:TabStrip ID="MenuCommentsEdit" runat="server" OnTabStripCommand="MenuCommentsEdit_TabStripCommand"></eluc:TabStrip>
        <br />
        <table width="60%">
            <tr style="height: 30px">
                <td width="10%">Accepted
                </td>
                <td colspan="2" style="width: 35%">
                    <asp:RadioButtonList ID="rblbtn1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="100px" OnTextChanged="rblbtn1_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <%--                <td width="10%">
                    Accepted
                </td>
                <td style="width: 35%">
                    <asp:CheckBox ID="chkAccepted" runat="server" AutoPostBack="true" />
                </td>--%>
            </tr>
            <tr></tr>
            <tr style="height: 30px">
                <td width="10%">Due
                </td>
                <td style="width: 35%">
                    <eluc:Date ID="ucCommentsDueDate" runat="server" DatePicker="true" Enabled="false" />
                </td>
            </tr>
            <tr></tr>
            <tr id="trCompletion" runat="server" style="height: 30px">
                <td width="10%">Completion
                </td>
                <td style="width: 35%">
                    <eluc:Date ID="ucCommentsCompletionDate" runat="server" DatePicker="true" />
                </td>
            </tr>
            <tr style="height: 30px">
                <td width="10%">Remarks
                </td>
                <td align="left" style="vertical-align: top;">
                    <telerik:RadTextBox ID="txtOfficeRemarks" runat="server" CssClass="gridinput" Height="100px"
                        TextMode="MultiLine" Width="400px">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
       </telerik:RadAjaxPanel>
    </form>
</body>
</html>
