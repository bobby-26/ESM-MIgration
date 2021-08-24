<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaExamVenueGeneral.aspx.cs"
    Inherits="PreSeaExamVenueGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SIMSZone" Src="~/UserControls/UserControlPreSeaZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Vessel Sign-On</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="" ShowMenu="false"></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divMain" runat="server" style="width: 100%;">
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td colspan="6" style="font-weight: bold">
                                Venue Details
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                Exam Venue
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenueName" runat="server" Width="175px" CssClass="input_mandatory">
                                </asp:TextBox>
                            </td>
                            <td>
                                Address
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                            <td>
                                Address 2
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress2" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Address 3
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress3" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                Address 4
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress4" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                Country
                            </td>
                            <td>
                                <eluc:Country runat="server" ID="ucCountry" AutoPostBack="true" AppendDataBoundItems="true"
                                    CssClass="input" OnTextChangedEvent="ucCountry_TextChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                State
                            </td>
                            <td>
                                <eluc:State ID="ucState" CssClass="input" runat="server" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" />
                            </td>
                            <td>
                                City
                            </td>
                            <td>
                                <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                            <td>
                                Phone
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhone1" runat="server" Width="175px" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fax
                            </td>
                            <td>
                                <asp:TextBox ID="txtFax1" runat="server" Width="175px" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                Email
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail1" runat="server" Width="175px" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                SIMS Zone
                            </td>
                            <td>
                                <eluc:SIMSZone ID="ddlSIMSZone" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="font-weight: bold">
                                Contact Person Details
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Name of the Person
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactName" runat="server" Width="175px" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                            <td>
                                Phone
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPhone" runat="server" Width="175px" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                Mobile
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactMobile" runat="server" Width="175px" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Email
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactEmail" runat="server" Width="175px" CssClass="input"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                Zone (Please fill, if the venue is a Field Office)
                            </td>
                            <td colspan="2">
                                <eluc:Zone ID="ddlZone" runat="server" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
