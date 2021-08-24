<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceQueryQuestions.aspx.cs" Inherits="AccountsInvoiceQueryQuestions" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Questions</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDebitCreditNote" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                            CssClass="hidden" />
            <eluc:TabStrip ID="MenuInvoiceQuestion" runat="server" OnTabStripCommand="MenuInvoiceQuestion_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" style="width: 100%">
                <tr>
                    <td width="50%" valign="top">
                        <table width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblReasons" runat="server" Text="Reasons :"></telerik:RadLabel>
                                    <br />
                                    <div id="Div2" runat="server" class="input" style="overflow: auto; width: 60%; height: 150px">
                                        <asp:CheckBoxList ID="chkReasons" runat="server" Height="100%" RepeatColumns="1"
                                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="50%" valign="top">
                        <table width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblQuestion" runat="server" Text="Question :"></telerik:RadLabel>
                                    <br />
                                   <telerik:RadTextBox ID="txtQuestion" runat="server" CssClass="input" TextMode="MultiLine" Width="500px" Height="50px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblInvoicePIC" runat="server" Text="Invoice PIC"></telerik:RadLabel>
                                    <telerik:RadDropDownList ID="ddlInvoicePIC" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlInvoicePIC_OnSelectedIndexChanged">
                                    </telerik:RadDropDownList>
                                    <span id="spnPickListFleetManager">
                                       <telerik:RadTextBox ID="txtMentorName" runat="server" CssClass="input_mandatory" MaxLength="100" AutoPostBack="true" OnTextChanged="txtMentorName_OnTextChanged"
                                            Width="30%"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtuserDesignation" runat="server" CssClass="hidden" Enabled="false"
                                            MaxLength="30" Width="5px" ReadOnly="true"></telerik:RadTextBox>
                                        <asp:ImageButton runat="server" ID="imguser" Style="cursor: pointer; vertical-align: top"
                                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); "
                                            ToolTip="Select Mentor" />
                                        <telerik:RadTextBox runat="server" ID="txtuserid" CssClass="hidden"></telerik:RadTextBox>
                                        <telerik:RadTextBox runat="server" ID="txtuserEmailHidden" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
