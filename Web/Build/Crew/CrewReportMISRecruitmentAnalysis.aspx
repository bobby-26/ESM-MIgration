<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMISRecruitmentAnalysis.aspx.cs"
    Inherits="Crew_CrewReportsMISReports_Recruitment_Analysis_" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EmpFleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MIS Reports[Recruitment Analysis]</title>
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
        <style type="text/css">
            .mlabel {
                color: blue !important;                
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
      <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           
                        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
               
             <b ><telerik:radlabel ID="ltrDesc" runat="server" CssClass="mlabel" Text="This report provides analysis on seafarers newly recruited from the market during the said period and the average number  of companies done by the recruited seafarers in last 3 years"></telerik:radlabel>
               <eluc:Status runat="server" ID="ucStatus" />
              <telerik:radlabel ID="lblGuidance" runat="server" CssClass="mlabel" Text="Note: Average=No.of Staffs/No.of Companies"></telerik:radlabel></b>
                <table width="70%">
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlDate" runat="server" GroupingText="Enter Period" >
                                        <table>
                                            <tr>
                                                <td>
                                                     <telerik:radlabel ID="lblFromDate" runat="server" Text="From Date"></telerik:radlabel>
                                                </td>
                                                <td>
                                                    <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory"  Width="150px" />
                                                </td>
                                                <td>
                                                     <telerik:radlabel ID="lblToDate" runat="server" Text="To Date"></telerik:radlabel>
                                                </td>
                                                <td>
                                                    <eluc:Date ID="ucDate1" runat="server" CssClass="input_mandatory"  Width="150px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                    
                                <td>
                                    <telerik:radlabel ID="lblZone" runat="server" Text="Zone"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Zone ID="ucZone" runat="server"  AppendDataBoundItems="true" Width="210px"  />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:radlabel ID="lblPool" runat="server" Text="Pool"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="210px" />
                                </td>
                                <td>
                                    <telerik:radlabel ID="lblEmpFleet" runat="server" Text="Emp Fleet"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:EmpFleet ID="ucFleet" AppendDataBoundItems="true" runat="server"  Width="210px"  />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:radlabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128"  Width="210px"  AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:radlabel ID="lblManager" runat="server" Text="Manager"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Manager ID="ucManager" AddressType="126" runat="server" AppendDataBoundItems="true" Width="210px" />
                                </td>                         
                            </tr>
                        </table>
                   
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                                       
            <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False"  OnSortCommand="gvCrew_Sorting"
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
                                    <telerik:RadLabel ForeColor="Black"   ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>

                            <telerik:gridtemplatecolumn HeaderText="Rank"   >
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblRankName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="No. of Staff"   >
                                                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblRank" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                                   
                                        <asp:LinkButton ID="lnkDetails" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFSTAFF") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="Total Companies Served for Last 3 Yrs"   >
                                   
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblComRank" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                                   
                                        <asp:LinkButton ID="lnkComDeails" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOMPANIES") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="Avg. Companies Served for Last 3Yrs"   >
                                    
                                <ItemTemplate>
                                 
                                        <telerik:radlabel ID="lblTimeonTanker" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
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