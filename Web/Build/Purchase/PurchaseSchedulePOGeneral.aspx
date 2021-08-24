<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSchedulePOGeneral.aspx.cs" Inherits="PurchaseSchedulePOGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Bulk Purchase</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmBulkPurchase" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%;">
        <asp:UpdatePanel runat="server" ID="pnlBulkPurchase">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ttlInvoice" Text="General" ShowMenu="false"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBulkPurchase" runat="server" OnTabStripCommand="MenuBulkPurchase_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                  <div class="navSelect3" style="position: relative; z-index: +2">
                    <table cellpadding="2" width="100%">
                        <tr>
                            <td width="15%">
                              <asp:Literal ID="lblFormNumber" runat="server" Text="Form Number"></asp:Literal>
                            </td>
                            <td width="35%">
                                <asp:TextBox ID="txtFormNumber" runat="server" CssClass="readonlytextbox"  ReadOnly="true"
                                    Width="240px"></asp:TextBox>
                            </td>
                            <td width="15%">
                              <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                </td>
                            <td width="35%">
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                              <asp:Literal ID="lblFormTitle" runat="server" Text="Form Title"></asp:Literal>
                                </td>
                            <td width="35%">
                                <asp:TextBox ID="txtFormTitle" runat="server" CssClass="input_mandatory" 
                                    Width="240px"></asp:TextBox>
                            </td>
                            <td width="15%">
                              <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                                 </td>
                            <td width="35%">
                                <span ID="spnPickListMaker">
                                <asp:TextBox ID="txtVendorCode" runat="server" CssClass="input_mandatory" 
                                    ReadOnly="false" Width="60px"></asp:TextBox>
                                <asp:TextBox ID="txtVenderName" runat="server" CssClass="input_mandatory" 
                                    ReadOnly="false" Width="180px"></asp:TextBox>
                                <img ID="ImgSupplierPickList" runat="server" 
                                    onclick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132', true);" 
                                    src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: middle;
                                        padding-bottom: 3px;" />
                                <asp:TextBox ID="txtVendorId" runat="server" Width="10px"></asp:TextBox>
                                </span></td>
                        </tr>
                        <tr>
                            <td width="15%">
                              <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                                </td>
                            <td width="35%">
                                
                                <span ID="spnPickListMainBudget">
                                <asp:TextBox ID="txtBudgetCode" runat="server" CssClass="input_mandatory" 
                                    Enabled="False" Width="60px"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetName" runat="server" CssClass="input_mandatory" 
                                    Enabled="False" Width="180px"></asp:TextBox>
                                <asp:ImageButton ID="btnShowBudget" runat="server" ImageAlign="AbsMiddle" 
                                    ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" Text=".." />
                                <asp:TextBox ID="txtBudgetId" runat="server" CssClass="input" Width="0px"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetgroupId" runat="server" CssClass="input" Width="0px"></asp:TextBox>
                                </span>
                                
                            </td>
                            <td width="15%">
                              <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>

                            </td>
                            <td width="35%">
                               
                                <eluc:UserControlCurrency ID="ddlCurrencyCode" runat="server" 
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" 
                                    CurrencyList="<%# PhoenixRegistersCurrency.ListCurrency(1)%>" />
                               
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <asp:Label ID="lblStokType" runat="server" Text="Stock Type" Visible="false"></asp:Label>
                            </td>
                            <td width="35%">
                                <asp:DropDownList ID="ddlStockType" runat="server" AppendDataBoundItems="true" 
                                    AutoPostBack="true" CssClass="dropdown_mandatory" 
                                    OnTextChanged="ddlStockType_Changed" Visible="false">
                                    <asp:ListItem Text="--Select--" Value="Dummy"></asp:ListItem>
                                    <asp:ListItem Text="SERVICE" Value="SERVICE"></asp:ListItem>
                                    <asp:ListItem Text="STORE" Value="STORE"></asp:ListItem>
                                </asp:DropDownList>
                                <eluc:Hard ID="ddlStockClassType" runat="server" AppendDataBoundItems="true" 
                                    CssClass="dropdown_mandatory" Visible="false" />
                            </td>
                            <td width="15%">                                
                                
                            </td>
                            <td width="35%">
                                
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4">
                        <hr />
                        </td>
                        </tr>
                        <tr>
                            <td width="15%">
                              <asp:Literal ID="lblStartDate" runat="server" Text="Start Date"></asp:Literal>
                                
                            </td>
                            <td width="35%">
                                <eluc:UserControlDate ID="ucStartDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                            <td width="15%">
                              <asp:Literal ID="lblEndDate" runat="server" Text="End Date"></asp:Literal>
                                
                            </td>
                            <td>
                                <eluc:UserControlDate ID="ucEndDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                              <asp:Literal ID="lblFrequency" runat="server" Text="Frequency"></asp:Literal>
                                
                            </td>
                            <td width="35%">
                                <eluc:Number ID="ucFrequencyValue" runat="server" CssClass="input_mandatory" IsPositive="true"
                                    DecimalPlace="0" MaxLength="3" Width="45px" />
                                <asp:DropDownList ID="ddlFrequencyType" runat="server" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Day(s)" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Week(s)" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Month(s)" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="One time only" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                              <asp:Literal ID="lblTemplateStatus" runat="server" Text="Template Status"></asp:Literal>
                                </td>
                            <td width="35%">
                                <div>
                                    <asp:RadioButton ID="rdoActive" Text="Active" runat="server"
                                        GroupName="Status" />
                                    <asp:RadioButton ID="rdoInActive" Text="Inactive" runat="server" 
                                        GroupName="Status" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                              <asp:Literal ID="lblLastScheduleDoneDate" runat="server" Text="Last Schedule Done Date"></asp:Literal>
                                </td>
                            <td width="35%">
                                <eluc:UserControlDate ID="ucLastScheduleDoneDate" runat="server" ReadOnly="true"
                                    CssClass="readonlytextbox"/>
                            </td>
                            <td>
                              <asp:Literal ID="lblNextScheduleDate" runat="server" Text="Next Schedule Date"></asp:Literal>
                                </td>
                            <td width="35%">
                                <eluc:UserControlDate ID="ucNextScheduleDate" runat="server" ReadOnly="true"
                                    CssClass="readonlytextbox"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
