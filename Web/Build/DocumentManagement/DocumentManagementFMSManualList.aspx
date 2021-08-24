<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSManualList.aspx.cs" Inherits="DocumentManagement_DocumentManagementFMSManualList" MaintainScrollPositionOnPostback="true" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PageNumber" Src="~/UserControls/UserControlPageNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FMS Manual List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFMSManual.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersFMSManual" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuFMSManual" runat="server" OnTabStripCommand="MenuFMSManual_TabStripCommand" Visible="false"
                TabStrip="true"></eluc:TabStrip>
            <table id="tblfilenosearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmanualno" runat="server" Text="Category">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlmanuals" runat="server" Width="200px" AppendDataBoundItems="true" AutoPostBack="true"
                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select source" OnSelectedIndexChanged="ddlmanuals_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselname" runat="server" Text="Vessel Name">
                        </telerik:RadLabel>
                    </td>
                    <td>
                       <eluc:Vessel ID="ddlvessel" runat="server" CssClass="input" AppendDataBoundItems="true"
                            SyncActiveVesselsOnly="true" Width="240px" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersFMSManual" runat="server" OnTabStripCommand="MenuRegistersFMSManual_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFMSManual" runat="server"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                GridLines="None" OnNeedDataSource="gvFMSManual_NeedDataSource" OnItemDataBound="gvFMSManual_ItemDataBound"
                OnItemCommand="gvFMSManual_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false"
                    GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center" DataKeyNames="FLDFMSMANUALSUBCATEGORYID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                        ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDMANUALCATEGORY" FieldAlias="manual" SortOrder="Ascending" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDMANUALCODE" FieldAlias="Order" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDSUBCATEGORYCODE" DetailKeyField="FLDSUBCATEGORYCODE" />
                        </ParentTableRelation>
                    </NestedViewSettings>
                    <NoRecordsTemplate>
                        <table id="Table1" runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                        Font-Bold="true">
                                    </telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Category" ColumnGroupName="FileNo" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDMANUALNUMBER">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmanualno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Plans">
                            <HeaderStyle Width="80%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNoId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMSMANUALSUBCATEGORYID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblisattachment" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'>
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lblManualname" CommandName="LINK" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action" HeaderStyle-Width="20%" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Justify"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="cmdSvyAtt" ToolTip="Certificate" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
