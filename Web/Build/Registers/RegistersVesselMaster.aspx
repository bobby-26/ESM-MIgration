<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselMaster.aspx.cs" Inherits="RegistersVesselMaster" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Master</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvVesselList.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
            function pageLoad() {
                Resize();
            }
         </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator ID="RadFormDecorator" runat="server" DecorationZoneID="table1" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvVesselList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvVesselList"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuRegistersVesselList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuRegistersVesselList"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvVesselList"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="table1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuRegisters" runat="server" OnTabStripCommand="MenuRegisters_TabStripCommand" TabStrip="true" />
        <eluc:Status runat="server" ID="ucStatus" />
        <table width="100%" id="table1" runat="server">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSearchVesselList" runat="server" MaxLength="100" Width="240px" EmptyMessage="Type vessel name here"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselCode" runat="server" Text="Vessel Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtVesselCode" runat="server" MaxLength="10" Width="240px" EmptyMessage="Type vessel code here"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList ID="ddlActiveyn" runat="server" AppendDataBoundItems="true">
                        <Items>
                            <telerik:DropDownListItem Value="Dummy" Text="--Select--" />
                            <telerik:DropDownListItem Value="1" Text="Yes" />
                            <telerik:DropDownListItem Value="0" Text="No" />
                        </Items>
                    </telerik:RadDropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:VesselType ID="ucVesselType" runat="server" Width="240px" AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" Width="240px" AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Flag ID="ucFlag" runat="server" AppendDataBoundItems="true" />
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuRegistersVesselList" runat="server" OnTabStripCommand="RegistersVesselList_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvVesselList" runat="server" OnNeedDataSource="gvVesselList_NeedDataSource" Height="441px"
            GridLines="None" AutoGenerateColumns="false" OnItemDataBound="gvVesselList_ItemDataBound" OnItemCommand="gvVesselList_ItemCommand"
            AllowPaging="true" AllowSorting="false" AllowCustomPaging="true" GroupingEnabled="false" EnableViewState="false">
            <MasterTableView InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AutoGenerateColumns="false"
                DataKeyNames="FLDVESSELID" TableLayout="Fixed" EnableHeaderContextMenu="true">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel Id" SortExpression="FLDVESSELID">
                        <HeaderStyle Width="6%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="true" SortExpression="FLDVESSELNAME">
                        <HeaderStyle Width="15%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselEditId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                            <asp:LinkButton ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' CommandName="REDIRECT"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Code">
                        <HeaderStyle Width="10%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="IMO No.">
                        <HeaderStyle Width="10%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselIMO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMONUMBER") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Flag">
                        <HeaderStyle Width="20%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELFLAG") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELFLAG") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type">
                        <HeaderStyle Width="20%" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" AllowDragToGroup="false">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="395px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
