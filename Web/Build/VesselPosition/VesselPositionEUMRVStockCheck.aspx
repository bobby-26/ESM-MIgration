<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVStockCheck.aspx.cs" Inherits="VesselPositionEUMRVStockCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByUserType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tank Sounding Log</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvTankSoundinglog.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmVoyage" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageList" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVoyageList">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" VesselsOnly="true" SyncActiveVesselsOnly="True" AssignedVessels="true" OnTextChangedEvent="ddlVessel_Changed" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtReportFrom" runat="server" CssClass="input" DatePicker="true" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblReportToDate" runat="server" Text="To Date"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Date ID="txtReportTo" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                        <table style="visibility: hidden; display:none">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>

                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlLocation" runat="server" CssClass="input">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="ATSEA" Text="At Sea"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="ATANCHOR" Text="At Anchor"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="INPORT" Text="In Port"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblFuelType" Text="P/S" runat="server"></telerik:RadLabel>

                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlPS" runat="server" CssClass="input">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="P" Text="P"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="S" Text="S"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuVoyageList" runat="server" OnTabStripCommand="VoyageList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvTankSoundinglog" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvTankSoundinglog_RowCommand" OnItemDataBound="gvTankSoundinglog_ItemDataBound"
                OnUpdateCommand="gvTankSoundinglog_RowUpdating" AllowPaging="true" AllowCustomPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnSortCommand="gvTankSoundinglog_Sorting" ShowFooter="false" OnNeedDataSource="gvTankSoundinglog_NeedDataSource"
                ShowHeader="true" EnableViewState="false" AllowSorting="true">

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDTANKSOUNDINGLOGID">
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
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true" SortExpression="FLDDATE" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" ></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTanksoundinlogid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKSOUNDINGLOGID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkSoundingDate" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATE")) + " " + DataBinder.Eval(Container, "DataItem.FLDDATE", "{0:HH:mm}") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTanksoundinlogidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKSOUNDINGLOGID") %>'></telerik:RadLabel>
                                <eluc:Date ID="txtReportDate" runat="server" CssClass="input_mandatory" DatePicker="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATE")) %>' />
                                <telerik:RadTimePicker ID="txtReportTime" runat="server" Width="80px" CssClass="input_mandatory" DbSelectedDate='<%# DataBinder.Eval(Container, "DataItem.FLDDATE", "{0:HH:mm}")%>'
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Location">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLocationActual" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlLocation" runat="server" CssClass="input_mandatory" Width="98%">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="ATSEA" Text="At Sea"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="ATANCHOR" Text="At Anchor"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="INPORT" Text="In Port"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Occasion">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOccasion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCASION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtOccasion" runat="server" CssClass="input" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCASION") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Draft F">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDraftF" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDDRAFTF")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtDraftf" DecimalPlace="2" MaxLength="5" CssClass="input" Width="98%" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDDRAFTF")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Draft A">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDraftA" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDDRAFTA")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtDrafta" DecimalPlace="2" MaxLength="5" Width="98%" CssClass="input" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDDRAFTA")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="List˚">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblList" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDLIST")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtList" DecimalPlace="1" MaxLength="4" Width="98%" CssClass="input" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDLIST")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="List P/S">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlPS" runat="server" CssClass="input" Width="98%">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="P" Text="P"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="S" Text="S"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Report sent" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReport" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIRMEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Sounding" CommandName="SOUNDING" ID="cmdSounding" ToolTip="Sounding Detail">
                                    <span class="icon"><i class="fas fa-info-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Send to Office" CommandName="SEND" ID="cmdSend" ToolTip="Send to Office">
                                    <span class="icon"><i class="fas fa-share-square"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Reset" CommandName="RESET" ID="cmdReset" ToolTip="Reset">
                                    <span class="icon"><i class="fas fa-redo"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                            </EditItemTemplate>
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
