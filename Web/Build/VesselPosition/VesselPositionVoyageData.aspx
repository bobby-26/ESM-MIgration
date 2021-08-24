<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="VesselPositionVoyageData.aspx.cs" Inherits="VesselPositionVoyageData" %>

<!DOCTYPE html >

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Voyage Data</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }
    </style>

</head>
<body>
    <form id="frmVoyageData" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvVoyagePort">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvVoyagePort" />
                        <telerik:AjaxUpdatedControl ControlID="gvVoyageCharter" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvVoyageCharter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvVoyageCharter" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:TabStrip ID="MenuVoyageTap" TabStrip="true" runat="server" OnTabStripCommand="VoyageTap_TabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" Title="Voyage Detail" OnTabStripCommand="VoyageNewSaveTap_TabStripCommand"></eluc:TabStrip>

        <telerik:RadAjaxPanel runat="server" ID="pnlVoyageData" Height="85%" CssClass="scrolpan">


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width="40%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoyageNo" runat="server" Text="Voyage No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoyageNo" CssClass="input_mandatory" Width="90px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCommenced" runat="server" Text="Commenced"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucCommencedDate" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        &nbsp;
                    <telerik:RadTimePicker ID="txtTimeOfCommenced" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                        DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                    </telerik:RadTimePicker>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCommencedPort" runat="server" Text="Commenced Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="UcCommencedPort" runat="server" CssClass="readonlytextbox" Width="300px" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompleted" runat="server" Text="Completed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucCompletedDate" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        &nbsp;
                    <telerik:RadTimePicker ID="txtTimeOfCompleted" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                        DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                    </telerik:RadTimePicker>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompletedPort" runat="server" Text="Completed Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="UcCompletedPort" runat="server" CssClass="readonlytextbox" Width="300px" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterers/Sub Charterers"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCharterer" CssClass="input" TextMode="MultiLine" Resize="Both"
                            Width="300px" Height="70px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCharterersVoyage" runat="server" Text="Charterers Voyage Instruction"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtChartererInstruction" CssClass="input" TextMode="MultiLine" Resize="Both"
                            Width="300px" Height="70px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadDockZone ID="RadDockZone3" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock3" runat="server" Title="<b>Charter Party Details</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <telerik:RadFormDecorator ID="RadFormDecorator1" DecorationZoneID="gvVoyageCharter" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
                        <telerik:RadGrid ID="gvVoyageCharter" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                            Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvVoyageCharter_NeedDataSource"
                            OnItemCommand="gvVoyageCharter_RowCommand" OnItemDataBound="gvVoyageCharter_ItemDataBound"
                            OnSortCommand="gvVoyageCharter_SortCommand" EnableHeaderContextMenu="true" GroupingEnabled="false"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDVOYAGECHARTERID">
                                <HeaderStyle Width="102px" />
                                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Ballast" Name="Ballast" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Laden" Name="Laden" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Cargo" Name="Cargo" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Measure">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOilTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVoyageCharterid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAGECHARTERID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOilType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblOilTypeIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVoyageCharteridEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAGECHARTERID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOilTypeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Eco-Speed" ColumnGroupName="Ballast">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBallastEcoSpeed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALLASTECOSPEED") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtBallastEcoSpeed" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALLASTECOSPEED") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Full Speed" ColumnGroupName="Ballast">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBallastFullSpeed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALLASTFULLSPEED") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtBallastFullSpeed" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALLASTFULLSPEED") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Eco-Speed" ColumnGroupName="Laden">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLadenEcoSpeed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLADENECOSPEED") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtLadenEcoSpeed" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLADENECOSPEED") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Full Speed" ColumnGroupName="Laden">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLadenFullSpeed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLADENFULLSPEED") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtLadenFullSpeed" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLADENFULLSPEED") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Loading" ColumnGroupName="Cargo">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCargoLoading" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOLOADING") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtCargoLoading" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOLOADING") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Discharging" ColumnGroupName="Cargo">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCargoDischarging" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGODISCHARGING") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtCargoDischarging" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGODISCHARGING") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Tank Cleaning">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTankCleaning" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKCLEANING") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtTankCleaning" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKCLEANING") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Heating - Maintenance">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHeatingMaintenance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEATINGMAINTENANCE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtHeatingMaintenance" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEATINGMAINTENANCE") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Heating - Heat Up">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHeatingHeatUp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEATINGHEATUP") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtHeatingHeatUp" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEATINGHEATUP") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Idle In Port / At Anchor">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIdle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIDLE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtIdle" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIDLE") %>'
                                                DecimalPlace="2" Width="80px" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock4" runat="server" Title="<b>Port Details</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <telerik:RadFormDecorator ID="RadFormDecorator2" DecorationZoneID="gvVoyagePort" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
                        <telerik:RadGrid ID="gvVoyagePort" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                            Width="100%" CellPadding="3" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemCommand="gvVoyagePort_RowCommand" OnItemDataBound="gvVoyagePort_ItemDataBound"
                            OnNeedDataSource="gvVoyagePort_NeedDataSource" OnSortCommand="gvVoyagePort_SortCommand" EnableHeaderContextMenu="true" GroupingEnabled="false"
                            ShowFooter="true" ShowHeader="true" EnableViewState="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDPORTCALLID">
                                <HeaderStyle Width="102px" />
                                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                <Columns>
                                    <telerik:GridTemplateColumn FooterText="" HeaderText="ID">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPortCallId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTCALLID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblPortCallNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblPortCallIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTCALLID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblSerilNoEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNOINT") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblPortCallNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Number runat="server" ID="ucSerialNoAdd" CssClass="input_mandatory" MaxLength="3" IsInteger="true" IsPositive="true" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Port">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="19%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPort" runat="server" Width="10%" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString().Length > 15 ? DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME").ToString()%>'></telerik:RadLabel>
                                            <eluc:Tooltip ID="uclblPortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:MultiPort EnableViewState="true" ID="ucPortEdit" runat="server" CssClass="gridinput_mandatory" Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:MultiPort EnableViewState="true" ID="ucPortAdd" runat="server" CssClass="gridinput_mandatory" Width="100%" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn  HeaderText=" ETA ">
                                        <ItemStyle HorizontalAlign="Left" Width="13%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblETA" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDETA")) + " " + DataBinder.Eval(Container, "DataItem.FLDETA", "{0:HH:mm}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblETAEdit" runat="server" Visible="false" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDETA")) + " " + DataBinder.Eval(Container, "DataItem.FLDETA", "{0:HH:mm}") %>'></telerik:RadLabel>
                                            <eluc:Date ID="ucETAEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDETA")) %>'
                                                CssClass="input_mandatory" DatePicker="true" />
                                            <telerik:RadTimePicker ID="txtTimeOfETAEdit" runat="server" Width="80px" CssClass="input_mandatory"
                                                DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm" DbSelectedDate='<%# DataBinder.Eval(Container, "DataItem.FLDETA", "{0:HH:mm}")%>'>
                                            </telerik:RadTimePicker>

                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Date ID="ucETAAdd" runat="server" CssClass="input_mandatory" DatePicker="true" />
                                            <telerik:RadTimePicker ID="txtTimeOfETAAdd" runat="server" Width="80px" CssClass="input_mandatory" DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm"></telerik:RadTimePicker>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText=" ETD ">
                                        <ItemStyle HorizontalAlign="Left" Width="13%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblETD" runat="server" Visible="true" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDETD")) + " " + DataBinder.Eval(Container, "DataItem.FLDETD", "{0:HH:mm}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Date ID="ucETDEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDETD")) %>'
                                                CssClass="grid_input" DatePicker="true" />
                                            <telerik:RadTimePicker ID="txtTimeOfETDEdit" runat="server" Width="80px" CssClass="grid_input"
                                                DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm" DbSelectedDate='<%# DataBinder.Eval(Container, "DataItem.FLDETD", "{0:HH:mm}")%>'>
                                            </telerik:RadTimePicker>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Date ID="ucETDAdd" runat="server" CssClass="grid_input" DatePicker="true" />
                                            <telerik:RadTimePicker ID="txtTimeOfETDAdd" runat="server" Width="80px" CssClass="grid_input" DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm"></telerik:RadTimePicker>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Charterer Agent">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCharterAgent" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCHTRAGENTNAME").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDCHTRAGENTNAME").ToString().Substring(0, 20) + "..." : DataBinder.Eval(Container, "DataItem.FLDCHTRAGENTNAME").ToString()%>'></telerik:RadLabel>
                                            <eluc:Tooltip ID="uclblCharterAgent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHTRAGENTNAME") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtCharterAgentNameEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHTRAGENTNAME") %>'
                                                runat="server" CssClass="input" TextMode="MultiLine" Height="20px" Width="100%" Resize="Both">
                                            </telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtCharterAgentNameAdd" runat="server" TextMode="MultiLine" Height="20px" CssClass="input"
                                                Width="100%" Resize="Both">
                                            </telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn FooterText="New Short" HeaderText="Owner Agent">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOwnerAgent" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOWNERAGENTNAME").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDOWNERAGENTNAME").ToString().Substring(0, 20) + "..." : DataBinder.Eval(Container, "DataItem.FLDOWNERAGENTNAME").ToString()%>'></telerik:RadLabel>
                                            <eluc:Tooltip ID="uclblOwnerAgent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERAGENTNAME") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtOwnerAgentNameEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERAGENTNAME") %>'
                                                runat="server" TextMode="MultiLine" Height="20px" CssClass="input" Width="100%" Resize="Both">
                                            </telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtOwnerAgentNameAdd" runat="server" CssClass="input"
                                                TextMode="MultiLine" Height="20px" Width="100%" Resize="Both">
                                            </telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="EU Port YN">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEUPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUPORTYN") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblEUPortEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEUPORTYN") %>'></telerik:RadLabel>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Cargo" CommandName="CARGO" ID="cmdCargoAdd" ToolTip="Add Cargo Operation details">
                                    <span class="icon"><i class="fas fa-shopping-basket"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
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
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd" ToolTip="Add">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling SaveScrollPosition="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
