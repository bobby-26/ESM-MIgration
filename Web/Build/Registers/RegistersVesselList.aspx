<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselList.aspx.cs" Inherits="RegistersVesselList"%>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersVesselList" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <%--        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxPanel1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
        <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
        <PersistenceSettings>
            <telerik:PersistenceSetting ControlID="gvVesselList" />
        </PersistenceSettings>
    </telerik:RadPersistenceManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureVesselList" width="100%">
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
                        <telerik:RadDropDownList ID="ddlActiveyn" runat="server" AppendDataBoundItems="true" AutoPostBack="true">
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
                        <eluc:VesselType ID="ucVesselType" runat="server" Width="240px" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" Width="240px" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag ID="ucFlag" runat="server" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersVesselList" runat="server" OnTabStripCommand="RegistersVesselList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselList" runat="server" OnNeedDataSource="gvVesselList_NeedDataSource"
                AutoGenerateColumns="False" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"  OnPageIndexChanged="gvVesselList_PageIndexChanged"
                OnPreRender="gvVesselList_PreRender" GridLines="None" OnItemCommand="gvVesselList_ItemCommand">
                <MasterTableView InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVESSELID" TableLayout="Fixed" EnableHeaderContextMenu="true" CurrentResetPageIndexAction="ReportError">
                    <Columns>
                        <telerik:GridEditCommandColumn>
                            <HeaderStyle Width="40px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Id" AllowSorting="true" SortExpression="FLDVESSELID">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Code">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IMO No.">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselIMO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMONUMBER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELFLAG") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Vessels matching your search criteria"
                        PageSizeLabelText="Vessels per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <%--<Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="25" LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="25" />--%>
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                <%--<ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>--%>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

