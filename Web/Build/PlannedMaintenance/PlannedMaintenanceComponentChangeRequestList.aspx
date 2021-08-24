<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentChangeRequestList.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceComponentChangeRequestList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Change Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">        
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
            <telerik:RadGrid RenderMode="Lightweight" ID="gvComponent" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" Width="100%"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvComponent_NeedDataSource" DataKeyNames="FLDCOMPONENTID" 
            GroupingEnabled="false" EnableHeaderContextMenu="true">
            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
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
                    <telerik:GridTemplateColumn>
                        <HeaderStyle Width="40px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20px"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Number" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME">
                        <HeaderStyle Width="242px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblParentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Type" Visible="false" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDMISCELLANEOUS1">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMISCELLANEOUS1") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Serial Number" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSERIALNUMBER">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblSerialNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Class Code" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCLASSCODE">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblComponentClassCodeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASSCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Request Type">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReqType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTTYPE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblReqTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQTYPENAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks">
                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="250px"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                 <%--   <telerik:GridTemplateColumn HeaderText="Action">
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>


