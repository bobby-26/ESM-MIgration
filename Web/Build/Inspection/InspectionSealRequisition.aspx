<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealRequisition.aspx.cs" Inherits="InspectionSealRequisition" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Requisition</title>
    <telerik:RadCodeBlock runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>


    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight);
            splitter.set_width("100%");
            var grid = $find("gvSealRequisition");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 130) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>

</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmSealReq" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvSealRequisition">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvSealRequisition" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>

        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadButton ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


            <%--<div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>--%>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">

                <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                    <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 500px; width: 100%" frameborder="0"></iframe>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                    <eluc:TabStrip ID="MenuSealReq" runat="server" OnTabStripCommand="MenuSealReq_TabStripCommand"></eluc:TabStrip>
                    <%--  <asp:GridView ID="gvSealRequisition" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvSealRequisition_RowDataBound" ShowHeader="true"
                        OnRowUpdating="gvSealRequisition_RowUpdating" EnableViewState="false" AllowSorting="true"
                        OnSorting="gvSealRequisition_Sorting" OnSelectedIndexChanging="gvSealRequisition_SelectedIndexChanging"
                        DataKeyNames="FLDREQUESTID" OnRowCommand="gvSealRequisition_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>

                    <telerik:RadGrid RenderMode="Lightweight" ID="gvSealRequisition" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None"
                        OnNeedDataSource="gvSealRequisition_NeedDataSource"
                        OnItemCommand="gvSealRequisition_ItemCommand"
                        OnItemDataBound="gvSealRequisition_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDREQUESTID" TableLayout="Fixed" Height="10px">
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
                                <telerik:GridBoundColumn Visible="false" DataField="FLDREQUESTID"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                
                                    <ItemTemplate>

                                        <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Request No">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                               
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="select"><%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Requested Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                    <ItemTemplate>
                                        <%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREQUESTDATE")) %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Request Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatusId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTSTATUSID"]%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTSTATUS"]%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                  
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Confirm"
                                            CommandName="CONFIRM" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdConfirm"
                                            ToolTip="Confirm Request">
                                        <span class="icon"><i class="fas fa-check-circle"></i></span>
                                        </asp:LinkButton>
                                       
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                            CommandName="CANCELREQUISITION" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </div>
    </form>
</body>
</html>
