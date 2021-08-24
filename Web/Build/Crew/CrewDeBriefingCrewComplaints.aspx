<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewDeBriefingCrewComplaints.aspx.cs"
    Inherits="CrewDeBriefingCrewComplaints" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew De-Briefing Crew Complaints</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmApprovalRemarks" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    
        <eluc:TabStrip ID="MenuRemarks" runat="server" OnTabStripCommand="MenuRemarks_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="80%">
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblrefnumber" runat="server" Text="Reference Number 	"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadTextBox ID="txtRefNumber" Width="150px" runat="server" CssClass="readonlytextbox"
                        Enabled="false"></telerik:RadTextBox>
                </td>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported by"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadTextBox ID="txtName" Width="150px" runat="server" CssClass="readonlytextbox"
                        Enabled="false"></telerik:RadTextBox>
                </td>

            </tr>
            <tr>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadTextBox ID="txtRank" Width="150px" runat="server" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                </td>
                <td style="width: 20px">
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td style="width: 30px">
                    <telerik:RadTextBox ID="txtVesselName" Width="150px" runat="server" CssClass="readonlytextbox"
                        Enabled="false"></telerik:RadTextBox>
                    <br />
                </td>

            </tr>
        </table>
        <br />
        <table cellpadding="1" cellspacing="1" width="80%">
            <tr>

                <td style="width: 100px">

                    <b>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Crew Complaint Remarks"></telerik:RadLabel></b>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" Height="100px"
                        Rows="4" TextMode="MultiLine" Width="75%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
                </td>
            </tr>
        </table>
        
            
      
    </form>
</body>
</html>
