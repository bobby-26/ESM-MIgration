<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElecticLogLoadLineDraughtWater.aspx.cs" Inherits="Log_ElecticLogLoadLineDraughtWater" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Load Line and Draught Water</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .container {
            padding: 10px 50px;
        }

        .bold {
            font-weight: bold;
        }

        .filter, .filter tr, .filter td {
            border: 1px solid black;
            border-collapse: collapse;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="SERVER" ID="RADSCRIPTMANAGER1" />
        <telerik:RadSkinManager ID="RADSKINMANAGER1" runat="SERVER" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>

        <eluc:TabStrip ID="gvMainTabStrip" runat="server" OnTabStripCommand="gvMainTabStrip_TabStripCommand"></eluc:TabStrip>

        <div class="container">
            <h3>Load Line and Draught of Water</h3>
            <table style="width: 100%" class="filter">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCenterOfDisc" runat="server" Text="Center of Disc is Placed at"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCenterOfDiscMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>m</span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCenterOfDiscCentiMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>cm</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineFreshWater" runat="server" Text="Maximum load-line in fresh water"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineFreshWaterMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>m</span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineFreshWaterCentiMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>cm below the center of the disc</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineIndianSummer" runat="server" Text="Maximum load-line in Indian summer"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineIndianSummerMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>m</span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineIndianSummerCentiMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>cm below the center of the disc</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineSummerCenterDisc" runat="server" Text="Maximum load-line in summer on the center of the disc"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaxLoadLineSummerCenterDiscs" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>m</span>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLoadLineWinter" runat="server" Text="Maximum load-line in winter"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLoadLineWinterMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>m</span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLoadLineWinterCentiMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>cm below the center of the disc</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLoadLineWinterNorthAtlantic" runat="server" Text="Maximum load-line in winter North Atlantic"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLoadLineWinterNorthAtlanticMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>m</span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLoadLineWinterNorthAtlanticCentiMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>cm below the center of the disc</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDraughtWaterSummer" runat="server" Text="Maximum draught of water in summer"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDraughtWaterSummerMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>m</span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDraughtWaterSummerCentiMeter" CssClass="bold" runat="server"></telerik:RadLabel>
                        <span>cm</span>
                    </td>
                </tr>
            </table>
        </div>

        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvLoadLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None"
            OnNeedDataSource="gvLoadLine_NeedDataSource"
            OnItemDataBound="gvLoadLine_ItemDataBound"
            OnItemCommand="gvLoadLine_ItemCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" CommandItemDisplay="Top">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                <HeaderStyle Width="102px" />
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="Departures" Name="Departure" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                    <telerik:GridColumnGroup HeaderText="Signatures" Name="Signatures" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                    <telerik:GridColumnGroup HeaderText="Arrivals" Name="Arrivals" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>

                    <telerik:GridColumnGroup HeaderText="Actual draught of water" Name="ActualDraught" ParentGroupName="Departure" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                    <telerik:GridColumnGroup HeaderText="Actual freeboard amidships" Name="ActualFreeboard" ParentGroupName="Departure">
                    </telerik:GridColumnGroup>
                    <telerik:GridColumnGroup HeaderText="Allowance" Name="Allowance" ParentGroupName="Departure">
                    </telerik:GridColumnGroup>
                </ColumnGroups>
                
                <Columns>

                    <telerik:GridTemplateColumn HeaderText="1. Date and Hour of Departure" AllowSorting="true">
                        <HeaderStyle  Width="250px"/>
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="2. Dock, wharf, place or harbour" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="3. Forward" ColumnGroupName="ActualDraught" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="4. Aft" ColumnGroupName="ActualDraught" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="5. Port" ColumnGroupName="ActualFreeboard" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="6. Starboard" ColumnGroupName="ActualFreeboard" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="7. Mean" ColumnGroupName="ActualFreeboard" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="8. For density of water" ColumnGroupName="Allowance" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="9. For allowance on stretch of inland water (For eg FO/FW etc.)" ColumnGroupName="Allowance" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="10. Total allowances" ColumnGroupName="Allowance" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="11. Mean draught in water as calculated after the making the appropriate allowances" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="12. Mean freeboard amidships in salt water as calculated after making the appropriate allowances" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="13. Date and Time of Posting and Notice" AllowSorting="true">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="14. Mate" AllowSorting="true" ColumnGroupName="Signatures">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                     <telerik:GridTemplateColumn HeaderText="15. Master" AllowSorting="true" ColumnGroupName="Signatures">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="16. Date and hour of arrival" AllowSorting="true" ColumnGroupName="Arrivals">
                        <HeaderStyle />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="17. Dock, wharf, place or harbour" AllowSorting="true" ColumnGroupName="Arrivals">
                        <HeaderStyle />
                        <ItemTemplate>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>

            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="460px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
