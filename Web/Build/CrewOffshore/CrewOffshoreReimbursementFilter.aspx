<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreReimbursementFilter.aspx.cs" Inherits="CrewOffshoreReimbursementFilter" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="Rank" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reimbursement Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">


            <eluc:TabStrip ID="MenuPD" runat="server" Title="Reimbursements/Recoveries Filter" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>


            <div id="divFind">
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFileno" Text="File No" runat="server"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFileNo" runat="server"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" Text="Name" runat="server"></telerik:RadLabel>

                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtName" runat="server"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRank" Text="Rank" runat="server"></telerik:RadLabel>

                        </td>
                        <td>
                            <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblActive" Text="Active Y/N" runat="server"></telerik:RadLabel>

                        </td>
                        <td>
                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblReimbursementRecovery" Text="Reimbursement/ Recovery" runat="server"></telerik:RadLabel>

                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlEarDed" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEarDed_SelectedIndexChanged"
                                Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select">
                                <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""/>
                                <telerik:RadComboBoxItem  Text="Reimbursement" Value="2"/>
                                <telerik:RadComboBoxItem  Text="Recovery" Value="-2"/>
                                </Items>
                               
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPurpose" Text="Purpose" runat="server"></telerik:RadLabel>

                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlPurpose" runat="server" AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select"></telerik:RadComboBox>
                            <eluc:Hard ID="ddlPurpose1" runat="server" Visible="false" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
