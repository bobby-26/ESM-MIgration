<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportSignoffFeedBack.aspx.cs"
    Inherits="Crew_CrewReportSignoffFeedBack" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PoolList" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EmpFleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Off FeedBack Report</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSignoffFB.ClientID %>"));
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

            <table width="95%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone runat="server" ID="ucZone" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ucPrincipal" AddressType="128" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBatch" runat="Server" Text="Batch"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td  colspan="2" >
                        <asp:Panel ID="pnlPeriod" runat="server" GroupingText="FeedBack" >
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblFBFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFBFromDate" runat="server" Width="120px"  />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblFBToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFBToDate" runat="server"  Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PoolList ID="ucPool" runat="server" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblManager" runat="server" Text="Manager"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true" Width="210px" />
                    </td>
                    <td colspan="2" >
                        <asp:Panel ID="pnlSignoff" runat="server" GroupingText="Signoff" >
                            <table>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblSFFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="UcSFFromDate" runat="server"  Width="120px" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblSFToDate" runat="server" Text="To Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="UcSFToDate" runat="server" Width="120px"  />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvSignoffFB" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnSortCommand="gvSignoffFB_Sorting"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvSignoffFB_ItemCommand"
                OnItemDataBound="gvSignoffFB_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvSignoffFB_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
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
                        <columns>

                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Emp Code">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Emp Name" allowsorting="true"         sortexpression="FLDEMPLOYEENAME">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                    <telerik:radlabel ID="lblSignonoffId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>' />
                                    <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Rank"    allowsorting="true"           sortexpression="FLDRANKNAME">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sign On Date">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSignOnDate" Visible="true" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE"))%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sign Off Date">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSignedOff" Visible="true" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNOFFDATE"))%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Vessel" allowsorting="true"               sortexpression="FLDVESSELNAME">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="FeedBack Y/N">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSignedOffFeedBackyn" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKSTATUS") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="FeedBack Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                  <telerik:radlabel ID="lblFBYN" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKYN") %>' />
                                    <telerik:radlabel ID="lblFeedBackDate" Visible="true" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFEEDBACKDATE"))%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                               <telerik:GridTemplateColumn HeaderText="Action">
                                   
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate >
                                        <asp:LinkButton runat="server" ID="cmdShowReport" Text="" ToolTip="Report" Visible="false" >
                                              <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                            </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                         </columns>

                        <pagerstyle mode="NextPrevNumericAndAdvanced" pagebuttoncount="10" pagertextformat="{4}<strong>{5}</strong> Records matching your search criteria"
                            pagesizelabeltext="Records per page:" alwaysvisible="true" cssclass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                <clientsettings enablerowhoverstyle="true" allowcolumnsreorder="true" reordercolumnsonclient="true" allowcolumnhide="true" columnsreordermethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </clientsettings>
                        </telerik:RadGrid>
                             <eluc:Status runat="server" ID="ucStatus" />
</telerik:RadAjaxPanel>
    </form>
</body>
</html> 