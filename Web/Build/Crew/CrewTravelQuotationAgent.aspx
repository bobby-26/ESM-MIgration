<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuotationAgent.aspx.cs"
    Inherits="CrewTravelQuotationAgent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewTravelQuotationAgent" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title2" Text="Agent Details" ShowMenu="false"></eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAgent" runat="server" OnTabStrip="true" OnTabStripCommand="MenuAgent_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlFormGeneral">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="Div2" style="top: 30px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                    border: none; width: 100%">
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblAgent" runat="server" Text="Agent"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <span id="spnPickListMaker">
                                    <asp:TextBox ID="txtAgentCode" runat="server" Width="70px" CssClass="input_mandatory"></asp:TextBox>
                                    <asp:TextBox ID="txtAgentName" runat="server" BorderWidth="1px" Width="180px" CssClass="input_mandatory"></asp:TextBox>
                                    <asp:ImageButton ID="btnPickAgent" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?productype=63&txtsupcode='+ document.getElementById('txtAgentCode').value +'&txtsupname='+ document.getElementById('txtAgentName').value, true);"
                                        Text=".." />
                                    <asp:TextBox ID="txtAgentID" runat="server" Width="1" CssClass="input"></asp:TextBox>
                                </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAgentReference" runat="server" Width="90px" MaxLength="50" Visible="false"
                                    CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top">
                               <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAgentAddress" runat="server" CssClass="input" TextMode="MultiLine"
                                    ReadOnly="true" Height="20px" Width="300px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFax" runat="server" CssClass="input" ReadOnly="true" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="input" ReadOnly="true" Width="120px"></asp:TextBox>
                            </td>
                            <tr>
                                <td colspan="8">
                                    <br />
                                </td>
                            </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
