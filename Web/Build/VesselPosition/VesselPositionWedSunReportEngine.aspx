<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionWedSunReportEngine.aspx.cs" Inherits="VesselPositionWedSunReportEngine" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Wed Sun Report Engine</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmWedSunReport" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
        <asp:UpdatePanel runat="server" ID="pnlWedSunReportData">
            <ContentTemplate>
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <eluc:Status runat="server" ID="ucStatus" />
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                        <eluc:Title runat="server" ID="Title1" Text="WedSun Report" ShowMenu="true">
                        </eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuWedSunReportTap" TabStrip="true" runat="server" OnTabStripCommand="WedSunReportTapp_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div class="subHeader" style="top: 28px; position: absolute; z-index: +1">
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                            <eluc:TabStrip ID="MenuNRSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuNRSubTab_TabStripCommand">
                            </eluc:TabStrip>
                        </div>
                    </div>
                    <div class="subHeader" style="top: 60px; position: absolute;">
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand">
                            </eluc:TabStrip>
                        </div>
                    </div>
                </div>
                <div id="div2" style="top: 80px; position: relative; z-index: +2">
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 15%;">
                                <asp:Literal ID="lblEngineDistance" runat="server" Text="Engine Distance"></asp:Literal>
                            </td>
                            <td style="width: 25%;">
                                <eluc:Number ID="txtEngineDistance" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                &nbsp; <asp:Literal ID="lblEngineDistancenm" runat="server" Text="nm"></asp:Literal>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td id="lblLogSpeed">
                                <asp:Literal ID="lblSlip" runat="server" Text="Slip"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtSlip" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" MaxLength = "9" />
                                &nbsp; <asp:Literal ID="lblSlipPercentage" runat="server" Text="%"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblERTemp" runat="server" Text="ER Temp"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtERExhTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                &nbsp; &deg; <asp:Literal ID="lblERTempC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSWTemp" runat="server" Text="SW Temp"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtSwellTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                &nbsp; &deg; <asp:Literal ID="lblSWTempC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSWPress" runat="server" Text="SW Press"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtSWPress" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                                &nbsp; <asp:Literal ID="lblSWPressbar" runat="server" Text="bar"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <h3> <u><asp:Literal ID="lblMainEngine" runat="server" Text="Main Engine"></asp:Literal></u> </h3>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRPM" runat="server" Text="RPM"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtMERPM" CssClass="input" MaxLength="9" IsPositive="true" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPowerOutput" runat="server" Text="Power Output"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtBHP" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" />
                                &nbsp; <asp:Literal ID="lblPowerOutputbhp" runat="server" Text="bhp"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblGovernorSetting" runat="server" Text="Governor Setting / Fuel rack"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtGovernorSetting" CssClass="input" MaxLength="9" DecimalPlace="2" IsPositive="true" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSpeedSetting" runat="server" Text="Speed Setting"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtSpeedSetting" CssClass="input" MaxLength="9" IsInteger="true" IsPositive="true" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblMaxExhTemp" runat="server" Text="Exh Temp Max"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtMaxExhTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                &nbsp; &deg; <asp:Literal ID="lblMaxExhTempC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblExhTempMin" runat="server" Text="Exh Temp Min"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtMinExhTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                &nbsp; &deg; <asp:Literal ID="lblExhTempMinC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblScavAirTemp" runat="server" Text="Scav Air Temp"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtScavAirTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                &nbsp; &deg; <asp:Literal ID="lblScavAirTempC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblFOInletTemp" runat="server" Text="FO Inlet Temp"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtFOInletTemp" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                                &nbsp; &deg; <asp:Literal ID="lblFOInletTempC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblScavAirPress" runat="server" Text="Scav Air Press"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtScavAirPress" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                                &nbsp; <asp:Literal ID="lblScavAirPressbar" runat="server" Text="bar"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblFuelOilPress" runat="server" Text="Fuel Oil Press"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtFuelOilPress" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                                &nbsp; <asp:Literal ID="lblFuelOilPressbar" runat="server" Text="bar"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblTC1RPM" runat="server" Text="T/C1 RPM"></asp:Literal> 
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtTCRPMInboard" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblTC2RPM" runat="server" Text="T/C2 RPM"></asp:Literal> 
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtTCRPMOutboard" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblExhTCInboardBefore" runat="server" Text="T/C1 Exh Gas Temp In"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtExhTCInboardBefore" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" />
                                &nbsp; &deg; <asp:Literal ID="lblExhTCInboardBeforeC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblExhTCInboardAfter" runat="server" Text="T/C1 Exh Gas Temp Out"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtExhTCInboardAfter" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" />
                                &nbsp; &deg; <asp:Literal ID="lblExhTCInboardAfterC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblExhTCOutboardBefore" runat="server" Text="T/C2 Exh Gas Temp In"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtExhTCOutboardBefore" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" />
                                &nbsp; &deg; <asp:Literal ID="lblExhTCOutboardBeforeC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblExhTCOutboardAfter" runat="server" Text="T/C2 Exh Gas Temp Out"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtExhTCOutboardAfter" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" />
                                &nbsp; &deg; <asp:Literal ID="lblExhTCOutboardAfterC" runat="server" Text="C"></asp:Literal>
                            </td>
                        </tr>                  
                        <tr>
                            <td colspan="4">
                                <h3><u><asp:Literal ID="lblAuxEngine" runat="server" Text="Aux Engine"></asp:Literal></u></h3>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblGeneralLoadAE1" runat="server" Text="A/E No 1. Generator Load"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtGeneralLoadAE1" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                                &nbsp; <asp:Literal ID="lblGeneralLoadAE1kw" runat="server" Text="kw"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblGeneralLoadAE2" runat="server" Text="A/E No 2. Generator Load"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtGeneralLoadAE2" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                                &nbsp; <asp:Literal ID="lblGeneralLoadAE2kw" runat="server" Text="kw"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblGeneralLoadAE3" runat="server" Text="A/E No 3. Generator Load"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number ID="txtGeneralLoadAE3" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                                &nbsp; <asp:Literal ID="lblGeneralLoadAE3kw" runat="server" Text="kw"></asp:Literal>
                            </td>
                        </tr>
                        </table>
                        <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td colspan="4">
                                <h3><u><asp:Literal ID="lblFreshWater" runat="server" Text="Fresh Water"></asp:Literal></u></h3>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="divGrid" style="position: relative; z-index: 0">
                                    <asp:GridView ID="gvOtherOilCons" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="3" OnRowCreated="gvOtherOilCons_RowCreated" OnRowCommand="gvOtherOilCons_RowCommand" OnRowDataBound="gvOtherOilCons_ItemDataBound"
                                        OnRowEditing="gvOtherOilCons_RowEditing" OnRowUpdating="gvOtherOilCons_RowUpdating" OnRowDeleting="gvOtherOilCons_RowDeleting"
                                        OnRowCancelingEdit="gvOtherOilCons_RowCancelingEdit" ShowFooter="false" ShowHeader="true"
                                        EnableViewState="false">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblFreshWaterHeader" runat="server" Text="Fresh Water"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOilTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></asp:Label>
                                                    <asp:Label ID="lblOilType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblPreviousROBHeader" runat="server" Text="Previous ROB"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSROB") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblROBHeader" runat="server" Text="ROB"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB") %>'></asp:Label>    
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:Number ID="txtROBEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB") %>' IsPositive="true"
                                                        MaxLength="9" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblProducedHeader" runat="server" Text="Produced"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProduced" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCED") %>'></asp:Label>   
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:Number ID="txtProducedEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCED") %>' IsPositive="true"
                                                        MaxLength="9" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblConsumptionHeader" runat="server" Text="Consumption"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConsumption" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMPTIONQTY") %>'></asp:Label> 
                                                    <asp:Label ID="lblOilConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></asp:Label>      
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblConsumptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMPTIONQTY") %>'></asp:Label> 
                                                    <asp:Label ID="lblOilConsumptionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></asp:Label>
                                                    <asp:Label ID="lblOilTypeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActionHeader" runat="server" Text="Action"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                                        ToolTip="Edit"></asp:ImageButton>
                                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                                        ToolTip="Delete"></asp:ImageButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                                        ToolTip="Save"></asp:ImageButton>
                                                    <img id="Img4" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                                        ToolTip="Cancel"></asp:ImageButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                
                            </td>
                        </tr>
                        </table>
                        <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 15%;">
                                <br />
                            </td>
                            <td style="width: 25%;">
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBoilerWaterChlorides" runat="server" Text="Boiler water Chlorides"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtBoilerWaterChlorides" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                                &nbsp; <asp:Literal ID="lblBoilerWaterChloridesppm" runat="server" Text="ppm"></asp:Literal>
                            </td>
                        </tr>                                 
                        <tr>
                            <td colspan="4">
                                <h3><u><asp:Literal ID="lblBilgeandSludge" runat="server" Text="Bilge and Sludge"></asp:Literal></u></h3>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBilgeROB" runat="server" Text="Bilge Tank ROB"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtBilgeROB" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                                &nbsp; <asp:Literal ID="lblBilgeROBcum" runat="server" Text="cu. m"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSludgeROB" runat="server" Text="Sludge Tank ROB"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Number runat="server" ID="txtSludgeROB" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                                &nbsp; <asp:Literal ID="lblSludgeROBcum" runat="server" Text="cu. m"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBilgeLanding" runat="server" Text="Last landing of Bilge Water"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Date runat="server" ID="txtBilgeLanding" Enabled="false" CssClass="readonlytextbox" />
                                <asp:TextBox ID="txtBilgeLandingTime" runat="server" Enabled="false" CssClass="readonlytextbox" Width="50px" Visible="false"/>
                                <ajaxToolkit:MaskedEditExtender ID="mskBilgeLandingTime" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtBilgeLandingTime" UserTimeFormat="TwentyFourHour" />
                                &nbsp;&nbsp;
                                <asp:Literal ID="lblBilgeLandingDays" runat="server" Text="Days"></asp:Literal>
                                <eluc:Number runat="server" ID="txtBilgeLandingDays" CssClass="readonlytextbox" Enabled="false" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblLastLandSludge" runat="server" Text="Last landing of Sludge"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <eluc:Date runat="server" ID="txtLastLandSludge" Enabled="false" CssClass="readonlytextbox" />
                                <asp:TextBox ID="txtLastLandSludgeTime" runat="server" Enabled="false" CssClass="readonlytextbox" Width="50px" Visible="false" />
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderHireTo" runat="server" AcceptAMPM="false"
                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                    TargetControlID="txtLastLandSludgeTime" UserTimeFormat="TwentyFourHour" />
                                &nbsp;&nbsp;
                                <asp:Literal ID="lblLastLandSludgeDays" runat="server" Text="Days"></asp:Literal>
                                <eluc:Number runat="server" ID="txtLastLandingDays" CssClass="readonlytextbox" Enabled="false" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="lblRemarksCE" runat="server" Text="Chief Engineer Remarks"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemarksCE" runat="server" CssClass="input" Height="50px" TextMode="MultiLine" Width="50%" />
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td colspan="4">
                                <h3><u><asp:Literal ID="lblmConsumption" runat="server" Text="Consumption"></asp:Literal></u></h3>
                            </td>
                        </tr>
                        <tr style="color: Blue">
                            <td colspan="4">
                               <asp:Literal ID="lblFuelOilCons" runat="server" Text="* Fuel Oil Consumption in mT"></asp:Literal>
                               <br />
                               <asp:Literal ID="lblLubOilCons" runat="server" Text="* Lub Oil Consumption in Ltr"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="gvConsumption" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="2" OnRowCommand="gvConsumption_RowCommand" OnRowDataBound="gvConsumption_ItemDataBound"
                                    OnRowDeleting="gvConsumption_RowDeleting" AllowSorting="true" OnRowEditing="gvConsumption_RowEditing"
                                    OnRowCreated="gvConsumption_RowCreated" OnRowCancelingEdit="gvConsumption_RowCancelingEdit"
                                    OnRowUpdating="gvConsumption_RowUpdating" ShowHeader="true" EnableViewState="false">
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPE")%>'></asp:Label>
                                                <asp:Label ID="lbloilconsumptiononlaterdateyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILCONSUMPTIONONLATERDATEYN")%>'></asp:Label>
                                                <asp:Label ID="lblOilTypeName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPENAME")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPreviousRob" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPREVIOUSROB")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtSeaMEHeader" Visible="true" Text=" M/E " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtSeaME" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAME")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtSeaMEEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAME")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtSeaAEHeader" Visible="true" Text=" A/E " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtSeaAE" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAAE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtSeaAEEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAAE")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtSeaBLRHeader" Visible="true" Text=" BLR " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtSeaBLR" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEABLR")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtSeaBLREdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEABLR")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtSeaIGGHeader" Visible="true" Text="IGG" runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtSeaIGG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAIGG")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtSeaIGGEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAIGG")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtSeaCARGOENGHeader" Visible="true" Text="CARGO ENG" runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtSeaCARGOENG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACARGOENG")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtSeaCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACARGOENG")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtSeaOTHHeader" Visible="true" Text=" OTH " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtSeaOTH" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAOTH")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtSeaOTHEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAOTH")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtHourbourMEHeader" Visible="true" Text=" M/E " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtHourbourME" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURME")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtHourbourMEEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURME")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtHourbourAEHeader" Visible="true" Text=" A/E " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtHourbourAE" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURAE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtHourbourAEEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURAE")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtHourbourBLRHeader" Visible="true" Text=" BLR " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtHourbourBLR" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURBLR")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtHourbourBLREdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURBLR")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false"> 
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtHourbourIGGHeader" Visible="true" Text="IGG" runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtHourbourIGG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURIGG")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtHourbourIGGEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURIGG")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtHourbourCARGOENGHeader" Visible="true" Text="CARGO ENG" runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtHourbourCARGOENG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCARGOENG")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtHourbourCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCARGOENG")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblAtHourbourOTHHeader" Visible="true" Text=" OTH " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAtHourbourOTH" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOUROTH")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucAtHourbourOTHEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOUROTH")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblInPortMEHeader" Visible="true" Text=" M/E " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInPortME" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTME")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucInPortMEEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTME")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblInPortAEHeader" Visible="true" Text=" A/E " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInPortAE" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTAE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucInPortAEEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTAE")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblInPortBLRHeader" Visible="true" Text=" BLR " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInPortBLR" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTBLR")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucInPortBLREdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTBLR")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblInPortIGGHeader" Visible="true" Text="IGG" runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInPortIGG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTIGG")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucInPortIGGEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTIGG")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblInPortCARGOENGHeader" Visible="true" Text="CARGO ENG" runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInPortCARGOENG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCARGOENG")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucInPortCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCARGOENG")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblInPortOTHHeader" Visible="true" Text=" OTH " runat="server">                                        
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInPortOTH" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTOTH")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Number ID="ucInPortOTHEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="30px"
                                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTOTH")%>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTOTALCONSUMPTION")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRobAtNoon" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROBATNOON")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                                    ToolTip="Edit"></asp:ImageButton>
                                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                    width="3" />
                                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                                    ToolTip="Delete"></asp:ImageButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                    CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                                    ToolTip="Save"></asp:ImageButton>
                                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                    width="3" />
                                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                    ToolTip="Cancel"></asp:ImageButton>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 15%;">
                                <asp:Literal ID="lblTankCleaning" runat="server" Text="HFO Cons for Tank Cleaning (if any)"></asp:Literal>
                            </td>
                            <td style="width: 25%;">
                                <eluc:Number runat="server" ID="txtHFOTankCleaning" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                <asp:Literal ID="lblCargoHeating" runat="server" Text="HFO Cons for Cargo Heating (if any)"></asp:Literal>
                            </td>
                            <td style="width: 25%;">
                                <eluc:Number runat="server" ID="txtHFOCargoHeating" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />   
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
