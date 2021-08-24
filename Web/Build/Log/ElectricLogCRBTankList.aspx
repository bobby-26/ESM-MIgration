<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogCRBTankList.aspx.cs" Inherits="Log_ElectricLogCRBTankList" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tank List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvTankList.ClientID %>"));
		        }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <style>
            .tankType {
                display: block;
                width: 500px;
                margin-left: 20px;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">


            <%-- For Popup Relaod --%>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="gvRevisionTabStrip" runat="server" Title="No Active Revision"></eluc:TabStrip>


            <div class="new-grid">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvTankList" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" ShowFooter="False" Style="margin-bottom: 0px" EnableViewState="true" MasterTableView-EnableViewState="false"
                    OnNeedDataSource="gvTankList_NeedDataSource"
                    OnItemCommand="gvTankList_ItemCommand"
                    OnItemDataBound="gvTankList_ItemDataBound" GroupingEnabled="true" EnableHeaderContextMenu="true">
                    <ExportSettings OpenInNewWindow="true" FileName="TankList">
                        <Pdf PageHeight="210mm" PageWidth="297mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="45mm"
                            BorderStyle="Medium" BorderColor="#666666">
                        </Pdf>
                    </ExportSettings>
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                        AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center" DataKeyNames="">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                        <GroupByExpressions>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldName="FLDTANKTYPE" FieldAlias="TankType" />
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="FLDTANKTYPE" FieldAlias="TankType" />
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                        </GroupByExpressions>
                        <ColumnGroups>
                            <telerik:GridColumnGroup Name="tanklocation" HeaderText="Tank Location" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup Name="volume" HeaderText="Volume-in-m3" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Tank Name">
                                <HeaderStyle Width="220px" HorizontalAlign="Center" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <%--<telerik:RadLabel ID="lblIsDraft" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISDRAFT") %>'></telerik:RadLabel>--%>
                                    <telerik:RadLabel ID="lblAttachment" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblTankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Tank Identification (IOPP NAME)">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblIOPPName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIOPPNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Tank Type">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTankType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>


                            <telerik:GridTemplateColumn HeaderText="Frames From - To" ColumnGroupName="tanklocation">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFramesFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFRAMESFROMTO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Lateral Posistion" ColumnGroupName="tanklocation">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLateralPosistion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLATERALPOSITION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="100%" ColumnGroupName="volume">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblvolume100" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="85%" ColumnGroupName="volume">
                                <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblvolume85" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD85PERCENT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="UPDATE" ID="cmdEdit"
                                        ToolTip="Edit Tank" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="DELETE" ID="cmdDelete"
                                        ToolTip="Delete Tank" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
