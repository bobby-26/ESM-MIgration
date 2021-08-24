<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportReasonwiseIllnessAndInjury.aspx.cs"
    Inherits="Crew_CrewReportReasonwiseIllnessAndInjury" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="../UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlPortList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EmpFleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RankList" Src="../UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Illness And Injury Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvReasonwiseRecord.ClientID %>"));
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
    <form id="frmReasonWiseReport" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReasonwiseReport" runat="server" OnTabStripCommand="MenuReasonwiseReport_TabStripCommand"></eluc:TabStrip>
            
            <table cellpadding="1" cellspacing="1" width="100%">

                <tr>
                    <td >
                        <telerik:RadLabel ID="lblZone" Text="Zone" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<font color="blue">(Press "ctrl" for Multiple Selection)</font>--%>
                        <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true"
                            Width="210px"  />
                    </td>
                    <td >
                        <telerik:RadLabel ID="lblPort" Text="Port" runat="server"></telerik:RadLabel>
                    </td>
                    <td >
                        <eluc:Port ID="ucPort" runat="server" AppendDataBoundItems="true"
                            Width="210px" />
                    </td>
                  <td>
                        <telerik:RadLabel ID="lblRank" Text="Rank" runat="server"></telerik:RadLabel>
                    </td>
                  <td>
                        <eluc:RankList ID="ucRank" runat="server" AppendDataBoundItems="true" Width="270px" />
                    </td>
                </tr>
                <tr>
                   <td>
                        <telerik:RadLabel ID="lblPool" Text="Pool" runat="server"></telerik:RadLabel>
                    </td>
                   <td>
                        <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                   <td>
                        <telerik:RadLabel ID="lblVesselType" Text="Vessel Type" runat="server"></telerik:RadLabel>
                    </td>
                   <td>
                        <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                  <td>
                        <telerik:RadLabel ID="lblPrincipal" Text="Principal" runat="server"></telerik:RadLabel>
                    </td>
                  <td>
                        <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true" Width="270px" />
                    </td>
                </tr>
                <tr>
                   <td>
                        <telerik:RadLabel ID="lblTypeofCase" runat="server" Text="Type of Case"></telerik:RadLabel>
                    </td>
                 <td>
                        <eluc:Hard ID="ucTypeofcase" runat="server" AppendDataBoundItems="true" HardTypeCode="174" AutoPostBack="true" OnTextChangedEvent="ucTypeofcase_Changed" Width="210px" />
                    </td>
                  <td>
                        <telerik:RadLabel ID="lblTypeofInjury" runat="server" Text="Injury Reason"></telerik:RadLabel>
                    </td>
                 <td>
                        <eluc:Quick runat="server" ID="ucTypeofInjury" AppendDataBoundItems="true" Width="210px" Enabled="false" QuickTypeCode="69" />
                    </td>
                    <td colspan="2" >
                        <asp:Panel ID="pnlperiod" runat="server" GroupingText="During the period" >
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblFromDate" Text="From Date" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" Width="120px" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblToDate" Text="To Date" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuReasonwiseRecord" runat="server" OnTabStripCommand="MenuReasonwiseRecord_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvReasonwiseRecord" runat="server" AutoGenerateColumns="False" CellPadding="3" RowStyle-Wrap="false" ShowHeader="true"
                EnableViewState="false" OnItemCommand="gvReasonwiseRecord_ItemCommand"
                OnItemDataBound="gvReasonwiseRecord_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvReasonwiseRecord_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                        <telerik:GridTemplateColumn HeaderText="Reason">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblinjurytypecode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPESOFINJURYCODE	") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPESOFINJURY	") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No Of Cases">

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkNoofcase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINJURYCOUNT") %>'></asp:LinkButton>
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
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
