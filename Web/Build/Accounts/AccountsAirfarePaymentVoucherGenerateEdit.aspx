<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfarePaymentVoucherGenerateEdit.aspx.cs" Inherits="AccountsAirfarePaymentVoucherGenerateEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiOwnerBudgetCode" Src="~/UserControls/UserControlMultipleColumnOwnerBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Generate Payment Voucher</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPVGenerate" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPVGenerate">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">                
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="False"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuPVGenerate" runat="server" OnTabStripCommand="MenuPVGenerate_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblCourse" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblWriteOff" runat="server" Text="Write Off to Aviation Income"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID ="chkWriteOff" runat="server" OnCheckedChanged="chkWriteOff_OnCheckedChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblAccountCode" runat="server" Text="Account Code"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAccount" runat="server" CssClass="input_mandatory" OnDataBound="ddlAccountDetails_DataBound"
                                    DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" OnTextChanged="ddlAccountDetails_TextChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                            </td>
                            <td>
                                <eluc:BudgetCode ID="ucBudgetCode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    OnTextChangedEvent="ucBudgetCode_Changed" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></asp:Literal>
                            </td>
                            <td>
                                <eluc:MultiOwnerBudgetCode ID="ucOwnerBudgetCode" runat="server" CssClass="input_mandatory" Width="40%" Enabled="true" />
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblChargedAmount" runat="server" Text="Charged Amount"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtChargedAmount" runat="server" CssClass="input_mandatory" DecimalPlace="2"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPayableAmount" runat="server" Text="Payable Amount"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtPayableAmount" runat="server" CssClass="input_mandatory" DecimalPlace="2"  />
                            </td>
                        </tr>
                        <tr id="trPayingCompany" runat="server" visible="false">
                            <td>
                                <asp:Literal ID="lblPayingCompanyCodeHdr" runat="server" Text="Paying Company"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="lblPayingCompanyCode" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass ="input" TextMode="MultiLine" Width ="50%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <eluc:Status runat="server" ID="ucStatus" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
