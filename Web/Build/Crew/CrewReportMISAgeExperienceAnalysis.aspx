<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMISAgeExperienceAnalysis.aspx.cs"
    Inherits="Crew_CrewReportMISAgeExperienceAnalysis" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PoolList" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <%-- <title>MIS Age Experience Analysis</title>--%>
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
  
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                
                        <table width="100%">
                            <tr>
                                <td colspan="2" style="padding-left:10px">
                                    <asp:Panel ID="Panel1" runat="server" GroupingText="Period" Width="80%">
                                        <table>
                                            <tr>
                                                <td>
                                                    <telerik:radlabel ID="lblFromDate" runat="Server" Text="From Date"></telerik:radlabel>
                                                </td>
                                                <td>
                                                    <eluc:Date ID="ucDate" runat="server"  />
                                                </td>
                                                <td>
                                                    <telerik:radlabel ID="lblToDate" runat="Server" Text="To Date"></telerik:radlabel>
                                                </td>
                                                <td>
                                                    <eluc:Date ID="ucDate1" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td>
                                    <telerik:radlabel ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Rank ID="ucCRank" runat="server" AppendDataBoundItems="true"  Width="210px"/>
                                </td>
                                <td>
                                    <telerik:radlabel ID="lblPool" runat="server" Text="Pool"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:PoolList ID="ucPool" runat="server" AppendDataBoundItems="true"  Width="210px"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left:10px">
                                    <telerik:radlabel ID="lblBatch" runat="server" Text="Batch" ></telerik:radlabel>
                                </td>
                                <td >
                                    <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true"  Width="240px"/>
                                </td>
                                <td>
                                    <telerik:radlabel ID="lblZone" runat="server" Text="Zone"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Zone ID="ucZone" AppendDataBoundItems="true" runat="server" Width="210px"/>
                                </td>
                                <td>
                                    <telerik:radlabel ID="lblAge" runat="server" Text="Age greater than"></telerik:radlabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtAge" runat="server"   IsInteger="true" Width="210px"/>
                                </td>
                            </tr>
                            <tr>
                                 <td style="padding-left:10px">
                                    <telerik:radlabel ID="lblNationality" runat="server" Text="Nationality" ></telerik:radlabel>
                                </td>
                                <td >
                                    <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true"   Width="240px"/>
                                </td>
                                 <td>
                                    <telerik:radlabel ID="lblStatus" runat="server" Text="Status"></telerik:radlabel>
                                </td>
                                <td>
                                    <telerik:RadListBox ID="lstStatus" runat="server"   SelectionMode="Multiple" Height="70px"
                                        DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" Width="210px"></telerik:RadListBox>
                                </td>
                            </tr>
                        </table>
                    
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                
                        <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkDetails" runat="server" Text="Click Here To See Seafarers Details"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>

                      <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False"  AllowPaging="true" AllowCustomPaging="true"
                 CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew_ItemCommand" 
                OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew_NeedDataSource" RenderMode="Lightweight" AllowSorting="true" ShowFooter="true"  OnItemCreated="gvCrew_ItemCreated" >
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
                  <ColumnGroups>
                <telerik:GridColumnGroup HeaderText="Rank" Name="Rank" HeaderStyle-HorizontalAlign="Center">
                </telerik:GridColumnGroup>
                <telerik:GridColumnGroup HeaderText="Age Ranges" Name="Age"  HeaderStyle-HorizontalAlign="Center"   >
                </telerik:GridColumnGroup>
                <telerik:GridColumnGroup HeaderText="Total Seafarers" Name="Seafarers"  HeaderStyle-HorizontalAlign="Center"    >
                </telerik:GridColumnGroup>
                <telerik:GridColumnGroup HeaderText="Avg.Exp(Months)" Name="Experience"  HeaderStyle-HorizontalAlign="Center"    >
                </telerik:GridColumnGroup>
            </ColumnGroups>
                      <Columns>
                            <telerik:gridtemplatecolumn headertext="" ColumnGroupName="Rank"  HeaderStyle-Width="18%"  >
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>Total</b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="18-25"   ColumnGroupName="Age"   >
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lbl1825" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.18-25") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotal1825" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="26-30"  ColumnGroupName="Age"  >
                             
                                <ItemTemplate>
                                    <telerik:radlabel ID="lbl2630" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.26-30") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotal2630" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="31-35"  ColumnGroupName="Age"  >
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lbl3135" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.31-35") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotal3135" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="36-40"  ColumnGroupName="Age"  >
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lbl3640" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.36-40") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotal3640" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="41-45"  ColumnGroupName="Age"  >
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lbl4145" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.41-45") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotal4145" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="46-50"  ColumnGroupName="Age"  >
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lbl4650" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.46-50") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotal4650" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="51-55"  ColumnGroupName="Age"  >
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lbl5155" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.51-55") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotal5155" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="56-60"  ColumnGroupName="Age"  >
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lbl5660" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.56-60") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotal5660" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext=">60"  ColumnGroupName="Age"  >
                             
                                <ItemTemplate>
                                    <telerik:radlabel ID="lbl60" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.>60") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotal60" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext=""   ColumnGroupName="Seafarers"  >
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblTotalSeafarers" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEADCOUNT") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <b>
                                        <telerik:radlabel ID="lblTotalHeadCount" runat="server" /></b>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext=""     ColumnGroupName="Experience"  >
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblAvgExp" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGRANKEXP") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </telerik:gridtemplatecolumn>
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