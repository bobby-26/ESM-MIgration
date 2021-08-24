<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRequestDetails.aspx.cs"
    Inherits="AccountsAllotmentRequestDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Allotment Request System Checking</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="CrewBankAccountListlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>        
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCrewBankAccountList" runat="server" submitdisabledcontrols="true">
    <ajaxtoolkit:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" combinescripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlCrewBankAccountListEntry">
        <ContentTemplate>
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:title runat="server" id="ucTitle" text="Allotment Request System Checking"
                            showmenu="true" />
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                            CssClass="hidden" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:tabstrip id="MenuAllotment" runat="server" ontabstripcommand="MenuAllotment_TabStripCommand"
                        tabstrip="true"></eluc:tabstrip>
                </div>
                <div class="subHeader">
                    <div class="divFloat" style="clear: right">
                        <eluc:tabstrip id="MenuChecking" runat="server" ontabstripcommand="MenuChecking_TabStripCommand">
                        </eluc:tabstrip>
                    </div>
                </div>
                <table width="100%" cellpadding="2" cellpadding="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblFileNo" runat="server" Text="File No."></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblBeneficiaryName" runat="server" Text="Beneficiary Name:"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBeneficiary" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRank" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblBeneficiaryBank" runat="server" Text="Beneficiary Bank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBeneficiaryBank" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblbEmployeeName" runat="server" Text="Employee Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblBeneficiaryAccountNo" runat="server" Text="Beneficiary Account No."></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAllotmentType" runat="server" Text="Allotment Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAllotmentType" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblBankAddress" runat="server" Text="Bank Address"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBankAddress" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                TextMode="MultiLine" Width="150px" Height="40"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFortheMonthof" runat="server" Text="For the Month of"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMonthAndYear" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblIFSCCode" runat="server" Text="IFSC Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIFSCCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSignOffReason" runat="server" Text="Reason for Sign Off"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSignoffReason" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <asp:RadioButtonList ID="rblAllotmentCheck" runat="server" AppendDataBoundItems="false"
                    Height="10" RepeatDirection="Horizontal" AutoPostBack="true" RepeatLayout="Table"
                    CssClass="readonlytextbox" OnSelectedIndexChanged="rblAllotmentCheck_SelectedIndexChanged"
                    Enabled="true">
                    <asp:ListItem Text="Side Letter" Value="4"></asp:ListItem>
                </asp:RadioButtonList>
                <eluc:status id="ucStatus" runat="server" visible="false" />
                <iframe runat="server" id="ifMoreInfo" style="min-height: 320px; width: 100%" scrolling="yes">
                </iframe>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
