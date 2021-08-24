<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceSubWorkOrderList.aspx.cs"
    Inherits="PlannedMaintenanceSubWorkOrderList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manual" Src="~/UserControls/UserControlPMSManual.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {

                var $ = $telerik.$;
                var height = $(window).height();
                var gvWorkOrder = $find("<%= gvWorkOrder.ClientID %>");
                var gvPartsSummary = $find("<%= gvPartsSummary.ClientID %>");

                var gvWorkOrderPagerHeight = (gvWorkOrder.PagerControl) ? gvWorkOrder.PagerControl.offsetHeight : 0;
                var gvPartsSummaryPagerHeight = (gvPartsSummary.PagerControl) ? gvPartsSummary.PagerControl.offsetHeight : 0;
                if (gvWorkOrderPagerHeight.GridDataDiv != null) {
                    gvWorkOrderPagerHeight.GridDataDiv.style.height = (Math.round(height / 3) - gvWorkOrderPagerHeight - 19) + "px";
                    gvPartsSummaryPagerHeight.GridDataDiv.style.height = (Math.round(height / 3) - gvPartsSummaryPagerHeight - 19) + "px";
                }
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPartsSummary.ClientID %>"));
                 }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
            function CloseUrlModelWindow() {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
                var masterTable = $find('<%=gvWorkOrder.ClientID %>').get_masterTableView();
                masterTable.rebind();
            }
            function refreshPart() {               
                var masterTable = $find('<%=gvPartsSummary.ClientID %>').get_masterTableView();
                masterTable.rebind();
            }
        </script>
    </telerik:RadCodeBlock>

    <style type="text/css">
        #divWorkorder .RadLabel_Windows7 {
            color: white !important;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Position="BottomCenter"
            Animation="Fade" AutoTooltipify="true" Width="300px" Font-Size="Large" RenderInPageRoot="true" AutoCloseDelay="80000">
            <TargetControls >
                
            </TargetControls>
        </telerik:RadToolTipManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadNotification ID="RadNotification1" RenderMode="Lightweight" runat="server" AutoCloseDelay="2000" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"
                EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="Center" Font-Bold="true" Font-Size="X-Large" Animation="Fade" BackColor="#99ccff" ShowTitleMenu="false">
            </telerik:RadNotification>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuSubWorkOrder" runat="server" OnTabStripCommand="MenuSubWorkOrder_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
            <table id="divWorkorder" style="width: 100%; border: 1px; background-color: #1c84c6; padding: 5px; border-color: white; border-bottom-style: solid">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblworkorderNo" Text="Work order No :" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" Text="Category :" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlanDate" Text="Plan Date :" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDuration" Text="Duration :" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblResponsible" Text="Responsibility :" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRoutine" Text="Routine :" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" Text="Status :" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                OnItemCommand="gvWorkOrder_ItemCommand1" EnableViewState="false" Height="60%" OnSortCommand="gvWorkOrder_SortCommand"
                OnItemDataBound="gvWorkOrder_ItemDataBound1" EnableHeaderContextMenu="true" OnPreRender="gvWorkOrder_PreRender">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID">
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="RA" Name="RA">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="PTW" Name="PTW">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Spares" Name="SPARES">
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridColumnGroup>                    
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="73px" HeaderText="Comp. No." AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERDETAILID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkorderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompjobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="110px" HeaderText="Comp.Name" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME" AllowFiltering="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCompName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="111px" HeaderText="Job No." AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDWORKORDERNUMBER" AllowFiltering="false" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="172px" HeaderText="Job Code and Title" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDWORKORDERNAME" DataField="FLDWORKORDERNAME" ShowFilterIcon="false" FilterDelay="200">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="Select"
                                    Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Interval">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Priority" UniqueName="FLDPLANINGPRIORITY" AllowSorting="false">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANINGPRIORITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Instruction" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="86px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJobDetaiil" runat="server" Text="Show" CommandName="JOBDETAILS" ToolTip="Click for job details">
                                    <span class="icon"><i class="fas fa-glasses"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date" HeaderStyle-Width="76px" AllowSorting="true" ShowFilterIcon="false"
                            ShowSortIcon="true" SortExpression="FLDPLANNINGDUEDATE" DataField="FLDPLANNINGDUEDATE" FilterDelay="200">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reqrd" HeaderStyle-HorizontalAlign="Center" 
                            HeaderStyle-Width="120px" UniqueName="FLDRAID" DataField="FLDRAID" ColumnGroupName="RA">
                            <ItemStyle HorizontalAlign="Center" Wrap="true" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRaId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAID") %>' Visible="false"></telerik:RadLabel>
                                <%--<asp:LinkButton ID="lnkRiskView" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANUMBER") %>' CommandName="RAVIEW" Visible="false"></asp:LinkButton>
                                <asp:Label ID="lblSeparator1" runat="server" Text="/" Visible="false"></asp:Label>--%>
                                <asp:LinkButton ID="lnkRaEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANUMBER") %>' CommandName="RAEDIT" Visible="false"></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="RA Delete" ID="cmdRADelete" CommandName="RADELETE" ToolTip="RA Delete" Width="20px" Height="20px" Visible="false">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <%--<asp:Label ID="lblSeparator2" runat="server" Text="/" Visible="false"></asp:Label>--%>
                                <asp:LinkButton ID="lnkRiskCreate" runat="server" Text="Required" CommandName="RACREATE" Visible="false"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Complied" UniqueName="RAMET" AllowFiltering="false" AllowSorting="false" ColumnGroupName="RA" HeaderStyle-Width="60px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDISRAMET")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="JHA" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkJHA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHACOUNT") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reqrd" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="PTW">
                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPTW" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPTWCOUNT") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Complied" UniqueName="PTWMET" AllowFiltering="false" AllowSorting="false"  ColumnGroupName="PTW" HeaderStyle-Width="60px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDISPTWMET")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Manuals / Docs">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <eluc:Manual ID="ucManual" runat="server" ComponentJobId='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reporting Templates" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="83px">
                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTemplates" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPLATESMAPPED")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Atch. Reqd" UniqueName="FLDATTACHMENTREQYN">
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle Width="40px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkAttachmentReqd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTREQYN")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reqrd" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="SPARES">
                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkParts" runat="server" Text="Show"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Complied" UniqueName="SPARESMET" AllowFiltering="false" AllowSorting="false" ColumnGroupName="SPARES" HeaderStyle-Width="60px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDISSPARESMET")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>  
                        <%--                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Reqrd" UniqueName="RAREQ" AllowFiltering="false" AllowSorting="false" Visible="false">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDISRAREQ")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                   
                    <telerik:GridTemplateColumn HeaderText="Reqrd" UniqueName="PTWREQ" AllowFiltering="false" AllowSorting="false" Visible="false">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                        <ItemTemplate>
                           <%# DataBinder.Eval(Container,"DataItem.FLDISPTWREQ")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    

                    <telerik:GridTemplateColumn HeaderText="Reqrd" UniqueName="SPARESREQ" AllowFiltering="false" AllowSorting="false"  Visible="false">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                        <ItemTemplate>
                           <%# DataBinder.Eval(Container,"DataItem.FLDISSPARESREQ")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Parameter" HeaderStyle-Width="65px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkParam" runat="server" Text="Show"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                                           
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:ImageButton ID="lnkPtwWaive" runat="server" ImageUrl="<%$ PhoenixTheme:images/45.png %>" ToolTip="Waive" CommandName="WAIVE" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="160px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br clear="all" />
            <eluc:TabStrip ID="MenuRA" runat="server" OnTabStripCommand="MenuRA_TabStripCommand" TabStrip="false" Title="Required Parts Summary" />
            <telerik:RadGrid ID="gvPartsSummary" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvPartsSummary_NeedDataSource"
                EnableHeaderContextMenu="true" AllowMultiRowSelection="true" OnItemDataBound="gvPartsSummary_ItemDataBound" OnItemCommand="gvPartsSummary_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSPAREITEMID" ClientDataKeyNames="FLDSPAREITEMID,FLDVESSELID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="Number">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblspareItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Name" DataField="FLDNAME"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Requisition" UniqueName="FLDFORMNO">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkFormNo" runat="server" CommandName="REQUISITION" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="ROB" DataField="FLDTOTALQUANTITY" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Required" DataField="FLDREQUIREDQTY" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Shortage" HeaderStyle-Width="50px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblshortageQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="99%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="80px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>--%>
        </telerik:RadAjaxPanel>
        <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="900px" Height="365px" Behaviors="Close,Maximize,Minimize,Move,Resize"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=RadWindow_NavigateUrl.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
