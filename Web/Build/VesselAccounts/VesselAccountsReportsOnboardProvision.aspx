<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReportsOnboardProvision.aspx.cs"
    Inherits="VesselAccountsReportsOnboardProvision" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvProvision.ClientID %>"));
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <table border="1" style="border-collapse: collapse;">
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="ltrMonth" runat="server" Text="Month"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" Width="120px" Filter="Contains" EmptyMessage="Type to select month" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="January" Value="1"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="February" Value="2"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="March" Value="3"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="April" Value="4"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="May" Value="5"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="June" Value="6"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="July" Value="7"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="August" Value="8"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="September" Value="9"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="October" Value="10"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="November" Value="11"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="December" Value="12"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="ltrYear" runat="server" Text="Year"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input_mandatory">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="ltrTradingArea" runat="server" Text="Trading Area"></telerik:RadLabel>
                                </td>
                                <td colspan="3">
                                    <eluc:Quick ID="ucNavigationArea" runat="server" AppendDataBoundItems="true" Width="100%"
                                        QuickTypeCode="121" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="ltrPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                                </td>
                                <td colspan="3">
                                    <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" Width="100%"
                                        AppendDataBoundItems="true" AutoPostBack="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="ltrVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                    <br />
                                    <eluc:Vessel ID="ucVesselList" runat="server" AppendDataBoundItems="true" Width="200px"
                                        CssClass="input_mandatory" AssignedVessel="true" Entitytype="VSL"  ActiveVesselsOnly="true" VesselsOnly="true" />
                                   
                                </td>
                                <td>
                                    <telerik:RadLabel ID="ltrVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>

                                    <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblnote" runat="server" CssClass="guideline_text" >
                            <font color="blue" size="1px">NC - Provision not yet closed in vessel for the given month</font>
                        </telerik:RadLabel>
                    </td>
                </tr>
              
            </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="ShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvProvision" runat="server"
                EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" OnNeedDataSource="gvProvision_NeedDataSource" OnItemCommand="gvProvision_ItemCommand"
                GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSortCommand="gvProvision_SortCommand"
                OnItemDataBound="gvProvision_ItemDataBound" ShowFooter="False" AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="45px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="120px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Month/Year" AllowSorting="false" HeaderStyle-Width="110px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMonthYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHYEAR") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Brought Forward" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBROUGHTFORWARD") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purchase" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurchase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Consumption" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConsumption" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMPTION") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Carried Forward" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARRIEDFORWARD") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Man Days" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMandays" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANDAYS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rate" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVctRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVICTRATE") %>' />
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
