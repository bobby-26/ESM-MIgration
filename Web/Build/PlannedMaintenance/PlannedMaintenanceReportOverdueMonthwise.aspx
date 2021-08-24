<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceReportOverdueMonthwise.aspx.cs" Inherits="PlannedMaintenanceReportOverdueMonthwise" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tables" Src="~/UserControls/UserControlVesselTables.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function setSize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= RadGrid1.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>

    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body onload="setSize();" onresize="setSize();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />


        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
            </telerik:RadWindowManager>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr style="height: 5px;">
                    <td colspan="6">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 9%; vertical-align: top">
                        <asp:Literal ID="lblDateBetween" runat="server" Text="Date Between"></asp:Literal>
                    </td>
                    <td style="width: 13%; vertical-align: top">
                        <eluc:Date runat="server" ID="txtDateFrom" CssClass="input" />
                    </td>
                    <td style="width: 1%; vertical-align: top"><b>-</b>
                    </td>
                    <td style="width: 13%; vertical-align: top; align-items: ">
                        <eluc:Date runat="server" ID="txtDateTo" CssClass="input" />
                    </td>
                    <td style="width: 4%; vertical-align: top">
                        <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                    </td>
                    <td style="width: 10%; vertical-align: top">
                        <telerik:RadDropDownList ID="drdwnFleet" runat="server" AutoPostBack="true" Width="100px"
                            CssClass="input" DataTextField="FLDFLEETDESCRIPTION"
                            DataValueField="FLDFLEETID"
                            OnSelectedIndexChanged="drdwnFleet_SelectedIndexChanged"
                            OnDataBound="drdwnFleet_DataBound">
                        </telerik:RadDropDownList>
                    </td>
                    <td style="width: 4%; vertical-align: top">
                        <asp:Literal ID="lblVessels" runat="server" Text="Vessels"></asp:Literal>
                    </td>
                    <td style="width: 43%; vertical-align: top">
                        <div runat="server" id="dvClass" class="input" style="overflow: auto; width: 60%; height: 100px;">
                            <telerik:RadCheckBoxList ID="chkVessels" runat="server" DataBindings-DataTextField="FLDVESSELNAME" DataBindings-DataValueField="FLDVESSELID"></telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuTables" runat="server" OnTabStripCommand="MenuTables_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />

            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowCustomPaging="true" AllowSorting="false" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender" OnItemCreated="_grid_Created"
                OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" AutoGenerateColumns="false">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="true" TableLayout="Fixed" CommandItemDisplay="Top">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" AlternateText="Edit" CommandName="GRAPH" ID="cmdGraph"
                                    ToolTip="Graph"><span class="icon"><i class="far fa-chart-bar"></i></span></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="1" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
