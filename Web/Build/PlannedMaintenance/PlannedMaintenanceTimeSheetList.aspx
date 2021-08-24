<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceTimeSheetList.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceTimeSheetList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">            
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvTimeSheet.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
            function pageLoad() {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .strikethrough {
            text-decoration: line-through;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvTimeSheet">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvTimeSheet" />
                         <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuTimeSheet">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuTimeSheet" />
                        <telerik:AjaxUpdatedControl ControlID="gvTimeSheet" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuTimeSheet" runat="server" OnTabStripCommand="MenuTimeSheet_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvTimeSheet" runat="server" AutoGenerateColumns="false"
            AllowSorting="false" GroupingEnabled="false" OnItemDataBound="gvTimeSheet_ItemDataBound" OnItemCommand="gvTimeSheet_ItemCommand"
            EnableHeaderContextMenu="true" AllowCustomPaging="true" AllowPaging="true" OnNeedDataSource="gvTimeSheet_NeedDataSource" 
            EnableLinqExpressions="false" EnableViewState="true" AllowFilteringByColumn="true">
            <MasterTableView TableLayout="Fixed" DataKeyNames="FLDTIMESHEETID" AutoGenerateColumns="false" AllowFilteringByColumn="true">
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
                    <telerik:GridTemplateColumn UniqueName="FLDVESSELSTATUSNAME" HeaderText="Vessel Status"  AllowSorting="false" ShowSortIcon="false" FilterControlWidth="99%"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" HeaderStyle-Width="150px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlVesselStatus" runat="server" OnDataBinding="ddlVesselStatus_DataBinding" AppendDataBoundItems="true"
                                SelectedValue='<%# ViewState["STATUS"].ToString() %>' OnClientSelectedIndexChanged="StatusIndexChanged">
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function StatusIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDVESSELSTATUSNAME", args.get_item().get_value(), "EqualTo");
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <ItemTemplate>
                            <%#  DataBinder.Eval(Container,"DataItem.FLDVESSELSTATUSNAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="FLDOPERATION" HeaderText="Operation"  AllowSorting="false" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  DataBinder.Eval(Container,"DataItem.FLDOPERATION") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="FLDDATETIME" HeaderText="Time" FilterDelay="2000" AllowSorting="false" HeaderStyle-Width="160px">
                        <FilterTemplate>
                            From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                DbSelectedDate='<%# ViewState["FDATE"].ToString() %>' />
                            <br />
                            To&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                DbSelectedDate='<%# ViewState["TDATE"].ToString() %>' />
                            <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                <script type="text/javascript">
                                    function FromDateSelected(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(sender);
                                        var toDate = FormatSelectedDate(ToPicker);

                                        tableView.filter("FLDDATETIME", fromDate + "~" + toDate, "Between");

                                    }
                                    function ToDateSelected(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(FromPicker);
                                        var toDate = FormatSelectedDate(sender);

                                        tableView.filter("FLDDATETIME", fromDate + "~" + toDate, "Between");
                                    }
                                    function FormatSelectedDate(picker) {
                                        var date = picker.get_selectedDate();
                                        var dateInput = picker.get_dateInput();
                                        var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());

                                        return formattedDate;
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATETIME"), DateDisplayOption.DateTimeHR24) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="FLDDETAIL" HeaderText="Details"  AllowSorting="false" AllowFiltering="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  DataBinder.Eval(Container,"DataItem.FLDDETAIL") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="FLDTYPE" HeaderText="Source"  AllowSorting="false" HeaderStyle-Width="110px" ShowSortIcon="false" FilterControlWidth="99%"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <FilterTemplate>
                            <telerik:RadComboBox ID="ddlSource" runat="server" Width="100%" SelectedValue='<%# ViewState["SOURCE"].ToString() %>' OnClientSelectedIndexChanged="SourceIndexChanged">
                                 <Items>
                                    <telerik:RadComboBoxItem Value="" Text="--All--" />
                                    <telerik:RadComboBoxItem Value="1" Text="Activity" />
                                    <telerik:RadComboBoxItem Value="2" Text="Maintenance" />
                                    <telerik:RadComboBoxItem Value="3" Text="Others" />       
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function SourceIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDTYPE", args.get_item().get_value(), "EqualTo");
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <ItemTemplate>
                            <%#  DataBinder.Eval(Container,"DataItem.FLDTYPE") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="FLDCREATEDBYNAME" HeaderText="Entered By" AllowSorting="false" FilterControlWidth="99%" FilterDelay="1000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  DataBinder.Eval(Container, "DataItem.FLDCREATEDBYNAME") + " on " + General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE"))%> 
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowSorting="false" AllowFiltering="false" HeaderStyle-Width="50px">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="185px" />
                <Resizing AllowColumnResize="true" />
            </ClientSettings>            
        </telerik:RadGrid>
    </form>
</body>
</html>
