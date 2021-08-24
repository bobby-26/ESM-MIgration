<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMISRankExperienceDetails.aspx.cs"
    Inherits="Crew_CrewReportMISRankExperienceDetails" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MIS Rank Experience Details</title>
   <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPB.ClientID %>"));
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
              
              <div style="color: Blue">
                    &nbsp<b>Note:</b> Select Manager and Rank To View Particular Details
                </div>
            
                            <table width="70%" style="margin:3px">
                            <tr>
                                <td>
                                    <telerik:radlabel ID="lblManager" runat="server" Text="Manager"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Manager ID="ucManager" AddressType="126" runat="server" AppendDataBoundItems="true" Width="150px"
                                        CssClass="dropdown_mandatory" />
                                </td>
                                <td>
                                    <telerik:radlabel ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Rank ID="ucRank" AppendDataBoundItems="true" runat="server" Width="150px" CssClass="dropdown_mandatory" />
                                </td>
                            </tr>
                        </table>
                     
                                <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                            </eluc:TabStrip>
                                            
                                 <telerik:RadGrid ID="gvPB" runat="server" AutoGenerateColumns="False" 
                  ShowHeader="true" EnableViewState="false" OnItemCommand="gvPB_ItemCommand"
                OnItemDataBound="gvPB_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="true" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvPB_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Auto">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black"   ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                  <ColumnGroups>
                      <telerik:GridColumnGroup  HeaderText="" Name="group1" HeaderStyle-HorizontalAlign="Center" ></telerik:GridColumnGroup>
                      <telerik:GridColumnGroup  HeaderText="Experience (in Months)" Name="group2" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>

                  </ColumnGroups>
                    
                    <HeaderStyle Width="70px" Wrap="true"  />
                      <Columns>

                                <telerik:gridtemplatecolumn HeaderText="Sl No"   ColumnGroupName="group1" >
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Height="15px"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDROWNO")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                               <telerik:gridtemplatecolumn HeaderText="File No"   ColumnGroupName="group1"   >
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Height="15px"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Batch"   ColumnGroupName="group1"   >
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDBATCH"].ToString()%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Employee Name"   ColumnGroupName="group1"   >
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"].ToString() %>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Rank"        ColumnGroupName="group1"   >
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDPOSTEDRANK"].ToString()%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                            </Columns>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="false" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
