<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePendingWaivingFilter.aspx.cs" Inherits="CrewOffshorePendingWaivingFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Src="../UserControls/UserControlRank.ascx" TagName="Rank" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlHard.ascx" TagName="Hard" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlVessel.ascx" TagName="Vessel" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pending Waiving Filter</title>
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
            <eluc:TabStrip ID="MenuPD" Title="PD Filter" runat="server" OnTabStripCommand="PD_TabStripCommand"></eluc:TabStrip>


            <div id="divFind">
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name Contains"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtName" runat="server" MaxLength="200" Width="200px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No Contains"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFileNo" runat="server" MaxLength="50" Width="200px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCurrentRank" runat="server" Text="Current Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" Width="200px" />
                        </td>
                    </tr>
                   
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblstatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlStatus" runat="server" Width="200px" MarkFirstMatch="true" Filter="Contains" EmptyMessage="Type to select status">
                                <Items>
                                   <telerik:RadComboBoxItem Text="Waive Requested" Value="2" />
                                   <telerik:RadComboBoxItem Text="Waiving Done" Value="1" />
                                </Items>

                            </telerik:RadComboBox>
                        </td>
                    </tr>

                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
