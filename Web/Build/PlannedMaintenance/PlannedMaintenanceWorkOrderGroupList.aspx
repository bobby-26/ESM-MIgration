<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderGroupList.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderGroupList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvWorkOrder.ClientID %>"));
                }, 200);
                setValue();
            }
            function CloseUrlModelWindow(keepOpen) {
                if (keepOpen == null || !keepOpen) {
                    var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                    wnd.close();
                }
                var masterTable = $find('<%=gvWorkOrder.ClientID %>').get_masterTableView();
                masterTable.rebind();
            }          
            function itemClicked(list, args) {
                var $ = $telerik.$;
                var gridRowElem = $(list._element).parents("tr").first()[0];
                var grid = $find("gvWorkOrder");               
                var masterTable = grid.get_masterTableView();                
                var item = masterTable.get_dataItems()[gridRowElem.rowIndex - 1];               
                var isUnPlanned = gridRowElem.querySelector("span[id*='lblIsUnPlannedJob']").innerText;
                var woname = $telerik.findElement(masterTable.get_element(), "lblWorkorderNumber").innerHTML;
                var cancel = $telerik.findElement(masterTable.get_element(), "cmdDelete");                
                var did = item.getDataKeyValue("FLDWODETAILID");
                var woid = item.getDataKeyValue("FLDWORKORDERGROUPID");
                var val = args.get_item().get_value();
                var txt = args.get_item().get_text();      
                var callBackFn = function (shouldSubmit, e) {
                    Telerik.Web.UI.RadWindowUtils.Localization =
                    {
                        "OK": "OK",
                        "Cancel": "Cancel"
                    };
                    var button = $find("<%= cmdHiddenActivity.ClientID %>");
                    if (shouldSubmit) {
                        if (val != "3") {
                            button.set_commandArgument(val + "~" + did + '~3');
                            button.click();
                        }                       
                    }
                    else {
                        if (isUnPlanned == "1" && val == "3") {
                            $(cancel).find("i").trigger("click");
                        }
                        if (e != null && e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                if (val == "5" || val == "1") {
                    radconfirm('Are you sure this WO is "' + txt + '"', callBackFn);
                }
                else if (val == "3") {
                    $modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReschedule.aspx?groupId=' + woid + '&woplanid=' + did + '&unplanned=' + isUnPlanned;
                    showDialog('Reschedule/Cancel WO - ' + woname);
                }
                else {
                    top.openNewWindow('activity', 'Maintenance Activity - ' + txt, SitePath + '/PlannedMaintenance/PlannedMaintenanceActivity.aspx?wopi=' + did + '&status=' + val); return false;
                }
            }         
            function pageLoad() {
                setValue();
            }
        </script>
         <style type="text/css">
             .align{
                 vertical-align:middle !important;
             }
            </style>
    </telerik:RadCodeBlock>
</head>
<body onload="Resize();" onresize="Resize();">
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <telerik:RadButton runat="server" ID="cmdHiddenActivity" CssClass="hidden" OnClick="cmdHiddenActivity_Click"/>
            <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" GroupingEnabled="false"
                OnItemCommand="gvWorkOrder_ItemCommand" EnableViewState="true" OnSortCommand="gvWorkOrder_SortCommand"
                OnItemDataBound="gvWorkOrder_ItemDataBound" EnableHeaderContextMenu="true" EnableLinqExpressions="false">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERGROUPID" ClientDataKeyNames="FLDWORKORDERGROUPID,FLDWODETAILID" AllowFilteringByColumn="true">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Work Order No." AllowSorting="false" FilterDelay="3000"
                             ShowFilterIcon="false" CurrentFilterFunction="Contains"
                            UniqueName="FLDWORKORDERNUMBER">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroupId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWOPID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWODETAILID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lblWorkorderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'
                                    CommandName="Select"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" AllowSorting="false" FilterDelay="3000"
                             ShowFilterIcon="false" CurrentFilterFunction="Contains" HeaderStyle-Width="150px" UniqueName="FLDWORKORDERNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTitleEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" FilterDelay="2000" UniqueName="FLDJOBCATEGORYID" HeaderStyle-Width="200px">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblJobCategory" runat="server" OnDataBinding="cblJobCategory_DataBinding" AppendDataBoundItems="true"
                                  OnClientItemChecked="OnClientItemChecked" CheckBoxes="true" Width="98%"  >
                                </telerik:RadComboBox>
                                 <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">                                   
                                    function setValue() {
                                        var combo = $find("<%# ((GridItem)Container).FindControl("cblJobCategory").ClientID %>");
                                        if (combo != null) {
                                            var val = ',' + '<%# ViewState["JobCategoryFilter"].ToString() %>' + ',';
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
                                        tableView.filter("FLDJOBCATEGORYID", val, "Contains");
                                     }
                                </script>
                            </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCATEGORY") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned Date" HeaderStyle-Width="100px" AllowSorting="true" FilterDelay="2000"
                            ShowSortIcon="true" SortExpression="FLDPLANNINGDUEDATE" DataField="FLDPLANNINGDUEDATE" UniqueName="FLDPLANNINGDUEDATE">
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# GetFilter("FromDate") %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# GetFilter("ToDate") %>' />
                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDPLANNINGDUEDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                            tableView.filter("FLDPLANNINGDUEDATE", fromDate + "~" + toDate, "Between");
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
                                <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--                            <EditItemTemplate>
                                <eluc:Date ID="ucPlannedDate" runat="server" Width="100%" CssClass="gridinput_mandatory" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></eluc:Date>
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Duration" AllowFiltering="false" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                Days&nbsp&nbsp<telerik:RadMaskedTextBox ID="txtDurationInDayEdit" runat="server" Mask="###" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANINGDURATIONINDAYS") %>'
                                    InputType="Number" Width="40px">
                                </telerik:RadMaskedTextBox><br />
                                Hours<telerik:RadMaskedTextBox ID="txtDurationEdit" runat="server" Mask="###" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNINGESTIMETDURATION") %>'
                                    InputType="Number" Width="40px">
                                </telerik:RadMaskedTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Assigned To" HeaderStyle-Width="120px" AllowSorting="true" ShowFilterIcon="false"
                            ShowSortIcon="true" SortExpression="FLDDISCIPLINENAME" FilterDelay="200" UniqueName="FLDPLANNINGDISCIPLINE">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblDiscipline" runat="server" OnDataBinding="cblDiscipline_DataBinding" Width="100%" AppendDataBoundItems="true"
                                    OnClientSelectedIndexChanged="DisciplineIndexChanged" SelectedValue='<%# ViewState["filterDiscipline"] %>'>
                                </telerik:RadComboBox>
                                <script type="text/javascript">
                                    function DisciplineIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDPLANNINGDISCIPLINE", args.get_item().get_value(), "EqualTo");
                                    }
                                </script>
                            </FilterTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Discipline ID="ucDisciplineEdit" runat="server" CssClass="input"
                                    AppendDataBoundItems="true" DisciplineList="<%# PhoenixRegistersDiscipline.ListDiscipline() %>" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="80px" ShowFilterIcon="false" UniqueName="FLDSTATUSCODE"
                            AllowSorting="true" ShowSortIcon="true" SortExpression="FLDHARDNAME" FilterDelay="200">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cmbStatus" runat="server" OnDataBinding="cmbStatus_DataBinding" OnClientSelectedIndexChanged="StatusIndexChanged"
                                    SelectedValue='<%# ViewState["filterStatus"] %>' AppendDataBoundItems="true"
                                    Width="100%">
                                </telerik:RadComboBox>
                                <script type="text/javascript">
                                    function StatusIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDSTATUSCODE", args.get_item().get_value(), "EqualTo");
                                    }
                                </script>
                            </FilterTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadToolTip ID="lblStatusDesc" runat="server" TargetControlID="lblStatus" Text="<br>Planned – WO created newly.<br>In Progress – WO in the course of being done. <br>Postponed – WO delayed to the stipulated time. <br>Report Pending – WO waiting for filling up of reporting templates/parts consumed, or verification rejected.<br>Superintendent Verification Pending – WO waiting for Superintendent verification/approval.<br>Vessel Verification Pending - WO waiting for verification/approval by Vessel." AutoCloseDelay="3000"></telerik:RadToolTip>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#Bind("FLDSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="RA" UniqueName="RA" AllowFiltering="false" AllowSorting="false" HeaderStyle-Width="25px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                 <%# DataBinder.Eval(Container,"DataItem.FLDISRA")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PTW" UniqueName="PTW" AllowFiltering="false" AllowSorting="false" HeaderStyle-Width="25px" >
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                                 <%# DataBinder.Eval(Container,"DataItem.FLDISPTW")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Spares" UniqueName="SPARES" AllowFiltering="false" AllowSorting="false" HeaderStyle-Width="25px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                            <ItemTemplate>
                               <%# DataBinder.Eval(Container,"DataItem.FLDISSPARES")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Routine WO. ?" HeaderStyle-Width="66px" ShowFilterIcon="false" FilterDelay="2000" UniqueName="FLDISUNPLANNEDJOB">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cmbIsunPlanned" runat="server" RenderMode="Lightweight" AutoPostBack="true" Width="100%"
                                    OnSelectedIndexChanged="cmbIsunPlanned_SelectedIndexChanged" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("FLDISUNPLANNEDJOB").CurrentFilterValue %>'>
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                        <telerik:RadComboBoxItem Text="Yes" Value="0" />
                                        <telerik:RadComboBoxItem Text="No" Value="1" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIsUnPlannedJob" runat="server" Text='<%#Bind("FLDISUNPLANNEDJOB")%>' CssClass="hidden"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsPlanned" runat="server" Text='<%#Bind("FLDISPLANNEDYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkIsPlannedEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISUNPLANNEDJOB").ToString().Equals("0"))? true:false %>'></telerik:RadCheckBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false" HeaderStyle-Width="280px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadRadioButtonList ID="rblInProgress" runat="server" ClientEvents-OnItemClicked="itemClicked" Width="140px" CssClass="align">
                                    <Items>
                                        <telerik:ButtonListItem Text="Work Completed" Value="4" />
                                        <telerik:ButtonListItem Text="Cont. to next day" Value="5" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                                <telerik:RadRadioButtonList ID="rblPlanned" runat="server" ClientEvents-OnItemClicked="itemClicked" Width="140px" CssClass="align">
                                    <Items>
                                        <%--<telerik:ButtonListItem Text="Done as Scheduled" Value="1" />--%>
                                        <telerik:ButtonListItem Text="Done" Value="2" />
                                        <telerik:ButtonListItem Text="Not Done" Value="3" />
                                    </Items>
                                </telerik:RadRadioButtonList>&nbsp&nbsp&nbsp&nbsp
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Toolbox" ID="cmdToolBox" CommandName="TOOLBOX" ToolTip="Tool box meet">
                                 <span class="icon"><i class="fas fa-toolbox"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Start" CommandName="START" ID="cmdStart" ToolTip="Start">
                                    <span class="icon"><i class="fas fa-hourglass-start"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReport" CommandName="REPORT" ToolTip="Report Work Order">
                                 <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Reschedule">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Communication"
                                    CommandName="COMMUNICATION" ID="lnkCommunication" ToolTip="Comments">
                                <span class="icon"><i class="fas fa-postcomment"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Verification" ID="cmdhistory" CommandName="VHISTORY" ToolTip="Verification History">
                                    <span class="icon"><i class="fas fa-history"></i></span>
                                </asp:LinkButton>
                                <asp:ImageButton runat="server" ID="cmdAtt" ToolTip="Attachment" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"  />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
         <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="900px" Height="365px" Behaviors="Close,Maximize,Minimize,Move"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false" OnClientClose="onClose">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                Sys.Application.add_load(function () {                    
                    setTimeout(function () {   
                        TelerikGridResize($find("<%= gvWorkOrder.ClientID %>"));
                    }, 200);
                });
                $modalWindow.modalWindowID = "<%=RadWindow_NavigateUrl.ClientID %>";
                function onClose() {
                    document.getElementById("cmdHiddenSubmit").click();
                }
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
