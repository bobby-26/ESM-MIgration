<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPurchaseFormType.aspx.cs" Inherits="InspectionPurchaseFormType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormType" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuStockItemGeneral" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AssignedVessels="true" VesselsOnly="true" OnTextChangedEvent="ddlStockType_TextChanged"
                            AutoPostBack="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlStockType" CssClass="input_mandatory" OnTextChanged="ddlStockType_TextChanged"
                            AutoPostBack="true" Filter="Contains" Width="240px">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" Selected="True" />
                                <telerik:RadComboBoxItem Text="Spares" Value="SPARE" />
                                <telerik:RadComboBoxItem Text="Stores" Value="STORE" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFormType" runat="server" Text="Form Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rdoFormType" runat="server" Direction="Horizontal" Width="360px">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCreationMethods" runat="server" Text="Creation Methods"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblCreation" runat="server" direction="Horizontal"
                            Width="360px">
                            <Items>
                                <telerik:ButtonListItem Text="Manual" Value="Manual" Selected="True"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Automatic" Value="Automatic" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClass" runat="server">
                            Component Class
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListClass">
                            <telerik:RadTextBox ID="txtStockClass" runat="server" Width="90px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtStockClassName" runat="server" Width="240px"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="btnStockClassPickList" OnClientClick="return showPickList('spnPickListClass', 'codehelp1', '', '../Common/CommonPickListComponentClass.aspx', true);">
                            <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtStockClassId" runat="server" Width="90px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
            </table>
            <table id="tblRange" runat="server" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblItemRangeFrom" runat="server" Text="Item Range From"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickStock">
                            <telerik:RadTextBox ID="txtStockItemFrom" runat="server" Width="120px"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="btnPicstockItem" OnClientClick="return showPickList('spnPickStock', 'codehelp1', '', '../Common/CommonPickListStockItem.aspx', true);">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtStockItemName" runat="server" Width="1px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtStockItemID" runat="server" Width="1px"></telerik:RadTextBox>
                        </span>To <span id="spnPickStockTo">
                            <telerik:RadTextBox ID="txtStockItemTo" runat="server" Width="120px"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="btnStocitemTo" OnClientClick="return showPickList('spnPickStock', 'codehelp1', '', '../Common/CommonPickListStockItem.aspx', true);">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtStockItemToName" runat="server" Width="1px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtStockItemToId" runat="server" Width="1px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListContract">
                            <telerik:RadTextBox ID="txtContractorCode" runat="server" Width="90px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtContractorName" runat="server" Width="240px"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="btnContractPickList" OnClientClick="return showPickList('spnPickListContract', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo', true);">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtContractorId" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
