<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNIAccounts.aspx.cs"
    Inherits="InspectionPNIAccounts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspection PNI Accounts</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="divInspectionPNIAccounts" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionPNIAccounts" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionPNI">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblAccounts" runat="server" Text="Accounts"></asp:Literal>
                    </div>
                </div>                
                <div style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuInspectionPNIAccounts" runat="server" OnTabStripCommand="InspectionPNIAccounts_TabStripCommand">
                        </eluc:TabStrip>                    
                </div>                
                <br />
                <div id="divFind" style="position: relative; z-index: 2">
                    <asp:Panel ID="pnlAccountsIndia" runat="server" GroupingText="Accounts-India">
                        <table id="tblInspectionPNIAccounts" width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblSPAfortheVesselAC" runat="server" Text="SPA for the Vessel A/C's"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAcctAdminId" runat="server" Visible="false" ></asp:TextBox>
                                    <asp:TextBox ID="txtSPA" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblCaseSubmittedToAC" runat="server" Text="Case submitted to A/C's"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtCaseSubmittedDate" runat="server" ReadOnly="true" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                        Height="40px" Width="80%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                   <br />
                    <asp:Panel ID="pnlAccountsSingapore" runat="server" GroupingText="Accounts-Singapore" >
                        <table id="tblInspectionPNIAccountsSingapore" width="100%">
                            <tr>
                                <td>
                                   <asp:Literal ID="lblTotalCost" runat="server" Text="Total Cost"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Number ID="txtTotalCost" runat="server" CssClass="input" DefaultZero="false"
                                        Width="90px" />
                                </td>
                                <td>
                                    <asp:literal ID="lblClaimSubmitted" runat="server" Text="Claim submitted to P & I"></asp:literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtPNIClaimDate" runat="server" CssClass="input" DatePicker="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblDateofAmountReceived" runat="server" Text="Date of Amount Received"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtDateAmtReceived" runat="server" CssClass="input" DatePicker="true" />
                                </td>
                                <td>
                                   <asp:literal ID="lblFinalClosureDate" runat="server" Text="Final Closure Date"></asp:literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtFinalClosure" runat="server" CssClass="input" DatePicker="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <asp:Literal ID="lblACIncharge" runat="server" Text="A/C Incharge"></asp:Literal>
                                </td>
                                <td>
                                    <span id="spnPickListAcctIncharge">
                                        <asp:TextBox ID="txtAcctInchargeName" runat="server" CssClass="input" Enabled="false"
                                            MaxLength="200" Width="35%"></asp:TextBox>
                                        <asp:TextBox ID="txtAcctInchargeDesignation" runat="server" CssClass="input" Enabled="false"
                                            MaxLength="50" Width="25%"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="imgShowSupt" Style="cursor: pointer; vertical-align: top"
                                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPickListAcctIncharge', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                                        <asp:TextBox runat="server" ID="txtAcctIncharge" Text="" CssClass="input"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="txtAcctInchargeEmailHidden" Text="" CssClass="input"
                                            Width="10px"></asp:TextBox>
                                    </span>
                                </td>
                                <td>
                                    <asp:Literal ID="lblCaseSubmittedToSingapore" runat="Server" Text="Case submitted to Singapore A/C's"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtSubmittedtoSingaporeAcct" runat="server" ReadOnly="true" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
