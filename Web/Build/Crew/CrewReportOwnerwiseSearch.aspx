<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportOwnerwiseSearch.aspx.cs"
    Inherits="Crew_CrewReportOwnerwiseSearch" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Ownerwise Search</title>
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
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"></telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReliefPlan" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           
                        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                
                        <eluc:TabStrip ID="ShowReports" runat="server" OnTabStripCommand="MenuShowReport_TabStripCommand">
                        </eluc:TabStrip>
      
                        <table width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblnote" runat="server" CssClass="mlabel" Text="Note: Pool and Zone are for the onboard seafarers">                                     
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                  
                        <table id="ReliefPlan" runat="server" border="1" style="border-collapse: collapse;" >
                            <tr valign="top" width="100%">
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblReliefPlanBetween" runat="server" Text="Relief Plan Between"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucDateFrom" runat="server"  />
                                                <eluc:Date ID="ucDateTo" runat="server"  />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true"
                                                    CssClass="dropdown_mandatory" Width="100%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblBatch" runat="server" Text="Batch"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr valign="top" width="100%">
                                            <td>
                                                <telerik:RadLabel ID="lblVesselType" runat="server">Vessel Type</telerik:RadLabel>
                                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true"  />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                                                <eluc:Pool ID="lstPool" runat="server" AppendDataBoundItems="true"  />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                                                <eluc:Zone ID="lstZone" runat="server" AppendDataBoundItems="true"  />
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" />
                                            </td>
                                        </tr>
                                    </table>
                    
                    </td> </tr> </table>
   <div runat="server" id="Default" Height="60%">
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
  
  
  <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" 
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
            EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound" OnItemCommand="gvCrew_ItemCommand"
            ShowFooter="False" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDEMPLOYEEID">
                <HeaderStyle Width="102px" />

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
                            <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                    <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Relief Due Date" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                             
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReliefDue" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE","{0:dd/MMM/yyyy}")) %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reliever" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOnsignerID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>' />
                                    <telerik:RadLabel ID="lblRelieverName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVER") %>' />
                                    <asp:LinkButton ID="lnkRelieverName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVER") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERREMARKS") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                        </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
           
        </telerik:RadGrid>
    </div>
     <div runat="server" id="HandOnLeave" Height="60%">
                    <eluc:TabStrip ID="MenuShowExcel1" runat="server" OnTabStripCommand="CrewShowExcel1_TabStripCommand">
                    </eluc:TabStrip>
             
                 <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew1" runat="server" 
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew1_NeedDataSource"
            EnableHeaderContextMenu="true" OnItemDataBound="gvCrew1_ItemDataBound" OnItemCommand="gvCrew1_ItemCommand"
            ShowFooter="False" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDEMPLOYEEID">
                <HeaderStyle Width="102px" />
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
                             <telerik:GridTemplateColumn HeaderText="First Name" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                    <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Surname" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSurname" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Last Vessel" AllowSorting="false" HeaderStyle-Width="140px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLastVesselName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Signed Off" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSignedOff" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE","{0:dd/MMM/yyyy}")) %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn HeaderText="Zone" AllowSorting="false" HeaderStyle-Width="55px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblZone" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                              <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" HeaderStyle-Width="520px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                                </ItemTemplate>
                             </telerik:GridTemplateColumn>
                        </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
          </div>   
    </telerik:RadAjaxPanel>
 
    </form>
</body>
</html>
