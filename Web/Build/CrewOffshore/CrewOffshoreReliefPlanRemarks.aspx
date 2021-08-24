<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreReliefPlanRemarks.aspx.cs" Inherits="CrewOffshore_CrewOffshoreReliefPlanRemarks" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remarks</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCancelReason" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        
                <eluc:TabStrip ID="MenuRemarks" runat="server" OnTabStripCommand="MenuRemarks_TabStripCommand"></eluc:TabStrip>
          
            <br />
            <table cellpadding="1" cellspacing="1" width="75%">
                <tr>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel></b>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" Height="50px" Rows="5"
                            TextMode="MultiLine" Width="97%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
                    </td>
                </tr>
            </table>
           
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
