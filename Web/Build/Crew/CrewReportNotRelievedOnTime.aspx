<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportNotRelievedOnTime.aspx.cs"
    Inherits="Crew_CrewReportNotRelievedOnTime" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vesseltype" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Relief Delayed</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrew.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
</script>
          <style type="text/css">
            .mlabel {
                color: blue !important;                
            }
        </style>
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
        <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="MenuReport_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <table id="tblReportNotRelievedOnTime" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblnote" runat="server" Text="Note: Period filter checks the seafarers actual relief date or if
                                        onboard." CssClass="mlabel">
                    </telerik:RadLabel>
                </td>
            </tr>
        </table>
        <table border="1" width="100%" style="border-collapse: collapse;">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text="Relief Delayed From">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="Literal1" runat="server" Text="Relief Delayed To">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                
                                <telerik:RadLabel ID="lblReliefDays" runat="server" Text="Relieved Date +/- <br/> Days From Relief Due">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSkipDays" runat="server" ToolTip="Enter the No.of Days"
                                    Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr valign="top">
                            <td colspan="2">
                                <telerik:RadLabel ID="lblVesselType" runat="Server" Text="Vessel Type">
                                </telerik:RadLabel>
                                <eluc:Vesseltype ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="200px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool">
                                </telerik:RadLabel>
                                <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="120px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone">
                                </telerik:RadLabel>
                                <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="110px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank1" runat="server" Text="Rank">
                                </telerik:RadLabel>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="200px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal">
                                </telerik:RadLabel>
                                <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" Width="220px" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvCrew" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" 
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
            GridLines="None" OnSortCommand="gvCrew_SortCommand" OnNeedDataSource="gvCrew_NeedDataSource"
            OnItemDataBound="gvCrew_ItemDataBound" ShowFooter="false" EnableViewState="false"
            OnItemCommand="gvCrew_ItemCommand" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEID" TableLayout="Fixed"
                CommandItemDisplay="Top">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false"
                    ShowRefreshButton="false" />
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
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="65px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="200px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                            <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="70px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBatch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="160px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign On Date" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignOnDate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Relief Due Date" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReliefDue" Visible="true" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATETOBERELIEVED","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Relieved Date" AllowSorting="false" HeaderStyle-Width="100px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReliever" Visible="true" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDFINALLYRELIEVED","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Duration" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDuration" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign Off Remarks" AllowSorting="false" HeaderStyle-Width="135px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignOffRemarks" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSIGNOFFREMARKS").ToString().Length > 15 ? DataBinder.Eval(Container, "DataItem.FLDSIGNOFFREMARKS").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDSIGNOFFREMARKS").ToString()%>'>
                            </telerik:RadLabel>
                            <eluc:ToolTip ID="ucSignOffRemarks" runat="server" Text='<%# String.Concat( String.Concat(Eval("FLDSIGNOFFREMARKS")," - "),Eval("FLDSIGNOFFREMARKS")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Relieved by/Planned relief" AllowSorting="false"
                        HeaderStyle-Width="200px" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOnsignerID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>' />
                            <telerik:RadLabel ID="lblRelieverName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' />
                            <asp:LinkButton ID="lnkRelieverName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true"
                        HeaderStyle-Width="115px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblRemarksHeader" runat="server">
                                Remarks</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemarks" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRCREMARKS") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Reason(Delayed/Early Relief)" AllowSorting="false"
                        HeaderStyle-Width="180px" ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDelayReason" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELAYEARLYREASON") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Zone(Reliever)" AllowSorting="false" HeaderStyle-Width="115px"
                        ShowSortIcon="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblZone" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>' />
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
