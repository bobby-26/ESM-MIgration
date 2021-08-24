<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceBatchUpdate.aspx.cs" Inherits="AccountsAllotmentRemittanceBatchUpdate" %>


<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Update</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlAddressEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <eluc:status runat="server" id="ucStatus" />
                <eluc:tabstrip id="MenuBatchUpdate" runat="server" ontabstripcommand="MenuBatchUpdate_TabStripCommand"></eluc:tabstrip>
            <telerik:RadAjaxPanel runat="server" ID="pnlAddressEntry">
                        <table cellspacing="1" cellpadding="1">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="Date Between"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:usercontroldate id="txtFromDate" runat="server" cssclass="input" />

                                    <eluc:usercontroldate id="txtToDate" runat="server" cssclass="input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:hard id="ddlPaymentmode" runat="server" cssclass="dropdown_mandatory" hardtypecode="132"
                                        hardlist='<%# PhoenixRegistersHard.ListHard(1, 132) %>' appenddatabounditems="true" shortnamefilter="ATT,NLT,ALT"
                                        width="240px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Bank Account"></telerik:RadLabel>
                                </td>
                                <td>

                                    <eluc:usercontrolbankaccount id="ddlBankAccount" bankaccountlist='<%# PhoenixRegistersAccount.ListBankAccount(null,null,iCompanyid,0)%>'
                                        appenddatabounditems="true" runat="server" ontextchangedevent="ddlBankAccount_SelectedIndexChanged" autopostback="true" cssclass="input_mandatory" width="240px" />
                                    <telerik:RadTextBox ID="txtCurrencyId" Visible="false" runat="server"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblBankchargebasis" runat="server" Text="Bank charge basis"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:hard id="ddlBankChargebasis" runat="server" cssclass="dropdown_mandatory" hardtypecode="133"
                                        hardlist='<%# PhoenixRegistersHard.ListHard(1, 133) %>' appenddatabounditems="true"
                                        width="240px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblDRCurrency" runat="server" Text="Remit in DR Currency"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkDRCurrency" runat="server" />
                                    <telerik:RadTextBox ID="txtSubAccountCode" Visible="false" runat="server"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountId" Visible="false" runat="server"> </telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
