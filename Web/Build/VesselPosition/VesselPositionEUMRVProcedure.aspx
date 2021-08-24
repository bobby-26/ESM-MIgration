<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVProcedure.aspx.cs" Inherits="VesselPositionEUMRVProcedure" %>

<%@ Import Namespace="SouthNests.Phoenix.VesselPosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvProcedure.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmVPRSLocation" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlVPRSLocation">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Panel ID="pnlSatcom" runat="server" GroupingText="Copy" Width="100%" Height="40%">
                <table width="100%">
                    <tr>
                        <td style="width: 80%;">
                            <div style="height: 100px; overflow: auto; border: 1px; color: Black; width: 100%;">
                                <asp:CheckBoxList ID="chkVesselList" Height="100%" Width="100%" runat="server"
                                    RepeatColumns="4" RepeatDirection="Horizontal" RepeatLayout="Table">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td align="center" style="width: 10%;">
                            <asp:ImageButton runat="server" AlternateText="Copy" ImageUrl="<%$ PhoenixTheme:images/copy.png %>"
                                ID="cmdCopy" OnClick="cmdCopy_OnClick" ToolTip="Copy"></asp:ImageButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel1" runat="server" GroupingText="Search" Width="100%" Height="40%">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCode" Text="Table" runat="server"></telerik:RadLabel></td>
                        <td>
                            <asp:TextBox ID="txtCode" runat="server" CssClass="input"></asp:TextBox></td>
                        <td>
                            <telerik:RadLabel ID="lblprocedurefilter" Text="Procedure" runat="server"></telerik:RadLabel></td>
                        <td>
                            <asp:TextBox ID="txtprocedurefilter" runat="server" CssClass="input"></asp:TextBox></td>
                    </tr>
                </table>
            </asp:Panel>

            <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="Location_TabStripCommand"></eluc:TabStrip>


            <telerik:RadGrid ID="gvProcedure" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvProcedure_RowCommand"
                AllowSorting="false" OnNeedDataSource="gvProcedure_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
                ShowFooter="false" RenderMode="Lightweight" AllowCustomPaging="true" AllowPaging="true"
                ShowHeader="true" EnableViewState="false" OnSortCommand="gvProcedure_Sorting">
                 <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEUMRVPROCEDUERID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                <Columns>

                    <telerik:GridTemplateColumn HeaderText="Table" AllowSorting="false" SortExpression="FLDCODE" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblProcedureCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblNewCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Procedure" AllowSorting="false" SortExpression="FLDPROCEDURE" HeaderStyle-Width="90%">
                        <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lblProcedure" runat="server" CommandName="NAV" CommandArgument='<%# Container.DataItem %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURE") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblProcedureId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUMRVPROCEDUERID") %>'></telerik:RadLabel>
                        </ItemTemplate>
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
