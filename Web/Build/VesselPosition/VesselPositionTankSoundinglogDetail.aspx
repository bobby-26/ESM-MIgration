<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionTankSoundinglogDetail.aspx.cs" Inherits="VesselPositionTankSoundinglogDetail" %>

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
    <title>Tank Sounding Log</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .table {
                border-collapse: collapse;
            }
                .table td {
                    text-align: center;
                    height: 25px;
                }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVoyageList" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVoyageList">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuStockCheckTap" runat="server" OnTabStripCommand="MenuStockCheckTap_abStripCommand"></eluc:TabStrip>
            <h3>
                <telerik:RadLabel runat="server" ID="lblTableHead" Text="Fuel Oil Total (mT)"></telerik:RadLabel>
            </h3>
            <div runat="server" id="sumtable"></div>
            <table class="rfdTable rfdOptionList table" width="100%" style="display:none;">
                <tr>
                    <th><b>
                        <telerik:RadLabel runat="server" ID="lblHFO" Text="Heavy Fuel Oil"></telerik:RadLabel>
                    </b></th>
                    <th><b>
                        <telerik:RadLabel runat="server" ID="lblLSFO" Text="Low Sulphur Fuel Oil"></telerik:RadLabel>
                    </b></th>
                    <th><b>
                        <telerik:RadLabel runat="server" ID="lblMDO" Text="Marine Diesel Oil"></telerik:RadLabel>
                    </b></th>
                    <th><b>
                        <telerik:RadLabel runat="server" ID="lblMGO" Text="Marine Gas Oil"></telerik:RadLabel>
                    </b></th>
                    <th><b>
                        <telerik:RadLabel runat="server" ID="lblLSMGO" Text="Low Sulphur Marine Gas Oil"></telerik:RadLabel>
                    </b></th>
                </tr>
                <tr>
                    <td><b>
                        <telerik:RadLabel runat="server" ID="lblHFOValue"></telerik:RadLabel>
                    </b></td>
                    <td><b>
                        <telerik:RadLabel runat="server" ID="lblLSFOValue"></telerik:RadLabel>
                    </b></td>
                    <td><b>
                        <telerik:RadLabel runat="server" ID="lblMDOValue" Text="MDO"></telerik:RadLabel>
                    </b></td>
                    <td><b>
                        <telerik:RadLabel runat="server" ID="lblMGOValue" Text="MGO"></telerik:RadLabel>
                    </b></td>
                    <td><b>
                        <telerik:RadLabel runat="server" ID="lblLSMGOValue" Text="LSMGO"></telerik:RadLabel>
                    </b></td>
                </tr>
            </table>
            <br />
            <telerik:RadGrid ID="gvTankSoundinglog" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvTankSoundinglog_RowCommand" OnItemDataBound="gvTankSoundinglog_ItemDataBound"
                OnUpdateCommand="gvTankSoundinglog_RowUpdating" OnNeedDataSource="gvTankSoundinglog_NeedDataSource"
                ShowFooter="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
                ShowHeader="true" EnableViewState="false" AllowSorting="false">
                <SortingSettings SortToolTip="" />    
                 <ClientSettings > 
                   <ClientMessages DragToGroupOrReorder="" DragToResize="" DropHereToReorder="" />    
                 </ClientSettings> 
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDTANKSOUNDINGLOGDETAILID">

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
                        <telerik:GridTemplateColumn HeaderText="Tank" HeaderStyle-Width="15%" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Product" HeaderStyle-Wrap="false" HeaderStyle-Width="15%" >
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTanksoundinlogid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKSOUNDINGLOGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTanksoundinlogDetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKSOUNDINGLOGDETAILID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblProduct" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTanksoundinlogidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKSOUNDINGLOGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTanksoundinlogDetailidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKSOUNDINGLOGDETAILID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblProductid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTID") %>'></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlOilType" runat="server" Width="98%" CssClass="gridinput" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME"></telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S/U" HeaderTooltip="Sounding / Ullage" HeaderStyle-Wrap="false" HeaderStyle-Width="14%" UniqueName="sounding" >
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSU" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOUNDINGORULLAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlSU" runat="server" CssClass="input_mandatory" Width="98%">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="S" Text="S"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="U" Text="U"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sounding (cm)" HeaderStyle-Wrap="false" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSounding" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOUNDINGCM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtSounding" DecimalPlace="1" MaxLength="5" Width="98%" CssClass="gridinput" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDSOUNDINGCM")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Temp ˚C" HeaderStyle-Wrap="false" HeaderStyle-Width="7%" >
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTemp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTEMPERATURE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtTemp" DecimalPlace="1" Width="98%" MaxLength="4" CssClass="gridinput" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDTEMPERATURE")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vol Corrected (m3)" HeaderStyle-Wrap="false" HeaderStyle-Width="12%" >
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVolCorrected" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDVOLUMECORRECTED")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtVolCorrected" Width="98%" DecimalPlace="2" MaxLength="6" CssClass="gridinput" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDVOLUMECORRECTED")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Density at 15˚C" HeaderStyle-Wrap="false" HeaderStyle-Width="10%" >
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDensity" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDDENSITY")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number runat="server" ID="txtDensity" DecimalPlace="4" Width="98%" MaxLength="6" CssClass="gridinput" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDDENSITY")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Mass (MT)" HeaderStyle-Wrap="false" HeaderStyle-Width="8%" >
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMass" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDMASS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Wrap="false" HeaderStyle-Width="8%" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
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
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
