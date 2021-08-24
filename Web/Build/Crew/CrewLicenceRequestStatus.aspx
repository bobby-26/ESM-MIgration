<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestStatus.aspx.cs"
    Inherits="CrewLicenceRequestStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Licence Request Status</title>
       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLicReqStatus" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <asp:Literal ID="lblLicenceRequestCRAStatus" runat="server" Text="Licence Request CRA Status"></asp:Literal>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuStatus" runat="server" OnTabStripCommand="Status_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <div class="subHeader" style="position: relative;">
            <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                <eluc:TabStrip ID="LicenceRequestStatus" runat="server" OnTabStripCommand="LicenceRequestStatus_TabStripCommand">
                </eluc:TabStrip>
            </span>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        </div>
        <table cellpadding="3" cellspacing="1" width="100%">
            <tr>
                <td>
                    <asp:Literal ID="lblEmployeeName" runat="server" Text="Employee Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEmployeeName" Width="240px" CssClass="readonlytextbox"
                        ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblDateofFlagstateprocess" runat="server" Text="Date of Flag state process"></asp:Literal>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtDateofFlagStateProcess" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCRAHandedoverto" runat="server" Text="CRA Handed over to"></asp:Literal>
                </td>
                <td>
                    <span id="spnCRAHandedOverTo">
                        <asp:TextBox ID="txtCRAPOName" runat="server" CssClass="input" Enabled="false" MaxLength="200"
                            Width="150px"></asp:TextBox>
                        <asp:TextBox ID="txtCRAPODesignation" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="100px"></asp:TextBox>
                        <asp:ImageButton runat="server" ID="imgShowCRAPO" Style="cursor: pointer; vertical-align: top"
                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnCRAHandedOverTo', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                        <asp:TextBox runat="server" ID="txtCRAPOId" CssClass="input" Width="10px"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txtCRAPOEmailHidden" CssClass="input" Width="10px"></asp:TextBox>
                    </span>
                </td>
                <td>
                    <asp:Literal ID="lblDateofHandover" runat="server" Text="Date of Handover"></asp:Literal>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtDateOfHandover" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCRANumber" runat="server" Text="CRA Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCRANumber" CssClass="input"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblExpiryDateofCRA" runat="server" Text="Expiry Date of CRA"></asp:Literal>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtExpiryDateofCRA" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblFullTermDocsReceived" runat="server" Text="Full Term Docs Received"></asp:Literal>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtFullTermDocsReceived" CssClass="input" />
                </td>
                <td>
                    <asp:Literal ID="lblFullTermHandedoverto" runat="server" Text="Full Term Handed over to"></asp:Literal>
                </td>
                <td>
                    <span id="spnFullTermHandedOverTo">
                        <asp:TextBox ID="txtFullTermPOName" runat="server" CssClass="input" Enabled="false"
                            MaxLength="200" Width="150px"></asp:TextBox>
                        <asp:TextBox ID="txtFullTermPODesignation" runat="server" CssClass="input" Enabled="false"
                            MaxLength="50" Width="100px"></asp:TextBox>
                        <asp:ImageButton runat="server" ID="imgShowFullTermPO" Style="cursor: pointer; vertical-align: top"
                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnFullTermHandedOverTo', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                        <asp:TextBox runat="server" ID="txtFullTermPOId" CssClass="input" Width="10px"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txtFullTermPOEmailHidden" CssClass="input" Width="10px"></asp:TextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblDateofHandoverofFullterm" runat="server" Text="Date of Handover of Full term"></asp:Literal>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtHandoverofFullTerm" CssClass="input" />
                </td>
                <td>
                    <asp:Literal ID="lblPaymentDetails" runat="server" Text="Payment Details"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPaymentDetails" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtRemarks" CssClass="input" runat="server" TextMode="MultiLine"
                        Width="350px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
