<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVPRSEvent.aspx.cs" Inherits="RegistersVPRSEvent" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EventType" Src="~/UserControls/UserControlVPRSEventType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Event</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVPRSEvent" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="Status1" />
            <eluc:TabStrip ID="MenuEvent" runat="server" OnTabStripCommand="Event_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvEvent" Height="93%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvEvent_ItemCommand" OnItemDataBound="gvEvent_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" OnUpdateCommand="gvEvent_UpdateCommand"
                OnNeedDataSource="gvEvent_NeedDataSource" OnSortCommand="gvEvent_SortCommand" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEVENTID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Event">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtShortNameEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' runat="server" CssClass="gridinput_mandatory" MaxLength="10" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtShortNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="10" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sort Order">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucSortOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSORTORDER") %>'
                                    CssClass="input_mandatory" DecimalPlace="0" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucSortOrderAdd" runat="server" CssClass="input_mandatory" DecimalPlace="0" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEventId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEvent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblEventIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENTID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtEventEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENT") %>'
                                    CssClass="gridinput_mandatory" MaxLength="100" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtEventAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="100" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Event Type">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEventTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENTTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEventTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENTTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblEventTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENTTYPEID") %>'></telerik:RadLabel>
                                <eluc:EventType runat="server" ID="ucEventTypeEdit" EventTypeList='<%# PhoenixRegistersVPRSEventType.ListEventType() %>'
                                    CssClass="dropdown_mandatory" AppendDataBoundItems="true" SelectedEventType='<%# DataBinder.Eval(Container,"DataItem.FLDEVENTTYPEID") %>' Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:EventType runat="server" ID="ucEventTypeAdd" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    EventTypeList='<%# PhoenixRegistersVPRSEventType.ListEventType() %>' Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
