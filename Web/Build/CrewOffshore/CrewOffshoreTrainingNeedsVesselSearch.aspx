<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingNeedsVesselSearch.aspx.cs" Inherits="CrewOffshoreTrainingNeedsVesselSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" DestroyOnClose="true"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="Title1" Text="Training Needs Search" ShowMenu="<%# Title1.ShowMenu %>" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="CrewQuery" runat="server" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>
            <table width="50%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name Contains"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" MaxLength="200" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No Contains"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="input" MaxLength="50" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrentRank" runat="server" Text="Current Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" CssClass="input" AppendDataBoundItems="true" Width="50%" />
                    </td>
                </tr>

                <%--  <tr>
                        <td>
                            <telerik:RadLabel ID="lblShowArchived" runat="server" Text="Status" Visible="false"></telerik:RadLabel>
                        </td>
                        <td>
                          <asp:DropDownList ID="ddlstatus" runat="server" >
                              <asp:ListItem Value="0">--SELECT--</asp:ListItem>
                               <asp:ListItem Value="PENDING">PENDING</asp:ListItem>
                               <asp:ListItem Value="OVERDUE">OVERDUE</asp:ListItem>
                               <asp:ListItem Value="BOTH">BOTH</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>--%>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
