<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobChangeRequest.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceComponentJobChangeRequest" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Change Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadGrid RenderMode="Lightweight" ID="gvComponentJob" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvComponentJob_NeedDataSource"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDREQUESTID">
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
                        <telerik:GridTemplateColumn HeaderText="Component No." SortExpression="FLDCOMPONENTNUMBER">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentNO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name" AllowSorting="true" SortExpression="FLDCOMPONENTNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Code" AllowSorting="true" SortExpression="FLDJOBCODE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFunctionCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Title" AllowSorting="true" SortExpression="FLDJOBTITLE">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></asp:LinkButton>&nbsp;     
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency" AllowSorting="true" SortExpression="FLDFREQUENCYNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Resp Discipline" AllowSorting="true" SortExpression="FLDDISCIPLINENAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="priority" AllowSorting="true" SortExpression="FLDPRIORITY" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Request Type" AllowSorting="true" SortExpression="FLDREQTYPENAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReqType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReqTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="priority" AllowSorting="true" SortExpression="FLDPRIORITY" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     <%--   <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Approve"
                                    CommandName="Approve" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>' ID="cmdApprove"
                                    ToolTip="Approve" Width="20PX" Height="20PX">
                                                <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="false"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnablePostBackOnRowClick="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>


