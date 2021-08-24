<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsSeniorOfficerAnalysisReport.aspx.cs"
    Inherits="Crew_CrewReportsSeniorOfficerAnalysisReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Senior Officer Analysis Report</title>
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
                
                            <table  Width="90%" >
                            <tr>
                                <td>
                                    Oil Major
                                </td>
                                <td>
                                    <eluc:Hard ID="ddlOilMajor" runat="server" HardTypeCode="100" AppendDataBoundItems="true" Width="270px"
                                        CssClass="dropdown_mandatory" />
                                </td>
                                <td>
                                    Contract
                                </td>
                                <td>
                                    <eluc:Hard ID="ddlContract" runat="server" HardTypeCode="101" AppendDataBoundItems="true"  Width="210px"    CssClass="dropdown_mandatory" />
                                </td>
                                <td>
                                    Principal
                                </td>
                                <td>
                                    <eluc:Principal ID="ucPrincipal" AddressType="128"  Width="210px"
runat="server" AppendDataBoundItems="true"   />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Vessel
                                </td>
                                <td>
                                    <eluc:Vessel ID="ucVessel" Width="100%" VesselsOnly="true" AppendDataBoundItems="true" runat="server" EntityType="VSL" AssignedVessels="true" />
                                </td>
                                <td>
                                    Vessel Type
                                </td>
                                <td>
                                    <eluc:VesselType ID="ucVesselType"  Width="210px" runat="server" AppendDataBoundItems="true"  />
                                </td>
                                <td>
                                    Pool
                                </td>
                                <td>
                                    <eluc:Pool ID="lstPool" runat="server"  Width="210px" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Show Seafarers
                                </td>
                                <td>
                                    <telerik:RadRadioButtonList ID="rblOfficers" runat="server" RepeatDirection="Horizontal">
                                     <Items>                                        
                                           <telerik:ButtonListItem Text="Junior Officers" Value="1"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Text="Senior Officers" Value="2" Selected="True"    />
                                         </Items>
                                    </telerik:RadRadioButtonList>
                                </td>
                                <td>
                                    As on Date
                                </td>
                                <td>
                                    <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" Width="210px"
 />
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
                    <Columns>

                          <telerik:GridTemplateColumn HeaderText="Vessel" allowsorting="true" SortExpression="FLDVESSEL"  >
                                   
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Rank Code" allowsorting="true" SortExpression="FLDRANK"  >
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRankName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" allowsorting="true" SortExpression="FLDNAME"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                    <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                       
                               <telerik:GridTemplateColumn HeaderText="S/On Date"   >
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSignOnDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Relief Due"   >
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReliefDueDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                  <telerik:GridTemplateColumn HeaderText="Time on Tanker (months)">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTimeonTanker" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKEREXP") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Time in Rank (months)">
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTimeInRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKEXP") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Time with Company (months)">
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTimeInCompany" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDESMEXP") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Time on Tanker (Aggregate Years)">
                                  
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTimeonTankerAgg" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGGREGATETANKEREXPINYEARS") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Time in Rank (Aggregate Years)">
                                 
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTimeInRankAgg" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGGREGATERANKEXPINYEARS") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Time with Company (Aggregate Years)">
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTimeInCompanyAgg" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGGREGATEESMEXPINYEARS") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Time on Tanker (Complies with Matrix)">
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTimeonTankerMatrix" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLIESWITHTANKEREXP") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Time in Rank (Complies with Matrix)">
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTimeInRankMatrix" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLIESWITHRANKEXP") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Time with Company (Complies with Matrix)">
                                   
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTimeInCompanyMatrix" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLIESWITHESMEXP") %>' />
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
            