<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportLongServiceRecord.aspx.cs" 
Inherits="Crew_CrewReportLongServiceRecord" %> 

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Vessel" Src="../UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EmpFleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RankList" Src="../UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Long Service Report</title>
  <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvLongServiceRecord.ClientID %>"));
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
    <form id="frmEmployeeGrowth" runat="server" submitdisabledcontrols="true">
   <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
                        <eluc:TabStrip ID="MenuLongServiceReport" runat="server" OnTabStripCommand="MenuLongServiceReport_TabStripCommand">
                    </eluc:TabStrip>
              
              <table cellpadding="1" cellspacing="1" width="100%">
                   
                    <tr>
                        <td>
                            Rank
                        </td>
                        <td>                              
                            <br />  
                            <eluc:RankList ID="ucRank" runat="server" AppendDataBoundItems="true"   Width="210px" />
                            
                        </td>
                        <td>
                            Zone
                        </td>
                        <td>
                            <eluc:Zone ID="ucZone" runat="server"  AppendDataBoundItems="true"   Width="210px" />
                        </td>
                        <td>
                            Batch
                        </td>
                        <td>
                            <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true"   Width="270px" />
                        </td>
                         <td>
                            Pool
                        </td>
                        <td>
                            <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true"   Width="210px"   />
                        </td>
                       
                    </tr>
                    <tr>
                     <td colspan="2">
                            <asp:Panel ID="pnlRecruitedperiod" runat="server" GroupingText="Recruited during the period"
                                Width="100%">
                                <table>
                                    <tr>
                                        <td>
                                            From Date
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory"   Width="150px" />
                                        </td>
                                        </tr> 
                                    <tr><td>
                                            To Date
                                        </td>
                                        <td>
                                            <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory"  Width="150px"  />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td>
                            Principal
                        </td>
                        <td>
                            <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128"     Width="210px"   AppendDataBoundItems="true" />
                        </td>
                       
                        <td>
                                    Manager
                                </td>
                                <td>
                                    <eluc:Manager ID="ucManager" AddressType="126" runat="server" AppendDataBoundItems="true"   Width="270px" />
                                </td>     
                      <td>
                                    Status
                                </td>
                                <td>
                                 <telerik:RadComboBox ID="ddlSelect" runat="server"   Width="210px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Status" >
                                     <Items>
                                        <telerik:RadComboBoxItem Text="All" Value=" "    ></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="OnBoard" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="OnLeave" Value="0"></telerik:RadComboBoxItem>
                                         </Items>
                                    </telerik:RadComboBox>
                                  
                                </td>
                    </tr>
                    <tr><td colspan="10">Experience details (continuous/ignore service break of <eluc:Number ID="ucMonths" runat="server" /> Months / ignore <eluc:Number ID="ucContracts" runat="server" /> number of contracts with other company) </td></tr>
                    <tr>
                      
                    </tr>
                </table>
                
                            <eluc:TabStrip ID="MenuLongServiceRecord" runat="server" OnTabStripCommand="MenuLongServiceRecord_TabStripCommand">
                        </eluc:TabStrip>
                                    <telerik:RadGrid ID="gvLongServiceRecord" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvLongServiceRecord_ItemCommand"
                OnItemDataBound="gvLongServiceRecord_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvLongServiceRecord_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
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
                                
                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                                 <telerik:gridtemplatecolumn  headertext="S.No">
                                  
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblslno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                 <telerik:gridtemplatecolumn  headertext="Name">
                                    
                                    <ItemTemplate>
                                      <telerik:radlabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:radlabel>
                                    
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                 <telerik:gridtemplatecolumn  headertext="Rank">
                                   
                                    <ItemTemplate>
                                         <telerik:radlabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                 <telerik:gridtemplatecolumn  headertext="File No">
                                    
                                    <ItemTemplate>
                                         <telerik:radlabel ID="lblFileno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                 <telerik:gridtemplatecolumn  headertext="Batch">
                                    
                                    <ItemTemplate>
                                       <telerik:radlabel ID="lblBatch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGBATCH") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                 <telerik:gridtemplatecolumn  headertext="DOB">
                                   
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblDob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                 <telerik:gridtemplatecolumn  headertext="Date 1st Joining">
                                   
                                    <ItemTemplate>
                                         <telerik:radlabel ID="lblDateOf1stJoin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDDATEOFJOIN") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                 <telerik:gridtemplatecolumn  headertext="Onboard Duration (YY/MM)">
                                  
                                    <ItemTemplate>
                                        <telerik:radlabel ID="lblOnBoardDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONBOARDDURATION") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                 <telerik:gridtemplatecolumn  headertext="Employment Duration (YY/MM)">
                                    
                                    <ItemTemplate>
                                      <telerik:radlabel ID="lblEmploymentDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYMENTDURATION") %>'></telerik:radlabel>
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
                                
<eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>