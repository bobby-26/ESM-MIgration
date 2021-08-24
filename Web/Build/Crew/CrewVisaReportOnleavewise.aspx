<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewVisaReportOnleavewise.aspx.cs"
    Inherits="Crew_CrewVisaReportOnleavewise" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PoolList" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Visa Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewVisaReport.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
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
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportsFilterVisa" runat="server" OnTabStripCommand="ReportsVisaFilter_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <table border="1" style="border-collapse: collapse;">
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblVisaCountry" runat="server" Text="Visa Country"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Country ID="ucCountry" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblNoOfEntries" Text="No of Entries" runat="server"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlNoofentryAdd" runat="server" AppendDataBoundItems="true" EmptyMessage="Type to select Noof Entry" Filter="Contains" MarkFirstMatch="true"
                                        Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="1" Text="Single"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="2" Text="Multiple"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                           
                        </table>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                                    <br />
                                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                    <br />
                                    <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true"
                                        VesselsOnly="true" Width="250px" Entitytype="VSL" AssignedVessel="true" ActiveVesselsOnly="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                                    <br />
                                    <eluc:Zone runat="server" ID="ucZone" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblManager" Text="Manager" runat="server"></telerik:RadLabel>
                                    <br />
                                    <eluc:Manager ID="ucManager" AddressType="126" runat="server" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewVisaReport" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrewVisaReport_NeedDataSource"
                EnableHeaderContextMenu="true" OnItemDataBound="gvCrewVisaReport_ItemDataBound" OnItemCommand="gvCrewVisaReport_ItemCommand"
                ShowFooter="False" AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="25px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="35px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="70px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEmployeeName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="35px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Vessel" AllowSorting="false" HeaderStyle-Width="70px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-off Date" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignoffDate" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Visa" AllowSorting="false" HeaderStyle-Width="60px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVisa" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Issue" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofIssue" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFISSUE","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Expiry" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No. of Entries" AllowSorting="false" HeaderStyle-Width="55px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoofEntries" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFENTRYNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone" AllowSorting="false" HeaderStyle-Width="50px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblZone" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>'></telerik:RadLabel>
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
