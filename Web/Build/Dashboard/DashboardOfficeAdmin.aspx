<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardOfficeAdmin.aspx.cs" Inherits="Dashboard_DashboardOfficeAdmin" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Vessel Communication Details</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegisterVesselCommunication" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
        position: absolute;">
        <asp:UpdatePanel runat="server" ID="pnlVesselOfficeAdmin">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
                </eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text=""  />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 2px; position: absolute;">
                    <eluc:TabStrip ID="MenuVesselOfficeAdmin" runat="server" OnTabStripCommand="MenuVesselOfficeAdmin_TabStripCommand" TabStrip="true">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="8" width="100%">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlSatcom" runat="server" GroupingText="Accounts" Width="100%" Height="40%">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 10%">
                                                        <asp:Literal ID="lblAccountsExecutive" runat="server" Text="Accounts Executive"></asp:Literal>
                                                    </td>
                                                    <td valign="baseline" style="width: 17%">
                                                            <asp:TextBox ID="txtAccountsExecName" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="200" Width="35%"></asp:TextBox>
                                                            <asp:TextBox ID="txtAccountsExecDesignation" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="50" Width="25%"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10%">
                                                        <asp:Literal ID="lblEmail" runat="server" Text="Email"></asp:Literal>
                                                    </td>
                                                    <td style="width: 17%">
                                                        <asp:TextBox runat="server" ID="txtAccountsExecEmail" CssClass="readonlytextbox"
                                                            ReadOnly="true" Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%">
                                                        <asp:Literal ID="lblInvoicePostingAsPer" runat="server" Text="Invoice Posting As Per"></asp:Literal>
                                                        <br />
                                                        <asp:Literal ID="lblSupplierConfiguration" runat="server" Text="Supplier Configuration"></asp:Literal>
                                                    </td>
                                                    <td style="width: 17%">
                                                        <asp:CheckBox ID="chkSupplierConfig" runat="server" Enabled="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlPurchase" runat="server" GroupingText="Purchase" Width="100%" Height="40%">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 10%">
                                                        <asp:Literal ID="lblPurchaseSuperintendent" runat="server" Text="Purchase Superintendent"></asp:Literal>
                                                    </td>
                                                    <td valign="baseline" style="width: 17%">
                                                            <asp:TextBox ID="txtPurchaseSupdtName" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="200" Width="35%"></asp:TextBox>
                                                            <asp:TextBox ID="txtPurchaseSupdtDesignation" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="50" Width="25%"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10%">
                                                        <asp:Literal ID="lblEmailHeader" runat="server" Text="Email"></asp:Literal>
                                                    </td>
                                                    <td style="width: 17%">
                                                        <asp:TextBox runat="server" ID="txtPurchaseSupdtEmail" CssClass="readonlytextbox"
                                                            ReadOnly="true" Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlQuality" runat="server" GroupingText="Quality" Width="100%" Height="40%">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 10%">
                                                        Quality Superintendent
                                                    </td>
                                                    <td valign="baseline" style="width: 17%">
                                                            <asp:TextBox ID="txtQualitySupdtName" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="200" Width="35%"></asp:TextBox>
                                                            <asp:TextBox ID="txtQualitySupdtDesignation" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="50" Width="25%"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10%">
                                                        Email
                                                    </td>
                                                    <td style="width: 17%">
                                                        <asp:TextBox runat="server" ID="txtQualitySupdtEmail" CssClass="readonlytextbox"
                                                            ReadOnly="true" Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%">
                                                        Quality Director
                                                    </td>
                                                    <td valign="baseline" style="width: 17%">
                                                            <asp:TextBox ID="txtQualityDirectorName" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="200" Width="35%"></asp:TextBox>
                                                            <asp:TextBox ID="txtQualityDirectorDesignation" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="50" Width="25%"></asp:TextBox>
                                                   </td>
                                                    <td style="width: 10%">
                                                        Email
                                                    </td>
                                                    <td style="width: 17%">
                                                        <asp:TextBox runat="server" ID="txtQualityDirectorEmail" CssClass="readonlytextbox"
                                                            ReadOnly="true" Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlTravel" runat="server" GroupingText="Travel" Width="100%" Height="40%">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 10%">
                                                        Travel PIC 2
                                                    </td>
                                                    <td valign="baseline" style="width: 17%">
                                                            <asp:TextBox ID="txtTravelPIC2Name" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="200" Width="35%"></asp:TextBox>
                                                            <asp:TextBox ID="txtTravelPIC2Designation" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="50" Width="25%"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10%">
                                                        Email
                                                    </td>
                                                    <td style="width: 17%">
                                                        <asp:TextBox runat="server" ID="txtTravelPIC2Email" CssClass="readonlytextbox" ReadOnly="true"
                                                            Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%">
                                                        Travel PIC 3
                                                    </td>
                                                    <td valign="baseline" style="width: 17%">
                                                            <asp:TextBox ID="txtTravelPIC3Name" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="200" Width="35%"></asp:TextBox>
                                                            <asp:TextBox ID="txtTravelPIC3Designation" runat="server" CssClass="input" ReadOnly="true"
                                                                MaxLength="50" Width="25%"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10%">
                                                        Email
                                                    </td>
                                                    <td style="width: 17%">
                                                        <asp:TextBox runat="server" ID="txtTravelPIC3Email" CssClass="readonlytextbox" ReadOnly="true"
                                                            Width="250px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
