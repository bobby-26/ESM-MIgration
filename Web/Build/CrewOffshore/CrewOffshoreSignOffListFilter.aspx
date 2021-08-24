<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSignOffListFilter.aspx.cs" Inherits="CrewOffshoreSignOffListFilter" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Off Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmSignOffFilter" runat="server">
        

        <eluc:TabStrip ID="MenuSignOffFilterMain" Title="Sign-Off List Filter" runat="server" OnTabStripCommand="MenuSignOffFilterMain_TabStripCommand"></eluc:TabStrip>

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true"
                        CssClass="input" VesselsOnly="true" AssignedVessels="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblRankHeader" runat="server" Text="Sign On Rank"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Rank ID="ucRank" runat="server" CssClass="input" AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNameHeader" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtName" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblFileNoHeader" runat="server" Text="File No"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtFileNo" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSignonFromDate" runat="server" Text="Sign On From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtSignonFromDate" runat="server" CssClass="input" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblSignonToDate" runat="server" Text="Sign On To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtSignonToDate" runat="server" CssClass="input" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReliefDueFromDate" runat="server" Text="Relief Due From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtReliefDueFromDate" runat="server" CssClass="input" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblReliefDueToDate" runat="server" Text="Relief Due To"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtReliefDueToDate" runat="server" CssClass="input" DatePicker="true" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
