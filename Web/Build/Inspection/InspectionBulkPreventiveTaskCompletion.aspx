<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionBulkPreventiveTaskCompletion.aspx.cs" Inherits="Inspection_InspectionBulkPreventiveTaskCompletion" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Remarks</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDirectorComment" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuOfficeRemarks" runat="server" OnTabStripCommand="MenuOfficeRemarks_TabStripCommand" Title="Bulk Task Completion"></eluc:TabStrip>
        <eluc:Title runat="server" ID="ucTitle" Text="Bulk Task Completion" ShowMenu="false" Visible="false"></eluc:Title>
        <br />
        <table cellpadding="1" cellspacing="1" width="40%">
            <%--<tr>--%>
                <td width="35%">
                    <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text="Completion Remarks"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtOfficeRemarks" runat="server" CssClass="input_mandatory" Height="100px" Rows="15"
                        TextMode="MultiLine" Width="180%" Resize="Both"></telerik:RadTextBox>
                </td>
            <%--</tr>--%>
            <tr style="height: 35px">
                <td style="width: 30%">
                     <telerik:RadLabel ID="lblCompletion" runat="server" Text="Completion"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Width="150%"></telerik:RadLabel>
                </td>
            </tr>
            <eluc:Status runat="server" ID="ucStatus" />
        </table>

        <%--    <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid;
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
                        <div style="height: 54px; width: 600px; float: left; border-width: 1px; overflow-y: auto; white-space: normal;
                            word-wrap: break-word; font-weight: bold">
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
    </table>    --%>
    </form>
</body>
</html>
