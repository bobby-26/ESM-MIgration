<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportReliefPlanningByVessel.aspx.cs"
    Inherits="Crew_CrewReportReliefPlanningByVessel" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vesseltype" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Relief Planning Format 1 Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewReliefPlanning.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <style type="text/css">
            .mlabel {
                color: blue !important;                
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmReliefPlanningReport" runat="server">
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
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="ShowReports" runat="server" OnTabStripCommand="MenuShowReport_TabStripCommand">
        </eluc:TabStrip>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNote" CssClass="mlabel" Text="Note : Please select the principal to see the experience with principal."
                        runat="server">
                    </telerik:RadLabel>
                </td>
            </tr>
        </table>
        <table border="1" style="border-collapse: collapse;">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblReliefPlanBetween" runat="server" Text="Relief Plan Between">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                                <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Principal ID="ucPrincipal" runat="server" AppendDataBoundItems="true" AddressType="128"
                                     />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td colspan="2">
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type">
                                </telerik:RadLabel>
                                <eluc:Vesseltype ID="ucVesselType" runat="server" AppendDataBoundItems="true"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                                </telerik:RadLabel>
                                <eluc:Vessel ID="ucVessel" AppendDataBoundItems="true" runat="server" VesselsOnly="true"
                                     Width="400px" Entitytype="VSL" ActiveVesselsOnly="true" AssignedVessels="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool">
                                </telerik:RadLabel>
                                <eluc:Pool ID="ucPool" AppendDataBoundItems="true" runat="server"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone">
                                </telerik:RadLabel>
                                <eluc:Zone ID="ucZone" AppendDataBoundItems="true" runat="server"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                                </telerik:RadLabel>
                                <eluc:Rank ID="ucRank" AppendDataBoundItems="true" runat="server"  />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
            </eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewReliefPlanning" runat="server"
                Height="82%" EnableViewState="false" AllowCustomPaging="true" AllowSorting="true"
                AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrewReliefPlanning_NeedDataSource"
                EnableHeaderContextMenu="true" OnItemDataBound="gvCrewReliefPlanning_ItemDataBound"
                OnItemCommand="gvCrewReliefPlanning_ItemCommand" ShowFooter="False" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" >
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
                        
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="55px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' />
                                <telerik:RadLabel ID="lblVesselName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERRANK") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Onboard" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="95px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOffsignerID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>' />
                                <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SignOn Date" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="65px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSignOnDateHeader" runat="server">
                                    SignOn Date</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnDate" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Relief Date" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="65px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblReliefDateHeader" runat="server">
                                    Relief Date</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRelifDue" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reliever" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="95px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOnsignerID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>' />
                                <telerik:RadLabel ID="lblRelieverName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' />
                                <asp:LinkButton ID="lnkRelieverName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DOA" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="65px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDOA" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDOA","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EOL(Including activities)" AllowSorting="false"
                            ShowSortIcon="true">
                            <HeaderStyle Width="110px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEOL" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLEAVECOMPLETIONDATE","{0:dd/MMM/yyyy}")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
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
