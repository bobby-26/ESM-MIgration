<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPhoneCardRequisitionGeneral.aspx.cs"
    Inherits="VesselAccountsPhoneCardRequisitionGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Phone Card Requisition</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlPhonReq">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="" ShowMenu="false"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuPhonReq" runat="server" OnTabStripCommand="MenuPhonReq_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Text="<%#SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.VesselName %>"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRequestNo" runat="server" Text="Request No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" Width="150px"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRequestDate" runat="server" Text="Request Date"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Date ID="txtRequestDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <%-- <td>
                            Ship Chandler Name
                        </td>
                        <td colspan="3">
                            <span id="spnPickListSupplier">
                                <asp:TextBox ID="txtSupplierCode" runat="server" Width="60px" CssClass="input_mandatory"
                                    Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtSupplierName" runat="server" Width="180px" CssClass="input_mandatory"
                                    Enabled="False"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="cmdShowSupplier" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListVesselSupplier.aspx', true);"
                                    Text=".." />
                                <asp:TextBox ID="txtSupplierId" runat="server" Width="1px" CssClass="input"></asp:TextBox>
                            </span>
                        </td>--%>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
