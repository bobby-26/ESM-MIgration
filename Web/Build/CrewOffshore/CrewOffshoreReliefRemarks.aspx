<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreReliefRemarks.aspx.cs" Inherits="CrewOffshoreReliefRemarks" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Relief Remarks</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmApprovalRemarks" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuRemarks" runat="server" OnTabStripCommand="MenuRemarks_TabStripCommand"></eluc:TabStrip>
        <br />
        <table cellpadding="1" cellspacing="1" width="30%">
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblReason" runat="server" Text="Sign Off Reason"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <eluc:SignOffReason runat="server" ID="ucSignOffReason" CssClass="input_mandatory" AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <b>
                        <telerik:RadLabel ID="lblReliefRemarks" runat="server" Text="Relief Remarks"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadTextBox ID="txtReliefRemarks" runat="server" CssClass="input_mandatory" Height="50px" Rows="4"
                        TextMode="MultiLine" Width="97%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></telerik:RadLabel>
                </td>
            </tr>
        </table>



    </form>
</body>
</html>
