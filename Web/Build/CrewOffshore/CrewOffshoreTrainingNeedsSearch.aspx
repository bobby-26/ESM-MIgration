<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingNeedsSearch.aspx.cs" Inherits="CrewOffshoreTrainingNeedsSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Training Needs</title>
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
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <eluc:TabStrip ID="CrewQuery" runat="server" Title="Training Needs Search" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

                <table width="50%" cellpadding="1" cellspacing="1">

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" Width="150" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name Contains"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" MaxLength="200" Width="150"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No Contains"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="input" MaxLength="50" Width="150"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCurrentRank" runat="server" Text="Current Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ddlRank" runat="server" CssClass="input" AppendDataBoundItems="true" Width="150" />
                        </td>
                    </tr>

                   
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
