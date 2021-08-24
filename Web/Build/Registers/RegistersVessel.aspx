<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVessel.aspx.cs"
    Inherits="RegistersVessel" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EngineModel" Src="~/UserControls/UserControlEngineModel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EngineType" Src="~/UserControls/UserControlEngineType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Propulsion" Src="~/UserControls/UserControlPropulsion.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CharterAddress" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DPClass" Src="~/UserControls/UserControlDPClass.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Particulars</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .table {
            border-collapse: collapse;
        }

            .table td, th {
                border: 1px solid black;
            }

        /*.accordian_voluntary {
            background-color: blue;
        }*/
    </style>
</head>
<body>
    <form id="frmRegisterVessel" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <%--        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>--%>
        <div style="font-weight: 600;" runat="server">
            <eluc:TabStrip ID="MenuVesselList" runat="server" OnTabStripCommand="MenuVesselList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" RenderMode="Lightweight" DecoratedControls="All" DecorationZoneID="table1,table2" EnableRoundedCorners="true" />
        <%-- <asp:UpdatePanel runat="server" ID="pnlVesselListEntry">
        <ContentTemplate>--%>
        <%--<div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">--%>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" Visible="false" />
        <eluc:TabStrip ID="MenuVessel" runat="server" OnTabStripCommand="Vessel_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td style="width: 25%">
                    <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td style="width: 25%">
                    <telerik:RadTextBox runat="server" ID="txtVesselName" MaxLength="100" Width="345px" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
                <td style="width: 25%; align-self: center">
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td style="width: 25%">
                    <eluc:VesselType ID="ucVesselType" runat="server" Width="345px" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblIMONumber" runat="server" Text="IMO Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtIMONumber" MaxLength="50" CssClass="input" Width="345px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblOfficialNumber" runat="server" Text="Official Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtOfficialNumber" CssClass="input" MaxLength="50"
                        Width="345px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCallSign" runat="server" Text="Call Sign"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtCallSign" CssClass="input" MaxLength="100" Width="345px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMMSINo" runat="server" Text="MMSI No."></telerik:RadLabel>
                </td>
                <td>
                    <%--<telerik:RadTextBox runat="server" ID="txtMMSINo" MaxLength="100" Width="345px" Style="text-align: right;"
                            CssClass="input"></telerik:RadTextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" TargetControlID="txtMMSINo"
                            Mask="999999999" MaskType="Number" InputDirection="RightToLeft">
                        </ajaxToolkit:MaskedEditExtender>--%>
                    <telerik:RadMaskedTextBox runat="server" ID="txtMMSINo" MaxLength="100" Width="345px" Style="text-align: right;" CssClass="input" Mask="#########"></telerik:RadMaskedTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblClassification" runat="server" Text="Classification"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiAddress runat="server" ID="ucClassName" AddressType='<%# ((int)PhoenixAddressType.CLASSIFICATIONSOCIETY).ToString() %>' />
                </td>
                <td>
                    <telerik:RadLabel ID="lblClassNotation" runat="server" Text="Class Notation"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtClassNotation" MaxLength="100"
                        Width="345px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVesselShortCode" runat="server" Text="Vessel Short Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtVesselCode" CssClass="input_mandatory" MaxLength="100"
                        Width="345px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblIceClass" runat="server" Text="Ice Class"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="ddlIceClassed" Visible="false">
                        <Items>
                            <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                            <telerik:RadComboBoxItem Value="1" Text="Yes" />
                            <telerik:RadComboBoxItem Value="0" Text="No" />
                        </Items>
                    </telerik:RadComboBox>
                    <telerik:RadComboBox runat="server" ID="ddlIceClass" Width="345px"></telerik:RadComboBox>

                    <%-- <asp:DropDownList runat="server" ID="ddlIceClassed" CssClass="input" Visible="false">
                            <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                        </asp:DropDownList>--%>
                    <%--<asp:DropDownList runat="server" ID="ddlIceClass" CssClass="input" Width="345px">
                        </asp:DropDownList>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Flag ID="ucFlag" runat="server" Width="345px" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPortofRegistry" runat="server" Text="Port of Registry"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Multiport ID="ucPortRegistered" runat="server" Width="345px" />
                    <%--<eluc:Port ID="ucPortRegistered" runat="server" Width="80%" AppendDataBoundItems="true"
                                CssClass="input" />--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                </td>
                <td style="width: 20%; z-index: -1;">
                    <eluc:MultiAddress runat="server" ID="ucAddrOwner" AddressType='<%# ((int)PhoenixAddressType.OWNER).ToString() %>'
                        Width="80%" CssClass="input_mandatory" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiAddress runat="server" ID="ucPrincipal" AddressType='<%# ((int)PhoenixAddressType.PRINCIPAL).ToString() %>'
                        Width="80%" CssClass="input_mandatory" />
                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblManager" runat="server" Text="Manager"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiAddress runat="server" ID="ucAddrPrimaryManager" AddressType='<%# ((int)PhoenixAddressType.MANAGER).ToString() %>'
                        Width="80%" CssClass="input_mandatory" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblCharterer" runat="server" Text="Charterer"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiAddress runat="server" ID="ucCharterer" Width="80%" Visible="false" />
                    <telerik:RadLabel ID="lblMatrixStandardId" runat="server" Visible="false"></telerik:RadLabel>
                    <telerik:RadTextBox runat="server" ID="txtMatrixStandard" MaxLength="20" Style="text-align: left;"
                        Width="78%" ReadOnly="true" CssClass="readonlytextbox" Visible="false">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>

                    <telerik:RadLabel ID="lblBuilder" runat="server" Text="Builder"></telerik:RadLabel>
                </td>
                <td>

                    <eluc:MultiAddress runat="server" ID="ucAddrYard" AddressType='<%# ((int)PhoenixAddressType.YARD).ToString() %>'
                        Width="80%" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblHullNo" runat="server" Text="Hull No."></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadMaskedTextBox runat="server" ID="txtHullNo" MaxLength="20" Style="text-align: right;"
                        Width="345px" CssClass="input" Mask="#######" DisplayFormatPosition="Right">
                    </telerik:RadMaskedTextBox>
                    <%--                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtHullNo"
                            Mask="9999999" MaskType="Number" InputDirection="RightToLeft">
                        </ajaxToolkit:MaskedEditExtender>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDisponentOwner" runat="server" Text="Disponent Owner" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiAddress runat="server" ID="ucDisponentOwner" AddressType='<%# ((int)PhoenixAddressType.OWNER).ToString() %>'
                        CssClass="input" Visible="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblClassNo" runat="server" Text="Class No" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtClassNo" CssClass="input" MaxLength="100" Visible="false"></telerik:RadTextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadLabel ID="lblKeelLaid" runat="server" Text="Keel Laid"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtKeelLaidDate" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblLaunched" runat="server" Text="Launched"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtLaunchedDate" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDelivery" runat="server" Text="Delivery"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtDeliveryDate" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblESMTakeover" runat="server" Text="Takeover date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtTakeoverDateByESM" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblManagementType" runat="server" Text="Management Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard runat="server" ID="ucManagementType" AppendDataBoundItems="true" AutoPostBack="true"
                        HardTypeCode="31" CssClass="dropdown_mandatory" Width="345px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblESMHandoverdate" runat="server" Text="Handover date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtESMHandoverDate" Enabled="false" />
                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOperatingArea" runat="server" Text="Operating Area"></telerik:RadLabel>
                </td>
                <td width="25%">
                    <eluc:Quick ID="ucNavigationArea" runat="server" AppendDataBoundItems="true"
                        QuickTypeCode="121" Width="345px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblFittedwithFramo" runat="server" Text="Fitted with Framo"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="ddlFittedwithFramo" Width="345px">
                        <Items>
                            <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                            <telerik:RadComboBoxItem Value="1" Text="Yes" />
                            <telerik:RadComboBoxItem Value="0" Text="No" />
                        </Items>
                    </telerik:RadComboBox>
                    <%--                    <asp:DropDownList runat="server" ID="ddlFittedwithFramo" CssClass="input" Width="345px">
                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                    </asp:DropDownList>--%>

                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblvesselyn" runat="server" Text="Entity Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="ddlentitytype" Width="345px">
                    </telerik:RadComboBox>
                    <%--<asp:DropDownList runat="server" ID="ddlentitytype" CssClass="input" Width="345px">
                    </asp:DropDownList>--%>
                </td>
                <td>
                    <telerik:RadLabel ID="lblpscalert" runat="server" Text="PSC Alert"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="ucpscalert" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" Type="Number" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOffshoreCharterer" runat="server" Text="Charterer" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:CharterAddress ID="ucOffshoreCharterer" runat="server" AppendDataBoundItems="true"
                        AutoPostBack="true" OnTextChangedEvent="OffshoreChartererStanderdbind" CssClass="input"
                        Width="260px" Visible="false" AddressType='<%# ((int)PhoenixAddressType.CHARTERER).ToString() %>' />
                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel17" runat="server" Text="US Visa required"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkusvisa" runat="server"></telerik:RadCheckBox>
                </td>
                <td>
                    <telerik:RadLabel ID="RadLabel18" runat="server" Text=" Australian MCV Visa required"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkaustralianvisa" runat="server"></telerik:RadCheckBox>

                </td>
            </tr>
        </table>
        <hr />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td colspan="4">
                    <b>
                        <telerik:RadLabel ID="lblPrincipalDimensions" runat="server" Text="Principal Dimensions"></telerik:RadLabel>
                    </b>
                </td>
                <td colspan="2">
                    <b>
                        <telerik:RadLabel ID="lblPropeller" runat="server" Text="Propeller"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLOA" runat="server" Text="LOA"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtLoa" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblLoaUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtLoa" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender12" runat="server" TargetControlID="txtLoa"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <telerik:RadLabel ID="lblLBP" runat="server" Text="LBP"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtLBP" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblLBPUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtLBP" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender8" runat="server" TargetControlID="txtLBP"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPropDiameter" runat="server" Text="Diameter"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtPropDiameter" runat="server" MaxLength="6" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblPropDiameterUnits" runat="server" Text="m"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblBreadthext" runat="server" Text="Breadth (ext)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtBreath" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblBreathUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtBreath" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="mskTxtBreath" runat="server" TargetControlID="txtBreath"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDepthmld" runat="server" Text="Depth (mld)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtDepth" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDepthUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDepth" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDepth"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPropPitch" runat="server" Text="Pitch"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtPropPitch" runat="server" MaxLength="6" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblPropPitchUnits" runat="server" Text="m"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblHeightmax" runat="server" Text="Height (max)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtHeight" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblHeightUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtHeight" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender20" runat="server" TargetControlID="txtHeight"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td colspan="2"></td>
            </tr>
        </table>
        <hr />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblTonnage" runat="server" Text="Tonnage"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <telerik:RadLabel ID="lblRegistered" runat="server" Text="Registered"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSuez" runat="server" Text="Suez"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPanama" runat="server" Text="Panama"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblGross" runat="server" Text="Gross"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtRegisteredGT" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <%--<telerik:RadTextBox runat="server" ID="txtRegisteredGT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender15" runat="server" TargetControlID="txtRegisteredGT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtSuezGT" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <%--<telerik:RadTextBox runat="server" ID="txtSuezGT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender22" runat="server" TargetControlID="txtSuezGT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtPanamaGT" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <%--<telerik:RadTextBox runat="server" ID="txtPanamaGT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender23" runat="server" TargetControlID="txtPanamaGT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNet" runat="server" Text="Net"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtRegisteredNT" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <%--<telerik:RadTextBox runat="server" ID="txtRegisteredNT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender24" runat="server" TargetControlID="txtRegisteredNT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtSuezNT" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <%--<telerik:RadTextBox runat="server" ID="txtSuezNT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender25" runat="server" TargetControlID="txtSuezNT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtPanamaNT" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <%--<telerik:RadTextBox runat="server" ID="txtPanamaNT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender26" runat="server" TargetControlID="txtPanamaNT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
            </tr>
        </table>
        <hr />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblLoadLine" runat="server" Text="Load Line"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <telerik:RadLabel ID="lblFreeboard" runat="server" Text="Freeboard"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDraft" runat="server" Text="Draft"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDWT" runat="server" Text="DWT"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTropical" runat="server" Text="Tropical"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtFreeboardTropical" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblFBTUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtFreeboardTropical" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender9" runat="server" TargetControlID="txtFreeboardTropical"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDraftTropical" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDTUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDraftTropical" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender14" runat="server" TargetControlID="txtDraftTropical"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDWTTropical" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDWTTUnits" runat="server" Text="mt"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDWTTropical" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender17" runat="server" TargetControlID="txtDWTTropical"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSummer" runat="server" Text="Summer"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtFreeboardSummer" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblFBSUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtFreeboardSummer" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender27" runat="server" TargetControlID="txtFreeboardSummer"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDraftSummer" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDSUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%-- <telerik:RadTextBox runat="server" ID="txtDraftSummer" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender28" runat="server" TargetControlID="txtDraftSummer"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDWTSummer" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDWTSUnits" runat="server" Text="mt"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDWTSummer" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender29" runat="server" TargetControlID="txtDWTSummer"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblWinter" runat="server" Text="Winter"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtFreeboardWinter" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblFBWUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtFreeboardWinter" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender30" runat="server" TargetControlID="txtFreeboardWinter"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDraftWinter" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDWUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDraftWinter" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender31" runat="server" TargetControlID="txtDraftWinter"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDWTWinter" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDWTWUnits" runat="server" Text="mt"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDWTWinter" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender32" runat="server" TargetControlID="txtDWTWinter"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLightship" runat="server" Text="Lightship"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtFreeboardLightship" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblFBLUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtFreeboardLightship" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender33" runat="server" TargetControlID="txtFreeboardLightship"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDraftLightship" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDLSUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDraftLightship" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender34" runat="server" TargetControlID="txtDraftLightship"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDWTLightship" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDWTLUnits" runat="server" Text="mt"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDWTLightship" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender35" runat="server" TargetControlID="txtDWTLightship"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblBallastCond" runat="server" Text="Ballast Cond"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtFreeboardBallastCond" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblFBBCUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtFreeboardBallastCond" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender36" runat="server" TargetControlID="txtFreeboardBallastCond"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDraftBallastCond" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDBCUnits" runat="server" Text="m"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDraftBallastCond" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender37" runat="server" TargetControlID="txtDraftBallastCond"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
                <td>
                    <eluc:Number ID="txtDWTBallastCond" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblDWTBCUnits" runat="server" Text="mt"></telerik:RadLabel>
                    <%--<telerik:RadTextBox runat="server" ID="txtDWTBallastCond" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></telerik:RadTextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender38" runat="server" TargetControlID="txtDWTBallastCond"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>--%>
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td colspan="2">
                    <b>
                        <telerik:RadLabel ID="ltTEU" runat="server" Text="TEU"></telerik:RadLabel>
                    </b>
                </td>
                <td>
                    <eluc:Number ID="txtTEU" runat="server" CssClass="input txtNumber" MaxLength="10" />
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <hr />
        <br />
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td colspan="3">
                    <b>
                        <telerik:RadLabel ID="lblMainMachinery" runat="server" Text="Main Machinery"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
        </table>
        <table class="table" style="width: 100%;" id="table1" runat="server">
            <tr class="DataGrid-HeaderStyle">
                <th style="text-align: left; width: 100px" rowspan="2">
                    <telerik:RadLabel ID="RadLabel10" runat="server" Text="Machinery"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 30px" rowspan="2">
                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Qty"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 100px" rowspan="2">
                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="Engine Type"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 100px" rowspan="2">
                    <telerik:RadLabel ID="RadLabel3" runat="server" Text="Maker"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 100px" rowspan="2">
                    <telerik:RadLabel ID="RadLabel4" runat="server" Text="Model"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 50px;" rowspan="2">
                    <telerik:RadLabel ID="RadLabel5" runat="server" Text="Rating"></telerik:RadLabel>
                </th>
                <th style="text-align: center; width: 100px" colspan="3">
                    <telerik:RadLabel ID="RadLabel6" runat="server" Text="Power Output"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 50px;" rowspan="2">
                    <telerik:RadLabel ID="RadLabel7" runat="server" Text="RPM"></telerik:RadLabel>
                </th>
                <th style="text-align: center" colspan="2">
                    <telerik:RadLabel ID="RadLabel8" runat="server" Text="SFOC"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 50px;" rowspan="2">
                    <telerik:RadLabel ID="RadLabel9" runat="server" Text="FO Cons"></telerik:RadLabel>
                    <br />
                    <telerik:RadLabel ID="RadLabel11" runat="server" Text="(mt/day)"></telerik:RadLabel>
                </th>
            </tr>
            <tr class="DataGrid-HeaderStyle">
                <th style="text-align: left; width: 50px;">
                    <telerik:RadLabel ID="RadLabel12" runat="server" Text="BHP"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 50px;">
                    <telerik:RadLabel ID="RadLabel13" runat="server" Text="kW"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 50px;">
                    <telerik:RadLabel ID="RadLabel14" runat="server" Text="kg/h"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 50px;">
                    <telerik:RadLabel ID="RadLabel15" runat="server" Text="g/bhp-h"></telerik:RadLabel>
                </th>
                <th style="text-align: left; width: 50px;">
                    <telerik:RadLabel ID="RadLabel16" runat="server" Text="g/kWh"></telerik:RadLabel>
                </th>
            </tr>
            <tr>
                <th style="text-align: left" rowspan="2">Main Engine
                </th>
                <td rowspan="2">
                    <eluc:MaskNumber runat="server" ID="txtMEQuantity" CssClass="input" IsInteger="true" IsPositive="true"
                        Width="96%" />
                </td>
                <td rowspan="2">
                    <eluc:EngineType runat="server" ID="ucEngineType" Width="96%" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory" />
                </td>
                <td rowspan="2">
                    <eluc:MultiAddress ID="txtMEMaker" AddressType="130" runat="server" Width="96%" />
                </td>
                <td rowspan="2">
                    <telerik:RadTextBox runat="server" ID="txtMainEngine" Width="96%"></telerik:RadTextBox>
                </td>
                <td style="text-align: left">MCR
                </td>
                <td>
                    <eluc:MaskNumber ID="txtBHP" runat="server" CssClass="input txtNumber" MaxLength="5"
                        AutoPostBack="true" OnTextChangedEvent="CalculateKW" DecimalPlace="0" Width="96%" />
                </td>
                <td>

                    <eluc:MaskNumber ID="txtKW" runat="server" CssClass="input txtNumber" MaxLength="5" DecimalPlace="0"
                        AutoPostBack="true" OnTextChangedEvent="CalculateBHP" Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MaskNumber ID="txtMCRRPM" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="0" Width="96%" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtMCRgbhph" runat="server" CssClass="input txtNumber" MaxLength="7"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtMESFOC" runat="server" CssClass="input txtNumber" MaxLength="7"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtMCRFOCons" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: left">NCR</td>
                <td>
                    <eluc:MaskNumber ID="txtNCRBHP" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="0" Width="96%" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtNCRkW" runat="server" CssClass="input txtNumber" MaxLength="5" DecimalPlace="0"
                        Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MaskNumber ID="txtNCRRPM" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="0" Width="96%" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtNCRgbhph" runat="server" CssClass="input txtNumber" MaxLength="7"
                        DecimalPlace="2" Width="96%" />

                </td>
                <td>
                    <eluc:MaskNumber ID="txtNCRgkWh" runat="server" CssClass="input txtNumber" MaxLength="7"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtNCRFOCons" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>
            </tr>
            <tr>
                <th style="text-align: left">Aux Engine
                </th>
                <td>
                    <eluc:MaskNumber runat="server" ID="txtAEQuantity" CssClass="input" IsInteger="true" IsPositive="true"
                        Width="96%" />
                </td>
                <td>
                    <eluc:EngineType runat="server" ID="ucAEEngineType" Width="96%" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory" />
                </td>
                <td>
                    <eluc:MultiAddress ID="txtAEMaker" AddressType="130" runat="server" Width="96%" />
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtAuxEngine" CssClass="input" Width="96%"></telerik:RadTextBox>
                </td>
                <td style="background-color: #d6d6d6;" colspan="2"></td>
                <td>
                    <eluc:MaskNumber ID="txtAEKW" runat="server" CssClass="input txtNumber" MaxLength="8" DecimalPlace="1" Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MaskNumber ID="txtAERPM" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="0" Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MaskNumber ID="txtAESFOC" runat="server" CssClass="input txtNumber" MaxLength="7" Width="96%"
                        DecimalPlace="2" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtAEFOCons" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>
            </tr>
            <tr>
                <th style="text-align: left">Aux Boiler
                </th>
                <td>
                    <eluc:MaskNumber runat="server" ID="txtABQuantity" IsInteger="true" IsPositive="true"
                        Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MultiAddress ID="txtABMaker" AddressType="130" runat="server" Width="96%" />
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtAuxBoiler" Width="96%"></telerik:RadTextBox>
                </td>
                <td style="background-color: #d6d6d6;" colspan="2"></td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MaskNumber ID="txtABkW" runat="server" CssClass="input txtNumber" MaxLength="5" DecimalPlace="0"
                        Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;" colspan="3">
                    <eluc:MaskNumber ID="txtABsfoc" runat="server" CssClass="input txtNumber" MaxLength="7"
                        DecimalPlace="2" IsPositive="true" Width="96%" Visible="false" />
                </td>
                <%--<td>
                    
                </td>--%>
                <td>
                    <eluc:MaskNumber ID="txtABFOCons" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>
            </tr>
            <tr>
                <th style="text-align: left">Thermal Oil Boiler
                </th>
                <td>
                    <eluc:MaskNumber runat="server" ID="txtTOBQuantity" IsInteger="true" IsPositive="true"
                        Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MultiAddress ID="txtTOBMaker" AddressType="130" runat="server" Width="96%" />
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtTOBModel" Width="96%"></telerik:RadTextBox>
                </td>
                <td style="background-color: #d6d6d6;" colspan="2"></td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MaskNumber ID="txtTOBkW" runat="server" CssClass="input txtNumber" MaxLength="5" DecimalPlace="0"
                        Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;" colspan="3">
                    <eluc:MaskNumber ID="txtTOBsfoc" runat="server" CssClass="input txtNumber" MaxLength="7"
                        DecimalPlace="2" IsPositive="true" Width="96%" Visible="false" />
                </td>
                <%--<td>
                    
                </td>--%>
                <td>
                    <eluc:MaskNumber ID="txtTOBFOCons" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>

            </tr>
            <tr>
                <th style="text-align: left">Inert Gas Generator
                </th>
                <td>
                    <eluc:MaskNumber runat="server" ID="txtIGGQuantity" CssClass="input" IsInteger="true" IsPositive="true"
                        Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MultiAddress ID="txtIGGMaker" AddressType="130" runat="server" Width="96%" />
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtIGGModel" Width="96%"></telerik:RadTextBox>
                </td>
                <td style="background-color: #d6d6d6;" colspan="2"></td>

                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MaskNumber ID="txtIGGkW" runat="server" CssClass="input txtNumber" MaxLength="5" DecimalPlace="0"
                        Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;" colspan="3">
                    <eluc:MaskNumber ID="txtIGGsfoc" runat="server" CssClass="input txtNumber" MaxLength="7"
                        DecimalPlace="2" IsPositive="true" Width="96%" Visible="false" />
                </td>
                <%--<td>
                    
                </td>--%>
                <td>
                    <eluc:MaskNumber ID="txtIGGFOCons" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>

            </tr>
            <tr>
                <th style="text-align: left">Aux Engine for COP
                    <%--Prime Mover Engine for COP--%>
                </th>
                <td>
                    <eluc:MaskNumber runat="server" ID="txtCEQuantity" CssClass="input" IsInteger="true" IsPositive="true"
                        Width="96%" />
                </td>
                <td>
                    <eluc:EngineType runat="server" ID="ucPMECEngineType" Width="96%" AppendDataBoundItems="true"
                        CssClass="dropdown_mandatory" />
                </td>
                <td>
                    <eluc:MultiAddress ID="txtCEMaker" AddressType="130" runat="server" Width="96%" />
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtCEModel" Width="96%"></telerik:RadTextBox>
                </td>
                <td style="background-color: #d6d6d6;" colspan="2"></td>
                <td>

                    <eluc:MaskNumber ID="txtPMECkw" runat="server" CssClass="input txtNumber" MaxLength="5" DecimalPlace="0"
                        Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MaskNumber ID="txtPMECrpm" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="0" Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;">
                    <eluc:MaskNumber ID="txtPMECgbhph" runat="server" CssClass="input txtNumber" MaxLength="7"
                        DecimalPlace="2" IsPositive="true" Width="96%" Visible="false" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtPMECgkwh" runat="server" CssClass="input txtNumber" MaxLength="7"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtPMECFOCons" runat="server" CssClass="input txtNumber" MaxLength="5"
                        DecimalPlace="2" IsPositive="true" Width="96%" />
                </td>

            </tr>
            <tr>
                <th style="text-align: left">ECDIS
                </th>
                <td>
                    <eluc:MaskNumber runat="server" ID="txtNoOfECDIS" CssClass="input" IsInteger="true" IsPositive="true"
                        Width="96%" />
                </td>
                <td style="background-color: #d6d6d6;"></td>
                <td>
                    <eluc:MultiAddress ID="txtECDISMaker" AddressType="130" runat="server" Width="96%" />
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtECDISModel" Width="96%"></telerik:RadTextBox>
                </td>
                <td style="background-color: #d6d6d6;" colspan="8"></td>
            </tr>
        </table>

        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td colspan="3">
                    <b>
                        <telerik:RadLabel ID="lblSeaTrial" runat="server" Text="Sea Trial"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
        </table>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td style="width: 240px;">
                    <telerik:RadLabel ID="lblDisplacement" runat="server" Text="Displacement"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtDisplacement" runat="server" MaxLength="6" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="0" />
                    <telerik:RadLabel ID="lblDisplacementUnits" runat="server" Text="mt"></telerik:RadLabel>
                </td>
                <td colspan="6"></td>
            </tr>
        </table>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td style="width: 240px;">
                    <telerik:RadLabel ID="lblServiceSpeed" runat="server" Text="Service Speed (Guaranteed Speed)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtSpeed" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblServiceSpeedUnit" runat="server" Text="kt"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblServiceBHP" runat="server" Text="Service BHP"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtServiceBHP" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="0" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblNCRSpeed" runat="server" Text="NCR Speed"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtNCRSpeed" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                    <telerik:RadLabel ID="lblNCRSpeedUnit" runat="server" Text="kt"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblNCRBHP" runat="server" Text="NCR BHP"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtSeaTrialNCRBHP" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="0" />
                </td>
            </tr>
        </table>
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td colspan="3">
                    <b>
                        <telerik:RadLabel ID="lblAuxEngine" runat="server" Text="Aux Engines"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
        </table>
        <table cellpadding="1" cellspacing="1" width="50%" class="table" id="table2" runat="server">
            <tr class="DataGrid-HeaderStyle">
                <th colspan="2"></th>
                <th>
                    <telerik:RadLabel ID="lblAE1" runat="server" Text="A/E1"></telerik:RadLabel>
                </th>
                <th>
                    <telerik:RadLabel ID="lblAE2" runat="server" Text="A/E2"></telerik:RadLabel>
                </th>
                <th>
                    <telerik:RadLabel ID="lblAE3" runat="server" Text="A/E3"></telerik:RadLabel>
                </th>
                <th>
                    <telerik:RadLabel ID="lblAE4" runat="server" Text="A/E4"></telerik:RadLabel>
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblPowerOutPut" runat="server" Text="Power Output (kW)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtPowerkWAE1" runat="server" MaxLength="7" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="1" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtPowerkWAE2" runat="server" MaxLength="7" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="1" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtPowerkWAE3" runat="server" MaxLength="7" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="1" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtPowerkWAE4" runat="server" MaxLength="7" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="1" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblRPM" runat="server" Text="RPM"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtRPMAE1" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="0" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtRPMAE2" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="0" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtRPMAE3" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="0" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtRPMAE4" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="0" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblSFOC" runat="server" Text="SFOC (g/kWh)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtSFOCAE1" runat="server" MaxLength="7" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtSFOCAE2" runat="server" MaxLength="7" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtSFOCAE3" runat="server" MaxLength="7" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtSFOCAE4" runat="server" MaxLength="7" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblFOCons" runat="server" Text="FO Cons (mt/day)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtFOConsAE1" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtFOConsAE2" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtFOConsAE3" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                </td>
                <td>
                    <eluc:MaskNumber ID="txtFOConsAE4" runat="server" MaxLength="5" CssClass="input" style="text-align: right;"
                        Width="70px" DecimalPlace="2" />
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>

                <td>
                    <telerik:RadLabel ID="lblTechnicalEfficiency" runat="server" Text="Technical Efficiency (gCO2/t-nm)"></telerik:RadLabel>
                </td>
                <td style="width: 10%;">
                    <telerik:RadLabel ID="lblEEDI" Text="EEDI" runat="server"></telerik:RadLabel>
                </td>
                <td style="width: 10%;">
                    <eluc:MaskNumber ID="txtEEDI" runat="server" MaxLength="11" CssClass="input" style="text-align: right;" Width="70px"
                        DecimalPlace="6" />

                    <eluc:MaskNumber ID="txtMCR" runat="server" MaxLength="100" CssClass="input" style="text-align: right;"
                        DecimalPlace="2" Visible="false" />
                </td>
                <td style="width: 10%;">
                    <telerik:RadLabel ID="lblEIV" Text="EIV" runat="server"></telerik:RadLabel>
                </td>
                <td style="width: 10%;">
                    <eluc:MaskNumber ID="txtEIV" runat="server" MaxLength="7" CssClass="input" style="text-align: right;"
                        DecimalPlace="2" />
                </td>
                <td style="width: 10%;"></td>
                <td style="width: 10%;"></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTier" runat="server" Text="NO<sub>x</sub> Tier"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList ID="ddlTier" runat="server">
                        <Items>
                            <telerik:DropDownListItem Text="--Select--" Value="Dummy"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Text="Tier 1" Value="1"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Text="Tier 2" Value="2"></telerik:DropDownListItem>
                            <telerik:DropDownListItem Text="Tier 3" Value="2"></telerik:DropDownListItem>
                        </Items>
                    </telerik:RadDropDownList>
                </td>
                <td colspan="5"></td>
            </tr>
            <tr>
                <td colspan="7">&nbsp
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="Literal1" runat="server" Text="EGCS Type"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadRadioButtonList runat="server" ID="rdlEGCS" RepeatColumns="4" Direction="Horizontal"></telerik:RadRadioButtonList>
                    <telerik:RadCheckBox runat="server" ID="chkEGCS" OnCheckedChanged="chkEGCS_CheckedChanged" Visible="false" />
                </td>
                <td colspan="3">&nbsp
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFuelComplianceMethod" runat="server" Text="Compliance Method"></telerik:RadLabel>
                </td>
                <td colspan="2">
                    <telerik:RadComboBox runat="server" ID="ddlFuelComplianceMethod" CssClass="input" Width="180px"></telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblFuelPipingVolume" runat="server" Text="Fuel Piping Volume (m3) "></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtFuelPipingVolume" runat="server" MaxLength="14" CssClass="input"
                        style="text-align: right;" DecimalPlace="3" />
                </td>
                <td>&nbsp
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLifeBoat" runat="server" Text="Life Boat"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblLifeBoatQty" runat="server" Text="Qty"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtLifeBoatQuantity" runat="server" MaxLength="100" CssClass="input" Width="70px"
                        style="text-align: right;" DecimalPlace="2" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblCapacity" runat="server" Text="Capacity"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="txtLifeBoatCapacity" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" DecimalPlace="2" />
                </td>
                <td style="width: 10%;">
                    <telerik:RadLabel ID="lblSeqCapacity" runat="server" Text="Seq Capacity"></telerik:RadLabel>
                </td>
                <td style="width: 10%;">
                    <eluc:Number ID="txtSecCapacity" runat="server" MaxLength="100" CssClass="input"
                        style="text-align: right;" DecimalPlace="2" />
                </td>
            </tr>
            <tr>

                <td>
                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td colspan="6">
                    <telerik:RadTextBox runat="server" ID="txtRemarks" CssClass="input" TextMode="MultiLine" Width="99%"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <table cellpadding="1" cellspacing="1" width="100%">

            <tr>
                <td>
                    <telerik:RadLabel ID="lblPropulsion" runat="server" Text="Propulsion" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Propulsion runat="server" ID="ucPropulsion" Width="80%" AppendDataBoundItems="true"
                        CssClass="input" Visible="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVoltage" runat="server" Text="Voltage" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number ID="Voltage" runat="server" MaxLength="3" CssClass="input" style="text-align: right;"
                        Width="70px" Visible="false" IsInteger="true" />
                    <%--<telerik:RadTextBox runat="server" ID="Voltage" CssClass="input" Height="70px" Width="80%"></telerik:RadTextBox>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDPMakeandModel" runat="server" Text="DP Make/Model" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="PMakeandModel" Visible="false" CssClass="readonlytextbox"
                        Width="79%">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDPClass" runat="server" Text="DP Class" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:DPClass runat="server" ID="DPClass" Width="80%" AppendDataBoundItems="true"
                        CssClass="input" Visible="false" />
                </td>
            </tr>
        </table>
        <hr />
        <%--<table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblEquipments" runat="server" Text="Equipments"></telerik:RadLabel></b>
                </td>
            </tr>
            <tr>
               <td>
                    <telerik:RadLabel ID="lblNoofECDIS" runat="server" Text="No. of ECDIS"></telerik:RadLabel>
                </td>
                <td style="width: 25%">
                    <eluc:Number runat="server" ID="txtNoOfECDIS" CssClass="input" IsInteger="true" IsPositive="true"
                        Width="70px" />
                </td>
               <td>
                    <telerik:RadLabel ID="lblModel" runat="server" Text="Model"></telerik:RadLabel>
                </td>
                <td style="width: 25%">
                    <telerik:RadTextBox runat="server" ID="txtECDISModel" CssClass="input" MaxLength="200" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMake" runat="server" Text="Make"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtECDISMakerCode" runat="server" Width="60px" CssClass="input"
                            Enabled="False"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtECDISMakerName" runat="server" Width="180px" CssClass="input"
                            Enabled="False"></telerik:RadTextBox>
                        <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130', true);"
                            Text=".." />
                        <telerik:RadTextBox ID="txtECDISMakerId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
        </table>--%>
        <hr />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td colspan="3">
                    <b>
                        <telerik:RadLabel ID="lblDockingInfo" runat="server" Text="Docking Info"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVesselParticulars" runat="server" Text="Vessel Particulars"></telerik:RadLabel>
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload" runat="server" CssClass="input" />
                    <%--<telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="FileUpload" OnClientFilesUploaded="OnClientFilesUploaded" MultipleFileSelection="Disabled" DropZones="FileUpload" Visible="true" EnableEmbeddedBaseStylesheet="true"></telerik:RadAsyncUpload>--%>
                </td>
                <td>
                    <%-- <asp:HyperLink ID="lnkParticulars" Target="_blank" Text="View Vessel Particulars"
                        runat="server" Height="14px" ToolTip="View Vessel Particulars"></asp:HyperLink>--%>
                    <asp:LinkButton ID="lnkParticulars" runat="server" Text="View Vessel Particulars"
                        OnClick="lnkParticulars_Click"></asp:LinkButton>
                </td>
                <td>
                    <%--     <asp:HyperLink ID="lnkfilename" Target="_blank" Text="Download Vessel Particulars Template" 
                        runat="server" Height="14px" ToolTip="Download Vessel Particulars Template"></asp:HyperLink>--%>
                    <asp:LinkButton ID="lnkfilename" runat="server" Text="Download Vessel Particulars Template"
                        OnClick="lnkfilename_Click"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;
                </td>
            </tr>
        </table>
        <hr />
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td colspan="4">
                    <b>
                        <telerik:RadLabel ID="lblSurveyHeader" runat="server" Text="Certificates Survey Info"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td style="width: 25%">
                    <telerik:RadLabel ID="lblAnniversaryDate" Text="Anniversary Date" runat="server"></telerik:RadLabel>
                </td>
                <td style="width: 25%">
                    <eluc:Date runat="server" ID="ucAnniversaryDate" />
                </td>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 25%">
                    <telerik:RadLabel ID="lblsplsurveylastdate" Text="Special Survey Hull Last Date" runat="server"></telerik:RadLabel>
                </td>
                <td style="width: 25%">
                    <eluc:Date runat="server" ID="ucsplsurveylastdate" />
                </td>
                <td style="width: 25%">
                    <telerik:RadLabel ID="lblsplsurveynextdate" Text="Special Survey Hull Next Date" runat="server"></telerik:RadLabel>
                </td>
                <td style="width: 25%">
                    <eluc:Date runat="server" ID="ucsplsurveynextdate" />
                </td>
            </tr>
            <tr>
                <td style="width: 25%">
                    <telerik:RadLabel ID="lbldrydocksurvey" Text="Dry-dock Surveys Last Date" runat="server"></telerik:RadLabel>
                </td>
                <td style="width: 25%">
                    <eluc:Date runat="server" ID="ucdrydocksurveydate" />
                </td>
                <td style="width: 25%">
                    <telerik:RadLabel ID="lblclassddwindownext" Text="Class DD window Next Date" runat="server"></telerik:RadLabel>
                </td>
                <td style="width: 25%">
                    <eluc:Date runat="server" ID="ucclassddwindownextdate" />
                </td>
            </tr>
            <tr>
                <td colspan="4">&nbsp;
                </td>
            </tr>
        </table>
        <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="txtVesselName" />
        </Triggers>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
