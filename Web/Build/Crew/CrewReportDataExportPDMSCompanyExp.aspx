<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportDataExportPDMSCompanyExp.aspx.cs" Inherits="Crew_CrewReportDataExportPDMSCompanyExp" %>

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
                                <asp:Literal ID="lblPool" Text="Pool" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Pool ID="ucPool" runat="server" CssClass="input" AppendDataBoundItems="true" Enabled="false" />
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
                                    <asp:Label ID="lblEmployeeCode" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                            <telerik:gridtemplatecolumn headertext="From Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPEXPSTARTDATE") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                           <telerik:gridtemplatecolumn headertext="To Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPEXPTODATE") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                           <telerik:gridtemplatecolumn headertext="Rank">
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPEXPRANK") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                            <telerik:gridtemplatecolumn headertext="Vessel">
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPVESSEL") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                              <telerik:gridtemplatecolumn headertext="Sign Off Reason">
                                <ItemTemplate>
                                    <asp:Label ID="lblSignOffReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPSIGNOFFREASON") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                                                      <telerik:gridtemplatecolumn headertext="Eng type">

                                <ItemTemplate>
                                    <asp:Label ID="lblEngineType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPENGTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                                                    <telerik:gridtemplatecolumn headertext="Bhp">

                                <ItemTemplate>
                                    <asp:Label ID="lblBhp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPVESSELBHP") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                                                    <telerik:gridtemplatecolumn headertext="Vessel Type">

                                <ItemTemplate>
                                    <asp:Label ID="lblVesselType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPVESSELTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                                                     <telerik:gridtemplatecolumn headertext="Vessel Category">

                                <ItemTemplate>
                                    <asp:Label ID="lblVesselCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPVESELCATEGORY") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn >
                            
                                                     <telerik:gridtemplatecolumn headertext="Source">

                                <ItemTemplate>
                                    <asp:Label ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCE") %>'></asp:Label>
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
