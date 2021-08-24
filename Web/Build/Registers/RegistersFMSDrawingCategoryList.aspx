<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersFMSDrawingCategoryList.aspx.cs"
    Inherits="RegistersFMSDrawingCategoryList" MaintainScrollPositionOnPostback="true" %>

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
    <title>FMS Drawing List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFMSDrawing.ClientID %>"));
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
    <form id="frmRegistersFMSDrawing" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblfilenosearch" width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfileno" runat="server" Text="Drawing No">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtdrawingNo" runat="server" MaxLength="100" CssClass="input"
                            Width="60%" EmptyMessage="Type the File No">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselname" runat="server" Text="Vessel Name" Visible="false">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlvessel" runat="server" CssClass="input" AppendDataBoundItems="true" Visible="false"
                            AssignedVessels="true" AutoPostBack="true" Width="240px" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersFMSDrawing" runat="server" OnTabStripCommand="RegistersFMSDrawing_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFMSDrawing" runat="server" Width="100%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                GridLines="None" OnNeedDataSource="gvFMSDrawing_NeedDataSource" OnItemDataBound="gvFMSDrawing_ItemDataBound"
                OnItemCommand="gvFMSDrawing_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false"
                    GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <%--   <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">--%>
                    <HeaderStyle Width="10px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                        ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldName="FLDDRAWINGCATEGORY" FieldAlias="Category" SortOrder="Ascending" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="FLDORDER" FieldAlias="Order" SortOrder="Ascending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDDRAWINGCODE" DetailKeyField="FLDDRAWINGCODE" />
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
                        <telerik:GridTemplateColumn HeaderText="Drawing No" ColumnGroupName="FileNo" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDDRAWINGNUMBER">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lbldrawingno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAWINGNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle Width="70%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNoId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMSDRAWINGID") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblisattachment" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDrawingname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRAWINGNAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" ColumnGroupName="Action" AllowSorting="true"
                            SortExpression="">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE"
                                    ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="MAP" ID="cmdMapVesselType" ToolTip="Map Vessel Types">
                                    <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="MAPVESSEL" ID="cmdMapVessel" ToolTip="Exclude Vessel List">
                                    <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                </asp:LinkButton>
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
