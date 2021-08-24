<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportDepartmentInformation.aspx.cs"
    Inherits="Crew_CrewReportDepartmentInformation" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselList" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CountryList" Src="~/UserControls/UserControlCountryList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RankList" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Department Information</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
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

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <table width="100%" border="1" style="border-collapse: collapse;">
                <tr valign="top">
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="Crew Change Between"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" />
                                    <eluc:Date ID="ucDate2" runat="server" CssClass="input_mandatory" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr valign="top">
                                <td>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                    <br />
                                    <eluc:VesselList ID="ucVesselList" runat="server" AppendDataBoundItems="true" Width="250px" Entitytype="VSL" AssignedVessel="true" VesselsOnly="true" ActiveVesselsOnly="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                                    <br />
                                    <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                                    <br />
                                    <eluc:CountryList ID="ucCountryList" runat="server" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                                    <br />
                                    <eluc:RankList ID="ucRankList" runat="server" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server"
                Height="70%" EnableViewState="false" AllowCustomPaging="true" AllowSorting="true"
                AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
                EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound"
                OnItemCommand="gvCrew_ItemCommand" ShowFooter="False" AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="100px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>

                                <telerik:RadLabel ID="lblVesselName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number of Crew Changes" AllowSorting="false" HeaderStyle-Width="100px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>

                                <telerik:RadLabel ID="lblCount" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWCHANGECOUNT") %>' />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="On\Off" AllowSorting="false" HeaderStyle-Width="100px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>

                                <asp:LinkButton ID="lnkOnOff" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONOFF") %>'></asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port" AllowSorting="false" HeaderStyle-Width="100px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>

                                <telerik:RadLabel ID="lblDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORT") %>' />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" HeaderStyle-Width="100px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>

                                <telerik:RadLabel ID="lblPort" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>' />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason" AllowSorting="false" HeaderStyle-Width="100px" Visible="false"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>

                                <telerik:RadLabel ID="lblVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
