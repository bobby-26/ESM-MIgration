<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportDataExportPDMSOtherExp.aspx.cs" Inherits="Crew_CrewReportDataExportPDMSOtherExp" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Export to PDMS</title>
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
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
            </eluc:Error>
           
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
              
                    <table>
                        <tr>
                            <td>
                                <telerik:radlabel ID="lblPool" Text="Pool" runat="server"></telerik:radlabel>
                            </td>
                            <td>
                                <eluc:Pool ID="ucPool" runat="server"  AppendDataBoundItems="true" Enabled="false" />
                            </td>
                           
                        </tr>
                    </table>
                
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                
                      <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False"  onsortcommand="gvCrew_Sorting"
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
                            <telerik:gridtemplatecolumn headertext="Global EmpCode">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmployeeCode" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                         <telerik:gridtemplatecolumn headertext="From Date">
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPSTARTDATE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                           
                           <telerik:gridtemplatecolumn headertext="To Date">
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPTODATE") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                         
                           <telerik:gridtemplatecolumn headertext="Rank">
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPRANK") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                         
                            <telerik:gridtemplatecolumn headertext="Vessel">
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPVESSEL") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                           
                              <telerik:gridtemplatecolumn headertext="Sign Off Reason">
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSignOffReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPSIGNOFFREASON") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                             <telerik:gridtemplatecolumn headertext="Eng type">

                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEngineType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPENGINETYPE") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                              <telerik:gridtemplatecolumn headertext="Bhp">

                                <ItemTemplate>
                                    <telerik:radlabel ID="lblBhp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPOTHERVESSELBHP") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                              <telerik:gridtemplatecolumn headertext="Vessel type">

                                <ItemTemplate>
                                    <telerik:radlabel ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPVESSELTYPE") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                              <telerik:gridtemplatecolumn headertext="Vessel Category">

                                <ItemTemplate>
                                    <telerik:radlabel ID="lblVesselCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPVESSELCATEGORY") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                            <telerik:gridtemplatecolumn headertext="Company">

                                <ItemTemplate>
                                    <telerik:radlabel ID="lblCompany" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPMANNINGCOMPANY") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                               <telerik:gridtemplatecolumn headertext="Source">

                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCE") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                           
                         </Columns>
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