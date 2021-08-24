<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingMatrixHistoryDetails.aspx.cs" Inherits="CrewOffshoreTrainingMatrixHistoryDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Training Matrix</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
       <style type="text/css">
        .hidden {
            display: none;
        }

        .center {
            background: fixed !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadLabel ID="lblHistoryid" runat="server" Visible="false"></telerik:RadLabel>


            <eluc:TabStrip ID="CrewTrainingMenu" Title="Details of Matrix" runat="server" OnTabStripCommand="CrewTrainingMenu_TabStripCommand"></eluc:TabStrip>

            <%--  <asp:GridView ID="gvTrainingMatrix" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowDataBound="gvTrainingMatrix_RowDataBound" OnPreRender="gvTrainingMatrix_PreRender"
                ShowHeader="true" ShowFooter="false" EnableViewState="false" AllowSorting="true">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTrainingMatrix" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvTrainingMatrix_NeedDataSource"
                OnItemCommand="gvTrainingMatrix_ItemCommand"
                 EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"  GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDGROUPBY" FieldAlias="Details" SortOrder="Ascending" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDGROUPBY" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn Visible="false" UniqueName="Data" DataField="FLDGROUPBY">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDGROUPBY"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px" VerticalAlign="Top"></ItemStyle>
                           <HeaderStyle Width="200px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHeading" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblType" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION") %>' Style="vertical-align: top"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Value">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="300%"></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValue" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Stage">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStage" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Experience">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="150px" VerticalAlign="Top"></ItemStyle>
                           <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExperience" runat="server" Style="text-align: left" Width="150px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>



        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
