<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionYearToDateBreakup.aspx.cs"
    Inherits="VesselPositionYearToDateBreakup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EEOI Breakup</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvMenuYeartodatequaterreport.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmNoonReportRangeConfig" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlYeartodatequaterreport" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlYeartodatequaterreport">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Status runat="server" ID="ucStatus" />

            <telerik:RadGrid ID="gvMenuYeartodatequaterreport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvMenuYeartodatequaterreport_RowCommand" OnItemDataBound="gvMenuYeartodatequaterreport_ItemDataBound"
               OnNeedDataSource="gvMenuYeartodatequaterreport_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="True" AllowSorting="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
                AllowCustomPaging="false" AllowPaging="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVOYAGESUMMARYALLVESSEL">
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
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />


                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Voyage Number" HeaderStyle-Width="7%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPeriod" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAENUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblsummaryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAGESUMMARYALLVESSEL") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblarrivalid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Port" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAchived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMPORT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Port" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="True" HorizontalAlign="left" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPORT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Commenced" HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="True" HorizontalAlign="left" Width="12%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMethodFollowed" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOSP")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOSP", "{0:HH:mm}") %>' ></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed" HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="True" HorizontalAlign="left" Width="12%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMethodPropossed" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEOSP")) + " " + DataBinder.Eval(Container,"DataItem.FLDEOSP", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Distance" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="True" HorizontalAlign="Right" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDistance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALDISTANCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Cargo On Board" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Right" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoOnBoard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOONBOARD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Co2 Emission" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Right" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCo2Emission" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCO2EMISSION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="5%"></ItemStyle>
                            <ItemTemplate>

                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
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
