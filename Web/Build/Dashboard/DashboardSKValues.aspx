<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKValues.aspx.cs" Inherits="Dashboard_DashboardSKValues" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vessellist" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
          function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvSPIlist.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <style>
.tooltip2 {
  position: relative;
  display: inline-block;
  
}

.tooltip2 .tooltiptext {
  visibility: hidden;
  
  background-color: #f0f8ff;
  color: #555;
  text-align: left;
  border-radius: 6px;
  padding: 5px 2px 5px 10px;
  position: absolute;
  z-index: 10000;
  top: 125%;
  left: 100%;
  margin-left: -60px;
  opacity: 0;
  transition: opacity 0.3s;
}

.tooltip2 .tooltiptext::after {
  
  position: absolute;
  top: 100%;
  left: 50%;
  margin-left: -5px;
  border-width: 5px;
  border-style: solid;
  border-color: #555 transparent transparent transparent;
}

.tooltip2:hover .tooltiptext {
  visibility: visible;
  opacity: 1;
}
.RadGrid .rgClipCells .rgHeader, .RadGrid .rgClipCells .rgFilterRow > td, .RadGrid .rgClipCells .rgRow > td, .RadGrid .rgClipCells .rgAltRow > td, .RadGrid .rgClipCells .rgEditRow > td, .RadGrid .rgClipCells .rgFooter > td {
    overflow: visible !important;
}
</style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvPIlist" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"  >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="Tabkpi" runat="server" OnTabStripCommand="KPI_TabStripMenuCommand" TabStrip="true" ></eluc:TabStrip>

            <table>
                <tr>
                    <td>
                        &nbsp &nbsp
                    </td>
                    <th>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Indicator " />
                    </th>
                    <td>&nbsp &nbsp
                    </td>
                    <td>
                        <telerik:RadComboBox ID="radcbindicator" runat="server"  AllowCustomText="true"  EmptyMessage="Type to Select Indicator" Width="175px" OnSelectedIndexChanged="radcbindicator_SelectedIndexChanged" AutoPostBack="true" >
                            <Items>
                                <%--<telerik:RadComboBoxItem runat="server" Text="SPI" Value="SPI"/>--%>
                                <telerik:RadComboBoxItem runat="server" Text="KPI" Value="KPI" />
                                <telerik:RadComboBoxItem runat="server" Text="PI" Value="PI" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>&nbsp &nbsp     
                    </td>
                    <th>
                        <telerik:RadLabel ID="Radlblscope" runat="server" Text="Scope " />
                    </th>
                    <th>&nbsp
                    </th>
                    <td>
                        <telerik:RadComboBox ID="Radcbscope" runat="server" AllowCustomText="true" EmptyMessage="Type to Select Scope" Width="150px"  AutoPostBack="true" OnTextChanged="Radcbscope_TextChanged"/>
                    </td>
                     <td>&nbsp &nbsp 
                    </td>
                    <th>
                         <telerik:RadLabel ID="radlblscopeselect" runat="server" />    
                    </th>
                     <th>&nbsp
                    </th>
                    <td>
                        <telerik:RadComboBox id="radcbscopeselect" runat="server" AllowCustomText="true" EmptyMessage="Type to Select " Width="150px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Visible="false"  AutoPostBack="true" OnSelectedIndexChanged="radcbscopeselect_SelectedIndexChanged"/>
                    </td>

                    <td>&nbsp &nbsp 
                    </td>
                    <th>
                        <telerik:RadLabel ID="radlblyear" runat="server" Text="Year " />
                    </th>
                    <th>&nbsp
                    </th>
                    <td>
                        <eluc:Year ID="radcbyear" runat="server" YearStartFrom="2018" NoofYearFromCurrent="0" OrderByAsc="True" Width="100px" AutoPostBack="true" OnTextChangedEvent="radcbyear_TextChangedEvent"/>
                    </td>


                </tr>

            </table>

            <eluc:TabStrip ID="Tabstripspivalues" runat="server" OnTabStripCommand="Tabstripspivalues_TabStripCommand" Visible="false"
                TabStrip="true"></eluc:TabStrip>

            <telerik:RadGrid runat="server" ID="gvSPIlist" AutoGenerateColumns="false" 
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvSPIlist_NeedDataSource" EnableViewState="true"
                OnItemDataBound="gvSPIlist_ItemDataBound" OnItemCommand="gvSPIlist_ItemCommand" ShowFooter="false" >
                <MasterTableView EditMode="InPlace" AutoGenerateColumns="false"
                    TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns></Columns>


                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

           
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
