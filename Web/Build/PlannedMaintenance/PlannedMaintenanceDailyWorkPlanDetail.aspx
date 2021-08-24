<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDailyWorkPlanDetail.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDailyWorkPlanDetail"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Element" Src="~/UserControls/UserControlRACategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CrewList" Src="~/UserControls/UserControlCrewList.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Daily Work Plan</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            var DWPgrid = null;            
            function CloseUrlModelWindow(gridid) {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
                if (gridid != null) {
                    var masterTable = $find(gridid).get_masterTableView();
                    masterTable.rebind();
                }
                if (gridid != null && (gridid == "gvActivity" || gridid == "gvActivityChart")) {
                    document.getElementById('cmdHiddenSubmitActivity').click();
                }
                if (gridid != null && gridid.includes("gvMPart")) {
                    document.getElementById('cmdHiddenSubmitWO').click();
                }
                closeTelerikWindow(null, 'dsd', null);
                var wnd = getRadWindow('dp');
                setRadWindowZIndex(wnd);
                wnd.setActive(true);
            }
            function refreshScheduler() {
                var wnd = getRadWindow('dsd');
                var button = wnd.GetContentFrame().contentWindow.document.getElementById("cmdHiddenSubmit");
                if (button != null)
                    button.click();
                wnd = getRadWindow('dp');
                setRadWindowZIndex(wnd);
                wnd.setActive(true);
            }
            function resize() {
                var $ = $telerik.$;
                var height = $(window).height();
                if (DWPgrid != null && DWPgrid.GridDataDiv != null) {
                    var gridPagerHeight = (DWPgrid.PagerControl) ? DWPgrid.PagerControl.offsetHeight : 0;
                    DWPgrid.GridDataDiv.style.height = (height - gridPagerHeight - 240) + "px";
                } else {
                    var gvPlanned = $find("<%= gvActivity.ClientID %>");
                    var gvProgress = $find("<%= gvMPartB.ClientID %>");
                    var gvCompleted = $find("<%= gvMPartC.ClientID %>");

                    var gvPlannedPagerHeight = (gvPlanned.PagerControl) ? gvPlanned.PagerControl.offsetHeight : 0;
                    var gvProgressPagerHeight = (gvProgress.PagerControl) ? gvProgress.PagerControl.offsetHeight : 0;
                    var gvCompletedPagerHeight = (gvCompleted.PagerControl) ? gvCompleted.PagerControl.offsetHeight : 0;

                    gvPlanned.GridDataDiv.style.height = (Math.round(height / 3) - gvPlannedPagerHeight - 118) + "px";
                    gvProgress.GridDataDiv.style.height = (Math.round(height / 3) - gvProgressPagerHeight - 118) + "px";
                    gvCompleted.GridDataDiv.style.height = (Math.round(height / 3) - gvCompletedPagerHeight - 118) + "px";

                }

            }
            function expandcollapse(gridid) {
                top.dwpexpandcollapse = gridid;
                var $ = $telerik.$;
                var height = $(window).height();
                var atab = document.querySelector('#MenuPartA_dlstTabs');
                var btab = document.querySelector('#MenuPartB_dlstTabs');
                var ctab = document.querySelector('#MenuPartC_dlstTabs');

                var gvPlanned = $find("<%= gvActivity.ClientID %>");
                var gvProgress = $find("<%= gvMPartB.ClientID %>");
                var gvCompleted = $find("<%= gvMPartC.ClientID %>");
                DWPgrid = $find(gridid);
                var collapse = false;
                if (DWPgrid != gvPlanned) {
                    var visible = gvPlanned.get_visible();
                    gvPlanned.set_visible(!visible);
                    atab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (DWPgrid != gvProgress) {
                    var visible = gvProgress.get_visible();
                    gvProgress.set_visible(!visible);
                    btab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (DWPgrid != gvCompleted) {
                    var visible = gvCompleted.get_visible();
                    gvCompleted.set_visible(!visible);
                    ctab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }                
                if (!collapse) {
                    DWPgrid = null;
                    top.dwpexpandcollapse = null;
                }
                resize();
            }
            window.onresize = window.onload = resize;
            function refreshDashboard() {
                var ajxmgr = parent.frames[1].$find("RadAjaxManager1");
                if (ajxmgr != null)
                    ajxmgr.ajaxRequest("OPERATION");
            }     
            function onClientTabSelecting(sender, args) {
                var tab = args.get_tab();
                var copy = document.querySelector("#MenuMain_dlstTabs ul li span[title=Copy]");      
                copy.style.display = "none";
                if (tab.get_index() == "0") {
                    copy.style.display = "block";
                }                
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .bg-success {
            background-color: #1c84c6;
            color: #ffffff;
            padding-left: 15PX;
        }

        .overflow {
            overflow-y: scroll;
        }

        .alignleft {
            text-align: left;
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
                <telerik:AjaxSetting AjaxControlID="gvActivity">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvActivity" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartB" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartC" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvMPartB">
                    <UpdatedControls>
                       <telerik:AjaxUpdatedControl ControlID="gvActivity" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartB" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartC" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvMPartC">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvActivity" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartB" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartC" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cmdHiddenSubmit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartB" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartC" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvActivity" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvActivityChart" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="95%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvCrewWorkHrs" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="95%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvCrewWorkHrs">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvCrewWorkHrs" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="95%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuCrewHrsReset">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuCrewHrsReset"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvCrewWorkHrs" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="95%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucNotification" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvActivityChart">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvActivityChart" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="95%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvActivity" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartB" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartC" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvCrewWorkHrs" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="95%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cmdHiddenSubmitActivity">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvActivity" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvActivityChart" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvCrewWorkHrs" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cmdHiddenSubmitWO">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartB" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMPartC" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvActivityChart" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%-- <telerik:AjaxSetting AjaxControlID="MenuMain">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuMain" />
                        <telerik:AjaxUpdatedControl ControlID="gvWorkOrder" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Position="BottomCenter"
            Animation="Fade" AutoTooltipify="false" Width="300px" RenderInPageRoot="true" AutoCloseDelay="8000">
            <TargetControls >
            </TargetControls>
        </telerik:RadToolTipManager>
        <div class="bg-success">
            <br />
            <h3>
                <asp:Literal ID="lblHeading" runat="server"></asp:Literal></h3>
            <br />
        </div>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
         <telerik:RadNotification ID="ucNotification" RenderMode="Lightweight" runat="server" AutoCloseDelay="8500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false"></telerik:RadNotification>
        <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MainMenu_TabStripCommand"></eluc:TabStrip>
        <telerik:RadTabStrip runat="server" ID="RadTabStrip1" MultiPageID="RadMultiPage1" SelectedIndex="0" OnClientTabSelecting="onClientTabSelecting">
            <Tabs>
                <telerik:RadTab Text="Work Plan" Width="200px"></telerik:RadTab>
                <telerik:RadTab Text="Activities Chart" Width="200px"></telerik:RadTab>
                <telerik:RadTab Text="Crew Work Hours" Width="200px"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
            <telerik:RadPageView runat="server" ID="RadPageView1">

                <eluc:TabStrip ID="MenuPartA" runat="server" OnTabStripCommand="MenuPartA_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid ID="gvActivity" runat="server" MasterTableView-ShowFooter="false" OnItemDataBound="gvActivity_ItemDataBound"
                    OnNeedDataSource="gvActivity_NeedDataSource" OnItemCommand="gvActivity_ItemCommand" OnSortCommand="gvActivity_SortCommand"
                    ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
                    <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDDAILYPLANACTIVITYID">
                        <Columns>
                             <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                <ItemTemplate>                                    
                                    <img id="imgActivity" runat="server" height="16" width="16"/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Process" UniqueName="FLDELEMENTNAME">
                                <HeaderStyle Width="150px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                                <ItemTemplate>                                    
                                    <telerik:RadLabel ID="lblElement" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDELEMENTNAME"]%>' ToolTip='<%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"])%>'></telerik:RadLabel>
                           <telerik:RadLabel ID="lbladid" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDAILYWORKPLANID"]%>' Visible="false"></telerik:RadLabel>
                           <telerik:RadLabel ID="lbladdate" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDATE"]%>' Visible="false"></telerik:RadLabel>

                                     </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Activity" UniqueName="FLDACTIVITYNAME">
                                <HeaderStyle Width="150px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActivity" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDACTIVITYID"]%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblActivityName" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDACTIVITYNAME"]%>'></telerik:RadLabel> <br /> Due on <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"])%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est Start Time">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#PadZero(((DataRowView)Container.DataItem)["FLDESTSTARTTIME"].ToString())%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlEstStartTime"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="End Time">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#PadZero(((DataRowView)Container.DataItem)["FLDDURATION"].ToString())%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlDuration"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PIC" Visible="false">
                                <HeaderStyle Width="0px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="0px" Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGENAME"]%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:CrewList ID="ddlPersonIncharge" runat="server" CheckBoxes="false" VesselId='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'
                                        Date='<%#ViewState["DATE"].ToString() %>' SelectedCrew='<%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGEID"]%>'
                                        Text='<%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGENAME"]%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Team Members">
                                <ItemTemplate>
                                    <telerik:RadLabel runat="server" ID="lblOtherMembers" RenderMode="Lightweight" Text='<%#((DataRowView)Container.DataItem)["FLDOTHERMEMBERSNAME"] %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="ddlCrewList" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID"
                                        EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                    </telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="RA">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRA" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDRANUMBER"]%>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Forms & Checklists" UniqueName="FLDFORMLIST">
                                <HeaderStyle Width="250px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="250px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status" UniqueName="FLDSTATUS">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <HeaderStyle />
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITR" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Postpone" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Re-schedule">
                                        <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPY" ID="cmdCopy" ToolTip="Copy">
                                        <span class="icon"><i class="fas fa-copy"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </FooterTemplate>
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                <asp:Label runat="server" Text="<html><font size=3 color=red>!</font> NC in current day <html>" ID="lblGuidenceText"></asp:Label>
                
                <br />
                <eluc:TabStrip ID="MenuPartB" runat="server" OnTabStripCommand="MenuPartB_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid ID="gvMPartB" runat="server" MasterTableView-ShowFooter="false" OnItemCreated="gvMPartB_ItemDataBound"
                    OnNeedDataSource="gvMPartB_NeedDataSource" OnItemCommand="gvMPartB_ItemCommand" OnSortCommand="gvMPartB_SortCommand"
                    ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
                    <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDWODETAILID,FLDWOGROUPID">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                <ItemTemplate>                                    
                                    <span runat="server" id="spnyellow" class="icon"><i class="fas fa-star-yellow"></i></span>
                                    <span runat="server" id="spnred" class="icon"><i class="fas fa-star-red"></i></span>
                                    <img id="imgPMS" runat="server" height="16" width="16"/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Order No.">
                                <HeaderStyle Width="90px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="90px" Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblWO" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>'></asp:LinkButton>     
                                     <telerik:RadLabel ID="lblWdid" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDAILYWORKPLANID"]%>' Visible="false"></telerik:RadLabel>                               
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Order">
                                <HeaderStyle Width="300px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="300px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWorkOrder" runat="server"></telerik:RadLabel>                                    
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Plan Date">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPlanDate" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDPLANDATE"])%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est Start Time">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#PadZero(((DataRowView)Container.DataItem)["FLDESTSTARTTIME"].ToString())%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlEstStartTime"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="End Time">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#PadZero(((DataRowView)Container.DataItem)["FLDDURATION"].ToString())%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlDuration"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PIC" Visible="false">
                                <HeaderStyle Width="0px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="0px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGENAME"]%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:CrewList ID="ddlPersonIncharge" runat="server" CheckBoxes="false" VesselId='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'
                                        Date='<%#ViewState["DATE"].ToString() %>' SelectedCrew='<%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGEID"]%>'
                                        Text='<%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGENAME"]%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Team Members">
                                <ItemTemplate>
                                    <telerik:RadLabel runat="server" ID="lblOtherMembers" RenderMode="Lightweight" >
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="ddlCrewList" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID"
                                        EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                    </telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status" UniqueName="FLDSTATUS">
                                <HeaderStyle Width="100px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                <HeaderStyle />
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITR" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Reschedule" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Re-schedule">
                                        <span class="icon"><i class="far fa-calendar-alt"></i></span>
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                <br />
                <eluc:TabStrip ID="MenuPartC" runat="server" OnTabStripCommand="MenuPartC_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid ID="gvMPartC" runat="server" MasterTableView-ShowFooter="false" OnItemCreated="gvMPartB_ItemDataBound"
                    OnNeedDataSource="gvMPartC_NeedDataSource" OnItemCommand="gvMPartB_ItemCommand" OnSortCommand="gvMPartC_SortCommand"
                    ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
                    <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDWODETAILID,FLDWOGROUPID">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                                <ItemTemplate>                                    
                                    <span runat="server" id="spnyellow" class="icon"><i class="fas fa-star-yellow"></i></span>
                                    <span runat="server" id="spnred" class="icon"><i class="fas fa-star-red"></i></span>
                                    <img id="imgPMS" runat="server" height="16" width="16"/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Order No.">
                                <HeaderStyle Width="90px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="90px" Wrap="false" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblWO" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>'></asp:LinkButton>
                              <telerik:RadLabel ID="lblWdid" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDAILYWORKPLANID"]%>' Visible="false"></telerik:RadLabel>                               

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Order">
                                <HeaderStyle Width="300px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="300px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWorkOrder" runat="server"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Plan Date">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPlanDate" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDPLANDATE"])%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est Start Time">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#PadZero(((DataRowView)Container.DataItem)["FLDESTSTARTTIME"].ToString())%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlEstStartTime"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est. End Time">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#PadZero(((DataRowView)Container.DataItem)["FLDDURATION"].ToString())%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlDuration"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PIC" Visible="false">
                                <HeaderStyle Width="0px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="0px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGENAME"]%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:CrewList ID="ddlPersonIncharge" runat="server" CheckBoxes="false" VesselId='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'
                                        Date='<%#ViewState["DATE"].ToString() %>' SelectedCrew='<%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGEID"]%>'
                                        Text='<%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGENAME"]%>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Team Members">
                                <ItemTemplate>
                                    <telerik:RadLabel runat="server" ID="lblOtherMembers" RenderMode="Lightweight" Text='<%#((DataRowView)Container.DataItem)["FLDOTHERMEMBERSNAME"] %>'>
                                    </telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="ddlCrewList" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID"
                                        EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                    </telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status" UniqueName="FLDSTATUS">
                                <HeaderStyle Width="100px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                <HeaderStyle Width="80px" Wrap="false" HorizontalAlign="Left" />
                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                <HeaderStyle />
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITR" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Reschedule" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Re-schedule">
                                        <span class="icon"><i class="far fa-calendar-alt"></i></span>
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPageView>
            <telerik:RadPageView runat="server" ID="RadPageView2">
                <telerik:RadGrid ID="gvActivityChart" runat="server" MasterTableView-ShowFooter="false" Height="100%"
                    OnNeedDataSource="gvActivityChart_NeedDataSource" OnItemDataBound="gvActivityChart_ItemDataBound" OnItemCommand="gvActivityChart_ItemCommand"
                    OnBatchEditCommand="gvActivityChart_BatchEditCommand"
                    ShowFooter="false" AllowCustomPaging="false" AllowPaging="false" EnableHeaderContextMenu="true" MasterTableView-DataKeyNames="FLDDAILYPLANACTIVITYID,FLDISOPERATION">
                    <MasterTableView CommandItemDisplay="Top" CommandItemSettings-ShowAddNewRecordButton="false" EditMode="Batch" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true">
                        <BatchEditingSettings EditType="Cell" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Activities Chart" UniqueName="FLDACTIVITY" ReadOnly="true">
                                <HeaderStyle Width="15%" />
                                <ItemStyle Width="15%" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActivityName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDACTIVITY"].ToString()%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                <HeaderStyle Width="60px" />
                                <ItemStyle Wrap="false" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPY" ID="cmdCopy" ToolTip="Copy">
                                        <span class="icon"><i class="fas fa-copy"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCELACTIVITY" ID="cmdCancelActivity" ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCELWO" ID="cmdCancelWO" ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est Start Time" UniqueName="FLDESTSTARTTIME">
                                <HeaderStyle Width="60px" />
                                <ItemTemplate>
                                    <%# PadZero(((DataRowView)Container.DataItem)["FLDESTSTARTTIME"].ToString())%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlEstStartTime" OnLoad="ddlEstStartTime_Load"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Est End Time" UniqueName="FLDDURATION">
                                <HeaderStyle Width="60px" />
                                <ItemTemplate>
                                    <%#PadZero(((DataRowView)Container.DataItem)["FLDDURATION"].ToString())%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox runat="server" ID="ddlDuration" OnLoad="ddlEstStartTime_Load"></telerik:RadComboBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="00-01" UniqueName="FLD01" DataField="FLD01" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="01-02" UniqueName="FLD02" DataField="FLD02" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="02-03" UniqueName="FLD03" DataField="FLD03" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="03-04" UniqueName="FLD04" DataField="FLD04" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="04-05" UniqueName="FLD05" DataField="FLD05" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="05-06" UniqueName="FLD06" DataField="FLD06" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="06-07" UniqueName="FLD07" DataField="FLD07" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="07-08" UniqueName="FLD08" DataField="FLD08" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="08-09" UniqueName="FLD09" DataField="FLD09" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="09-10" UniqueName="FLD10" DataField="FLD10" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="10-11" UniqueName="FLD11" DataField="FLD11" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="11-12" UniqueName="FLD12" DataField="FLD12" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="12-13" UniqueName="FLD13" DataField="FLD13" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="13-14" UniqueName="FLD14" DataField="FLD14" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="14-15" UniqueName="FLD15" DataField="FLD15" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="15-16" UniqueName="FLD16" DataField="FLD16" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="16-17" UniqueName="FLD17" DataField="FLD17" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="17-18" UniqueName="FLD18" DataField="FLD18" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="18-19" UniqueName="FLD19" DataField="FLD19" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="19-20" UniqueName="FLD20" DataField="FLD20" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="20-21" UniqueName="FLD21" DataField="FLD21" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="21-22" UniqueName="FLD22" DataField="FLD22" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="22-23" UniqueName="FLD23" DataField="FLD23" ReadOnly="true"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="23-24" UniqueName="FLD24" DataField="FLD24" ReadOnly="true"></telerik:GridBoundColumn>

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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPageView>
            <telerik:RadPageView runat="server" ID="RadPageView3">
                <eluc:TabStrip ID="MenuCrewHrsReset" runat="server" OnTabStripCommand="MenuCrewHrsReset_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid ID="gvCrewWorkHrs" runat="server" MasterTableView-ShowFooter="false" Height="100%"
                    OnNeedDataSource="gvCrewWorkHrs_NeedDataSource" OnItemDataBound="gvCrewWorkHrs_ItemDataBound" OnItemCommand="gvCrewWorkHrs_ItemCommand"
                    ShowFooter="false" AllowCustomPaging="false" AllowPaging="false" EnableHeaderContextMenu="true">
                    <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDEMPLOYEEID,FLDSHIPCALENDARID,FLDREPORTINGDAY,FLDEMPLOYEESIGNONOFFID">
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="Compliment Onboard" UniqueName="FLDEMPLOYEENAME" DataField="FLDEMPLOYEENAME">
                                <HeaderStyle Width="15%" />
                                <ItemStyle Width="15%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn CommandName="RESET" UniqueName="RESET" ButtonType="ImageButton" ImageUrl="~/css/Theme1/images/refresh.png" Text="Reset"></telerik:GridButtonColumn> 
                            <telerik:GridBoundColumn HeaderText="Rank" UniqueName="FLDRANKNAME" DataField="FLDRANKNAME">                               
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="00-01" UniqueName="1" DataField="1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="01-02" UniqueName="2" DataField="2"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="02-03" UniqueName="3" DataField="3"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="03-04" UniqueName="4" DataField="4"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="04-05" UniqueName="5" DataField="5"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="05-06" UniqueName="6" DataField="6"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="06-07" UniqueName="7" DataField="7"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="07-08" UniqueName="8" DataField="8"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="08-09" UniqueName="9" DataField="9"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="09-10" UniqueName="10" DataField="10"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="10-11" UniqueName="11" DataField="11"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="11-12" UniqueName="12" DataField="12"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="12-13" UniqueName="13" DataField="13"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="13-14" UniqueName="14" DataField="14"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="14-15" UniqueName="15" DataField="15"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="15-16" UniqueName="16" DataField="16"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="16-17" UniqueName="17" DataField="17"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="17-18" UniqueName="18" DataField="18"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="18-19" UniqueName="19" DataField="19"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="19-20" UniqueName="20" DataField="20"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="20-21" UniqueName="21" DataField="21"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="21-22" UniqueName="22" DataField="22"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="22-23" UniqueName="23" DataField="23"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="23-24" UniqueName="24" DataField="24"></telerik:GridBoundColumn>

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
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                        <Selecting AllowRowSelect="false" EnableDragToSelectRows="true" CellSelectionMode="MultiColumn" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
      
        <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="900px" Height="465px" OnClientClose="onClose" Behaviors="Close,Maximize,Minimize,Move,Resize"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=RadWindow_NavigateUrl.ClientID %>";
                function pageLoad() {

                    var $ = $telerik.$;
                    var height = $(window).height();

                    var multiPage = $find("<%=RadMultiPage1.ClientID %>");
                    var totalHeight = height - 150;
                    multiPage.get_element().style.height = totalHeight + "px";
                    if (top.dwpexpandcollapse != null) {
                        expandcollapse(top.dwpexpandcollapse);
                    }
                }
            </script>
            <script type="text/javascript">
                function onClose() {
                    document.getElementById("cmdHiddenSubmit").click();
                }
            </script>
        </telerik:RadCodeBlock>
        <asp:Button runat="server" ID="cmdHiddenSubmit" Text="Refresh the grids" CssClass="hidden" OnClick="btnRefresh_Click" />
        <asp:Button runat="server" ID="cmdHiddenSubmitActivity" Text="Refresh the grids" CssClass="hidden" OnClick="cmdHiddenSubmitActivity_Click" />
        <asp:Button runat="server" ID="cmdHiddenSubmitWO" Text="Refresh the grids" CssClass="hidden" OnClick="btnRefresh_Click" />
    </form>
</body>
</html>
