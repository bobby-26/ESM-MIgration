<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersNoonReportRangeConfigPossibelCause.aspx.cs"
    Inherits="RegistersNoonReportRangeConfigPossibelCause" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Possible Cause</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegisterVesselDirection" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="Status1" />
            <telerik:RadTextBox ID="lblPossibleCause" runat="server" Enabled="false" Width="100%" Font-Bold="true"></telerik:RadTextBox>
            <eluc:TabStrip ID="MenuRegistersRangePossibleCause" runat="server" OnTabStripCommand="RegistersVesselDirection_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRangePossibleCause" Height="90%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvRangePossibleCause_ItemCommand" OnItemDataBound="gvRangePossibleCause_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvRangePossibleCause_NeedDataSource" OnUpdateCommand="gvRangePossibleCause_UpdateCommand"
                EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPOSSIBLECAUSEID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Cause">
                            <HeaderStyle Width="45%" />
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="45%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCauseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSSIBLECAUSE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCauseNameEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSSIBLECAUSE") %>' TextMode="MultiLine" runat="server" CssClass="gridinput_mandatory" Width="98%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCauseNameAdd" runat="server" CssClass="gridinput_mandatory" TextMode="MultiLine" ToolTip="Enter Cause" Width="98%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Recommended Action">
                            <HeaderStyle Width="45%" />
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="45%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcauseid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSSIBLECAUSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSSIBLECAUSEDESCRIOPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCauseidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSSIBLECAUSEID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSSIBLECAUSEDESCRIOPTION") %>'
                                    CssClass="gridinput_mandatory" Width="98%"></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="gridinput_mandatory" TextMode="MultiLine" Width="98%"
                                    ToolTip="Enter Recommended Action"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
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
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
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
