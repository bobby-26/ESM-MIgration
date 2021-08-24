<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitMonthlybilling.aspx.cs" Inherits="AccountsVesselVisitMonthlybilling" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLedgerGeneral" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Title runat="server" ID="Title1" Text="Monthly billing" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="MenuTravelClaimMain" runat="server" OnTabStripCommand="MenuTravelClaimMain_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <table id="tbldiv" runat="server" cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <eluc:TabStrip ID="MenuTravelclaim" runat="server" OnTabStripCommand="MenuTravelClaim_TabStripCommand"></eluc:TabStrip>
                </td>
            </tr>
            <tr></tr>
        </table>
        <table cellpadding="2" cellspacing="1" style="width: 100%" border="1">

            <tr></tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Form No:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblFormno" runat="server"></telerik:RadLabel>
                </td>

                <td style="width: 25%">
                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="Claim Status:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblClaimstatus" runat="server"></telerik:RadLabel>
                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel3" runat="server" Text="Employee ID:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblEmpId" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="RadLabel4" runat="server" Text="Employee Name:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblEmpname" runat="server"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>Vessel Account:
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselaccount" runat="server"></telerik:RadLabel>
                </td>
                <td>Fin. Year:
                </td>
                <td>
                    <telerik:RadLabel ID="lblFinyear" runat="server"></telerik:RadLabel>
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel5" runat="server" Text="From Date:"></telerik:RadLabel>
                    <td>
                        <telerik:RadLabel ID="lblFromdate" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="To Date:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTodate" runat="server"></telerik:RadLabel>
                    </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel7" runat="server" Text="Budgeted days:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblBudgeteddays" runat="server"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <telerik:RadLabel ID="RadLabel8" runat="server" Text="Current Visit:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCurrentvisit" runat="server"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel9" runat="server" Text="Total including current visit:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lbltotalinclucurrentdays" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="RadLabel10" runat="server" Text="Charged Days:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblChargeddays" runat="server"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel11" runat="server" Text="Charging Days:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblchargingdays" runat="server"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="RadLabel12" runat="server" Text="Vessel Budget Code:"></telerik:RadLabel>
                </td>
                <td>

                    <span id="spnPickListBulkBudget">
                        <telerik:RadTextBox ID="txtBulkBudgetCode" runat="server" MaxLength="20" CssClass="input"
                            Width="120px">
                        </telerik:RadTextBox>
                        <asp:TextBox ID="txtBulkBudgetName" runat="server" Width="0px" CssClass="input" Enabled="False"></asp:TextBox>
                        <asp:ImageButton ID="btnShowBulkBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                        <asp:TextBox ID="txtBulkBudgetId" runat="server" Width="0px" CssClass="input" OnTextChanged="txtBulkBudgetIdClick"></asp:TextBox>
                        <asp:TextBox ID="txtBulkBudgetgroupId" runat="server" Width="0px" CssClass="input"></asp:TextBox></span>

                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel13" runat="server" Text="Owner Budget Code:"></telerik:RadLabel>
                </td>
                <td>



                    <span id="spnPickListBulkOwnerBudget">
                        <telerik:RadTextBox ID="txtBulkOwnerBudgetCode" runat="server" MaxLength="20" CssClass="input"
                            Width="135px">
                        </telerik:RadTextBox>
                        <asp:TextBox ID="txtBulkOwnerBudgetName" runat="server" Width="0px" CssClass="input"
                            Enabled="False"></asp:TextBox>
                        <asp:ImageButton ID="btnShowBulkOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" Text=".." />
                        <asp:TextBox ID="txtBulkOwnerBudgetId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                        <asp:TextBox ID="txtBulkOwnerBudgetgroupId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                    </span>
                    &nbsp;</td>
                <td>
                    <telerik:RadLabel ID="RadLabel14" runat="server" Text="Billing Unit:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblBillingunit" runat="server"></telerik:RadLabel>
                </td>
            </tr>
            <tr>

                <td>
                    <telerik:RadLabel ID="RadLabel15" runat="server" Text="Visit Type:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlBudgetedVisit" runat="server" CssClass="input_mandatory">
                        <Items>
                            <telerik:RadComboBoxItem Text="Budgeted" Value="1"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Dry Dock" Value="3"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Predelivery" Value="4"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Non - Budgeted" Value="2"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Retrofit" Value="5"></telerik:RadComboBoxItem>
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel16" runat="server" Text="Rate:"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Numeber ID="txtRate" runat="server" CssClass="gridinput_mandatory"
                        Mask="999.99" Text=''
                        Width="60px" MaxLength="6" DecimalPlace="2" />
                </td>

                <td>
                    <telerik:RadLabel ID="RadLabel17" runat="server" Text="Amount:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblAmount" runat="server" Text=""></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel18" runat="server" Text="Voucher number:"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblVoouchernumber" runat="server"></telerik:RadLabel>
                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                        CommandName="ATTACHMENT" ID="cmdAtt"
                        ToolTip="Attachment"></asp:ImageButton>
                </td>

                <td>
                    <telerik:RadLabel ID="RadLabel19" runat="server" Text="Voucher date:"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtPostdate" runat="server" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
