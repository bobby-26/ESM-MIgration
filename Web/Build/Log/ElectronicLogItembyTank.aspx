<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectronicLogItembyTank.aspx.cs" Inherits="ElectronicLog_ElectronicLogItembyTank" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Item's by Tank</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentCounter" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvComponentCounter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvComponentCounter" LoadingPanelID="RadAjaxLoadingPanel" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--<eluc:Status runat="server" ID="ucStatus" />--%>

        <eluc:TabStrip ID="MenuComponentCounter" runat="server" OnTabStripCommand="ComponentCounter_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvElogItembyTank" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnItemCommand="gvElogItembyTank_ItemCommand" Width="100%"
            OnNeedDataSource="gvElogItembyTank_NeedDataSource" ShowFooter="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemDataBound="gvElogItembyTank_ItemDataBound">
            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Location" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDLOCATION">
                        <HeaderStyle Width="200px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%--<telerik:RadLabel ID="lblItembyTank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>--%>
                            <telerik:RadLabel ID="lbllocation" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                          <telerik:RadLabel ID="RadLabel1" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATION") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Capacity (m3)">
                        <HeaderStyle Width="200px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCapacity" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtCapacityEdit" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITY") %>'></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="ROB (m3)">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <FooterStyle Wrap="False" HorizontalAlign="Right" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblROB" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Decimal runat="server" ID="txtROBEdit" Width="80px" MaxLength="14" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB Date">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblROBDate" runat="server" Visible="True" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDROBDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Date runat="server" ID="txtROBDateEdit" CssClass="input" Width="117px" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDROBDATE")) %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Initial Y/N">
                        <HeaderStyle Width="99px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <FooterStyle Wrap="False" HorizontalAlign="Right" />
                        <ItemTemplate>
                            <%--<telerik:RadTextBox ID="lblInitialYNEdit" runat="server" Width="80px" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINITIALYN") %>'></telerik:RadTextBox>--%>
                            <telerik:RadLabel ID="lblInitialYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINITIALYN") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadRadioButtonList runat="server" AutoPostBack="false" ID="chkInitialYNEdit" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="true" />
                                    <telerik:ButtonListItem Text="No" Value="false" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle Width="70px" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save"
                                CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                        <%--<FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save"
                                CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                ToolTip="Add New" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>--%>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="false" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="460px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>

