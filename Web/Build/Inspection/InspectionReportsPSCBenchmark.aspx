<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReportsPSCBenchmark.aspx.cs"
    Inherits="InspectionReportsPSCBenchmark" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PSC</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPSC" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="ucTitle" Text="PSC Benchmarking Data" Visible="false"></eluc:Title>
        <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:VesselByCompany AutoPostBack="true" ID="ucVessel" runat="server" AppendDataBoundItems="true" Width="220px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Country ID="ucCountry" runat="server" AppendDataBoundItems="true"
                        OnTextChangedEvent="ucCountry_Cahnged" AutoPostBack="true" Width="220px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:SeaPort ID="ucPort" runat="server" AppendDataBoundItems="true"
                        AutoPostBack="true" Width="220px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromdate" runat="server" Text="From Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Flag ID="ucFlag" runat="server" AppendDataBoundItems="true"
                        Width="220px" AutoPostBack="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDetainedYN" runat="server" Text="Detained Y/N" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <asp:CheckBox ID="chkDetainedYN" runat="server" Visible="false" />
                </td>
            </tr>
        </table>
        <br />
        <eluc:TabStrip ID="MenuPSCList" runat="server" OnTabStripCommand="MenuPSCList_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvPSC" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowSorting="true"
            Width="100%" CellPadding="3" OnItemDataBound="gvPSC_ItemDataBound" ShowFooter="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
            ShowHeader="true" EnableViewState="false" OnSorting="gvPSC_Sorting" OnNeedDataSource="gvPSC_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <ColumnGroups>
                    <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                </ColumnGroups>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <Columns>
                    <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                Width="95%"></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="IMO.No">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblIMONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMONUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date of PSC Inspection">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblInspectionDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDONEDATE") %>'
                                Width="95%"></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Port of PSC Inspection">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPortOfInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <b>
                                <telerik:RadLabel runat="server" ID="lblTotalPSCInspectionFooter" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSCTOTALCOUNT") %>'></telerik:RadLabel></b>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Country of PSC Inspection">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCountryOfInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="No. of Deficiencies">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="7%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDeficiencyCount1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFDEFICIENCIES") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <b>
                                <telerik:RadLabel runat="server" ID="lblTotalDeficienciesCountFooter"></telerik:RadLabel></b>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Detained(Yes/No)">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="7%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDeficiencyCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAINEDYN") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="No. of Detainable Deficiencies">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="7%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDetainedDeficienciesCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFDETAINEDDEFICIENCIES") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
