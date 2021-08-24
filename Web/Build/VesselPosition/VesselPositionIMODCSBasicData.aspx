<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionIMODCSBasicData.aspx.cs" Inherits="VesselPositionIMODCSBasicData" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voyage</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .table {
                border-collapse: collapse;
            }

                .table td, th {
                    border: 1px solid black;
                }

            .accordian_voluntary {
                background-color: blue;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVoyage" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageList" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVoyageList">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuVoyageTap" Title="Data Collection Plan" TabStrip="false" runat="server" OnTabStripCommand="VoyageTap_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" VesselsOnly="true" />
                    </td>
                </tr>
            </table>

            <telerik:RadDockZone ID="RadDockZone1" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleA" runat="server" Title="<b>1 Ship Particulars</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="false" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td style="width: 20%"><b>
                                    <telerik:RadLabel ID="lblvessel" Text="Name of the ship" runat="server"></telerik:RadLabel>
                                </b></td>
                                <td style="width: 80%">
                                    <telerik:RadLabel ID="txtvessel" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblIMOIdentificationNumber" runat="server" Text="IMO Number"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadLabel ID="txtIMOIdentificationNumber" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblIMOUniqueCompany" runat="server" Text="Company"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadLabel ID="txtIMOUniqueCompany" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblFlagState" runat="server" Text="Flag"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadLabel ID="txtFlagState" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblshiptype" runat="server" Text="Ship Type"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadLabel ID="txtshiptype" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblGrosstonnage" runat="server" Text="Gross Tonnage"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadLabel ID="txtGrosstonnage" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblnt" runat="server" Text="NT"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadLabel ID="txtnt" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblDeadweight" runat="server" Text="DWT"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadLabel ID="txtDeadweight" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lbleedi" runat="server" Text="EEDI (if applicable)"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadLabel ID="txtEEDI" runat="server"></telerik:RadLabel>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <b>
                                                    <telerik:RadLabel ID="lblEVI" runat="server" Text="EIV"></telerik:RadLabel>
                                                </b>&nbsp;&nbsp;
                                                <telerik:RadLabel ID="txtEVI" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblIceClass" runat="server" Text="Ice Class"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadLabel ID="txtIceClass" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>

                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock1" runat="server" Title="<b>2  Record of revision of Fuel Oil Consumption Data Collection Plan</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="false" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>

                        <div id="Recordofrevision">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="gvOilType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                            Width="100%" CellPadding="3" ShowFooter="false"
                                            ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvOilType_NeedDataSource">
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

                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="Date of revision">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblDateofrevision" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREVISIONDATE"))%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Revised Provision">
                                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lbloilconsumers" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONPROVISION") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lbloilconsumerid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMODCSREVISIONOFFOCONSUMPTIONID") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </telerik:RadDock>

                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock2" runat="server" Title="<b>3  Ship engines and other fuel oil consumers and fuel oil types used</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="false" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>

                        <div id="Shipengines">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="gvEmissionSource" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                            Width="100%" CellPadding="3" ShowFooter="false"
                                            ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvEmissionSource_NeedDataSource">
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
                                                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="No.">
                                                        <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblComponentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>' Visible="false"></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblrownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblEmissionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONANDFUELTYPEID") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONSOURCEID") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblpmscomponentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Engines or other fuel oil consumers">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="text-align: left; width: 400px">
                                                                        <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="lblComponentdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="lblemissionsourceid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONSOURCEID") %>'></telerik:RadLabel>
                                                                        ,&nbsp
                                                                                <telerik:RadLabel ID="lblType" runat="server" Text="Type :"></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="txtType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINETYPE") %>'></telerik:RadLabel>
                                                                        ,&nbsp
                                                                                <telerik:RadLabel ID="lblModel" runat="server" Text="Model :"></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="txtmodel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENGINEMODEL") %>'></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn Visible="false">
                                                        <HeaderStyle Wrap="false" />
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lnkSerNoHeader" runat="server"
                                                                ForeColor="White">
                                                                ID No 
                                                            </telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblIDNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIDNO") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn HeaderText="Power">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                        <HeaderStyle Wrap="true" />
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="text-align: left; width: 400px">
                                                                        <telerik:RadLabel ID="lblSerialNo" runat="server" Text="Ser No :"></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="lblSerialNoItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                                                                        ,&nbsp
                                                                                <telerik:RadLabel ID="lblPowerHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOWERCAPTION") %>'></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="lblPower" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPEFORMENCEPOWER") %>'></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="lblPowerUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOWERUNITS") %>'></telerik:RadLabel>
                                                                        ,&nbsp            
                                                                                <telerik:RadLabel ID="lblSFOCHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFOCLABEL") %>'></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="lblSFOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIDENTIFICATIONNO") %>'></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="lblSFOCUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFOCUNITS") %>'></telerik:RadLabel>
                                                                        ,&nbsp
                                                                                <telerik:RadLabel ID="lblYrInstHeader" runat="server" Text="Year of Inst :"></telerik:RadLabel>
                                                                        <telerik:RadLabel ID="lblYrInst" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAROFINSTALLATION") %>'></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Fuel Types Used">
                                                        <ItemStyle Wrap="False" HorizontalAlign="left" Width="10%"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblFuelType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblFuelTypeedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPEID") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Applicable for vessel" Visible="false">
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblApplicableyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLICABLEYN").ToString()=="1" ? "Yes":"No" %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </telerik:RadDock>

                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock3" runat="server" Title="<b>4  Emission factors</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="false" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <div id="EmissionFactor">
                            <table cellpadding="2" cellspacing="2" width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadGrid ID="gvEUMRVFuelType" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true"
                                            ShowFooter="false" EnableViewState="false" OnNeedDataSource="gvEUMRVFuelType_NeedDataSource">
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
                                                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="Fuel oil Type">
                                                        <ItemStyle Wrap="False" HorizontalAlign="left" Width="20%"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lbldescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCATEGORY") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="CF(t-CO2 / t- Fuel)">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>

                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblEmissionFactor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONFACTOR") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </telerik:RadDock>

                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock4" runat="server" Title="<b>5  Methods to measure fuel oil consumption</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="false" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <div id="Method">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel runat="server" ID="lblBackupmethod" Text="Method"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="txtmethodname" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10%">
                                        <telerik:RadLabel runat="server" ID="lblreference" Text="Reference to Existing Procedure"></telerik:RadLabel>
                                    </td>
                                    <td width="90%">
                                        <telerik:RadLabel ID="txtDocumentName" runat="server" Visible="false"></telerik:RadLabel>
                                        <asp:Repeater ID="rptC2_1" runat="server" OnItemDataBound="R1_ItemDataBound">
                                            <ItemTemplate>
                                                <a id="link" href="" class="applinks">
                                                    <hyperlink id="hl5" runat="server"></hyperlink>
                                                </a>
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel runat="server" ID="lbleuprocedure" Text="Description"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:CustomEditor ID="txteuprocedure" runat="server" Width="100%" Height="250px"
                                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </telerik:RadDock>

                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock5" runat="server" Title="<b>6  Method to measure distance travelled</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="false" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <div id="Methodtravelled">
                            <table width="100%">
                                <tr>
                                    <td width="10%">
                                        <telerik:RadLabel runat="server" ID="lblref" Text="Reference to Existing Procedure"></telerik:RadLabel>
                                    </td>
                                    <td width="90%">
                                        <asp:Repeater ID="R6" runat="server" OnItemDataBound="R6_ItemDataBound">
                                            <ItemTemplate>
                                                <a id="link" href="" class="applinks">
                                                    <hyperlink id="hl6" runat="server"></hyperlink>
                                                </a>
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel runat="server" ID="lbldescription" Text="Description"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:CustomEditor ID="txtprocedure" runat="server" Width="100%" Height="250px"
                                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </telerik:RadDock>

                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock6" runat="server" Title="<b>7  Method to measure hours under way</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="false" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <div id="hoursunderway">
                            <table width="100%">
                                <tr>
                                    <td width="10%">
                                        <telerik:RadLabel runat="server" ID="lblref1" Text="Reference to Existing Procedure"></telerik:RadLabel>
                                    </td>
                                    <td width="90%">
                                        <asp:Repeater ID="R7" runat="server" OnItemDataBound="R7_ItemDataBound">
                                            <ItemTemplate>
                                                <a id="link" href="" class="applinks">
                                                    <hyperlink id="hl7" runat="server"></hyperlink>
                                                </a>
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel runat="server" ID="lbldescription1" Text="Description"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:CustomEditor ID="txtprocedure1" runat="server" Width="100%" Height="250px"
                                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </telerik:RadDock>

                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock7" runat="server" Title="<b>8  Processes that will be used to report the data to the Administration</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="false" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <div id="Administration">
                            <table width="100%">
                                <tr>
                                    <td width="10%">
                                        <telerik:RadLabel runat="server" ID="lblref2" Text="Reference to Existing Procedure"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <asp:Repeater ID="R8" runat="server" OnItemDataBound="R8_ItemDataBound">
                                            <ItemTemplate>
                                                <a id="link" href="" class="applinks">
                                                    <hyperlink id="hl8" runat="server"></hyperlink>
                                                </a>
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel runat="server" ID="lbldescription2" Text="Description"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:CustomEditor ID="txtprocedure2" runat="server" Width="100%" Height="250px"
                                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </telerik:RadDock>

                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock8" runat="server" Title="<b>9  Data Quality</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="false" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <div id="DataQuality">
                            <table style="width: 100%;" class="table">
                                <thead>
                                    <tr>
                                        <td colspan="2" style="font-weight: bold;">
                                            <telerik:RadLabel ID="lbldata" runat="server" Text="The Quality of Data received is checked for any Gaps prior finalizing and submitting the data. The possible data gaps are listed below with the measures in place to treat any gaps"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td width="20%" style="font-weight: bold;">
                                            <telerik:RadLabel runat="server" ID="lbldataheader" Text="Data"></telerik:RadLabel>
                                        </td>
                                        <td style="font-weight: bold;">
                                            <telerik:RadLabel ID="lblmathoddata" runat="server" Text="Method to treat the Gaps"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="20%">
                                            <telerik:RadLabel runat="server" ID="lblFuelConsumption" Text="Fuel Consumption"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <asp:Repeater ID="Q1" runat="server" OnItemDataBound="Q1_ItemDataBound">
                                                <ItemTemplate>
                                                    <a id="link" href="" class="applinks">
                                                        <hyperlink id="QL1" runat="server"></hyperlink>
                                                    </a>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <eluc:CustomEditor ID="txteuprocedure1" runat="server" Width="100%" Height="150px"
                                                Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                                PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel runat="server" ID="lblDistanceTravelled" Text="Distance Travelled"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <asp:Repeater ID="Q2" runat="server" OnItemDataBound="Q2_ItemDataBound">
                                                <ItemTemplate>
                                                    <a id="link" href="" class="applinks">
                                                        <hyperlink id="QL2" runat="server"></hyperlink>
                                                    </a>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <eluc:CustomEditor ID="txteuprocedure2" runat="server" Width="100%" Height="150px"
                                                Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                                PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel runat="server" ID="Literal1" Text="Hours Underway"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <asp:Repeater ID="Q3" runat="server" OnItemDataBound="Q3_ItemDataBound">
                                                <ItemTemplate>
                                                    <a id="link" href="" class="applinks">
                                                        <hyperlink id="QL3" runat="server"></hyperlink>
                                                    </a>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <eluc:CustomEditor ID="txteuprocedure3" runat="server" Width="100%" Height="150px"
                                                Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                                PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel runat="server" ID="lblAdequecy" Text="Adequacy of the Data Collection Plan"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <asp:Repeater ID="Q4" runat="server" OnItemDataBound="Q4_ItemDataBound">
                                                <ItemTemplate>
                                                    <a id="link" href="" class="applinks">
                                                        <hyperlink id="QL4" runat="server"></hyperlink>
                                                    </a>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <eluc:CustomEditor ID="txteuprocedure4" runat="server" Width="100%" Height="150px"
                                                Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                                PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel runat="server" ID="lblInformationTechnology" Text="Information Technology"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <asp:Repeater ID="Q5" runat="server" OnItemDataBound="Q5_ItemDataBound">
                                                <ItemTemplate>
                                                    <a id="link" href="" class="applinks">
                                                        <hyperlink id="QL5" runat="server"></hyperlink>
                                                    </a>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <eluc:CustomEditor ID="txteuprocedure5" runat="server" Width="100%" Height="150px"
                                                Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                                PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel runat="server" ID="lblInternal" Text="Internal reviews and Validation of Data"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <asp:Repeater ID="Q6" runat="server" OnItemDataBound="Q6_ItemDataBound">
                                                <ItemTemplate>
                                                    <a id="link" href="" class="applinks">
                                                        <hyperlink id="QL6" runat="server"></hyperlink>
                                                    </a>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <eluc:CustomEditor ID="txteuprocedure6" runat="server" Width="100%" Height="150px"
                                                Visible="true" PictureButton="false" TextOnly="true" DesgMode="false" HTMLMode="false"
                                                PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </telerik:RadDock>

            </telerik:RadDockZone>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
