<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaPlacementDetails.aspx.cs"
    Inherits="Presea_PreSeaPlacementDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Feedback</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCompanyExperienceList" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewCompanyExperienceListEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Placement Details" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCrewCompanyExperienceList" runat="server" OnTabStripCommand="CrewCompanyExperienceList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table runat="server" width="100%">
                        <tr>
                            <td>
                                Name of Ship
                            </td>
                            <td>
                                <asp:TextBox ID="txtNameOfShip" runat="server" CssClass="input" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                Flag of Ship
                            </td>
                            <td>
                                <asp:TextBox ID="txtFlagOfShip" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                IMO No.
                            </td>
                            <td>
                                <asp:TextBox ID="txtIMO" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                Official No.
                            </td>
                            <td>
                                <asp:TextBox ID="txtOfficialNo" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Name of Shipping Company
                            </td>
                            <td>
                                <asp:TextBox ID="txtNameOfShippingCompany" runat="server" CssClass="input" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                Sign On Date
                            </td>
                            <td>
                                <eluc:Date ID="ucSignOnDate" runat="server" CssClass="input" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
