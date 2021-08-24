<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewPromotionCopy.aspx.cs" Inherits="RegisterCrewPromotionCopy" %>


<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Promotion</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%" />
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="menucopy" runat="server" OnTabStripCommand="menucopy_TabStripCommand"></eluc:TabStrip>
            <table id="Table2" style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRankFrom" runat="server" Text="Rank From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRankFrom" runat="server" Width="200px" Enabled="false" AutoPostBack="true" AppendDataBoundItems="true" 
                            CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCopyToRank" runat="server" Text="Rank To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" CssClass="dropdown_mandatory" ID="ucRankTo" runat="server" EnableLoadOnDemand="True" EmptyMessage="Type to select Rank" 
                            Filter="Contains" Width="200px" AppendDataBoundItems="true"
                            AutoPostBack="true" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
