<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicencePaymentDatailsEdit.aspx.cs"
    Inherits="Crew_CrewLicencePaymentDatailsEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Licence Requests Payment Edit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOrderForm">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <div class="divFloatLeft">
                        <eluc:Title runat="server" ID="frmTitle" Text="Edit" ShowMenu="false" ></eluc:Title>
                    </div>
                    <div class="divFloat">
                        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div id="find">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBilltoCompany" runat="server" Text="Bill to Company"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBankName" runat="server" Text="Bank Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    OnTextChanged="BankCurrency" AutoPostBack="true">
                                    <asp:ListItem Value="Dummy">--Select--</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="30px"></asp:TextBox>
                                <asp:TextBox ID="txtBeneficiaryName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    ToolTip="Beneficiary Name"></asp:TextBox>
                                <asp:TextBox ID="txtBankAccount" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    ToolTip="Bank Account"></asp:TextBox>
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
