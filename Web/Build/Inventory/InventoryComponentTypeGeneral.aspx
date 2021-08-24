<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentTypeGeneral.aspx.cs"
    Inherits="InventoryComponentTypeGeneral" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register Src="../UserControls/UserControlVesselType.ascx" TagName="UserControlVesselType"
    TagPrefix="uc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPlannedMaintenanceComponentTypeGeneral" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="General" ShowMenu="false"></eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuComponentTypeGeneral" runat="server" OnTabStripCommand="PlannedMaintenanceComponentType_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlComponentTypeGeneral">
        <ContentTemplate>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 15%">
                        <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtComponentTypeNumber" CssClass="input_mandatory"
                            MaxLength="50" Width="80px"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtComponentTypeNumber"
                            Mask="999.99.99" MaskType="None" Filtered="0123456789" InputDirection="LeftToRight"
                            ClearMaskOnLostFocus="false">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td style="width: 15%">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="width: 15%">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComponentTypeName" runat="server" CssClass="input_mandatory"
                            MaxLength="200" Width="240px"></asp:TextBox>
                    </td>
                    <td style="width: 15%">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="width: 15%">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <asp:Literal ID="lblMaker" runat="server" Text="Maker"></asp:Literal>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <asp:TextBox ID="txtMakerCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="txtMakerName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="120px"></asp:TextBox>
                            <img runat="server" id="imgShowMaker" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true);" />
                            <asp:TextBox ID="txtMakerId" runat="server" CssClass="input" Width="10px"></asp:TextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdMakerClear_Click" />
                    </td>
                    <td style="width: 25">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="width: 20">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 20">
                        <asp:Literal ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></asp:Literal>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <asp:TextBox ID="txtPreferredVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="txtPreferredVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="120px"></asp:TextBox>
                            <img runat="server" id="imgShowVendor" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListVendor', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131', true); " />
                            <asp:TextBox ID="txtVendorId" runat="server" CssClass="input" Width="10px"></asp:TextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click" />
                    </td>
                    <td style="width: 15%">
                    </td>
                    <td>
                    </td>
                    <td style="width: 15%">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <asp:Literal ID="lblParent" runat="server" Text="Parent"></asp:Literal>
                    </td>
                    <td>
                        <span id="spnPickListComponentTypeParent">
                            <asp:TextBox ID="txtParentNumber" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="txtParentName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="120px"></asp:TextBox>
                            <img runat="server" id="imgShowComponentTypeParent" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListComponentTypeParent', 'codehelp1', '', '../Common/CommonPickListComponentType.aspx', true);" />
                            <asp:TextBox ID="txtParentId" runat="server" CssClass="input" MaxLength="20" Width="10px"></asp:TextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdComponentTypeParentClear_Click" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="width: 15%">
                        &nbsp;
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <asp:Literal ID="lblComponentClass" runat="server" Text="Component Class"></asp:Literal>
                    </td>
                    <td>
                        <eluc:UserControlQuick ID="ddlComponentClass" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="width: 15%">
                        &nbsp;
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top" style="width: 15%">
                    <td>
                        <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                    </td>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtType" CssClass="input" MaxLength="50" Width="240px"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="width: 15%">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
