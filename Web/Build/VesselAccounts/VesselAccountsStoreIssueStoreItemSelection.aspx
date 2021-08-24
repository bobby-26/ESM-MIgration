<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsStoreIssueStoreItemSelection.aspx.cs"
    Inherits="VesselAccountsStoreIssueStoreItemSelection" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Item List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvStoreItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvStoreItem" UpdatePanelHeight="85%" />
                        <telerik:AjaxUpdatedControl ControlID="ddlStockClass" />
                         <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlStockClass">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlStockClass" />
                        <telerik:AjaxUpdatedControl ControlID="gvStoreItem" UpdatePanelHeight="85%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuStockItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuStockItem" />
                        <telerik:AjaxUpdatedControl ControlID="gvStoreItem" UpdatePanelHeight="85%" />
                        <telerik:AjaxUpdatedControl ControlID="ddlStockClass" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="MenuStockItem_TabStripCommand"></eluc:TabStrip>

        <div id="search">
            <table width="100%">
                <tr>
                    <td>
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtNumberSearch" CssClass="input_mandatory" MaxLength="20"
                            Width="80px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStoreItemName" runat="server" Text="Store Item Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtStockItemNameSearch" CssClass="input" Width="240px"
                            Text="">
                        </telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblStoreType" runat="server" Text="Store Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlStockClass" runat="server" Visible="true" CssClass="input"
                            AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Text="Red Line item is not in Market."></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </div>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvStoreItem" Height="95%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnItemCommand="gvStoreItem_ItemCommand" OnItemDataBound="gvStoreItem_ItemDataBound" EnableHeaderContextMenu="true"
            ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvStoreItem_NeedDataSource" OnUpdateCommand="gvStoreItem_UpdateCommand" EnableViewState="true"
            OnPreRender="gvStoreItem_PreRender" OnEditCommand="gvStoreItem_EditCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Number">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblIssueItemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblStockItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNamedesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Maker">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMakerdesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDROB").ToString().TrimEnd('0').TrimEnd('.') %>
                            <telerik:RadLabel ID="lblRob" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB").ToString() %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Quantity">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadNumericTextBox ID="txtQuantity" runat="server" CssClass="txtNumber" Width="100%" NumberFormat-DecimalDigits="2" IncrementSettings-InterceptArrowKeys="true"></telerik:RadNumericTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Issue On">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Date ID="txtIssueDate" runat="server" CssClass="input" Text='<%# DateTime.Now.ToString("dd/MM/yyyy") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                        </ItemTemplate>
                        <EditItemTemplate>

                            <telerik:RadTextBox ID="txtRemarks" runat="server" Text=""></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItem%>" ID="cmdEdit"
                                ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save"
                                CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdUpdate"
                                ToolTip="Update"><span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
