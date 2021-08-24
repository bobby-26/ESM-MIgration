<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicencePaymentRequestsApprove.aspx.cs" Inherits="Crew_CrewLicencePaymentRequestsApprove" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx"%>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Licence Requests Payment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOrderForm">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ucTitle" ShowMenu="false" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuLicenceStatus" runat="server" OnTabStripCommand="MenuLicenceStatus_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divamoutn">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblApplicationAmount" runat="server" Text="Application Amount"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApplicationAmount" runat="server" CssClass="readonlytextbox" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDepositBalance" runat="server" Text="Deposit Balance"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDepositBalance" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblUpfrontPaymentRequired" runat="server" Text="Upfront Payment Required"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUpfrontPaymentRequired" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlUpfrontPaymentRequired_Changed">
                                   <%-- <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblUpfrontPaymentTypes" runat="server" Text="Upfront Payment Types"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ucPaymentTypes" runat="server" CssClass="input" HardTypeCode="209" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucPayment_Changed" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblNewDepositCurrencyAmount" runat="server" Text="New Deposit Currency/Amount"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Currency ID="ucCurrency" runat="server" CssClass="input" Enabled="false" AppendDataBoundItems="true" />
                                <eluc:Number ID="ucAmount" runat="server" CssClass="input" Enabled="false" DecimalPlace="2"/>
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
