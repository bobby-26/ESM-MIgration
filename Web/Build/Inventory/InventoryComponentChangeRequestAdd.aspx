<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentChangeRequestAdd.aspx.cs" Inherits="Inventory_InventoryComponentChangeRequestAdd" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentTypeTreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentJob" Src="../UserControls/UserControlMultiColumnComponents.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized() {
            var sender = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight - 40);
            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 72);
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmPlannedMaintenanceComponentTypeGeneral" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuRegistersComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="Details" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuRegistersComponent" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>


        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuRegistersComponent" runat="server" OnTabStripCommand="MenuRegistersComponent_TabStripCommand"></eluc:TabStrip>

            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="200px" Width="100%">
                <eluc:Status runat="server" ID="ucStatus" />
                <telerik:RadPane ID="navigationPane" runat="server" Width="200">
                    <eluc:ComponentTypeTreeView ID="tvwComponent" runat="server" OnNodeClickEvent="tvwComponent_NodeClickEvent" EmptyMessage="Type to search component" />
                    <asp:Label runat="server" ID="lblSelectedNode"></asp:Label>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server">
                    <div id="Details" runat="server">
                        <br clear="all" />
                        <br clear="all" />
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblChangeRequestType" runat="server" Text="Change Request Type"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadDropDownList ID="ddlChangeReqType" CssClass="input_mandatory" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlChangeReqType_SelectedIndexChanged">
                                        <Items>
                                            <telerik:DropDownListItem Value="" Text="--Select--" />
                                            <telerik:DropDownListItem Value="0" Text="Insert New" />
                                            <telerik:DropDownListItem Value="1" Text="Update" />
                                            <telerik:DropDownListItem Value="2" Text="Delete" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                    <telerik:RadLabel ID="lbldtkeyChange" runat="server" Visible="false" Text=""></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtRemarksChange" runat="server" Rows="2" TextMode="MultiLine" Width="240px"
                                        CssClass="input">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblComponentNumberChange" runat="server" Text="Number"></telerik:RadLabel>
                                </td>
                                <td>
                                    <%-- <asp:TextBox runat="server" ID="txtComponentNumberChange" CssClass="input_mandatory"
                                        MaxLength="50" Width="80px"></asp:TextBox>--%>
                                    <telerik:RadMaskedTextBox ID="txtComponentNumberChange" Width="80px" runat="server" Mask="###.##.##"></telerik:RadMaskedTextBox>
                                    <%--  <ajaxtoolkit:maskededitextender id="MaskedEditExtender1Change" runat="server" targetcontrolid="txtComponentNumberChange"
                                        mask="999.99.99" masktype="None" filtered="0123456789" inputdirection="LeftToRight"
                                        clearmaskonlostfocus="false">
                                </ajaxtoolkit:maskededitextender>--%>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblSerialNumberChange" runat="server" Text="Serial Number"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSerialNumberChange" runat="server" CssClass="input" MaxLength="50" Width="80px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                                </td>
                                <td colspan="3">
                                    <telerik:RadTextBox ID="txtComponentNameChange" runat="server" CssClass="input_mandatory" MaxLength="200" Width="240px"></telerik:RadTextBox>
                                    <telerik:RadLabel ID="lblComponentTypeChange" runat="server" Visible="false" Text=""></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblMakerChange" runat="server" Text="Maker"></telerik:RadLabel>
                                </td>
                                <td>
                                   <%-- <span id="spnPickListMakerChange">
                                        <telerik:RadTextBox ID="txtMakerCodeChange" runat="server" CssClass="input" MaxLength="20"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtMakerNameChange" runat="server" CssClass="input" MaxLength="20"
                                            Width="120px">
                                        </telerik:RadTextBox>
                                        <img runat="server" id="imgShowMakerChange" style="cursor: pointer; vertical-align: top" src="<%$ PhoenixTheme:images/picklist.png %>" />
                                        <telerik:RadTextBox ID="txtMakerIdChange" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                                    </span>&nbsp;--%>
                                    <eluc:UserControlAddress ID="txtMakerIdChange" runat="server" AddressType="130,131" Width="240px" />
                                <asp:LinkButton ID="cmdMakerChangeClear" runat="server" 
                                    ImageAlign="AbsMiddle" Text=".." OnClick="cmdMakerChangeClear_Click1">
                                    <span class="ïcon"><i class="fas fa-broom"></i></span>
                                </asp:LinkButton>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblVendorChange" runat="server" Text="Vendor"></telerik:RadLabel>
                                </td>
                                <td>
                                   <%-- <span id="spnPickListVendorChange">
                                        <telerik:RadTextBox ID="txtVendorCodeChange" runat="server" CssClass="input" MaxLength="20"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtVendorNameChange" runat="server" CssClass="input" MaxLength="20"
                                            Width="120px">
                                        </telerik:RadTextBox>
                                        <img runat="server" id="imgShowVendorChange" style="cursor: pointer; vertical-align: top" src="<%$ PhoenixTheme:images/picklist.png %>" />
                                        <telerik:RadTextBox ID="txtVendorIdChange" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                                    </span>&nbsp;--%>
                                     <eluc:UserControlAddress ID="txtVendorIdChange" runat="server" AddressType="130,131" Width="240px" />
                                <asp:LinkButton ID="cmdVendorChangeClear" runat="server" 
                                    ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorChangeClear_Click" >
                                    <span class="ïcon"><i class="fas fa-broom"></i></span>
                                </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblLocationChange" runat="server" Text="Location"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtLocationChange" runat="server" CssClass="input" MaxLength="200"
                                        Width="240px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblParentComponentChange" runat="server" Text="Parent Component"></telerik:RadLabel>
                                </td>
                                <td>
                                   <%-- <span id="spnPickListParentComponentChange">
                                        <telerik:RadTextBox ID="txtParentComponentNumberChange" runat="server" CssClass="input"
                                            MaxLength="20" Width="80px">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtParentComponentNameChange" runat="server" CssClass="input" MaxLength="20"
                                            Width="120px">
                                        </telerik:RadTextBox>
                                        <img runat="server" id="imgShowParentComponentChange" style="cursor: pointer; vertical-align: top" src="<%$ PhoenixTheme:images/picklist.png %>" />
                                        <telerik:RadTextBox ID="txtParentComponentIDChange" runat="server" CssClass="input" MaxLength="20"
                                            Width="10px">
                                        </telerik:RadTextBox>
                                    </span>&nbsp;--%>
                                    <eluc:ComponentJob ID="txtParentComponentIDChange" runat="server"  Width="240px"/>
                                <asp:LinkButton ID="cmdParentChangeClear" runat="server" 
                                    ImageAlign="AbsMiddle" Text=".." OnClick="cmdParentComponentChangeClear_Click">
                                    <span class="ïcon"><i class="fas fa-broom"></i></span>
                                </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblClassCodeChange" runat="server" Text="Class Code"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtClassCodeChange" runat="server" CssClass="input" MaxLength="200"
                                        Width="240px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTypeChange" runat="server" Text="Type"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtTypeChange" runat="server" CssClass="input" MaxLength="50" Width="240px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCriticalChange" runat="server" Text="Critical"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkIsCriticalChange" Checked="false" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblComponentStatusChange" runat="server" Text="Status"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:UserControlHard runat="server" ID="ucComponentStatusChange" CssClass="dropdown_mandatory" HardTypeCode="13" AppendDataBoundItems="true" Width="240px"/>
                                </td>
                            </tr>
                        </table>
                        <br clear="all" />
                        <div id="divOldValues" runat="server">
                            <hr />
                            <b>Actual Values</b>
                            <br clear="all" />
                            <br clear="all" />
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Number"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <%--<telerik:RadTextBox runat="server" ID="txtComponentNumber" CssClass="input" MaxLength="50"
                                            Width="80px"></telerik:RadTextBox>--%>
                                        <telerik:RadMaskedTextBox ID="txtComponentNumber" runat="server" Mask="###.##.##"></telerik:RadMaskedTextBox>
                                        <%-- <ajaxtoolkit:maskededitextender id="MaskedEditExtender1" runat="server" targetcontrolid="txtComponentNumber"
                                            mask="999.99.99" masktype="None" filtered="0123456789" inputdirection="LeftToRight"
                                            clearmaskonlostfocus="false">
                                    </ajaxtoolkit:maskededitextender>--%>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblSerialNumber" runat="server" Text="Serial Number"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSerialNumber" runat="server" CssClass="input" MaxLength="50"
                                            Width="80px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Name"></telerik:RadLabel>
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input" MaxLength="200"
                                            Width="240px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <span id="spnPickListMaker">
                                            <telerik:RadTextBox ID="txtMakerCode" runat="server" CssClass="input" MaxLength="20" Width="80px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtMakerName" runat="server" CssClass="input" MaxLength="20" Width="120px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtMakerId" runat="server" CssClass="readonly" Width="10px"></telerik:RadTextBox>
                                        </span>&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <span id="spnPickListVendor">
                                            <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" MaxLength="20" Width="80px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtVendorName" runat="server" CssClass="input" MaxLength="20" Width="120px"></telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="readonly" Width="10px"></telerik:RadTextBox>
                                        </span>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input" MaxLength="200" Width="240px"></telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblParentComponent" runat="server" Text="Parent Component"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <span id="spnPickListParentComponent">
                                            <telerik:RadTextBox ID="txtParentComponentNumber" runat="server" CssClass="input" MaxLength="20"
                                                Width="80px">
                                            </telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtParentComponentName" runat="server" CssClass="input" MaxLength="20"
                                                Width="120px">
                                            </telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtParentComponentID" runat="server" CssClass="input" MaxLength="20"
                                                Width="10px">
                                            </telerik:RadTextBox>
                                        </span>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblClassCode" runat="server" Text="Class Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtClassCode" runat="server" CssClass="input" MaxLength="200" Width="240px"></telerik:RadTextBox>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtType" runat="server" CssClass="input" MaxLength="50" Width="240px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCritical" runat="server" Text="Critical"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadCheckBox ID="chkIsCritical" Checked="false" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblComponentStatus" runat="server" Text="Status"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:UserControlHard runat="server" ID="UcComponentStatus" CssClass="input" Enabled="false"
                                            HardTypeCode="13" AppendDataBoundItems="true" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                </telerik:RadPane>
            </telerik:RadSplitter>
        </div>
        <%--        <script type="text/javascript">
            resizeFrame(document.getElementById('divComponent'));
        </script>--%>
    </form>
</body>
</html>
