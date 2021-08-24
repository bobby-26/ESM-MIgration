<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobsLink.aspx.cs" Inherits="PlannedMaintenanceJobsLink" %>

<!DOCTYPE html>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Jobs List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="MenuWorkOrderRequestion" runat="server" OnTabStripCommand="MenuWorkOrderRequestion_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" AllowFilteringByColumn="true" FilterType="CheckList"
                OnItemCommand="gvWorkOrder_ItemCommand" Height="90%" OnSortCommand="gvWorkOrder_SortCommand" EnableViewState="true" EnableLinqExpressions="false" OnItemDataBound="gvWorkOrder_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID" AllowFilteringByColumn="true">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="4%" AllowSorting="false" AllowFiltering="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true" />
                                <telerik:RadLabel ID="lblalreadyexist" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALREADYEXIST") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Comp. No." DataField="FLDCOMPONENTNUMBER" AutoPostBackOnFilter="false" UniqueName="FLDCOMPONENTNUMBER"
                            AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER" ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterDelay="2000">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderText="Comp. Name" AllowSorting="true" DataField="FLDCOMPONENTNAME" AutoPostBackOnFilter="false"
                            ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME" UniqueName="FLDCOMPONENTNAME" CurrentFilterFunction="Contains"
                            FilterDelay="2000" ShowFilterIcon="false" FilterControlWidth="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTID"]%>'></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderText="Job Code/Title" AllowSorting="true" DataField="FLDWORKORDERNAME" AutoPostBackOnFilter="false"
                            ShowSortIcon="true" SortExpression="FLDWORKORDERNAME" UniqueName="FLDWORKORDERNAME" CurrentFilterFunction="Contains"
                            FilterDelay="2000" ShowFilterIcon="false" FilterControlWidth="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERID"] %>'></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="135px" HeaderText="Job Category" AllowFiltering="false" AllowSorting="true" UniqueName="FLDJOBCATEGORYID">
                            <%--<FilterTemplate>
                                <telerik:RadComboBox ID="cblJobCategory" runat="server" OnDataBinding="cblJobCategory_DataBinding" AutoPostBack="true" Width="100%" AppendDataBoundItems="true"
                                    OnSelectedIndexChanged="cblJobCategory_SelectedIndexChanged" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("FLDJOBCATEGORYID").CurrentFilterValue %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate> --%>

                           <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobCategory" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDJOBCATEGORYID"]%>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container,"DataItem.FLDJOBCATEGORY") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <%-- <telerik:GridTemplateColumn HeaderStyle-Width="135px" HeaderText="Maintenance Interval" AllowSorting="true"
                            AllowFiltering="true" DataField="FLDFREQUENCYNAME" UniqueName="FLDFREQUENCYTYPE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblFrequencyType" runat="server" OnDataBinding="cblFrequencyType_DataBinding" AutoPostBack="true" Width="100%" AppendDataBoundItems="true"
                                    OnSelectedIndexChanged="cblFrequencyType_SelectedIndexChanged" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>--%>

                         <telerik:GridTemplateColumn HeaderText="Maintenance Interval" HeaderStyle-Width="135px" AllowSorting="true" AllowFiltering="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFrequencyType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYTYPE") %>' Visible="false"></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Last Done" HeaderStyle-Width="108px" AllowSorting="true" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" Visible="false" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkMap" runat="server" Text="Map" ToolTip="Link to Defect" CommandName="MAP">
                                    <span class="icon"><i class="fa-user-plus-add"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
    </form>
</body>
</html>
