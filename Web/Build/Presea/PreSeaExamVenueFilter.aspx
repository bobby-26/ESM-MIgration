<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaExamVenueFilter.aspx.cs"
    Inherits="PreSeaExamVenueFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
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
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Exam Venue Filter"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuPreSeaFilterMain" runat="server" OnTabStripCommand="MenuPreSeaFilterMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaFilter">
        <ContentTemplate>
            <table width="50%">
                <tr>
                    <td colspan="4">
                        <font color="blue"><b>Note: </b>For embedded search, use '%' symbol. (Eg. Name: %xxxx)</font>
                    </td>
                </tr>
                <tr>
                    <td>
                        Venue Name
                    </td>
                    <td>
                        <asp:TextBox ID="txtVenueName" runat="server" CssClass="input">
                        </asp:TextBox>
                    </td>
                    <td>
                        Address
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="input_mandatory"></asp:TextBox>
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
                </tr>
                <tr>
                    <td>
                        Zone
                    </td>
                    <td>
                        <eluc:Zone ID="ddlZone" CssClass="input" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        Phone 1
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone1" runat="server" CssClass="input">
                        </asp:TextBox>
                    </td>
                    <td>
                        Email 1
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail1" runat="server" CssClass="input">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Contact Person
                    </td>
                    <td>
                        <asp:TextBox ID="txtContactName" runat="server" CssClass="input">
                        </asp:TextBox>
                    </td>
                    <td>
                        Contact Person Phone
                    </td>
                    <td>
                        <asp:TextBox ID="txtContactPhone" runat="server" CssClass="input">
                        </asp:TextBox>
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Contact Person Mobile
                    </td>
                    <td>
                        <asp:TextBox ID="txtContactMobile" runat="server" CssClass="input">
                        </asp:TextBox>
                    </td>
                    <td>
                        Contact Person Mail
                    </td>
                    <td>
                        <asp:TextBox ID="txtContactMail" runat="server" CssClass="input">
                        </asp:TextBox>
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
