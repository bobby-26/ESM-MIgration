<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPhoneCardIssueFilter.aspx.cs"
    Inherits="VesselAccounts_VesselAccountsPhoneCardIssueFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneCardsStatus" Src="~/UserControls/UserControlPhoneCardStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Issue of Phone Card Filter"></asp:Label>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblRequestNo" runat="server" Text="Request No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRefNo" MaxLength="50" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPhoneCardNo" runat="server" Text="Phone Card No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCardNo" MaxLength="50" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRequestStatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:PhoneCardsStatus ID="ddlStatus" runat="server" AppendDataBoundItems="true"
                                CssClass="input" ShortNameFilter="ISS,PRO" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFromDate" runat="server" Text="From"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblToDate" runat="server" Text="To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblEmpName" runat="server" Text="Employee"></asp:Literal>
                        </td>
                        <td>
                            <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
