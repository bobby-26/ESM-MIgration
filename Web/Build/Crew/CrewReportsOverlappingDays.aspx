<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsOverlappingDays.aspx.cs" Inherits="Crew_CrewReportsOverlappingDays" %>


<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCRank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCNTBRReason" Src="~/UserControls/UserControlNTBRReasonList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                            TabStrip="false"></eluc:TabStrip>
                    
                                <table cellpadding="1" cellspacing="1" width="75%">
                                <tr >
                                    <td style="padding-left:20px">
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Principal runat="server" ID="ucAddrOwner" AddressType="128" Width="240px"
                                            AppendDataBoundItems="true"  CssClass="input_mandatory" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel  ForeColor="Black"  ID="Literal1" runat="server" Text="Rank"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:UCRank ID="ddlrank" Width="200" runat="server" AppendDataBoundItems="true" />
                                    </td>
                                </tr>
                            </table>

                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
                        
                        <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew_ItemCommand"
                        OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                        OnNeedDataSource="gvCrew_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel  ForeColor="Black"  ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                           
                             <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="" Name="Owner" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                            
                                 </ColumnGroups>
                            
                             <Columns>

                                <%--   <telerik:GridTemplateColumn>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSlNoHeader" runat="server">S.No.</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSlNo" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                                  <telerik:GridTemplateColumn HeaderText="Vessel Name"      ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblvesselname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Type"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblvesseltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                               <telerik:GridTemplateColumn HeaderText="Name"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblempname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblRankname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                               <telerik:GridTemplateColumn HeaderText="DOB"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblDOB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOB") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Age"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblempage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGE") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Nationality"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblempnation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                               <telerik:GridTemplateColumn HeaderText="Sign on date"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblempsignon" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                               <telerik:GridTemplateColumn HeaderText="Sign off date"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblempsignoff" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                              <telerik:GridTemplateColumn HeaderText="No. of days"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblempodays" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERLAPDAYS") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Port"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblempport" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTNAME") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks"    ColumnGroupName="Owner">
                                    <ItemTemplate>
                                        <telerik:RadLabel  ForeColor="Black"  ID="lblempremark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
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
