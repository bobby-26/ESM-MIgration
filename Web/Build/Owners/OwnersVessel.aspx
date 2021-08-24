<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersVessel.aspx.cs" Inherits="OwnersVessel" MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
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
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div id="ds" runat="server">    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>     
    </div>   
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegisterVessel" runat="server">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>          
    <asp:UpdatePanel runat="server" ID="pnlVesselListEntry">
        <ContentTemplate>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
                </eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" Visible="false" />
                <div class="subHeader" style="position: relative">
                    <div id="div1" style="vertical-align: top">
                       <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </div>
                </div>
                <%--<div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuVessel" runat="server" OnTabStripCommand="Vessel_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>
                                <table cellpadding="1" cellspacing="1" width="100%">
                                    <tr>                            
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox runat="server" ID="txtVesselName" MaxLength="100" Width="80%" CssClass="input_mandatory"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:VesselType ID="ucVesselType" runat="server" Width="80%" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                                        </td>
                                    </tr>
                                    <tr> 
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblIMONumber" runat="server" Text="IMO Number"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox runat="server" ID="txtIMONumber" MaxLength="50" CssClass="input" Width="80%"></asp:TextBox>
                                        </td> 
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblOfficialNumber" runat="server" Text="Official Number"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox runat="server" ID="txtOfficialNumber" CssClass="input" MaxLength="50" Width="80%" ></asp:TextBox>
                                        </td>   
                                    </tr>
                                    <tr> 
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblCallSign" runat="server" Text="Call Sign"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox runat="server" ID="txtCallSign" CssClass="input" MaxLength="100" Width="80%"></asp:TextBox>
                                        </td> 
                                        <td  style="width: 10%">
                                            <asp:Literal ID="lblMMSINo" runat="server" Text="MMSI No."></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox runat="server" ID="txtMMSINo" MaxLength="100" Width="90px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" TargetControlID="txtMMSINo"
                                                        Mask="999999999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblClassification" runat="server" Text="Classification"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <eluc:AddressType runat="server" ID="ucClassName" AddressType='<%# ((int)PhoenixAddressType.CLASSIFICATIONSOCIETY).ToString() %>' Width="80%" AppendDataBoundItems="true" CssClass="input" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblClassNotation" runat="server" Text="Class Notation"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtClassNotation" CssClass="input" MaxLength="100" Width="80%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblVesselShortCode" runat="server" Text="Vessel Short Code"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox runat="server" ID="txtVesselCode" CssClass="input_mandatory" MaxLength="100" Width="80%"></asp:TextBox>
                                        </td>   
                                        <td>
                                            <asp:Literal ID="lblIceClass" runat="server" Text="Ice Class"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlIceClassed" CssClass="input" >
                                                <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblFlag" runat="server" Text="Flag"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:Flag ID="ucFlag" runat="server" Width="80%" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />                                 
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblPortofRegistry" runat="server" Text="Port of Registry"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:Port ID="ucPortRegistered" runat="server" Width="80%" AppendDataBoundItems="true" CssClass="input" />
                                        </td>    
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:AddressType runat="server" ID="ucAddrOwner" AddressType='<%# Enum.GetName(typeof(SouthNests.Phoenix.Common.PhoenixAddressType),Convert.ToInt32(Eval("OWNER"))) %>' 
                                                Width="80%" AppendDataBoundItems="true" AutoPostBack="true" CssClass="dropdown_mandatory" />                                               
                                            <img ID="imgViewBillingParties" style="cursor:hand" alt="Billing Party" runat="server" class="imgalign" src="<%$ PhoenixTheme:images/billingparties.png %>"  onmousedown="javascript:closeMoreInformation()" />
                                        </td> 
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblDisponentOwner" runat="server" Text="Disponent Owner"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:AddressType runat="server" ID="ucDisponentOwner" AddressType='<%# Enum.GetName(typeof(SouthNests.Phoenix.Common.PhoenixAddressType),Convert.ToInt32(Eval("OWNER"))) %>' 
                                                Width="80%" AppendDataBoundItems="true" CssClass="input" /> 
                                        </td>                         
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblManager" runat="server" Text="Manager"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:AddressType runat="server" ID="ucAddrPrimaryManager" AddressType="126" Width="80%" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblCharterer" runat="server" Text="Charterer"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:AddressType runat="server" ID="ucCharterer" CssClass="input" AppendDataBoundItems="true" Width="80%"  />
                                        </td>                          
                                    </tr>
                                    <tr>  
                                        <td>
                                            <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                                        </td>        
                                        <td>
                                            <eluc:AddressType runat="server" ID="ucPrincipal" AddressType="128" CssClass="input_mandatory" AppendDataBoundItems="true" Width="80%" />
                                        </td>                  
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblHullNo" runat="server" Text="Hull No."></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox runat="server" ID="txtHullNo" MaxLength="20" style="text-align:right;" Width="90px" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtHullNo"
                                                        Mask="9999999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblKeelLaid" runat="server" Text="Keel Laid"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:Date runat="server" ID="txtKeelLaidDate" CssClass="input" />  
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblLaunched" runat="server" Text="Launched"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:Date runat="server" ID="txtLaunchedDate" CssClass="input" /> 
                                        </td>                            
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblDelivery" runat="server" Text="Delivery"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:Date runat="server" ID="txtDeliveryDate" CssClass="input" />  
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblESMTakeover" runat="server" Text="Takeover"></asp:Literal> 
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:Date runat="server" ID="txtTakeoverDateByESM" CssClass="input" /> 
                                        </td>                            
                                    </tr> 
                                    <tr> 
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblNavigationArea" runat="server" Text="Navigation Area"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtNavigationArea" CssClass="input" MaxLength="100" Width="80%"></asp:TextBox>
                                        </td>  
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblClassNo" runat="server" Text="Class No"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtClassNo" CssClass="input" MaxLength="100" Width="80%"></asp:TextBox>
                                        </td>  
                                    </tr>
                                    <tr>                                           
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblBuilder" runat="server" Text="Builder"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <eluc:AddressType runat="server" ID="ucAddrYard" AddressType='<%# ((int)PhoenixAddressType.YARD).ToString() %>' CssClass="input" AppendDataBoundItems="true" Width="80%"  />
                                        </td>   
                                        <td>
                                            <asp:Literal ID="lblFittedwithFramo" runat="server" Text="Fitted with Framo"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlFittedwithFramo" CssClass="input" >
                                                <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblESMHandoverdate" runat="server" Text="Handover date"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Date runat="server" ID="txtESMHandoverDate" CssClass="input" Enabled="false" />
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
                                            <asp:TextBox runat="server" ID="txtLoa" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender12" runat="server" TargetControlID="txtLoa"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td> 
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblLBP" runat="server" Text="LBP"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtLBP" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender8" runat="server" TargetControlID="txtLBP"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>                          
                                    </tr> 
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblBreadthext" runat="server" Text="Breadth (ext)"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtBreath" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="mskTxtBreath" runat="server" TargetControlID="txtBreath"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td> 
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblDepthmld" runat="server" Text="Depth (mld)"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtDepth" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDepth"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>                           
                                    </tr> 
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblHeightmax" runat="server" Text="Height (max)"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtHeight" CssClass="input" MaxLength="100" style="text-align:right;" Width="70px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender20" runat="server" TargetControlID="txtHeight"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>   
                                        <td>
                                            <asp:Literal ID="lblServiceSpeed" runat="server" Text="Service Speed"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" CssClass="input" ID="txtSpeed" MaxLength="100" style="text-align:right;" Width="70px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtNumberMask1" runat="server" TargetControlID="txtSpeed"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
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
                                            <asp:Literal ID="lblRegistered" runat="server" Text="Registered"></asp:Literal>
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
                                            <asp:TextBox runat="server" ID="txtRegisteredGT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender15" runat="server" TargetControlID="txtRegisteredGT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtSuezGT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender22" runat="server" TargetControlID="txtSuezGT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtPanamaGT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender23" runat="server" TargetControlID="txtPanamaGT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblNet" runat="server" Text="Net"></asp:Literal>
                                        </td>
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtRegisteredNT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender24" runat="server" TargetControlID="txtRegisteredNT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtSuezNT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender25" runat="server" TargetControlID="txtSuezNT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtPanamaNT" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender26" runat="server" TargetControlID="txtPanamaNT"
                                                        Mask="999,999" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
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
                                            <asp:TextBox runat="server" ID="txtFreeboardTropical" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender9" runat="server" TargetControlID="txtFreeboardTropical"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDraftTropical" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender14" runat="server" TargetControlID="txtDraftTropical"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDWTTropical" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender17" runat="server" TargetControlID="txtDWTTropical"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblSummer" runat="server" Text="Summer"></asp:Literal>
                                        </td>
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtFreeboardSummer" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender27" runat="server" TargetControlID="txtFreeboardSummer"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDraftSummer" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender28" runat="server" TargetControlID="txtDraftSummer"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDWTSummer" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender29" runat="server" TargetControlID="txtDWTSummer"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblWinter" runat="server" Text="Winter"></asp:Literal>
                                        </td>
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtFreeboardWinter" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender30" runat="server" TargetControlID="txtFreeboardWinter"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDraftWinter" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender31" runat="server" TargetControlID="txtDraftWinter"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDWTWinter" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender32" runat="server" TargetControlID="txtDWTWinter"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblLightship" runat="server" Text="Lightship"></asp:Literal>
                                        </td>
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtFreeboardLightship" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender33" runat="server" TargetControlID="txtFreeboardLightship"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDraftLightship" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender34" runat="server" TargetControlID="txtDraftLightship"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDWTLightship" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender35" runat="server" TargetControlID="txtDWTLightship"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblBallastCond" runat="server" Text="Ballast Cond"></asp:Literal>
                                        </td>
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtFreeboardBallastCond" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender36" runat="server" TargetControlID="txtFreeboardBallastCond"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDraftBallastCond" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender37" runat="server" TargetControlID="txtDraftBallastCond"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>  
                                        <td>                                            
                                            <asp:TextBox runat="server" ID="txtDWTBallastCond" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender38" runat="server" TargetControlID="txtDWTBallastCond"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
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
                                            <eluc:EngineType runat="server" ID="ucEngineType" Width="80%" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblEngineModel" runat="server" Text="Engine Model"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtEngineModel" CssClass="input" ></asp:TextBox>
                                        </td>
                                    </tr>                       
                                    <tr>                            
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblMainEngine" runat="server" Text="Main Engine"></asp:Literal> 
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtMainEngine" CssClass="input" ></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblMCR" runat="server" Text="MCR"></asp:Literal>
                                        </td>
                                        <td width="25%">                                           
                                            <asp:TextBox runat="server" ID="txtMCR" MaxLength="12" Width="70px" style="text-align:right;" CssClass="input"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender39" runat="server" TargetControlID="txtMCR"
                                                        Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>
                                        </td>
                                    </tr>                       
                                    <tr>                            
                                        <td style="width: 10%">
                                             <asp:Literal ID="lblAuxEngine" runat="server" Text="Aux Engine"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtAuxEngine" CssClass="input" ></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblAuxBoiler" runat="server" Text="Aux Boiler"></asp:Literal>
                                        </td>
                                        <td width="25%"> 
                                            <asp:TextBox runat="server" ID="txtAuxBoiler" CssClass="input" ></asp:TextBox>
                                        </td>
                                    </tr>                      
                                    <tr>                            
                                        <td style="width: 10%">
                                             <asp:Literal ID="lblKW" runat="server" Text="KW"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <eluc:Number ID="txtKW" runat="server" CssClass="input txtNumber" MaxLength="10" IsInteger="true" AutoPostBack="true" OnTextChangedEvent="CalculateBHP" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblBHP" runat="server" Text="BHP"></asp:Literal>
                                        </td>
                                        <td width="25%">                                           
                                            <eluc:Number ID="txtBHP" runat="server" CssClass="input txtNumber" MaxLength="10" IsInteger="true" AutoPostBack="true" OnTextChangedEvent="CalculateKW" />
                                        </td>
                                    </tr> 
                                    <tr>
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblLifeBoat" runat="server" Text="Life Boat"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox runat="server" ID="txtLifeBoatQuantity" CssClass="input" style="text-align:right;" MaxLength="3" Width="50px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender21" runat="server" TargetControlID="txtLifeBoatQuantity"
                                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>  
                                        </td>  
                                        <td style="width: 10%">
                                            <asp:Literal ID="lblCapacity" runat="server" Text="Capacity"></asp:Literal>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:TextBox runat="server" ID="txtLifeBoatCapacity" CssClass="input" style="text-align:right;" MaxLength="100" Width="50px"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="txtLifeBoatCapacity"
                                                        Mask="99" MaskType="Number" InputDirection="RightToLeft">
                                            </ajaxToolkit:MaskedEditExtender>  
                                        </td>                
                                    </tr>  
                                    <tr>    
                                        <td>
                                            <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                                        </td>
                                        <td width="25%">
                                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="input" TextMode="MultiLine" Height="70px" Width="80%"></asp:TextBox>
                                        </td>      
                                    </tr> 
                                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="txtVesselName" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
