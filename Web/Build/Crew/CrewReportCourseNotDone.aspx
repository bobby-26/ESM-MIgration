<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportCourseNotDone.aspx.cs"
    Inherits="CrewReportCourseNotDone" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationalityList" Src="../UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRankList" Src="../UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselList" Src="../UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselTypeList" Src="../UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlPool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
 <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewCourseNotDone.ClientID %>"));
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
    <form id="frmCoursesDoneReport" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        
            <eluc:TabStrip ID="MenuCourseNotDoneReport" runat="server" OnTabStripCommand="MenuCourseNotDoneReport_TabStripCommand">
            </eluc:TabStrip>
       
        <div id="divGuidance" runat="server">
            <table id="tblGuidance">
                <tr>
                    <td>
                        <td>
                            <telerik:RadLabel ID="lblcourse1" runat="server" CssClass="mlabel">Note:<br />&nbsp 1.When File No is entered course filter is not mandatory.<br />&nbsp
                                        2.Vessel filter is applicable only if the status is selected as Onboard or Onleave.<br />&nbsp
                                        3.When Sign on/off dates are entered course and issue period filter is not mandatory</telerik:RadLabel>
                        </td>
                    </td>
                </tr>
            </table>
        </div>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    Course Type
                </td>
                <td>
                    <eluc:hard runat="server" id="ucDocumentType" appenddatabounditems="true"
                        hardtypecode="103" autopostback="true" ontextchangedevent="DocumentTypeSelection"   Width="270px" />
                </td>
                <td rowspan="2">
                   <telerik:RadLabel Text="Vessel Type" ID="lblvesseltype" runat="server" ></telerik:RadLabel>
                </td>
                <td rowspan="2" style="padding-right:10px">
                    <eluc:usercontrolvesseltypelist id="lstVesselType" runat="server" autopostback="true"    
                        appenddatabounditems="true" ontextchangedevent="ucPrincipal_TextChangedEvent" />
                </td>
                <td rowspan="2">
                   <telerik:RadLabel Text="Batch" ID="lblbatch" runat="server" ></telerik:RadLabel>
                </td>
                <td rowspan="2">
                    <eluc:batch id="ucBatch" runat="server" appenddatabounditems="true" Width="92%"  />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel Text="Course" ID="lblcourse" runat="server" ></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlCourse" runat="server" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Course" CssClass="dropdown_mandatory" OnDataBound="ddlCourse_DataBound" Width="270px">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                  <telerik:RadLabel Text="File number" ID="lblfileno" runat="server" ></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFileNo" runat="server"  Width="270px" ></telerik:RadTextBox>
                </td>
                <td rowspan="2">
                   <telerik:RadLabel Text="Vessel" ID="lblvessel" runat="server" ></telerik:RadLabel>
                </td>
                <td rowspan="2" style="padding-right:10px">
                    <eluc:UserControlVesselList ID="ucVessel" runat="server" AppendDataBoundItems="true" EntityType="VSL" AssignedVessels="true" VesselsOnly="true" ActiveVesselsOnly="true"/>
                </td>
                <td rowspan="2">
                   <telerik:RadLabel Text="Pool" ID="lblpool" runat="server" ></telerik:RadLabel>
                </td>
                <td rowspan="2">
                    <eluc:usercontrolpool id="ucPool" runat="server" appenddatabounditems="true" Width="240px" />
                </td>
            </tr>
            <tr>
                <td>
                   <telerik:RadLabel Text="Status" ID="lblstatus" runat="server" ></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlStatus" runat="server"   Width="270px" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Status">
                      <Items>
                        <telerik:RadComboBoxItem Text="All" Value=" "></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Text="OnBoard" Value="1" />
                        <telerik:RadComboBoxItem Text="OnLeave" Value="0" />
                          </Items>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                  <telerik:RadLabel Text="Principal" ID="lblprincipal" runat="server" ></telerik:RadLabel>
                </td>
                <td>
                    <eluc:addresstype runat="server" id="ucPrincipal" addresstype="128"  Width="270px" 
                        autopostback="true" ontextchangedevent="ucPrincipal_TextChangedEvent" appenddatabounditems="true" />
                </td>
                <td rowspan="2">
                    <telerik:RadLabel Text="Rank" ID="lblrank" runat="server" ></telerik:RadLabel>
                </td>
                <td rowspan="2" style="padding-right:10px">
                    <eluc:usercontrolranklist id="ucRank" runat="server"  appenddatabounditems="true" />
                </td>
                <td rowspan="2">
                   <telerik:RadLabel Text="Zone" ID="lblzone" runat="server" ></telerik:RadLabel>
                </td>
                <td rowspan="2">
                    <eluc:zone id="ucZone" runat="server"  appenddatabounditems="true"  Width="240px"/>
                </td>                
            </tr>
            <tr>
                <td>
                   <telerik:RadLabel Text="Manager" ID="lblmgr" runat="server" ></telerik:RadLabel>
                </td>
                <td>
                    <eluc:addresstype id="ucManager" runat="server" addresstype="126" appenddatabounditems="true"  Width="270px"   />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlSignon" runat="server" GroupingText="SignOn Date" Width="90%">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel Text="From" ID="lblfrom1" runat="server" ></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:date id="ucSignonFromDate" runat="server" />
                                </td>
                                <td>
                                   <telerik:RadLabel Text="To" ID="lblto1" runat="server" ></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:date id="ucSignonToDate" runat="server"  />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td colspan="2">
                    <asp:Panel ID="pnlSignoff" runat="server" GroupingText="SignOff Date" Width="90%">
                        <table>
                            <tr>
                                <td>
                                   <telerik:RadLabel Text="From" ID="lblfrom" runat="server" ></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:date id="ucSignoffFromDate" runat="server"  />
                                </td>
                                <td>
                                <telerik:RadLabel Text="To" ID="lblto" runat="server" ></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:date id="ucSignoffToDate" runat="server"  />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td colspan="2"  >
                    <telerik:RadLabel Text="Include new applicant" ID="lblnewapplnt" runat="server" ></telerik:RadLabel>
                    <telerik:RadCheckBox ID="chkIncludeNewApp" runat="server" />
                </td>
            </tr>
        </table>
        <br />
        
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
            </eluc:TabStrip>
        
                <telerik:RadGrid ID="gvCrewCourseNotDone" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnSorting="gvCrewCourseNotDone_Sorting"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrewCourseNotDone_ItemCommand"
                OnItemDataBound="gvCrewCourseNotDone_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrewCourseNotDone_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                     AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                    <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                    
                    <telerik:GridTemplateColumn HeaderText="Sl No.">
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn  HeaderText="File No">
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn  HeaderText="Batch">
                        
                          <ItemTemplate>
                            <telerik:RadLabel ID="lblBatch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn  HeaderText="Name" AllowSorting="true" SortExpression="FLDEMPLOYEENAME">
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCrewId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lnkCrew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>                    
                    <telerik:GridTemplateColumn   HeaderText="Rank" AllowSorting="true" SortExpression="FLDRANK">
                       
                         <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn   HeaderText="1st join dt" >
                        
                         <ItemTemplate>
                            <telerik:RadLabel ID="lblJoinDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTJOINDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                   <telerik:GridTemplateColumn   HeaderText="Zone" >
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblZone" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>' Width="200px"></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn   HeaderText="Onboard" >
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPresentVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSEL") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn   HeaderText="S/on Date" >
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignOnDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn   HeaderText="Last Vessel" >
                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblExVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSEL") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn   HeaderText="S/off Date" >
                       
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblIssueDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTSIGNOFFDATE") %>'></telerik:RadLabel>
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