<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectBillingChargingFilter.aspx.cs"
    Inherits="AccountsProjectBillingChargingFilter" %>
    
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
    
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
   
      <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
           
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblProjectBillingName" runat="server" Text="Project Billing Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtProjectBillingName" MaxLength="200" CssClass="input"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblProjectBillingGroup" runat="server" Text="Project Billing Group"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="Div2" runat="server" class="input" style="overflow: auto; width: 40%;
                                height: 80px">
                                <asp:CheckBoxList ID="ChkProjectBillingGroup" runat="server" Height="100%" 
                                    RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblIssueFromDate" runat="server" Text="Issue From Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtIssueFromDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblIssueToDate" runat="server" Text="Issue To Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtIssueToDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBillingCompany" runat="server" Text="Billing Company"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlCompany ID="ddlLiabilitycompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    Readonly="true" CssClass="input" runat="server" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVoucherStatus" runat="server" Text="Voucher Status"></telerik:RadLabel>
                        </td>
                        <td>
                             <telerik:RadComboBox RenderMode="Lightweight" runat="server" CssClass="input" ID="ddlVoucherStatus"  AutoPostBack="true" EnableLoadOnDemand="true">
                          <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                            <telerik:RadComboBoxItem Text="Posted Only" Value="2" />
                            <telerik:RadComboBoxItem Text="Not Posted Only" Value="1" />
                        </Items>
                    </telerik:RadComboBox>
                            <%--<asp:DropDownList ID="ddlVoucherStatus" runat="server" CssClass="input">
                            <asp:ListItem Text="--Select--" Value ="Dummy"></asp:ListItem>
                            <asp:ListItem Text="Posted Only" Value ="2"></asp:ListItem>
                            <asp:ListItem Text="Not Posted Only" Value ="1"></asp:ListItem>
                            </asp:DropDownList> --%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <div id="Div1" runat="server" class="input" style="overflow: auto; width: 40%;
                                height: 80px">
                                <asp:CheckBoxList ID="chkVesselList" runat="server" Height="100%" OnSelectedIndexChanged="chkVesselList_Changed"
                                    RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCancelled" runat="server" Text="Cancelled"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox ID ="chkCancelled" runat ="server" />
                        </td>
                    </tr>
                </table>
           </telerik:RadAjaxPanel>
    </form>
</body>
</html>
