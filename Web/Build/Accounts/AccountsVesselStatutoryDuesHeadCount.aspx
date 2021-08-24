<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselStatutoryDuesHeadCount.aspx.cs"
    Inherits="AccountsVesselStatutoryDuesHeadCount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="Status" TagPrefix="eluc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Statutory Dues For Head Count</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvStock.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>



            <eluc:TabStrip ID="MenuStatoryDuesMain" runat="server" OnTabStripCommand="MenuStatoryDuesMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <eluc:TabStrip ID="MenuStatoryDues" runat="server" OnTabStripCommand="MenuStatoryDues_TabStripCommand"></eluc:TabStrip>

            <table width="75%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                            CssClass="dropdown_mandatory" Width="250px" AutoPostBack="true" OnTextChangedEvent="ddlVessel_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" Width="250px"
                            AutoPostBack="true" OnTextChanged="ddlVessel_Changed" Filter="Contains" EmptyMessage="Type to select">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Jan" />
                                <telerik:RadComboBoxItem Value="2" Text="Feb" />
                                <telerik:RadComboBoxItem Value="3" Text="Mar" />
                                <telerik:RadComboBoxItem Value="4" Text="Apr" />
                                <telerik:RadComboBoxItem Value="5" Text="May" />
                                <telerik:RadComboBoxItem Value="6" Text="Jun" />
                                <telerik:RadComboBoxItem Value="7" Text="Jul" />
                                <telerik:RadComboBoxItem Value="8" Text="Aug" />
                                <telerik:RadComboBoxItem Value="9" Text="Sep" />
                                <telerik:RadComboBoxItem Value="10" Text="Oct" />
                                <telerik:RadComboBoxItem Value="11" Text="Nov" />
                                <telerik:RadComboBoxItem Value="12" Text="Dec" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input_mandatory" Width="250px"
                            AutoPostBack="true" OnTextChanged="ddlVessel_Changed"
                            Filter="Contains" EmptyMessage="Type to select">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlComponent" runat="server" CssClass="dropdown_mandatory" Width="250px" AutoPostBack="true"
                            OnTextChanged="ddlComponent_Changed" Filter="Contains" EmptyMessage="Type to select">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbloffrating" runat="server" Text="Officer/Rating"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlHard" runat="server" AppendDataBoundItems="true"
                            HardTypeCode="50" Width="250px" />
                        <%--ShortNameFilter="OFF,RAT" --%>
                    </td>
                </tr>
            </table>
            <br />

            <eluc:TabStrip ID="MenuStock" runat="server" OnTabStripCommand="MenuStock_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%--   <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" ShowFooter="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvStock" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvStock_NeedDataSource"
                    OnItemDataBound="gvStock_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Sr No." ItemStyle-Width="10%">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# Container.DataSetIndex + 1 %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" ItemStyle-Width="20%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lnkEployeeName" runat="server"
                                        Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%>' CommandName="EDIT">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank" ItemStyle-Width="10%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDRANKNAME"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Basic Wages(USD)" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDBASIC"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="From" ItemStyle-Width="10%">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Total" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Head Count" ItemStyle-Width="10%">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDHEADCOUNT"] %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rate per month" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDRATE"] %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Amount" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDAMOUNT"] %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </div>

        <script type="text/javascript">
            Sys.Application.add_load(function () {
                var groups = {};
                $("select[id='ddlComponent'] option[OptionGroup]").each(function () {
                    groups[$.trim($(this).attr("OptionGroup"))] = true;
                });
                $.each(groups, function (c) {
                    $("select[id='ddlComponent'] option[OptionGroup='" + c + "']").wrapAll('<optgroup label="' + c + '">');
                });
            });
        </script>
    </form>
</body>
</html>
