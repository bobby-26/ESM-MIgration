<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionBunkerReceiptList.aspx.cs"
    Inherits="VesselPositionBunkerReceiptList" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OilType" Src="~/UserControls/UserControlOilType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bunker Receipt List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvFuelOil.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVoyageCargoData" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlVoyageData">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuBunkerReceipt" runat="server" OnTabStripCommand="MenuBunkerReceipt_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" SyncActiveVesselsOnly="True" OnTextChangedEvent="UcVessel_OnTextChangedEvent" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileOil" runat="server" Text="Fuel Oil:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblFuelOilType" runat="server" AppendDataBoundItems="false"
                            Direction="Horizontal" AutoPostBack="true" RepeatLayout="Table" OnSelectedIndexChanged="SetFuelOilValue"
                            Enabled="true">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLubeOil" runat="server" Text="Lube Oil:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblLubeOilType" runat="server" AppendDataBoundItems="false"
                            Direction="Horizontal" AutoPostBack="true" RepeatLayout="Table" OnSelectedIndexChanged="SetLubeOilValue"
                             Enabled="true">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvFuelOil" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemDataBound="gvFuelOil_ItemDataBound" OnNeedDataSource="gvFuelOil_NeedDataSource"
                AllowSorting="false" ShowFooter="false" ShowHeader="true" RenderMode="Lightweight" AllowCustomPaging="true" AllowPaging="true"
                EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvFuelOil_ItemCommand">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDBUNKERRECEIPTID">
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
                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBunkerReceiptID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKERRECEIPTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMPTIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Product" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProduct" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Supplier" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSupplier" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" UniqueName="WTHeader">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWEIGHTINTONNES") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Density @ 15˚C" HeaderStyle-HorizontalAlign="Center" UniqueName="Dencity">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="30px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDensity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDENSITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S%" HeaderStyle-HorizontalAlign="Center" UniqueName="sulphur">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSulphur" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSULPHURPERCENTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="6-mth Avg S%" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAvgSulphur" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGSULPHURPERCENTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment">
                                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
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
