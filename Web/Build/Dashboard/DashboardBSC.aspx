<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSC.aspx.cs" Inherits="Dashboard_DashboardBSC" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="year" Src="~/UserControls/UserControlYear.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Strategy Maps</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvPI" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <br />
            <table width="100%" border="0">
                <tr>
                    <td align="center">
                        <telerik:RadLabel ID="radlblvision" runat="server" Text="Vision : A Premium offshore services provider offering diverse services to the Oil and Gas Industry" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <table  style="margin-right:0px;margin-left:auto;" >
                    <tr>
                        <td>
                            <telerik:RadLabel runat="server" Text="Year" />
                        </td>
                        <td>&nbsp
                        </td>
                        <td>
                            <eluc:year runat="server" YearStartFrom="2019" NoofYearFromCurrent="5" ID="radcbyear" AutoPostBack="true" OnTextChangedEvent="radcbyear_TextChangedEvent" />
                        </td>
                    </tr>

                </table>
            </div>
            <br />

            <telerik:RadGrid runat="server" ID="gvBSCMap" AutoGenerateColumns="false" Height="67.5%"
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvBSCMap_NeedDataSource" EnableViewState="true"
                OnItemDataBound="gvBSCMap_ItemDataBound" OnItemCommand="gvBSCMap_ItemCommand" ShowFooter="false">
                <MasterTableView EditMode="InPlace" AutoGenerateColumns="false"
                    TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="true" DataKeyNames="FLDBSSTRATEGICPERSPECTIVEID">

                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns></Columns>



                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" CellSelectionMode="SingleCell" />

                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
            <telerik:RadContextMenu runat="server" ID="ContextMenu1"
                EnableRoundedCorners="true" EnableShadows="true"
                OnItemClick="ContextMenu1_ItemClick">
                <Targets>
                    <telerik:ContextMenuControlTarget ControlID="gvBSCMap" />
                </Targets>
                <Items>
                    <telerik:RadMenuItem Text="Add/Edit Cell" Value="addkpi" />

                </Items>
            </telerik:RadContextMenu>

            <%--<telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvBSC" AutoGenerateColumns="false"
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvBSC_NeedDataSource" Height="93.5%"
                OnItemDataBound="gvBSC_ItemDataBound" OnItemCommand="gvBSC_ItemCommand" ShowFooter="false">
                <MasterTableView EditMode="InPlace" DataKeyNames="FLDBSSTRATEGYMAPID" AutoGenerateColumns="false"
                    TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" Width="200px" />
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>


                                <asp:LinkButton ID="nameanchor" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBSSMNAME")%>'>

                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vision">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" Width="800px" />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblsmvision" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVISION")%>'>
                                </telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75px" Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
