<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVBasicData.aspx.cs" Inherits="VesselPositionEUMRVBasicData" %>

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
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageList" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVoyageList">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuVoyageTap" Title="Basic Data" TabStrip="false" runat="server" OnTabStripCommand="VoyageTap_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" VesselsOnly="true" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>

            <h3>Part A</h3>
            <telerik:RadDockZone ID="RadDockZone1" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleA" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <asp:Repeater ID="rptA" runat="server">
                            <HeaderTemplate>
                                <table class="rfdTable rfdOptionList" style="width: 100%;" class="table">
                                    <tr>
                                        <th style="width: 15%">Version No</th>
                                        <th style="width: 15%">Reference date</th>
                                        <th style="width: 25%">Status at reference date</th>
                                        <th style="width: 45%">Reference to Chapters where revision or modifications have been made, including a brief explanation of changes</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 15%;">
                                        <telerik:RadLabel ID="lblA_1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERSIONNO") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 15%;">
                                        <telerik:RadLabel ID="lblA_2" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDREFERENCEDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 25%;">
                                        <telerik:RadLabel ID="lblA_3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 45%;">
                                        <telerik:RadLabel ID="lblA_4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPLANATION") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="4" style="text-align: center">
                                        <telerik:RadLabel ID="lblrptA" runat="server" Font-Bold="true" Text="NO RECORDS FOUND"></telerik:RadLabel>
                                    </td>
                                </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </telerik:RadDock>

            </telerik:RadDockZone>

            <h3>Part B Basic data</h3>
            <telerik:RadDockZone ID="RadDockZone2" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleB1" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>

                        <div id="ShipInformation">
                            <table width="100%">
                                <tr>
                                    <td style="width: 20%"><b>
                                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Name"></telerik:RadLabel>
                                    </b></td>
                                    <td style="width: 80%">
                                        <telerik:RadTextBox ID="txtVesselName" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblIMOIdentificationNumber" runat="server" Text="IMO Identification Number"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtIMOIdentificationNumber" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblPortofRegistry" runat="server" Text="Port of Registry"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtPortofRegistry" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblHomePort" runat="server" Text="Home Port"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtHomePort" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblNameoftheShipOwner" runat="server" Text="Name of the Ship Owner"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtNameoftheShipOwner" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblIMOUniqueCompany" runat="server" Text="IMO Unique Company"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtIMOUniqueCompany" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblTypeoftheShip" runat="server" Text="Type of the Ship"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtTypeoftheShip" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblDeadweight" runat="server" Text="Deadweight"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtDeadweight" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblGrosstonnage" runat="server" Text="Gross tonnage"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtGrosstonnage" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblClassificationSociety" runat="server" Text="Classification Society"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtClassificationSociety" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblIceClass" runat="server" Text="Ice Class"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtIceClass" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblFlagState" runat="server" Text="Flag State"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtFlagState" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                                <tr>
                                    <td><b>
                                        <telerik:RadLabel ID="lblAdditionalDescription" runat="server" Text="Additional Description"></telerik:RadLabel>
                                    </b></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtAdditionalDescription" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                                </tr>
                            </table>

                        </div>
                    </ContentTemplate>
                </telerik:RadDock>


                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleB2" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td style="width: 20%"><b>
                                    <telerik:RadLabel ID="lblNameofthecompany" runat="server" Text="Name of the company"></telerik:RadLabel>
                                </b></td>
                                <td style="width: 80%">
                                    <telerik:RadTextBox ID="txtNameofthecompany" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 20%"><b>
                                    <telerik:RadLabel ID="lblIMONo" runat="server" Text="IMO No"></telerik:RadLabel>
                                </b></td>
                                <td style="width: 80%">
                                    <telerik:RadTextBox ID="txtIMONo" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblAddress1" runat="server" Text="Address 1"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadTextBox ID="txtAddress1" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblAddress2" runat="server" Text="Address 2"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadTextBox ID="txtAddress2" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadTextBox ID="txtCity" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblStateProvinceRegion" runat="server" Text="State/Province/Region"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadTextBox ID="txtStateProvinceRegion" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblPostalCode" runat="server" Text="Postal Code"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadTextBox ID="txtPostalCode" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadTextBox ID="txtCountry" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblContactPerson" runat="server" Text="Contact Person"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadTextBox ID="txtContactPerson" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblTelephoneNumber" runat="server" Text="Telephone Number"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadTextBox ID="txtTelephoneNumber" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                            <tr>
                                <td><b>
                                    <telerik:RadLabel ID="lblEmailAddress" runat="server" Text="Email Address"></telerik:RadLabel>
                                </b></td>
                                <td>
                                    <telerik:RadTextBox ID="txtEmailAddress" Width="80%" CssClass="input" runat="server"></telerik:RadTextBox></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>


                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleB3" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvEngine" runat="server" AutoGenerateColumns="False" Font-Size="11px" Width="100%" CellPadding="3"
                            ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvEngine_NeedDataSource">
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
                                    <telerik:GridTemplateColumn HeaderText="Reference Number" HeaderStyle-Width="10%">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblComponentNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="15%">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblComponentitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="ID No" HeaderStyle-Width="5%">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIDNoItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIDNO") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderStyle-Width="50%" HeaderText="Technical description of emission source (performance/power,<br /> specific fuel oil consumption (SFOC), year of installation,<br /> identification number in case of multiple identical emission sources, etc.)">
                                        <HeaderStyle Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td style="width: 400px; text-align: left;">
                                                        <telerik:RadLabel ID="lblSerNo" runat="server" Text="Ser No :"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblIDNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
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

                                    <telerik:GridTemplateColumn HeaderText="Fuel Type" HeaderStyle-Width="20%">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblFuelType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
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

                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleB4" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvFuelType" runat="server" AutoGenerateColumns="False" Font-Size="11px" Width="100%" CellPadding="3"
                            ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvFuelType_NeedDataSource">
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
                                    <telerik:GridTemplateColumn HeaderText="Fuel Type">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Reference">
                                        <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblReferenceItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Emission Factor">
                                        <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVoyageId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONFACTOR") %>'></telerik:RadLabel>
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
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleB5" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>

                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Managing the completeness of the list of emission sources</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <asp:HyperLink ID="hlb5_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    <telerik:RadLabel ID="lblb5_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblb5_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblb5_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblb5_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblb5_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblb5_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>

            </telerik:RadDockZone>

            <h3>Part C Activity data</h3>
            <telerik:RadDockZone ID="RadDockZone3" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC1" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Item</th>
                                <th style="width: 50%">Confirmation field</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Minimum number of expected voyages per reporting period falling under the scope of the EU MRV Regulation according to the ship's schedule
                                </td>
                                <td style="width: 50%">
                                    <telerik:RadLabel runat="server" ID="lblC1_1"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Are there expected voyages per reporting period not falling under the scope of the EU MRV Regulation according to the ship's schedule?
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblC1_2"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Conditions of Article 9 (2) fulfilled?
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblC1_3"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>If yes, do you intend to make use of the derogation for monitoring the amount of fuel consumed on a per-voyage basis?
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblC1_4"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>

                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_1" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <asp:Repeater ID="rptC2_1" runat="server">
                            <HeaderTemplate>
                                <table style="width: 100%;" class="table rfdTable rfdOptionList">
                                    <tr>
                                        <th style="width: 50%">Emission source</th>
                                        <th style="width: 50%">Chosen methods for fuel consumption</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 50%;">
                                        <telerik:RadLabel ID="lblC2_1_1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONSOURCENAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 50%;">
                                        <telerik:RadLabel ID="lblC2_1_2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONITORINGMETHODNAME") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="2" style="text-align: center">
                                        <telerik:RadLabel ID="lblrptC2_1" runat="server" Font-Bold="true"  Text="NO RECORDS FOUND"></telerik:RadLabel>
                                    </td>
                                </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_2" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Determining fuel bunkered and fuel in tanks</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <asp:HyperLink ID="hlC2_2_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    <telerik:RadLabel ID="lblC2_2_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_2_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_2_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_2_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_2_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_2_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_3" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Regular cross-checks between bunkering quantity as provided by BDN and bunkering quantity indicated by on-board  measurement</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <asp:HyperLink ID="hlC2_3_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    <telerik:RadLabel ID="lblC2_3_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_3_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_3_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_3_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_4" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>

                        <asp:Repeater ID="rptC2_4" runat="server">
                            <HeaderTemplate>
                                <table style="width: 100%;" class="table rfdTable rfdOptionList">
                                    <tr>
                                        <th style="width: 33%">Measurement equipment</th>
                                        <th style="width: 33%">Elements applied to (eg.emission sources,tanks)</th>
                                        <th style="width: 33%">Technical description (specification, age, maintenance intervals)</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 33%;">
                                        <telerik:RadLabel ID="lblC2_4_1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASUREMENTEQUIPMENT") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 33%;">
                                        <telerik:RadLabel ID="lblC2_4_2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMISSIONSOURCENAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 33%;">
                                        <telerik:RadLabel ID="lblC2_4_3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTECHNICALDESC") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="3" style="text-align: center">
                                        <telerik:RadLabel ID="lblrptC2_4" runat="server" Font-Bold="true"  Text="NO RECORDS FOUND"></telerik:RadLabel>
                                    </td>
                                </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>

                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_5" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Recording, retrieving, transmitting and storing information regarding measurements</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <asp:HyperLink ID="hlC2_5_1" Target="_blank" runat="server" ToolTip="Download Form"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>

                                    <telerik:RadLabel ID="lblC2_5_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_5_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_5_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_5_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_5_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_5_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_6" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <asp:Repeater ID="rptC2_6" runat="server">
                            <HeaderTemplate>
                                <table style="width: 100%;" class="table rfdTable rfdOptionList">
                                    <tr>
                                        <th style="width: 33%">Fuel type/tank</th>
                                        <th style="width: 33%">Method to determine actual density values of fuel bunkered</th>
                                        <th style="width: 33%">Method to determine actual density values of fuel in tanks</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 33%;">
                                        <telerik:RadLabel ID="lblC2_6_1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELTYPENAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 33%;">
                                        <telerik:RadLabel ID="lblC2_6_2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULEBUNKEREDNAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 33%;">
                                        <telerik:RadLabel ID="lblC2_6_3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFULETANKNAME") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="3" style="text-align: center">
                                        <telerik:RadLabel ID="lblrptC2_6" runat="server" Font-Bold="true" Text="NO RECORDS FOUND"></telerik:RadLabel>
                                    </td>
                                </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_7" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <asp:Repeater ID="rptC2_7" runat="server">
                            <HeaderTemplate>
                                <table style="width: 100%;" class="table rfdTable rfdOptionList">
                                    <tr>
                                        <th style="width: 33%">Monitoring method</th>
                                        <th style="width: 33%">Approach used</th>
                                        <th style="width: 33%">Value</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 33%;">
                                        <telerik:RadLabel ID="lblC2_7_1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONITORINGMETHODNAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 33%;">
                                        <telerik:RadLabel ID="lblC2_7_2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROACHUSEDNAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 33%;">
                                        <telerik:RadLabel ID="lblC2_7_3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE")+" %"%>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="3" style="text-align: center">
                                        <telerik:RadLabel ID="lblrptC2_7" runat="server" Font-Bold="true" Text="NO RECORDS FOUND"></telerik:RadLabel>
                                    </td>
                                </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>

                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_8" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Ensuring quality assurance of measuring equipment</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC2_8_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC2_8_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_8_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_8_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_8_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_8_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_8_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>

                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_9" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Determining the split of fuel consumption into freight and passenger part</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Applied allocation method according to EN16258</td>
                                <td style="width: 50%">
                                    <telerik:RadLabel ID="lblC2_9_1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of method to determine the mass of freight and passengers including possible use of default values for the weight of cargo units/ lane meters (if mass method is used)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_9_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of method to determine the deck area assigned to freight and passengers including the consideration of hanging decks and of passenger cars on freight decks (if area method is used only)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_9_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Split of fuel consumption into freight and passenger part (if area method is used only)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_9_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_9_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formulae and data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_9_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_9_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_9_8" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_10" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Determining and recording the fuel consumption on laden Voyages</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC2_10_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC2_10_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_10_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_10_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_10_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formulae and data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_10_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_10_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_10_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_11" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Determining and recording the fuel consumption for cargo heating</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC2_11_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC2_11_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_11_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_11_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_11_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formulae and data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_11_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_11_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_11_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC2_12" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Determining and recording the fuel consumption for dynamic positioning</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC2_12_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC2_12_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_12_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_12_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_12_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formulae and data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_12_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_12_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC2_12_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC3" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Recording and safeguarding completeness of voyages</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC3_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC3_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC3_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure (including recording voyages, monitoring voyages etc.) if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC3_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC3_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Data Sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC3_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC3_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC3_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC4_1" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Recording and determining the distance per voyage made</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC4_1_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC4_1_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_1_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure (including recording and managing distance information) if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_1_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_1_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Data Sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_1_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_1_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_1_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC4_2" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Determining and recording the distance travelled when navigating through ice</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC4_2_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC4_2_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_2_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure (including recording and managing distance and winter conditions information) if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_2_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_2_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formulae and data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_2_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_2_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC4_2_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC5_1" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Recording and determining the amount of cargo carried and/ or the number of passengers</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC5_1_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC5_1_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_1_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure (including recording and determining the amount of cargo carried and/or the number of passengers and the use of default values for the mass of cargo units, if applicable) if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_1_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Unit of cargo/passengers</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_1_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_1_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formulae and data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_1_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_1_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_1_8" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC5_2" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Determining and recording the average density of the cargoes transported</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC5_2_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC5_2_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_2_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure (including recording and managing cargo density information) if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_2_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_2_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formulae and data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_2_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_2_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC5_2_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC6_1" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Determining and recording the time spent at sea from berth of port of departure to berth of the port of arrival</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC6_1_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC6_1_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_1_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure (including recording and managing port departure and arrival information) if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_1_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_1_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formulae and data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_1_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_1_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_1_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleC6_2" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of procedure</th>
                                <th style="width: 50%">Procedures for determining and recording the time spent at sea when navigating through ice</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Reference to existing procedure</td>
                                <td style="width: 50%">
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlC6_2_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblC6_2_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_2_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedure (including recording and managing port departure and arrival and winter conditions information) if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_2_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_2_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formulae and data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_2_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_2_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblC6_2_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>

            <h3>Part D Data gaps</h3>
            <telerik:RadDockZone ID="RadDockZone4" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleD1" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Method to be used to estimate fuel consumption</th>
                            </tr>
                            <tr>
                                <td style="width: 50%">Back-up monitoring method (A/B/C/D)</td>
                                <td style="width: 50%">
                                    <telerik:RadLabel ID="lblD1_1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Formula used</td>
                                <td>
                                    <telerik:RadLabel ID="lblD1_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of method to estimate fuel consumption</td>
                                <td>
                                    <telerik:RadLabel ID="lblD1_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblD1_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblD1_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblD1_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblD1_7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleD2" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Method to treat data gaps regarding distance travelled</th>
                            </tr>
                            <tr>
                                <td>Formula used</td>
                                <td>
                                    <telerik:RadLabel ID="lblD2_1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of method to treat data gaps</td>
                                <td>
                                    <telerik:RadLabel ID="lblD2_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblD2_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblD2_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblD2_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblD2_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleD3" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Method to treat data gaps regarding cargo carried</th>
                            </tr>
                            <tr>
                                <td>Formula used</td>
                                <td>
                                    <telerik:RadLabel ID="lblD3_1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of method to treat data gaps</td>
                                <td>
                                    <telerik:RadLabel ID="lblD3_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblD3_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblD3_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblD3_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblD3_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleD4" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Method to treat data gaps regarding time spent at sea</th>
                            </tr>
                            <tr>
                                <td>Formula used</td>
                                <td>
                                    <telerik:RadLabel ID="lblD4_1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of method to treat data gaps</td>
                                <td>
                                    <telerik:RadLabel ID="lblD4_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblD4_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Data sources</td>
                                <td>
                                    <telerik:RadLabel ID="lblD4_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblD4_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblD4_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>

            <h3>Part E Management</h3>
            <telerik:RadDockZone ID="RadDockZone5" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleE1" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Regular check of the adequancy of the monitoring plan</th>
                            </tr>
                            <tr>
                                <td>Reference to existing procedure</td>
                                <td>
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlE1_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblE1_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblE1_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblE1_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblE1_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblE1_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblE1_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleE2" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of Procedure</th>
                                <th style="width: 50%">Information Technology Management (e.g. access controls, back up, recovery and security)</th>
                            </tr>
                            <tr>
                                <td>Reference to existing procedure</td>
                                <td>
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlE2_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblE2_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Brief description of procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblE2_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblE2_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblE2_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblE2_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>List of relevant existing management systems</td>
                                <td>
                                    <telerik:RadLabel ID="lblE2_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleE3" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Internal reviews and validation of EU MRV relevant data</th>
                            </tr>
                            <tr>
                                <td>Reference to existing procedure</td>
                                <td>
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlE3_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblE3_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblE3_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblE3_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblE3_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblE3_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblE3_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleE4" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Corrections and corrective actions</th>
                            </tr>
                            <tr>
                                <td>Reference to existing procedure</td>
                                <td>
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlE4_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblE4_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblE4_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblE4_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblE4_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblE4_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblE4_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleE5" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Outsourced activities</th>
                            </tr>
                            <tr>
                                <td>Reference to existing procedure</td>
                                <td>
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlE5_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblE5_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblE5_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblE5_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblE5_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblE5_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblE5_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleE6" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table style="width: 100%;" class="table rfdTable rfdOptionList">
                            <tr>
                                <th style="width: 50%">Title of method</th>
                                <th style="width: 50%">Documentation</th>
                            </tr>
                            <tr>
                                <td>Reference to existing procedure</td>
                                <td>
                                    <a id="link" href="" class="applinks">
                                        <asp:HyperLink ID="hlE6_1" Target="_blank" runat="server" ToolTip="Download"
                                    Style="text-decoration: underline; cursor: pointer; color: Blue;">
                                </asp:HyperLink>
                                    </a>
                                    <telerik:RadLabel ID="lblE6_1" runat="server" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Version of existing procedure</td>
                                <td>
                                    <telerik:RadLabel ID="lblE6_2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Description of EU MRV procedures if not already existing outside the MP</td>
                                <td>
                                    <telerik:RadLabel ID="lblE6_3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of person or position responsible for this method</td>
                                <td>
                                    <telerik:RadLabel ID="lblE6_4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Location where records are kept</td>
                                <td>
                                    <telerik:RadLabel ID="lblE6_5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of IT system used (where applicable)</td>
                                <td>
                                    <telerik:RadLabel ID="lblE6_6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>

            </telerik:RadDockZone>
            <h3>Part F Further information</h3>

            <telerik:RadDockZone ID="RadDockZone6" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleF1" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <asp:Repeater ID="rptF1" runat="server">
                            <HeaderTemplate>
                                <table style="width: 100%;" class="table rfdTable rfdOptionList">
                                    <tr>
                                        <th style="width: 50%">Abbreviation,acronym,definition</th>
                                        <th style="width: 50%">Explanation</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 50%;">
                                        <telerik:RadLabel ID="lblF1_1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>
                                    </td>
                                    <td style="width: 50%;">
                                        <telerik:RadLabel ID="lblF1_2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPLANATION") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="2" style="text-align: center">
                                        <telerik:RadLabel ID="lblrptF1" runat="server" Font-Bold="true" Text="NO RECORDS FOUND"></telerik:RadLabel>
                                    </td>
                                </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </telerik:RadDock>
                <telerik:RadDock Width="99%" RenderMode="Lightweight" EnableDrag="false" ID="docTitleF2" runat="server" Title="" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" Closed="False" Collapsed="true">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table class="table rfdTable rfdOptionList" style="width: 100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblF2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <br />
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
