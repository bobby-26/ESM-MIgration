<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceRejectionFilter.aspx.cs" Inherits="AccountsAllotmentRemittanceRejectionFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="AllotmentRemittanceRejectionFilter" runat="server">
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Allotment Remittance Rejection Filter "></asp:Label>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuRemittanceRejectionFilter" runat="server" OnTabStripCommand="MenuRemittanceRejectionFilter_TabStripCommand"></eluc:TabStrip>
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlRejection">
            <ContentTemplate>
                <div id="divFind">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblRemittanceNumber" runat="server" Text="Remittance No."></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRemittanceNumber" CssClass="input" Width="90px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblFileNumber" Text="File No." runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFileNumber" runat="server" CssClass="input" Width="90px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblName" Text="Name" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="input" Width="240px" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblFromDate" runat="server" Text="Rejection From Date"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromdateSearch" runat="server" Width="90px" CssClass="input"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtFromdateSearch" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <asp:Literal ID="lblToDate" runat="server" Text="Rejection To Date"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTodateSearch" runat="server" Width="90px" CssClass="input"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtTodateSearch" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <asp:Literal ID="lblActiveYN" runat="server" Text="Active Y/N"></asp:Literal>
                            </td>
                            <td>
                                <input type="checkbox" id="chkActiveYN" runat="server" checked="checked" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRejectionReason" runat="server" Text="Rejection Reason"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick ID="ddlRejectionReason" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    QuickTypeCode="162" QuickList='<%#PhoenixRegistersQuick.ListQuick(1,162)%>' Width="250px" />
                            </td>
                            <td id="tdvessel">
                                <asp:Literal ID="lblVessel" Text="Vessel" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlVessel ID="ddlVessel" runat="server" AppendDataBoundItems="true"
                                    Width="240px" CssClass="input" VesselsOnly="true" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>

                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>
