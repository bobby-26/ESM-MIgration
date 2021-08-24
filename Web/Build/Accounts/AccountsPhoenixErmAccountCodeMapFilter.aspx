<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsPhoenixErmAccountCodeMapFilter.aspx.cs" Inherits="AccountsPhoenixErmAccountCodeMapFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register src="../UserControls/UserControlCompany.ascx" tagname="UserControlCompany" tagprefix="eluc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Phoenix-Erm Supplier Code Map Filter"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <div id="divFind">
                <table width="100%">
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblAccount" runat="server" Text="Account"></asp:Literal>
                            <br />
                            <asp:Literal ID="lblSourceUsage" runat="server" Text="Source/Usage"></asp:Literal>
                        </td>
                        <td rowspan="2">
                            <span id="spnPickListExpenseAccount">
                                <asp:TextBox ID="txtAccountCode" runat="server" CssClass="input" ReadOnly="false"
                                    MaxLength="20" Width="30%"></asp:TextBox>
                                <asp:TextBox ID="txtAccountDescription" runat="server" CssClass="input"
                                    ReadOnly="false" MaxLength="50" Width="35%"></asp:TextBox>
                                <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                    src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx',true); " />                                
                                <asp:TextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="15%"></asp:TextBox>                                
                                <asp:TextBox ID="txtAccountSource" CssClass="readonlytextbox" runat="server" Width="25%"></asp:TextBox>
                                &nbsp;/&nbsp;<asp:TextBox ID="txtAccountUsage" CssClass="readonlytextbox" runat="server" Width="25%"></asp:TextBox>
                                <br />
                                <asp:TextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input"></asp:TextBox>
                                <%--<img runat="server" id="imgShowBudget" style="cursor: pointer; vertical-align: top"
                                    src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListSubAccount.aspx',true); " />--%>
                                <asp:TextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input_mandatory"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input_mandatory"></asp:TextBox>
                            </span>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
