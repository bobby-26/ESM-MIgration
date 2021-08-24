<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVesselRAJHAApproval.aspx.cs"
    Inherits="InspectionVesselRAJHAApproval" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JHA Approval Remarks</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmApprovalRemarks" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuApprovalRemarks" runat="server" OnTabStripCommand="MenuApprovalRemarks_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="65%">
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <b>
                        <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text="Office Remarks"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadTextBox ID="txtApprovalRemarks" runat="server" CssClass="input_mandatory" Height="50px"
                        Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></telerik:RadLabel>
                </td>
            </tr>
        </table>
        <div>
            <b><font color="blue">
            <telerik:RadLabel ID="lblNote" runat="server" Text="* Note: Once the JHA is approved, It cannot be edited."></telerik:RadLabel></font></b>
        </div>
    </form>
</body>
</html>
