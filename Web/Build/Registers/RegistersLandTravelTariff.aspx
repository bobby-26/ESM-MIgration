<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersLandTravelTariff.aspx.cs" Inherits="RegistersLandTravelTariff" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlType" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="52%"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Text="Meter Tariff" Value="1" />
                                <telerik:RadComboBoxItem Text="City Hourly Package" Value="2" />
                                <telerik:RadComboBoxItem Text="Out of City (Only Drop)" Value="3" />
                                <telerik:RadComboBoxItem Text="Out of City (Up & Down)" Value="4" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAgentName" runat="server" Text="Agent Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="15%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="50%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                ID="ImgSupplierPickList"
                                ToolTip="Select Agent">
                                <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblVehicleType" runat="server" Text="Vehicle Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVehicleType" runat="server" CssClass="input_mandatory" DataTextField="FLDVEHICLENAME" DataValueField="FLDVEHICLETYPE" AppendDataBoundItems="true" Width="52%"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHours" runat="server" Text="Hours"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtHours" Width="52%" CssClass="input" IsInteger="true" IsPositive="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblkms" runat="server" Text="Kms"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtKms" Width="52%" CssClass="input" IsInteger="true" IsPositive="true" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ddlCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="52%"
                            AppendDataBoundItems="true" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtAmount" Width="52%" CssClass="input" DecimalPlace="2" IsPositive="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAddlchargesperkm" runat="server" Text="Additional Charges / Km"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtAddlchargesperkm" Width="52%" CssClass="input" DecimalPlace="2" IsPositive="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWtgchargesforonemin" runat="server" Text="Wtg Charges for 1 min"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtWtgchargesforonemin" Width="52%" CssClass="input" DecimalPlace="2" IsPositive="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReason" runat="server" Text="Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReason" runat="server" CssClass="input" Width="52%"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTariff" runat="server" AutoGenerateColumns="False" Height="75%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="true"
                OnNeedDataSource="gvTariff_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvTariff_ItemCommand"
                OnItemDataBound="gvTariff_ItemDataBound1">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" DataKeyNames="FLDTARIFFID" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPACKAGETYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Agent Name" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTariffId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARIFFID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkAgentName" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex%>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTNAME")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vehicle Type" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVehicleType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVEHICLENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hours" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHours" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOURS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Kms" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblKms" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKMS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Additional Charges / Km" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdditionalCharges" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDLCHARGESPERKM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Wtg Charges for 1 min" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWtgCharges" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWTGCHARGESFORONEMIN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
