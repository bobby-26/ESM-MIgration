<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCrewExpiryDocumentDetails.aspx.cs" Inherits="Dashboard_DashboardCrewExpiryDocumentDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlVesselCertificateCategoryList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Expiry Document List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadSkinManager ID="radSkinManager1" runat="server"></telerik:RadSkinManager>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvdocumentlist" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvdocumentlist_NeedDataSource"
                GroupingEnabled="true" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
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
                        <telerik:GridTemplateColumn HeaderText="Document Type" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldoctype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Document Name" HeaderStyle-Width="250px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldocname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry date" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblexpdate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEXPIRYDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>



                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
