<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsEmployeeQueryUpdate.aspx.cs" Inherits="VesselAccounts_VesselAccountsEmployeeQueryUpdate" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
     
         <telerik:RadScriptManager ID="radscript1" runat="server">
            </telerik:RadScriptManager>
           <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuCrewList" runat="server" OnTabStripCommand="MenuCrewList_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table>
                  <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" Text="Name" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" ReadOnly="true" Enabled="false" Width="210px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileno" runat="server" Text="File No"></telerik:RadLabel>
                    </td>
                    <td>
                      <telerik:RadTextBox ID="txtFileno" runat="server" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
            
                    <td>
                        <telerik:RadLabel ID="lblRank" Text="Rank" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                  <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignondate" Text="Sign On Date" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtSignondate" runat="server" ReadOnly="true" Enabled="false" />
                    </td>
                       <td>
                        <telerik:RadLabel ID="lblReliefDue" Text="Relief Due" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtReliefDue" runat="server" ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountsYN" Text="Accounts Y/N" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                          <telerik:RadCheckBox ID="chkAllowPayrollYN" runat="server" AutoPostBack="true" RenderMode="Lightweight" Width="100%"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

