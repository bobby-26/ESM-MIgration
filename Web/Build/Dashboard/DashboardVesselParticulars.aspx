<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVesselParticulars.aspx.cs"
    Inherits="Dashboard_DashboardVesselParticulars" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Particulars</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDashboradVesselPrticulars" runat="server">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
     <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table id="tblfind" width="99.5%" border="1" bordercolor="black">
                    <tr style="width: 25%">
                        <td style="width: 35%; background-color: Blue; color: White" colspan="1">
                           <center><asp:Label ID="lblVesselName" runat="server"></asp:Label></center>
                        </td>
                        <td>
                            <b><asp:Literal ID="lblMaster" runat="server" Text="Master"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkMasterName" runat="server" Visible="false" ></asp:LinkButton>
                            <asp:Label ID="lblMasterName" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="9">
                            <a id="aVesselImg" runat="server">
                                <asp:Image ID="imgPhoto" runat="server" Height="180px"
                                    Width="390px" /></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblChiefEngineerHeader" runat="server" Text="Chief Engineer"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkChiefEnginner" runat="server" Visible="false"></asp:LinkButton>
                            <asp:Label ID="lblChiefEngineer" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblLastExportSequenceHeader" runat="server" Text="Last Export Sequence"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:Label ID="lblLastExportSequence" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblLastExportDateHeader" runat="server" Text="Last Export Date"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:Label ID="lblLastExportDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblLastImportSequenceHeader" runat="server" Text="Last Import Sequence"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:Label ID="lblLastImportSequence" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblLastImportDateHeader" runat="server" Text="Last Import Date"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:Label ID="lblLastImportDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblLatitudeHeader" runat="server" Text="Latitude"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:Label ID="lblLatitude" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblLongitudeHeader" runat="server" Text="Longitude"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:Label ID="lblLongitude" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20%">
                            <b><asp:Literal ID="lblSpareLifeBoatCapacityHeader" runat="server" Text="Spare Life Boat Capacity"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:Label ID="lblSpareLifeBoatCapacity" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox runat="server" ID="txtVesselName" Width="80%" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtType" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblIMONumber" runat="server" Text="IMO Number"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox runat="server" ID="txtIMONumber" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblOfficialNumber" runat="server" Text="Official Number"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox runat="server" ID="txtOfficialNumber" CssClass="input" ReadOnly="true"
                                Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblCallSign" runat="server" Text="Call Sign"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox runat="server" ID="txtCallSign" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblMMSINo" runat="server" Text="MMSI No."></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox runat="server" ID="txtMMSINo" MaxLength="100" Width="80%" ReadOnly="true"
                                CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblClassification" runat="server" Text="Classification"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtClassification" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblClassNotation" runat="server" Text="Class Notation"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtClassNotation" CssClass="input" ReadOnly="true"
                                Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblVesselShortCode" runat="server" Text="Vessel Short Code"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox runat="server" ID="txtVesselCode" CssClass="input" ReadOnly="true"
                                Width="80%"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblIceClass" runat="server" Text="Ice Class"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIceClass" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblFlag" runat="server" Text="Flag"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtFlag" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblPortofRegistry" runat="server" Text="Port of Registry"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtPortofRegistery" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtOwner" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblDisponentOwner" runat="server" Text="Disponent Owner"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtDisponentOwner" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblManager" runat="server" Text="Manager"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtManager" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblCharterer" runat="server" Text="Charterer"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                           <asp:TextBox ID="txtCharterer" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrincipal" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblHullNo" runat="server" Text="Hull No."></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox runat="server" ID="txtHullNo" MaxLength="20" ReadOnly="true"
                                Width="80%" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblKeelLaid" runat="server" Text="Keel Laid"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtKeelLaid" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblLaunched" runat="server" Text="Launched"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtLaunched" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblDelivery" runat="server" Text="Delivery"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtDelivery" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblESMTakeover" runat="server" Text="Vessel Takeover"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtESMTakeover" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblNavigationArea" runat="server" Text="Navigation Area"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtNavigationArea" CssClass="input" ReadOnly="true"
                                Width="80%"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblClassNo" runat="server" Text="Class No"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtClassNo" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblBuilder" runat="server" Text="Builder"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txtBuilder" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblFittedwithFramo" runat="server" Text="Fitted with Framo"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFittedwithFramo" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblESMHandoverdate" runat="server" Text="Vessel Handover date"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtESMHandoverDate" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblManagementType" runat="server" Text="Management Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtManagementType" runat="server" CssClass="input" Width="80%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td colspan="2">
                            <b><asp:Literal ID="lblPrincipalDimensions" runat="server" Text="Principal Dimensions"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblLOA" runat="server" Text="LOA"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtLoa" CssClass="input" Width="70px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblLBP" runat="server" Text="LBP"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtLBP" CssClass="input" Width="70px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblBreadthext" runat="server" Text="Breadth (ext)"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtBreath" CssClass="input" Width="70px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblDepthmld" runat="server" Text="Depth (mld)"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtDepth" CssClass="input" Width="70px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblHeightmax" runat="server" Text="Height (max)"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtHeight" CssClass="input" Width="70px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblServiceSpeed" runat="server" Text="Service Speed"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" CssClass="input" ID="txtSpeed" Width="70px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblTonnage" runat="server" Text="Tonnage"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                        </td>
                        <td style="width: 20%">
                            <asp:Literal  ID="lblRegistered" runat="server" Text="Registered"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:Literal ID="lblSuez" runat="server" Text="Suez"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:Literal ID="lblPanama" runat="server" Text="Panama"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblGross" runat="server" Text="Gross"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRegisteredGT" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSuezGT" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPanamaGT" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblNet" runat="server" Text="Net"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRegisteredNT" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSuezNT" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPanamaNT" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblLoadLine" runat="server" Text="Load Line"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                        </td>
                        <td style="width: 20%">
                            <asp:Literal ID="lblFreeboard" runat="server" Text="Freeboard"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:Literal ID="lblDraft" runat="server" Text="Draft"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:Literal ID="lblDWT" runat="server" Text="DWT"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTropical" runat="server" Text="Tropical"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFreeboardTropical" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDraftTropical" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDWTTropical" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSummer" runat="server" Text="Summer"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFreeboardSummer" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDraftSummer" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDWTSummer" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblWinter" runat="server" Text="Winter"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFreeboardWinter" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDraftWinter" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDWTWinter" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblLightship" runat="server" Text="Lightship"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFreeboardLightship" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDraftLightship" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDWTLightship" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblBallastCond" runat="server" Text="Ballast Cond"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFreeboardBallastCond" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDraftBallastCond" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDWTBallastCond" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <hr />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblMainMachinery" runat="server" Text="Main Machinery"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblEngineType" runat="server" Text="Engine Type"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtEngineType" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblEngineModel" runat="server" Text="Engine Model"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtEngineModel" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblMainEngine" runat="server" Text="Main Engine"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtMainEngine" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblMCR" runat="server" Text="MCR"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtMCR" Width="70px" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblAuxEngine" runat="server" Text="Aux Engine"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtAuxEngine" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblAuxBoiler" runat="server" Text="Aux Boiler"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtAuxBoiler" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblKW" runat="server" Text="KW"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtKW" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblBHP" runat="server" Text="BHP"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtBhp" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblLifeBoat" runat="server" Text="Life Boat"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox runat="server" ID="txtLifeBoatQuantity" CssClass="input" Width="50px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblCapacity" runat="server" Text="Capacity"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox runat="server" ID="txtLifeBoatCapacity" CssClass="input" Width="50px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSeqCapacity" runat="server" Text="Seq Capacity"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSecCapacity" runat="server" CssClass="input" Width="50px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                        </td>
                        <td width="25%">
                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="input" TextMode="MultiLine" ReadOnly="true"
                                Height="70px" Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <hr />
                <br />
                <table width="100%" id="tblCommunication">
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblCommunication" runat="server" Text="Communication"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlSatcom" runat="server" GroupingText="Satcom" Width="100%" Height="40%">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblSATBPhone" runat="server" Text="SAT B Phone"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtBPhone" CssClass="input" MaxLength="50" Width="85%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblSATBFax" runat="server" Text="SAT B Fax"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtBFax" CssClass="input" MaxLength="50" Width="85%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblFleet77Phone" runat="server" Text="Fleet77 Phone"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtFPhone" CssClass="input" MaxLength="50" Width="85%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblFleet77Fax" runat="server" Text="Fleet77 Fax"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtFFax" CssClass="input" MaxLength="50" Width="85%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblFBBPhone" runat="server" Text="FBB Phone"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtAPhone" CssClass="input" MaxLength="50" Width="85%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblFBBFax" runat="server" Text="FBB Fax"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtAFax" CssClass="input" MaxLength="50" Width="85%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblSATCTelex" runat="server" Text="SAT C Telex"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtCTalex" CssClass="input" MaxLength="50" Width="85%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlEmail" runat="server" GroupingText="E-mail" Width="100%" Height="40%">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblEmail" runat="server" Text="E-mail"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtEmail" MaxLength="50" Width="85%" CssClass="input" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblNotificationEmail" runat="server" Text="Notification E-mail"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtNotificationEmail" CssClass="input" MaxLength="50" ReadOnly="true"
                                                Width="85%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblAccountInchargeEmail" runat="server" Text="Account Incharge E-mail"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAccInchargeEmail" CssClass="input" MaxLength="50" ReadOnly="true"
                                                Width="85%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlPhone" runat="server" GroupingText="Phone" Width="100%" Height="40%">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblVSATPhone" runat="server" Text="VSAT Phone"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtPhone" CssClass="input" MaxLength="50" Width="85%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblVSATFax" runat="server" Text="VSAT Fax"></asp:Literal>
                                        </td>
                                        <td style="width: 17%">
                                            <asp:TextBox runat="server" ID="txtFax" CssClass="input" MaxLength="50" Width="85%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblMobileNumber" runat="server" Text="Mobile Number"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
