<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDailyWorkPlanList.aspx.cs" Inherits="InspectionDailyWorkPlanList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Deficiency List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDeficiency.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDeficiency" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadButton ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
        <eluc:Status ID="ucStatus" runat="server" />

        <br />
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:VesselByCompany ID="ucVessel" runat="server" AppendDataBoundItems="true"  VesselsOnly="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Fleet ID="ucFleet" runat="server" AppendDataBoundItems="true"  />
                </td>
            </tr>
        </table>
        <%-- <table width="100%">
                    <tr>
                        <td>
                            <font color="blue" size="0"><b>Notes:</b>
                                <li>1. To create a new NC, click on '+' button above the scroll list.</li>
                                <li>2. Click on the reference number to navigate to 'Non conformity' screen.</li>
                                <li>3. Record the causes, corrective, preventive actions in 'CAR' screen.</li>
                                <li>4. To complete recording, click on 'Complete' button in the action column.</li>
                                <li>5. To verify NC, give the verification details and click on 'Verify' button in 'CAR' screen.</li>
                                <li>6. To close the NC, click on 'Close' button in the action column.</li>
                            </font>
                        </td>
                    </tr>
                </table>--%>
     
            <eluc:TabStrip ID="MenuDeficiency" runat="server" OnTabStripCommand="Deficiency_TabStripCommand"></eluc:TabStrip>
<%--       
        <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">--%>
            <%-- <asp:GridView ID="gvDeficiency" runat="server" AutoGenerateColumns="False" OnRowCommand="gvDeficiency_RowCommand"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvDeficiency_ItemDataBound"
                    OnRowDeleting="gvDeficiency_RowDeleting" ShowHeader="true" EnableViewState="false"
                    OnSelectedIndexChanging="gvDeficiency_SelectIndexChanging" AllowSorting="true"
                    OnSorting="gvDeficiency_Sorting" DataKeyNames="FLDWORKPLANID"
                    OnRowEditing="gvDeficiency_RowEditing">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDeficiency" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                GroupingEnabled="false" EnableHeaderContextMenu="true"  
                OnItemDataBound="gvDeficiency_ItemDataBound1"
                OnNeedDataSource="gvDeficiency_NeedDataSource"
                OnItemCommand="gvDeficiency_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKPLANID" TableLayout="Fixed" Width="100%">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLANID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>' Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Daily Work Plan"
                                    CommandName="WORKPLAN" CommandArgument="<%# Container.DataSetIndex %>" ID="imgWorkPlan"
                                    ToolTip="Daily Work Plan">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Copy"
                                    CommandName="COPY" CommandArgument="<%# Container.DataSetIndex %>" ID="imgCopy"
                                    ToolTip="Copy">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
<%--        </div>--%>
            </telerik:RadAjaxPanel>


    </form>
</body>
</html>
