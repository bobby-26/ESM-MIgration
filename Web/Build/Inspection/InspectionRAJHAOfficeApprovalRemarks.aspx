<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAJHAOfficeApprovalRemarks.aspx.cs"
    Inherits="InspectionRAJHAOfficeApprovalRemarks" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JHA Approval Remarks</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmApprovalRemarks" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <br />
    <table cellpadding="1" cellspacing="1" width="30%">
        <tr>
            <td colspan="2">
                <b>
                    <asp:Literal ID="lblOfficeRemarks" runat="server" Text="Office Remarks"></asp:Literal></b>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtApprovalRemarks" runat="server" CssClass="input_mandatory" Height="50px"
                    Rows="4" TextMode="MultiLine" Width="97%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
            </td>
        </tr>        
    </table>
    <div>
        <b><font color="blue">
            <asp:Literal ID="lblNote" runat="server" Text="* Note: Once the JHA is Issued, It cannot be edited."></asp:Literal></font></b>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuApprovalRemarks" runat="server" OnTabStripCommand="MenuApprovalRemarks_TabStripCommand">
        </eluc:TabStrip>
    </div>
    </form>
</body>
</html>
