<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkingGearStockPerMonth.aspx.cs"
    Inherits="CrewWorkingGearStockPerMonth" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Working Gear Items Stock</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkingGearItemsStock" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuCrewWorkingGear" runat="server" OnTabStripCommand="CrewWorkingGear_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory">
                            <Items>
                                <telerik:RadComboBoxItem Value="01" Text="Jan"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="02" Text="Feb"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="03" Text="Mar"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="04" Text="Apr"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="05" Text="May"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="06" Text="Jun"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="07" Text="Jul"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="08" Text="Aug"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="09" Text="Sep"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtYear" runat="server"  Width="50px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br style="clear: both" />

            <eluc:TabStrip ID="MenuWorkingGearItemStock" runat="server" OnTabStripCommand="WorkingGearItemStock_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkingGearItemStockPerMonth" runat="server"
                Height="80%" EnableViewState="false" AllowCustomPaging="true" AllowSorting="true"
                AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvWorkingGearItemStockPerMonth_NeedDataSource"
                EnableHeaderContextMenu="true" OnItemDataBound="gvWorkingGearItemStockPerMonth_ItemDataBound"
                OnItemCommand="gvWorkingGearItemStockPerMonth_ItemCommand" ShowFooter="False" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Item Name" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkingGearitemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkWorkingGearItemName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item Size" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkingGearitemSize" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkWorkingGearSizeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkGearType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUnitId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Opening Stock" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOpening" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceived" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRECEIVED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssued" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDISSUED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Closing" AllowSorting="false" HeaderStyle-Width="30px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosing" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCLOSING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
