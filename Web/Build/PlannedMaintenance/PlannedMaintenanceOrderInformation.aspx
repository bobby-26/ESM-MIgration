<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceOrderInformation.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceOrderInformation" %>
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
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvOrderInformation.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvOrderInformation.ClientID %>"));
                }, 200);
                setValue();
            }
            window.onresize = window.onload = Resize;
                      
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="CmdHiddenSubmit_Click" CssClass="hidden" />
    <%--    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuOrderInformation" runat="server" OnTabStripCommand="MenuOrderInformation_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvOrderInformation" runat="server" OnItemDataBound="gvOrderInformation_ItemDataBound"
                OnNeedDataSource="gvOrderInformation_NeedDataSource" OnItemCommand="gvOrderInformation_ItemCommand"                 
                EnableHeaderContextFilterMenu="true" AllowFilteringByColumn="true" EnableLinqExpressions="false" OnPreRender="gvOrderInformation_PreRender"
                EnableViewState="true" ShowFooter="true" AllowCustomPaging="true" AllowPaging="true" MasterTableView-ShowFooter="false">
                <MasterTableView AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDORDERINFORMATIONID">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date" Groupable="false" UniqueName="FLDDATE" DataField="FLDDATE"
                            AllowSorting="false" FilterDelay="2000" AutoPostBackOnFilter="false" CurrentFilterFunction="Between" HeaderStyle-Width="280px">
                            <FilterTemplate>
                                <telerik:RadLabel runat="server" AssociatedControlID="FromOrderDatePicker" Text="From"></telerik:RadLabel>
                                <telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="100px" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FDATE"].ToString() %>' />
                                &nbsp;
                                <telerik:RadLabel runat="server" AssociatedControlID="ToOrderDatePicker" Text="To" Style="padding-left: 5px;"></telerik:RadLabel>
                                <telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="100px" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["TDATE"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

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
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                                               
                        <telerik:GridTemplateColumn HeaderText="Detail" AllowFiltering="false" Groupable="false" >
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDDETAIL"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Applicable To" AllowFiltering="false" Groupable="false">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDAPPLICABLETONAME"].ToString().Trim(',')%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="FLDSTATUSID" 
                            FilterControlAltText="Filter Status column" HeaderText="Status" UniqueName="FLDSTATUSID" Groupable="false" FilterDelay="2000"
                             AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains" HeaderStyle-Width="150px">
                            <FilterTemplate>
                                <telerik:RadCheckBox runat="server" ID="chkReadOrders" Text="Include Read Items" OnCheckedChanged="chkReadOrders_CheckedChanged" Checked='<%#ViewState["ISREADORDERS"].ToString() == "1" ? true : false %>'></telerik:RadCheckBox>
                                <telerik:RadComboBox ID="ddlStatus" runat="server" OnDataBinding="ddlStatus_DataBinding" AppendDataBoundItems="true"
                                OnClientItemChecked="OnClientItemChecked" CheckBoxes="true">                                                                    
                            </telerik:RadComboBox>    
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function setValue() {
                                        var combo = $find("<%# ((GridItem)Container).FindControl("ddlStatus").ClientID %>");
                                        if (combo != null) {
                                            var val = ',' + '<%# ViewState["STATUS"].ToString() %>' + ',';
                                            for (var i = 0; i < combo.get_items().get_count(); i++) {
                                                var item = combo.get_items().getItem(i);
                                                if (val.includes(',' + item.get_value() + ',')) {
                                                    combo.trackChanges();
                                                    item.set_checked(true);
                                                    combo.commitChanges();
                                                }
                                            }
                                        }
                                    }
                                    function OnClientItemChecked(sender, args) {
                                        var items = sender.get_checkedItems();                                        
                                        var val = '';
                                        for (var i = 0; i < items.length; i++) {                                            
                                            val = val + items[i].get_value() + ',';
                                        }
                                        val = val.length > 0 ? val.substring(0, val.length - 1) : val;
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDSTATUSID", val, "Contains");
                                    }
                                    function OnIncludeItemChecked(sender, args) {                                        
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        if (sender.get_checked())
                                            tableView.filter("FLDSTATUSID", "1,2,3,4", "Custom");
                                        else
                                            tableView.filter("FLDSTATUSID", "", "Contains");
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                 <%#((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>    
                        
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false" Groupable="false" HeaderStyle-Width="100px">
                            <HeaderStyle />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITR" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>                                
                                <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPY" ID="cmdCopy" ToolTip="Copy">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Issue" CommandName="ISSUE" ID="cmdIssue" ToolTip="Issue">
                                    <span class="icon"><i class="fas fa-check-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Read & Acknowledge" CommandName="ACK" ID="cmdAck" ToolTip="Read & Acknowledge">
                                    <span class="icon"><i class="fab fa-readme"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Show Read & Acknowledge List" CommandName="ACKLIST" ID="cmdAckList" ToolTip="Show Read & Acknowledge List">
                                    <span class="icon"><i class="fas fa-user-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>                            
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        <%--</telerik:RadAjaxPanel>--%>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="600px" Height="400px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
             VisibleStatusbar="false" KeepInScreenBounds="true" NavigateUrl="PlannedMaintenanceOrderInformationAdd.aspx" ReloadOnShow="true" ShowContentDuringLoad="false">            
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
