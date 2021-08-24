<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportsVesselTrailBalance.aspx.cs" Inherits="AccountsReportsVesselTrailBalance" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Trail Balance</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="from1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmVesselTrailBalance" runat="server">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />
    <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title3" Text="Vessel Summary Balance" ShowMenu="true"></eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuVessel" runat="server" OnTabStripCommand="MenuVessel_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlVesselAccount" runat="server" CssClass="dropdown_mandatory" >                        
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblYearMonth" runat="server" Text="Year / Month"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory"
                            AppendDataBoundItems="true" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlMonth" runat="server" DataSource='<%#PhoenixRegistersHard.ListHardOrderByHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 55) %>'
                          CssClass="dropdown_mandatory" DataTextField="FLDHARDNAME" DataValueField="FLDSHORTNAME" AppendDataBoundItems="true">                        
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px;
                width: 100%;"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
