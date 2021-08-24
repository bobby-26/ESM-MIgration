<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaCandidateFees.aspx.cs"
    Inherits="Presea_PreSeaCandidateFees" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fees" Src="~/UserControls/UserControlPreSeaFees.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PreSea Candidate Fees </title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaNewApplicant" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaNewApplicant">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:UserControlStatus ID="ucStatus" runat="server" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div class="divFloatLeft">
                        <eluc:Title runat="server" ID="Title1" Text="Candidate Fees" ShowMenu="false"></eluc:Title>
                    </div>
                    <div class="divFloat">
                        <eluc:TabStrip ID="PreSeaCandidateFees" runat="server" OnTabStripCommand="PreSeaCandidateFees_TabStripCommand"
                            ></eluc:TabStrip>
                    </div>
                </div>
                <br style="clear: both;" />
                <table>
                    <tr>
                        <td>
                            Select Fees Type
                        </td>
                        <td>
                            <eluc:Fees ID="ucFeesType" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            Payment Mode
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblPaymentType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0" Selected="True">Cash</asp:ListItem>
                                <asp:ListItem Value="1">DD</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Enter Amount
                        </td>
                        <td>
                            <eluc:Number ID="ucAmount" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                        </td>
                        <td>
                            Enter DD No./Bank Details
                        </td>
                        <td>
                            <asp:TextBox ID="txtBankDetails" runat="server" CssClass="input" TextMode="MultiLine"
                                Width="256px" Height="50px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
