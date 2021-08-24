<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPettyCashLineItemAddNew.aspx.cs"
    Inherits="VesselAccountsPettyCashLineItemAddNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Petty Cash Expenses</title>
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
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="Petty Expenses" ShowMenu="<%# Title1.ShowMenu %>"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuPettyCash" runat="server" OnTabStripCommand="MenuPettyCash_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div class="subHeader">
                        <div style="position: absolute; right: 0px">
                            <eluc:TabStrip ID="MenuPettyCash1" runat="server" OnTabStripCommand="MenuPettyCash1_TabStripCommand"></eluc:TabStrip>
                        </div>
                    </div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                            </td>
                            <td>
                                <eluc:MultiPort ID="ddlSeaPortAdd" runat="server" CssClass="input_mandatory" Width="300px" />
                            </td>
                            <td>
                                <asp:Literal ID="lblExpensesOn" runat="server" Text="Expenses On"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtDateAdd" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <asp:Literal ID="lblPurpose" runat="server" Text="Purpose"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurposeAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="500"
                                    Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbPayment" runat="server" Text="Payment" GroupName="rbSelection"
                                    Checked="true" />
                                <asp:RadioButton ID="rbReceipts" runat="server" Text="Receipts" GroupName="rbSelection" />
                            </td>
                            <td>
                                <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory" Width="90px"
                                    MaxLength="8" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
