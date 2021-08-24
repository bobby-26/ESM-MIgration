<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDrillsvsVesselList.aspx.cs"
    Inherits="Registers_DrillsvsVesselList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="vessellist" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
   
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
        DecorationZoneID="gvDrillvsVessels" DecoratedControls="All" EnableRoundedCorners="true" />
  
   
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
         <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
    <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="drillvsvessels_TabStripMenuCommand" TabStrip="true">
            </eluc:TabStrip>
    <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvDrillvsVessels" AutoGenerateColumns="false"
        AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvDrillvsVessels_NeedDataSource" Height="92.5%" AllowFilteringByColumn="true"
        OnItemDataBound="gvDrillvsVessels_ItemDataBound"  OnItemCommand="gvDrillvsVessels_ItemCommand">
        <MasterTableView EditMode="InPlace" DataKeyNames="FLDDRILLSCHEDULEID" AutoGenerateColumns="false" AllowFilteringByColumn="true" 
            TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState ="false"
            InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="true">
             <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true" ></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
            <Columns>
                <telerik:GridTemplateColumn HeaderText="Drill " DataField="FLDDRILLNAME" UniqueName="FLDDRILLNAME" AllowFiltering="true" >
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="true" Width="300px"/>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <telerik:RadLabel ID="RadlblDrillName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLNAME")%>'>
                        </telerik:RadLabel>
                    </ItemTemplate>
                    <FilterTemplate>
                        <telerik:RadTextBox ID="radtbdrill" runat="server"  Text='<%# ViewState["DRILL"].ToString() %>' Width="120px" ClientEvents-OnValueChanged="DrillFilter" />
                        &nbsp
                        <telerik:RadDropDownList ID="radtype" runat="server" Width="140Px" OnClientSelectedIndexChanged="TypeFilter" >
                            <Items>
                                <telerik:DropDownListItem  Text="Mandatory" Value="Mandatory"/>
                                <telerik:DropDownListItem  Text="Company Specified" Value="COMPANYSPECIFIED"/>
                            </Items>
                            </telerik:RadDropDownList>
                    </FilterTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Vessel " UniqueName="FLDVESSELID"  FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" >
                    <FilterTemplate>
                        <eluc:vessellist ID="ddlvessellist" runat="server" Width="220px" CssClass="input" SyncActiveVesselsOnly="true"  ManagementType="FUL" AutoPostBack="true" VesselList='<%#PhoenixRegistersVessel.VesselListCommon(General.GetNullableByte("0")
                                                                        , General.GetNullableByte("1")
                                                                        , General.GetNullableByte("1")
                                                                        , General.GetNullableByte("1")
                                                                        , PhoenixVesselEntityType.VSL
                                                                        , PhoenixVesselManagementType.FUL)%>'
                      Entitytype="VSL"   ActiveVesselsOnly="true" VesselsOnly="true" AssignedVessels="true" SelectedVessel='<%# ViewState["VESSELID"].ToString() %>' OnTextChangedEvent="ddlvessellist_TextChangedEvent"/>
                        
                    </FilterTemplate>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:LinkButton ID="RadlblVesselName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>' ></asp:LinkButton>
                      
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText=" Due on " UniqueName="FLDDATE">
                    <HeaderStyle HorizontalAlign="Center"  Width="300PX" Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Center" />
                     <FilterTemplate>
                      
                         <telerik:RadDatePicker ID="tbfromdateentry" runat="server" Width="130px" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FROMDATE"].ToString() %>' /> 
                         To
                         
                         <telerik:RadDatePicker ID="tbtodate" runat="server" Width="130px" ClientEvents-OnDateSelected="ToDateSelected" AutoPostBack="true"
                                    DbSelectedDate='<%# ViewState["TODATE"].ToString() %>' />  
                         <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("tbtodate").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var FromPicker = $find('<%# ((GridItem)Container).FindControl("tbfromdateentry").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                            tableView.filter("FLDDATE", fromDate + "~" + toDate, "Between");
                                        }
                                        function FormatSelectedDate(picker) {
                                            var date = picker.get_selectedDate();
                                            var dateInput = picker.get_dateInput();
                                            var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());

                                            return formattedDate;
                                        }
                                        function DrillFilter(sender, args) {
                                            var drill = sender.get_value();
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var type = $find('<%# ((GridItem)Container).FindControl("radtype").ClientID %>');
                                            tableView.filter("FLDDRILLNAME", drill + "~" + type.get_selectedItem().get_value(), "NoFilter");
                                            
                                        }
                                        function TypeFilter(sender, args) {
                                            var type = sender.get_selectedItem().get_value();
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var drill = $find('<%# ((GridItem)Container).FindControl("radtbdrill").ClientID %>');
                                            tableView.filter("FLDDRILLNAME", drill.get_value() + "~" + type, "NoFilter");
                                            
                                        }
                                        
                                    </script>
                                </telerik:RadScriptBlock>      
                    </FilterTemplate>
                    <ItemTemplate>
                        <telerik:RadLabel ID="Radlblduedate" runat="server" Text='<%#General.GetDateTimeToString((DataBinder.Eval(Container, "DataItem.FLDDRILLDUEDATE")).ToString())%>'>
                        </telerik:RadLabel>
                      
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText=" Overdue by " DataField="DUEIN" UniqueName="DUEIN" FilterControlWidth="120px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                    
                    <HeaderStyle HorizontalAlign="Left"  Font-Bold="true"/>
                    <ItemStyle HorizontalAlign="Left" Wrap="true" />
                    
                    <ItemTemplate>
                        <telerik:RadLabel ID="Radlblduein" runat="server" Text='<%#(DataBinder.Eval(Container, "DataItem.DUEIN"))%>'>
                        </telerik:RadLabel>
                       
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

            </Columns>
            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                AlwaysVisible="true" />
        </MasterTableView>
        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"  ColumnsReorderMethod="Reorder">
                    
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="370px"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
    </telerik:RadGrid>
             </telerik:RadAjaxPanel>
   
    </form>
</body>
</html>
