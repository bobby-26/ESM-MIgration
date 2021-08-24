<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersNOX.aspx.cs" Inherits="RegistersNOX" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add/Edit NOx</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmNOxDetails" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>

        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlNOx" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlNOx" Height="100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuNOx" runat="server" OnTabStripCommand="NOx_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td width="20%">Vessel
                    </td>
                    <td width="40%">
                        <eluc:Vessel ID="ucVesselName" runat="server" VesselsOnly="true" AppendDataBoundItems="true"
                            CssClass="input_mandatory" />
                    </td>
                    <td width="40%"></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <b>M/E</b>
                    </td>
                    <td>
                        <b>A/E</b>
                    </td>
                </tr>
                <tr>
                    <td>Maker
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMEMaker" CssClass="input" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAEMaker" CssClass="input" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Model 
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMEModel" CssClass="input" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAEModel" CssClass="input" Width="200px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Built Date
                    </td>
                    <td>
                        <eluc:Date ID="txtBuiltDate" runat="server" CssClass="input_mandatory" DatePicker="true" Format="dd-MM-yyyy" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Use 
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMEUse" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlMEUse_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="E2" Value="For const speed main engines, incl Diesel Electric & CPP"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="E3" Value="For propeller law operated main and aux engines"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="D2" Value="For const speed aux engines"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="C1" Value="For variable speed, variable load aux engines not incl above"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAEUse" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlAEUse_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="E2" Value="For const speed main engines, incl Diesel Electric & CPP"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="E3" Value="For propeller law operated main and aux engines"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="D2" Value="For const speed aux engines"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="C1" Value="For variable speed, variable load aux engines not incl above"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMEUse" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAEUse" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>Rated Speed(rpm)
                    </td>
                    <td>
                        <eluc:Number ID="txtRPM" runat="server" CssClass="input_mandatory" Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtAERPM" runat="server" CssClass="input_mandatory" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>Rated Power(kW)
                    </td>
                    <td>
                        <eluc:Number ID="txtMEPowerOutput" runat="server" CssClass="input_mandatory" Width="80px" MaxLength="9" />
                    </td>
                    <td>
                        <eluc:Number ID="txtAEPowerOutput" runat="server" CssClass="input_mandatory" Width="80px" MaxLength="9" />
                    </td>
                </tr>
                <tr>
                    <td>No. of Units
                    </td>
                    <td>
                        <eluc:Number ID="txtMENoOfUnits" runat="server" CssClass="input_mandatory" Width="80px" MaxLength="4" />
                    </td>
                    <td>
                        <eluc:Number ID="txtAENoOfUnits" runat="server" CssClass="input_mandatory" Width="80px" MaxLength="4" />
                    </td>
                </tr>
                <tr>
                    <td>NOx Tier
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtTierName" CssClass="readonlytextbox" Width="80px" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadLabel runat="server" ID="lblTierId" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--Use NOx Tier I--%>
                        <telerik:RadCheckBox ID="chkTier1Yn" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>Allowed NOx emission value
                    </td>
                    <td>
                        <eluc:Number ID="txtMEAllowedEmission" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" MaxLength="9" />
                    </td>
                    <td>
                        <eluc:Number ID="txtAEAllowedEmission" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" MaxLength="9" />
                    </td>
                </tr>
                <tr>
                    <td>Actual NOx emission value
                    </td>
                    <td>
                        <eluc:Number ID="txtMEActualEmission" runat="server" CssClass="input" Width="80px" MaxLength="9" />
                    </td>
                    <td>
                        <eluc:Number ID="txtAEActualEmission" runat="server" CssClass="input" Width="80px" MaxLength="9" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td width="25%">ESI_NOx
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtESINox" runat="server" CssClass="readonlytextbox" Width="80px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="25%">EEOI Reported?
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkEEOI" runat="server" />
                        Y/N 
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td width="25%">OPS onboard?
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOPS" runat="server" />
                        Y/N
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center"><b> Baseline Sulphur limit in fuel (% m/m)</b>
                    </td>
                </tr>
            </table>

            <telerik:RadGrid ID="gvSOX" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnItemDataBound="gvSOX_RowDataBound"
                Width="100%" CellPadding="3" AllowSorting="false" ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvSOX_NeedDataSource"
                EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Date WEF">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDateWEF" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUILTDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="High">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHighSea" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBSOUT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Mid">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblECA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBSIN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Low">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBerth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBSBERTH") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
