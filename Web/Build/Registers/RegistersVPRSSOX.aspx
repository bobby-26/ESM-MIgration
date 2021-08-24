<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVPRSSOX.aspx.cs" Inherits="RegistersVPRSSOX" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add/Edit SOx</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSOxDetails" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSOX" Height="100%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" ShowHeader="true" ShowFooter="false" EnableViewState="false" OnItemDataBound="gvSOX_ItemDataBound"
                OnItemCommand="gvSOX_ItemCommand" OnRowUpdating="gvSOX_RowUpdating" OnUpdateCommand="gvSOX_UpdateCommand" OnNeedDataSource="gvSOX_NeedDataSource"
                EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSOXID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="22.5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblsoxid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSOXID"]%>'></asp:Label>
                                <asp:Label ID="lbltierid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTIER"]%>'></asp:Label>
                                <asp:Label ID="lbltier" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTIERNAME"]%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblsoxidEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSOXID"]%>'></asp:Label>
                                <asp:Label ID="lbltieridEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTIER"]%>'></asp:Label>
                                <asp:Label ID="lbltierEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTIERNAME"]%>'></asp:Label>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="High Sea">
                            <HeaderStyle Width="22.5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblbsout" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDBSOUT"]%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucbsoutEdit" runat="server" CssClass="input" DecimalPlace="3"
                                    Text='<%# ((DataRowView)Container.DataItem)["FLDBSOUT"]%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ECA">
                            <HeaderStyle Width="22.5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblbsinHeader" runat="server" Text="ECA"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblbsin" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDBSIN"]%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucbsinEdit" runat="server" CssClass="input" DecimalPlace="3"
                                    Text='<%# ((DataRowView)Container.DataItem)["FLDBSIN"]%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Berth">
                            <HeaderStyle Width="22.5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblbsberth" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDBSBERTH"]%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucbsberthEdit" runat="server" CssClass="input" DecimalPlace="3"
                                    Text='<%# ((DataRowView)Container.DataItem)["FLDBSBERTH"]%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
