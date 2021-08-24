<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionROBReInitialization.aspx.cs" Inherits="VesselPositionROBReInitialization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnVoyagePort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ROB Re-Initialization</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCourseRegister" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlCourseRegister">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuRobMain" runat="server" OnTabStripCommand="MenuRobMain_TabStripCommand" Title="Re - Initialization Detail"
                TabStrip="false"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" Width="220px" CssClass="readonlytextbox" Enabled="False"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoyageNo" runat="server" Text="Current Voyage Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVoyage">
                            <telerik:RadTextBox ID="txtVoyageName" runat="server" Width="180px" CssClass="input_mandatory" Enabled="False"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowVoyage" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." Visible="false" />
                            <telerik:RadTextBox ID="txtVoyageId" runat="server" Width="0px" CssClass="input" Visible="false"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastReportDate" runat="server" Text="Last Report Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtLastReportDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="220px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastReportType" runat="server" Text="Last Report Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastReportType" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="<b>Re-Initialize commences on</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Re-Initialize Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtReintializeDate" runat="server" CssClass="input_mandatory"
                            DatePicker="true" />
                        <telerik:RadTimePicker ID="txtReintializeTime" runat="server" Width="80px" CssClass="input_mandatory"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                    <td>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Voyage Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnNewVoyage">
                            <telerik:RadTextBox ID="txtNewVoyageName" runat="server" Width="180px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" AlternateText=".." ID="imgShowNewVoyage" ToolTip="Voyage">
                             <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtNewVoyageId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmNextPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Multiport ID="ucNextPort" runat="server" CssClass="input_mandatory" Width="220px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportType" runat="server" Text="On"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVoyageStartFrom" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" Width="180px">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="--Select--" Value="" />
                                <telerik:RadComboBoxItem runat="server" Text="Arrival" Value="ARRIVAL" />
                                <telerik:RadComboBoxItem runat="server" Text="Departure" Value="DEPARTURE" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReason" runat="server" Text="Reason for Re-Initialization"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="txtReason" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" Width="220px">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Re-commence reporting after break" Value="Re-commence reporting after break"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Reset ROB" Value="Reset ROB"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Resize="Both" Width="360px" Height="30px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGuidanceText" Style="color: Blue;" runat="server" Text="* After Re-Initialization check the ROB and Cargo Details, if Required correct the ROB and Cargo details"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <hr />
            <b>
                <telerik:RadLabel ID="lblROBonCommencingVoyage" runat="server" Text="ROB on Commencing Voyage"></telerik:RadLabel>
            </b>

            <telerik:RadDockZone ID="RadDockZone2" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock1" runat="server" Title="<b>Fuel Oil</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvFuel" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                            Width="100%" CellPadding="3" OnItemCommand="gvFuel_ItemCommand" OnItemDataBound="gvFuel_ItemDataBound"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvFuel_NeedDataSource"
                            AllowSorting="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDOILTYPECODE">
                                <HeaderStyle Width="102px" />
                                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                <Columns>

                                    <telerik:GridTemplateColumn HeaderText="Fuel Oil" HeaderStyle-Width="50%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblFuelOil" runat="server" Text="Fuel Oil"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblFuelTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblFuelShortName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblFuelTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblNoonReportId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOONREPORTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblrobAndConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBANDCONSUMPTIONID") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn Visible="false" HeaderText="Previous ROB">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblROB" runat="server" Text="Previous ROB (mT)"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSROB") %>'
                                                DecimalPlace="2">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="ROB (mT)" HeaderStyle-Width="20%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblROB1" runat="server" Text="ROB (mT)"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblFuelROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBATNOON") %>'
                                                DecimalPlace="2">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtFuelROBEdit" runat="server" CssClass="input_mandatory txtNumber" DecimalPlace="2"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBATNOON") %>' IsPositive="true" MaxLength="9" Width="18%" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Sulphur %" HeaderStyle-Width="20%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblSulphur" runat="server" Text="Sulphur %"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAvgSulphurPercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGSULPHUR") %>'
                                                DecimalPlace="2">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtAvgSulphurPercentageEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGSULPHUR") %>' IsPositive="true" MaxLength="9" Width="40%" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                                        </HeaderTemplate>
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
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>

            <telerik:RadDockZone ID="RadDockZone1" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock2" runat="server" Title="<b>Lube Oil</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvLubOil" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnItemCommand="gvLubOil_RowCommand" OnItemDataBound="gvLubOil_ItemDataBound"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvLubOil_NeedDataSource"
                            AllowSorting="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDOILTYPECODE">
                                <HeaderStyle Width="102px" />
                                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Lube Oil" HeaderStyle-Width="70%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblLubeOilHeader" runat="server" Text="Lube Oil"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLubOilId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblLubOilShortName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblLubOilTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblNoonReportId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOONREPORTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblrobAndConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBANDCONSUMPTIONID") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn Visible="false" HeaderText="Previous ROB">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblROB" runat="server" Text="Previous ROB (Ltr)"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSROB") %>'
                                                DecimalPlace="2">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ROB (Ltr)" HeaderStyle-Width="20%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="200px"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblROBLtrHeader" runat="server" Text="ROB (Ltr)"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLubOilROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBATNOON") %>'
                                                DecimalPlace="2">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="txtLubOilROBEdit" runat="server" CssClass="input_mandatory txtNumber" DecimalPlace="2"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBATNOON") %>' IsPositive="true" MaxLength="9" Width="40%" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                                        </HeaderTemplate>
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
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>

            <telerik:RadDockZone ID="RadDockZone3" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock3" runat="server" Title="<b>Fresh Water</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvWater" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnItemCommand="gvWater_RowCommand" OnItemDataBound="gvWater_ItemDataBound"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvWater_NeedDataSource"
                            AllowSorting="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDOILTYPECODE">
                                <HeaderStyle Width="102px" />
                                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Fresh Water" HeaderStyle-Width="70%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblFreshWaterHeader" runat="server" Text="Fresh Water"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblWaterTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblWaterShortName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblWaterTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false" HeaderText="Previous ROB">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="200px"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblROB" runat="server" Text="Previous ROB (mT)"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSROB") %>'
                                                DecimalPlace="2">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ROB (mT)" HeaderStyle-Width="20%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="200px"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblROBmTHeader" runat="server" Text="ROB (mT)"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblWaterROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBATNOON") %>'
                                                DecimalPlace="2">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblWaterTypeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblrobAndConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBANDCONSUMPTIONID") %>'></telerik:RadLabel>
                                            <eluc:Number ID="txtWaterROBEdit" runat="server" CssClass="input_mandatory txtNumber" DecimalPlace="2"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBATNOON") %>' IsPositive="true" MaxLength="9" Width="40%" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                                        </HeaderTemplate>
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
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>

            <telerik:RadDockZone ID="RadDockZone4" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock4" runat="server" Title="<b>Cargo Operations</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <eluc:TabStrip ID="MenuOperation" runat="server" OnTabStripCommand="Operation_TabStripCommand"></eluc:TabStrip>
                        <telerik:RadGrid ID="gvCargoOperation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnItemCommand="gvCargoOperation_RowCommand" OnItemDataBound="gvCargoOperation_ItemDataBound"
                            AllowSorting="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvCargoOperation_NeedDataSource"
                            EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDOPERATIONID">
                                <HeaderStyle Width="102px" />
                                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Cargo Operation" HeaderStyle-Width="20%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCargoOperationHeader" runat="server" Text="Cargo Operation"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOperationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOPERATIONID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVoyageid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOYAGEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOperationName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOPERATIONNAME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Cargo" HeaderStyle-Width="20%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCargoHeader" runat="server" Text="Cargo"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCargo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGONAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Quantity(mT)" HeaderStyle-Width="15%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblQuantityHeader" runat="server" Text="Quantity(mT)"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Port" HeaderStyle-Width="20%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblPortHeader" runat="server" Text="Port"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Date Loaded" HeaderStyle-Width="15%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="200px"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblDateLoadedHeader" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDateLoaded" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATELOADED")) %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCommencedHeader" runat="server" Text="Commenced" Visible="false"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCommenced" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMMENCED")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMMENCED", "{0:HH:mm}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCompletedHeader" runat="server" Text="Completed"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETED")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMPLETED", "{0:HH:mm}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment">
                                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
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
