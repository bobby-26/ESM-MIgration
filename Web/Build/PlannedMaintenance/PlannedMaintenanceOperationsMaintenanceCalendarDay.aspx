<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceOperationsMaintenanceCalendarDay.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceOperationsMaintenanceCalendarDay" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Operation/Maintenance Record</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>        
        <script type="text/javascript">            
            function CloseModelWindow(gridid) {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                masterTable.rebind();
            }
            var DWPgrid = null;
            function resize() {
                var $ = $telerik.$;
                var height = $(window).height();
                if (DWPgrid != null && DWPgrid.GridDataDiv != null) {                    
                    var gridPagerHeight = (DWPgrid.PagerControl) ? DWPgrid.PagerControl.offsetHeight : 0;
                    DWPgrid.GridDataDiv.style.height = (height - gridPagerHeight - 100) + "px";
                } else {                    
                    var gvMaintCompleted = $find("<%= gvMaintCompleted.ClientID %>");
                    var gvOperationsCompleted = $find("<%= gvOperationsCompleted.ClientID %>");

                    var gvMaintCompletedPagerHeight = (gvMaintCompleted.PagerControl) ? gvMaintCompleted.PagerControl.offsetHeight : 0;
                    var gvOperationsCompletedPagerHeight = (gvOperationsCompleted.PagerControl) ? gvOperationsCompleted.PagerControl.offsetHeight : 0;

                    gvMaintCompleted.GridDataDiv.style.height = (Math.round(height / 2) - gvMaintCompletedPagerHeight - 65) + "px";
                    gvOperationsCompleted.GridDataDiv.style.height = (Math.round(height / 2) - gvOperationsCompletedPagerHeight - 65) + "px";
                }
            }
            window.onresize = window.onload = resize;
            function CloseUrlModelWindow(gridid) {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
                if (gridid != null) {
                    var masterTable = $find(gridid).get_masterTableView();
                    masterTable.rebind();
                }
            }   
            function refresh() {
                var ajxmgr = parent.frames[1].$find("RadAjaxManager1");
                if (ajxmgr != null)
                    ajxmgr.ajaxRequest("OPERATION");
            }
            function expandcollapse(gridid) {
                top.operationexpandcollapse = gridid;
                var $ = $telerik.$;
                var height = $(window).height();
                var btab = document.querySelector('#MenuMaintCompleted_dlstTabs');
                var ctab = document.querySelector('#MenuOperationsCompleted_dlstTabs');

                var gvMaintCompleted = $find("<%= gvMaintCompleted.ClientID %>");
                var gvOperationsCompleted = $find("<%= gvOperationsCompleted.ClientID %>");
                DWPgrid = $find(gridid);
                var collapse = false;
                if (DWPgrid != gvMaintCompleted) {
                    var visible = gvMaintCompleted.get_visible();
                    gvMaintCompleted.set_visible(!visible);
                    btab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (DWPgrid != gvOperationsCompleted) {
                    var visible = gvOperationsCompleted.get_visible();
                    gvOperationsCompleted.set_visible(!visible);
                    ctab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (!collapse) {
                    DWPgrid = null;
                    top.operationexpandcollapse = null;
                }
                resize();                
            }
            function pageLoad() {
                if (top.operationexpandcollapse != null) {
                    expandcollapse(top.operationexpandcollapse);
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvProgress">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvMaintCompleted" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvOperationsCompleted" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuMaintCompleted"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuOperationsCompleted"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>                         
                    </UpdatedControls>
                </telerik:AjaxSetting>                
                <telerik:AjaxSetting AjaxControlID="gvCompleted">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvMaintCompleted" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvOperationsCompleted" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuMaintCompleted"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuOperationsCompleted"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>                        
                    </UpdatedControls>
                </telerik:AjaxSetting>                               
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />  
        <br />
        <table cellspacing="0" cellpadding="0">
            <tr>
                <td>
                </td>
                <td>
                    <telerik:RadLabel ID="lblActivityDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>
                    <button type="button" runat="server" onserverclick="Prev_ServerClick"  title="Prev" style="width:25px; border-radius:4px; background-image:linear-gradient(#f4f8fa,#e9f2fb 50%,#dde7f5 50%,#dde8f6)" >
                        <span class="icon"><i class="fas fa-caret-left"></i></span>                        
                    </button>
                </td>
                <td>
                    <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input" Height="23px" ID="txtStartDate" Width="100px" MaxLength="12" AutoPostBack="true" OnSelectedDateChanged="txtStartDate_TextChangedEvent">
                        <DateInput DateFormat="dd-MM-yyyy" runat="server"></DateInput>
                    </telerik:RadDatePicker>
                </td>
                <td>
                    <button type="submit" runat="server" onserverclick="Next_ServerClick" title="Next" style="width:25px; border-radius:4px; background-image:linear-gradient(#f4f8fa,#e9f2fb 50%,#dde7f5 50%,#dde8f6)" >
                         <span class="icon"><i class="fas fa-caret-right"></i></span>     
                    </button>
                </td>
            </tr>
        </table>  
        <br />          
        <eluc:TabStrip ID="MenuOperationsCompleted" runat="server" OnTabStripCommand="MenuOperationsCompleted_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvOperationsCompleted" runat="server" OnItemDataBound="gvOperationsCompleted_ItemDataBound"
            OnNeedDataSource="gvOperationsCompleted_NeedDataSource" OnItemCommand="gvOperationsCompleted_ItemCommand"  GroupingEnabled="false" AllowFilteringByColumn="true"
            ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" EnableLinqExpressions="false">
            <MasterTableView AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDDAILYPLANACTIVITYID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Process" AllowFiltering="true" UniqueName="FLDELEMENTNAME"
                        FilterControlWidth="98%" FilterDelay="2000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDELEMENTNAME"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Activity" AllowSorting="false" AllowFiltering="true" UniqueName="FLDACTIVITYNAME"
                       FilterControlWidth="98%" FilterDelay="2000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDACTIVITYNAME"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Est. Start Time" UniqueName="FLDESTSTARTTIME" AllowFiltering="false">                       
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDESTSTARTTIME"].ToString())%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                   <telerik:GridTemplateColumn HeaderText="Est. End Time" AllowFiltering="false">
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDDURATION"].ToString())%>
                        </ItemTemplate>                                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Start Time" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                           <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDSTARTTIME"], DateDisplayOption.TimeHR24).Replace(":", "")%>                     
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="End Time" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                            <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCOMPLETEDTIME"], DateDisplayOption.TimeHR24).Replace(":", "")%>
                        </ItemTemplate>                                       
                    </telerik:GridTemplateColumn>                    
                    <telerik:GridTemplateColumn HeaderText="Team Members" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDOTHERMEMBERSNAME"].ToString().Trim(',')%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>  
                     <telerik:GridTemplateColumn HeaderText="RA" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRA" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDRANUMBER"]%>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Forms & Checklists" UniqueName="FLDFORMLIST" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" Visible="false" HeaderText="Action">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDHARDNAME"]%>
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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
        <eluc:TabStrip ID="MenuMaintCompleted" runat="server" OnTabStripCommand="MenuMaintCompleted_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvMaintCompleted" runat="server" OnItemDataBound="gvMaintCompleted_ItemDataBound" OnItemCommand="gvMaintCompleted_ItemCommand"
            OnNeedDataSource="gvMaintCompleted_NeedDataSource" GroupingEnabled="false" EnableLinqExpressions="false"
            ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" AllowFilteringByColumn="true">
            <MasterTableView AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDWODETAILID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Work Order No." AllowSorting="false" AllowFiltering="true" UniqueName="FLDWORKORDERNUMBER"
                       FilterControlWidth="98%" FilterDelay="2000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemTemplate>
                            <asp:LinkButton ID="lblWO" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>'></asp:LinkButton>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Work Order" AllowSorting="false" AllowFiltering="true" UniqueName="FLDWORKORDERNAME"
                       FilterControlWidth="98%" FilterDelay="2000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Dept" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDDEPT"]%>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Est. Start Time" UniqueName="FLDESTSTARTTIME" AllowFiltering="false">                       
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDESTSTARTTIME"].ToString())%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                   <telerik:GridTemplateColumn HeaderText="Est. End Time" AllowFiltering="false">
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDDURATION"].ToString())%>
                        </ItemTemplate>                                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Start Time" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                           <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDSTARTTIME"], DateDisplayOption.TimeHR24).Replace(":", "")%>                     
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="End Time" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                            <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCOMPLETEDTIME"], DateDisplayOption.TimeHR24).Replace(":", "")%>
                        </ItemTemplate>                                       
                    </telerik:GridTemplateColumn>                    
                    <telerik:GridTemplateColumn HeaderText="Team Members" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDOTHERMEMBERSNAME"].ToString().Trim(',')%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" AllowFiltering="false">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDHARDNAME"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>                          
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" Visible="false">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Complete" CommandName="COMPLETE" ID="cmdComplete" ToolTip="Complete">
                                    <span class="icon"><i class="fas fa-check"></i></span>
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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all" DecorationZoneID="modalPopup"></telerik:RadFormDecorator>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="600px" Height="365px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
             VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false">
            <ContentTemplate>         
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1">
                    <table border="0" id="tblActivityAdd" style="width: 100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblElement" runat="server" Text="Process"></telerik:RadLabel>
                            </td>
                            <td>
                               <telerik:RadCheckBoxList runat="server" ID="cblElement" Columns="2" Layout="Flow" AutoPostBack="true" 
                                   OnSelectedIndexChanged="cblElement_SelectedIndexChanged" CausesValidation="false">
                               </telerik:RadCheckBoxList>                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity"></telerik:RadLabel>
                            </td>
                            <td>
                               <telerik:RadCheckBoxList runat="server" ID="cblActivity" Columns="2" Layout="Flow"
                                   DataBindings-DataTextField="FLDNAME" DataBindings-DataValueField="FLDACTIVITYID" AppendDataBoundItems="true">
                               </telerik:RadCheckBoxList>
                                <asp:RequiredFieldValidator ID="ActivityValidator" runat="server"
                                    ControlToValidate="cblActivity"
                                    Display="None"
                                    ValidationGroup="group1"
                                    ErrorMessage="select atleast one activity" ></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">                                
                                <telerik:RadButton ID="btnCreate" Text="Create" runat="server" OnClick="btnCreate_Click" ValidationGroup="group1"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                 <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="200px" CssClass="rfdValidationSummaryControl alignleft"
                                        BorderWidth="1px" HeaderText="List of errors" ValidationGroup="group1"></asp:ValidationSummary>
                            </td>
                        </tr>
                    </table>                    
                </telerik:RadAjaxPanel> 
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="900px" Height="365px"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false">
        </telerik:RadWindow>
         <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>

