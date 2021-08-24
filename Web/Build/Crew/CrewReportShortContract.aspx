<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportShortContract.aspx.cs"
    Inherits="CrewReportShortContract" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Short Contract</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
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
        <table border="1" style="border-collapse: collapse;" >
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblShortContractBetween" runat="server" Text="Short Contract Between"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                                <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBatch" runat="server" Text="Batch"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" Width="100%" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Address ID="ucPrincipal" AddressType="128" runat="server" AppendDataBoundItems="true"
                                    Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                                    Width="240px" Entitytype="VSL" ActiveVesselsOnly="true" AssignedVessels="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                                <eluc:Pool ID="lstPool" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                                <eluc:Zone runat="server" ID="ucZone" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                                <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </td>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="62%"
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
            EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound" OnItemCommand="gvCrew_ItemCommand"
            ShowFooter="False" OnSortCommand="gvCrew_SortCommand" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDEMPLOYEEID"
                CommandItemDisplay="Top">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false"
                    ShowRefreshButton="false" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="50px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSrNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="240px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                            <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Zone" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblZone" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBatch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Joined" AllowSorting="false" HeaderStyle-Width="175px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselJoined" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="SignOn Date" AllowSorting="false" HeaderStyle-Width="95px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignOnDate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Source" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSourcedFrom" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCEDFROM") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Engaged/<br />Re-engaged" AllowSorting="false"
                        HeaderStyle-Width="120px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEngaged" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGAGED") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="On Promotion" AllowSorting="false" HeaderStyle-Width="105px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOnPromo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROMO") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Convertee" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblConvertee" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERTEE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Full Contract" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFullContract" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULLCONTRACT") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" HeaderStyle-Width="240px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDSIGNONREMARKS") %>
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
