<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOwnerSOAConfirmationcheck.aspx.cs"
    Inherits="AccountsOwnerSOAConfirmationcheck" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Mask" Src="../UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Verify</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            .style1 {
                height: 20px;
            }
        </style>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />


                <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuFormMain_TabStripCommand"
                    TabStrip="false"></eluc:TabStrip>

                <br />
                <br />
                <div id="checkpost" runat="server">
                    <table width="75%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblckInvoicenumber" runat="server" Text="Vessel Account Description"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlVesselAccount" runat="server" Width="200" Filter="Contains" EmptyMessage="Type to select"
                                    AppendDataBoundItems="true">
                                </telerik:RadComboBox>
                            </td>

                            <td>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text="Vessel Disbursement Amount">Vessel Disbursement Amount</telerik:RadLabel>
                            </td>

                            <td>
                                <eluc:Mask ID="txtAmount" runat="server" MaskText="#,###,###.##"  Width="120px" DecimalPlace="2"  />
                                <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="9,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtAmount" />


                                <telerik:RadTextBox ID="txtAmount" runat="server" CssClass="input_mandatory txtNumber"
                                    DecimalPlace="2" Width="120px"></telerik:RadTextBox>--%>

                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <br />
                <div>
                    <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 550px; width: 100%"
                        frameborder="0"></iframe>
                    <asp:HiddenField ID="hdnScroll" runat="server" />
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
