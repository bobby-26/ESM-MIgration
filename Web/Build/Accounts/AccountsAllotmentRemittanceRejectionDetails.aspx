<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceRejectionDetails.aspx.cs" Inherits="AccountsAllotmentRemittanceRejectionDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rejection Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRejectionDetails" runat="server">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="PnlRejectionDetails">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="Allotment Remittance Rejection" ShowMenu="false" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuRejectionDetails" runat="server" OnTabStripCommand="MenuRejectionDetails_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblRejectionReason" runat="server" Text="Rejection Reason"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick ID="ddlRejectionReason" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    QuickTypeCode="162" QuickList='<%#PhoenixRegistersQuick.ListQuick(1,162)%>' Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRejectionRemarks" runat="server" Text="Rejection Remarks"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRejectionRemarks" CssClass="input_mandatory" TextMode="MultiLine" Width="244px" Height="78px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRejectedBy" runat="server" Text="Rejected By"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtRejectedBy" CssClass="readonlytextbox" Width="249px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblApprovedBy" runat="server" Text="Approved By"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtApprovedby" CssClass="readonlytextbox" Width="249px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <eluc:Status ID="ucStatus" runat="server" />
                    <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes" CancelText="No" Visible="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
        </div>
    </form>
</body>
</html>
