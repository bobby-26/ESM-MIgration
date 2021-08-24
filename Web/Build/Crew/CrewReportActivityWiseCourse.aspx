<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportActivityWiseCourse.aspx.cs" Inherits="Crew_CrewReportActivityWiseCourse" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PoolList" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvBatch.ClientID %>"));
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
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <div id="divFind" style="position: relative; z-index: 2">
                <table width="100%">
                    <tr style="height:70px">
                        <td>
                            <telerik:RadLabel ID="lblZone" ForeColor="Black" runat="server" Text="Zone"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Zone runat="server" ID="ucZone" AppendDataBoundItems="true" Height="80px" Width="200px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVessel" ForeColor="Black"  runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td >
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true"  Height="80px"
                                Width="200px" Entitytype="VSL" AssignedVessels="true" VesselsOnly="true" />
                        </td>

                        <td>
                            <telerik:RadLabel ID="lblPool"  ForeColor="Black" runat="server" Text="Pool"></telerik:RadLabel>
                        </td>
                        <td >
                            <eluc:PoolList ID="ucPool" runat="server" AppendDataBoundItems="true" Width="200px"   Height="75px"/>
                        </td>

                    </tr>
                    <tr style="height:70px">
                        <td>
                            <telerik:RadLabel runat="server"  ForeColor="Black" ID="lblCourse" Text="Course"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadListBox ID="lstCourse" runat="server" AppendDataBoundItems="true" Width="200px"
                                SelectionMode="Multiple" Height="80px">
                            </telerik:RadListBox>
                        </td>
                        <td>
                            <telerik:RadLabel ForeColor="Black"  ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadListBox ID="lstStatus" runat="server" SelectionMode="Multiple"
                                DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" Width="200px" Height="80px">
                            </telerik:RadListBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCourseType"  ForeColor="Black" runat="server" Text="Course Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard runat="server" ID="ucDocumentType"  AppendDataBoundItems="true"
                                HardTypeCode="103" AutoPostBack="true" OnTextChangedEvent="DocumentTypeSelection"
                                ShortNameFilter="1,2,3,4,5,6" Width="200px" />
                        </td>
                    </tr>
                    <tr style="height:70px">

                        <td>
                            <telerik:RadLabel ForeColor="Black"  ID="lblFileNo" runat="server" Text="File Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFileNo" runat="server" Width="200px" ></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPeriod"  ForeColor="Black" runat="server" Text="Period"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                            <telerik:RadLabel  ForeColor="Black" ID="lblFromDate" runat="server" Text="-"></telerik:RadLabel>
                            <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                        </td>
                    </tr>
                </table>
            </div>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvBatch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvBatch_ItemCommand"
                OnItemDataBound="gvBatch_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvBatch_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black"  ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="S.No.">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblCourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel  ForeColor="Black" ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <ItemStyle Width="" Wrap="true" />
                            <HeaderStyle Width="" Wrap="true" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course Venue">
                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblVenue" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEVENUE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No.">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Date">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblFromDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDFROMDATE")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Date">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblToDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTODATE")) %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">

                            <ItemTemplate>
                                <telerik:RadLabel ForeColor="Black"  ID="lblActivityVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Vessel">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblLastVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Travel Days">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblTravelDays" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDAYS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">

                            <ItemTemplate>
                                <telerik:RadLabel  ForeColor="Black" ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
