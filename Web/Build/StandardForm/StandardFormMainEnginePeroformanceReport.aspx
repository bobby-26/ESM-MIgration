<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormMainEnginePeroformanceReport.aspx.cs" Inherits="StandardForm_StandardFormMainEnginePeroformanceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Main Engines Performance</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
    <style type="text/css">
        .Sftblclass
        {
            border-collapse: collapse;
        }
        .Sftblclass tr td
        {
            border: 1px solid black;
        }
        .Sftblclass tr td input
        {
            width: 96%;
        }
        .verticaltext
        {
            writing-mode: tb-rl;
            filter: flipv fliph; 
            width: 15px;
            height: 70px;
        }
        .verticaltext1
        {
            writing-mode: tb-rl;
            filter: flipv fliph;
            width: 70px;
            height: 70px;
        }
    </style>
      <script language="javascript" type="text/javascript">
        function cmdPrint_Click()
        {            
            document.getElementById('cmdPrint').style.visibility = "hidden";
            window.print();
        }
    </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="FrmMainEnginePerformanceReport" runat="server">
      <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Me Inspection Through Scavenge Ports"
                ShowMenu="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
       </div>        
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
            <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                    <table width="95%">
    <tr>
     <td colspan="5" align="left">
                            <b><asp:Literal ID="lblExecutiveOffshore" runat="server" Text="EXECUTIVE OFFSHORE"></asp:Literal></b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblFileRef" runat="server" Text="File Ref – 151.2"></asp:Literal>
                        </td>
                        <td align="right" colspan="2">
                            <asp:literal ID="lblE8" runat="server" Text="E8"></asp:literal>
                        </td>
                    
    </tr>
    <tr>
                        <td colspan="5" align="left">
                            
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblCE20" runat="server" Text="C/E 20"></asp:Literal>
                        </td >
                        <td align="right" colspan="2">
                            <asp:literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl813Rev1" runat="server" Text="(8/13 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h2>
                                <asp:literal ID="lblMainEnginePerformance" runat="server" Text="MAIN ENGINES PERFORMANCE"></asp:literal>
                            </h2>
                        </td>
                    </tr>
                    
                    <tr>
                    <td><asp:literal ID="lblVessel" runat="server" Text="VESSEL:"></asp:literal></td>
                    <td colspan="2"> <asp:TextBox ID="txtVessel" runat="server"   CssClass="input"></asp:TextBox></td>
                    </tr>
                    <tr>
                    <td><asp:literal ID="lblDateofTest" runat="server" Text="DATE OF TEST"></asp:literal></td>
                    <td colspan="2"><eluc:Date ID="txtDate" runat="server"   CssClass="input_mandatory"  DatePicker="true" /></td>
                    <td><asp:Literal runat="server" Text="ENGINE MAKE & TYPE :" ID="lblEngineMakeType"></asp:Literal></td>
                      <td colspan="2"> <asp:TextBox ID="txtEngineMake" runat="server"   CssClass="input"></asp:TextBox></td>
                    </tr>
                    <tr>
                     <td><asp:Literal ID="lblEngineNo1" runat="server" Text="ENGINE NO.1 SERIAL NO. :"></asp:Literal></td>
                      <td colspan="2"> <asp:TextBox ID="txtEngin1serialno" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td><asp:Literal ID="lblEngineNO2" runat="server" Text="ENGINE NO. 2 SERIAL NO.:"></asp:Literal></td>
                      <td colspan="2"> <asp:TextBox ID="txtEngine2serialno" runat="server"   CssClass="input"></asp:TextBox></td>
                    </tr>
                    
                    <tr>
                     <td><asp:Literal ID="lblGovernorMakeType" runat="server" Text="GOVERNOR MAKE, TYPE"></asp:Literal></td>
                      <td colspan="2"> <asp:TextBox ID="txtGovernorMakeType" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td><asp:literal ID="lblGearBoxMakeType" runat="server" Text="GEAR BOX MAKE & TYPE ;"></asp:literal></td>
                      <td colspan="2"> <asp:TextBox ID="txtGearBoxMakeType" runat="server"   CssClass="input"></asp:TextBox></td>
                    </tr>
                    
                    
                     <tr>
                     <td><asp:literal ID="lblSeaState" runat="server" Text="Sea State"></asp:literal></td>
                      <td> <asp:TextBox ID="txtSeaState" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td><asp:literal ID="lblWind" runat="server" Text="Wind"></asp:literal></td>
                      <td> <asp:TextBox ID="txtWind" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td><asp:Literal ID="lblVesselMaxSpeed" runat="server" Text="Vessel Max Speed:"></asp:Literal></td>
                      <td> <asp:TextBox ID="txtVesselMaxSpeed" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td><asp:Literal ID="lblVesselHeading" runat="server" Text="Vessel Heading"></asp:Literal></td>
                      <td> <asp:TextBox ID="txtVesselHeading" runat="server"   CssClass="input"></asp:TextBox></td>
                    </tr>
                    
                    
                     <tr>
                     <td><asp:literal ID="lblAtmosphericTemp" runat="server" Text="Atmospheric Temp"></asp:literal></td>
                      <td> <asp:TextBox ID="txtAtmosphericTemp" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td><asp:literal ID="lblWindDirection" runat="server" Text="Wind Direction"></asp:literal></td>
                      <td> <asp:TextBox ID="txtWindDirection" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td><asp:Literal ID="lblAveSpeed" runat="server" Text="Ave Speed:"></asp:Literal></td>
                      <td> <asp:TextBox ID="txtAvgSpeed" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td><asp:Literal ID="lblEngineRoomTemp" runat="server" Text="Engine Room Temp:"></asp:Literal></td>
                      <td> <asp:TextBox ID="txtEngineRoomTemp" runat="server"   CssClass="input"></asp:TextBox></td>
                    </tr>
                    </table>
                    
                    <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                    <td colspan="13">
                                    
                    <asp:Literal ID="lblMainEnginePort" runat="server" Text="Main Engine Port"></asp:Literal>
                    </td>
                    </tr>
                    <tr>
                    <td></td>
                    <td> <asp:literal ID="lblRPM" runat="server" Text="RPM"></asp:literal></td><td> <asp:TextBox ID="txtRPM" runat="server"   CssClass="input"></asp:TextBox></td>
                     <td> <asp:Literal ID="lblLoad" runat="server" Text="%Load"></asp:Literal></td><td> <asp:TextBox ID="txtLoad" runat="server"   CssClass="input"></asp:TextBox></td>
                      <td> <asp:Literal ID="lblPitch" runat="server" Text="Pitch"></asp:Literal></td><td> <asp:TextBox ID="txtPitch" runat="server"   CssClass="input"></asp:TextBox></td>
                       <td> <asp:literal ID="lblEngineRunningHours" runat="server" Text="Engine Running Hours"></asp:literal></td><td> <asp:TextBox ID="txtEngineRunningHrs" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td> <asp:literal ID="lblGovernorLoad" runat="server" Text="GovernorLoad"></asp:literal></td><td> <asp:TextBox ID="txtGovernorLoad" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td colspan="2"></td>
                  </tr>
                
              
                <tr>
                <td><asp:Literal ID="lblCylinder" runat="server" Text="Cylinder"></asp:Literal></td><td><asp:Literal ID="lblRPM1" runat="server" Text="1"></asp:Literal></td><td><asp:Literal ID="lbl2" runat="server" Text="2"></asp:Literal></td><td><asp:Literal ID="lblLoad3" runat="server" Text="3"></asp:Literal></td><td><asp:Literal ID="lbl4" runat="server" Text="4"></asp:Literal></td><td><asp:Literal ID="lblPitch5" runat="server" Text="5"></asp:Literal></td><td><asp:Literal ID="lbl6" runat="server" Text="6"></asp:Literal></td><td><asp:Literal ID="lblEngineRunningHours7" runat="server" Text="7"></asp:Literal></td><td><asp:Literal ID="lbl8" runat="server" Text="8"></asp:Literal></td><td><asp:Literal ID="lblGovernorLoad9" runat="server" Text="9"></asp:Literal></td>
                <td><asp:Literal ID="lbl10" runat="server" Text="10"></asp:Literal></td><td><asp:Literal ID="lbl11" runat="server" Text="11"></asp:Literal></td><td><asp:Literal ID="lbl12" runat="server" Text="12"></asp:Literal></td>
                </tr>
                 <tr>
                <td><asp:literal ID="lblFuelRackIndex" runat="server" Text="Fuel Rack Index"></asp:literal></td>
                <td> <eluc:Number ID="txtFuelRackIndex1" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex2" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex3" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex4" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex5" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex6" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex7" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex8" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex9" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex10" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex11" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtFuelRackIndex12" runat="server"   CssClass="input"/></td>
                </tr>
                
                <tr>
                <td><asp:Literal ID="lblExhaustTemp" runat="server" Text="Exhaust Temp"></asp:Literal></td>
                <td> <eluc:Number ID="txtExhaustTemp1" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp2" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp3" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp4" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp5" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp6" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp7" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp8" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp9" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp10" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp11" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtExhaustTemp12" runat="server"   CssClass="input"/></td>
                </tr>
                <tr>
                <td><asp:literal ID="lblMaxPressure" runat="server" Text="Max. Pressure"></asp:literal></td>
                <td> <eluc:Number ID="txtMaxPressure1" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure2" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure3" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure4" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure5" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure6" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure7" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure8" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure9" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure10" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtMaxPressure11" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="vtxtMaxPressure12" runat="server"   CssClass="input"/></td>
                </tr>
               <tr>
                <td><asp:Literal ID="lblCompressionPressure" runat="server" Text="Compression Pressure"></asp:Literal></td>
                <td> <eluc:Number ID="txtCompressionPressure1" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure2" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure3" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure4" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure5" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure6" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure7" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure8" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure9" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure10" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure11" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtCompressionPressure12" runat="server"   CssClass="input"/></td>
                </tr>
              
                  <tr>
                   <td colspan="13">
                    <asp:Literal ID="lblMainEngineStarBoard" runat="server" Text="Main Engine Starboard"></asp:Literal>
                    </td>
                    </tr>
                    <tr>
                    <td></td>
                    <td><asp:Literal ID="lblRpone" runat="server" Text="RPM"></asp:Literal></td><td> <asp:TextBox ID="txtEngineStarBoadrpm" runat="server"   CssClass="input"></asp:TextBox></td>
                     <td> <asp:Literal ID="lblloadone" runat="server" Text="%Load"></asp:Literal></td><td> <asp:TextBox ID="txtEngineStarBoardPercLoad" runat="server"   CssClass="input"></asp:TextBox></td>
                      <td><asp:Literal ID="lblPitchone" runat="server" Text="Pitch"></asp:Literal></td><td> <asp:TextBox ID="txtEngineStarBoardPitch" runat="server"   CssClass="input"></asp:TextBox></td>
                       <td> <asp:Literal ID="lblEngineRunningOne" runat="server" Text="Engine Running Hours"></asp:Literal></td><td> <asp:TextBox ID="txtEngineStarBoardRunningHrs" runat="server"   CssClass="input"></asp:TextBox></td>
                        <td> <asp:Literal ID="lblGovernorLoadOne" runat="server" Text="GovernorLoad"></asp:Literal></td><td> <asp:TextBox ID="txtEngineStarBoardGovernorLoad" runat="server"   CssClass="input"></asp:TextBox></td>
                 <td colspan="2"> </td> </tr>
                               
                <tr>
                <td><asp:Literal ID="lblCylinder2" runat="server" Text="Cylinder"></asp:Literal></td>
                <td><asp:Literal ID="lblRPmone" runat="server" Text="1"></asp:Literal></td>
                <td><asp:Literal ID="lblTwo" runat="server" Text="2"></asp:Literal></td>
                <td><asp:Literal ID="lblthree" runat="server" Text="3"></asp:Literal></td>
                <td><asp:Literal ID="lblfour" runat="server" Text="4"></asp:Literal></td>
                <td><asp:Literal ID="lblfive" runat="server" Text="5"></asp:Literal></td>
                <td><asp:Literal ID="lblsix" runat="server" Text="6"></asp:Literal></td>
                <td><asp:Literal ID="lblEngineRunninghours2" runat="server" Text="7"></asp:Literal>
                </td><td><asp:Literal ID="lbleight" runat="server" Text="8"></asp:Literal></td>
                <td><asp:Literal ID="lblnine" runat="server" Text="9"></asp:Literal></td>
                <td><asp:Literal ID="lblten" runat="server" Text="10"></asp:Literal></td>
                <td><asp:Literal ID="lbleleven" runat="server" Text="11"></asp:Literal></td>
                <td><asp:Literal ID="lbltwelve" runat="server" Text="12"></asp:Literal></td>
                </tr>
                 <tr>
                <td><asp:Literal ID="lblFuelRackIndex2" runat="server" Text="Fuel Rack Index"></asp:Literal></td>
                <td> <eluc:Number ID="txtStarBoardFuelRackIndex1" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex2" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex3" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex4" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex5" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex6" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex7" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex8" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex9" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex01" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex11" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardFuelRackIndex12" runat="server"   CssClass="input"/></td>
                </tr>
                
                <tr>
                <td><asp:Literal ID="lblExhaustTemp2" runat="server" Text="Exhaust Temp"></asp:Literal></td>
                <td> <eluc:Number ID="txtStarBoardExhaustTemp1" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp2" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp3" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp4" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp5" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp6" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp7" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp8" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp9" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp10" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp11" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardExhaustTemp12" runat="server"   CssClass="input"/></td>
                </tr>
                <tr>
                <td><asp:Literal ID="lblMaxPressure2" runat="server" Text="Max. Pressure"></asp:Literal></td>
                <td> <eluc:Number ID="txtStarBoardMaxPressure1" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure2" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure3" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure4" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure5" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure6" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure7" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure8" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure9" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure10" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure11" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardMaxPressure12" runat="server"   CssClass="input"/></td>
                </tr>
               <tr>
                <td><asp:Literal ID="lblCompressionPressure2" runat="server" Text="Compression Pressure"></asp:Literal></td>
                <td> <eluc:Number ID="txtStarBoardCompressionPressure1" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure2" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure3" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure4" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure5" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure6" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure7" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure8" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure9" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure10" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure11" runat="server"   CssClass="input"/></td>
                <td><eluc:Number ID="txtStarBoardCompressionPressure12" runat="server"   CssClass="input"/></td>
                </tr>
              
              
                  
                 
                 
                  
                  
                  
                 
                    <tr>
                    <td colspan="6">
                    <asp:Literal ID="lblTurboChargers" runat="server" Text="Turbochargers"></asp:Literal>
                    </td>
                    <td colspan="4">
                    <asp:Literal ID="lblCoolingWaterLubeOilandFuel" runat="server" Text="Cooling Water,Lube Oil and Fuel"></asp:Literal>
                    </td>
                    </tr>
                   
                   
                   
                    <tr>
                    <td></td><td colspan="2"><asp:Literal ID="lblPlPort" runat="server" Text="PLPort"></asp:Literal></td><td colspan="2"><asp:literal ID="lblStd" runat="server" Text="Stbd"></asp:literal></td><td rowspan="13"></td><td></td><td><asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal></td>
                    <td><asp:Literal ID="lblStbd" runat="server" Text="Stbd"></asp:Literal></td><td><asp:Literal ID="lblCommnets" runat="server" Text="Comments"></asp:Literal></td></tr><tr>
                     <td><asp:Literal ID="lblRPM3" runat="server" Text="RPM"></asp:Literal></td><td><eluc:Number ID="txtRPMPort1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtRPMPort2" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtStbdPort1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtStbdPort2" runat="server"   CssClass="input"/></td>
                   
                     <td><asp:Literal ID="lblSeaWaterTempDeg" runat="server" Text="Sea Water Temp Deg C"></asp:Literal></td><td><eluc:Number ID="txtSeaWaterTempPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtSeaWaterTempStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtSeaWaterTempComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                     <td><asp:literal ID="lblExhaustTempIN" runat="server" Text="Exhaust Temp IN - Deg C"></asp:literal></td><td><eluc:Number ID="txtETIDegPort1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtETIDegPort2" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtETIDegStbd1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtETIDegStbd2" runat="server"   CssClass="input"/></td>
                     
                     <td><asp:Literal ID="lblswPressureBefore" runat="server" Text="S.W. Pressure Before Cooler - Bar"></asp:Literal></td><td><eluc:Number ID="txtSWPressurePort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtSWPressureStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtSWPressureComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                     <td><asp:Literal ID="lblExhaustTempOut" runat="server" Text="Exhaust Temp OUT - Deg C"></asp:Literal></td><td><eluc:Number ID="txtExhaustTempOUTPort1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtExhaustTempOUTPort2" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtExhaustTempOUTStbd1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtExhaustTempOUTStbd2" runat="server"   CssClass="input"/></td>
                    
                     <td><asp:Literal ID="lblLTWaterPressure" runat="server" Text="L.T Water Pressure - Bar"></asp:Literal></td><td><eluc:Number ID="txtLTWPPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtLTWPStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtLTWPComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                     <td><asp:Literal ID="lblChargeAirTempBeforeCooler" runat="server" Text="Charge Air Temp Before Cooler - Deg C"></asp:Literal></td><td><eluc:Number ID="txtCATBPort1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtCATBPort2" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtCATBStbd1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtCATBStbd2" runat="server"   CssClass="input"/></td>
                    
                     <td><asp:Literal ID="lblLTWaterBeforeEngineDeg" runat="server" Text="L.T. Water Before Engine- Deg C"></asp:Literal></td><td><eluc:Number ID="txtLTWBPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtLTWBStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtLTWBVomments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                     <td><asp:Literal ID="lblChargeAirPressure" runat="server" Text="Charge Air Pressure - Bar"></asp:Literal></td><td><eluc:Number ID="txtChangeAPPort1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtChangeAPPort2" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtChangeAPStbd1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtChangeAPStbd2" runat="server"   CssClass="input"/></td>
                    
                     <td><asp:Literal ID="lblLTWaterAfterEngine" runat="server" Text="L.T. Water After Engine- Deg C"></asp:Literal></td><td><eluc:Number ID="txtLTWAPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtLTWAStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtLTWAComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                     <td><asp:Literal ID="lblShaftGeneratorLoadinKW" runat="server" Text="Shaft Generator Load in KW"></asp:Literal></td><td colspan="2"><eluc:Number ID="txtShaftGeneratorPort" runat="server"   CssClass="input"/></td>
                    
                     <td colspan="2"><eluc:Number ID="txtShaftGeneratorStbd" runat="server"   CssClass="input"/></td>
                    
                     
                     <td><asp:Literal ID="lblHTPRessure" runat="server" Text="H.T. Pressure- Bar"></asp:Literal></td><td><eluc:Number ID="txtHTPPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtHTPStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtHTPComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                    <td rowspan="2"></td>
                    <td colspan="2" rowspan="2"></td>
                    <td colspan="2" rowspan="2"></td>
                   
                    <td><asp:literal ID="lblHTWaterBeforeEngine" runat="server" Text="H.T. Water before Engine-Deg C"></asp:literal></td><td><eluc:Number ID="txtHTWBPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtHTWBStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtHTWBComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                    
                    <td><asp:Literal ID="lblHTWaterAfterEngineDeg" runat="server" Text="H.T. Water after Engine-Deg C"></asp:Literal></td><td><eluc:Number ID="txtHTWAPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtHTWAStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtHTWAComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                    <td><asp:Literal ID="lblMGOConsumptionDuringThisTest" runat="server" Text="MGO consumption during this test - Ltrs / hour"></asp:Literal></td><td colspan="2"><eluc:Number ID="txtMGOConsumptionDuringThisTestPort" runat="server"   CssClass="input"/></td>
                    <td colspan="2"><eluc:Number ID="txtMGOConsumptionDuringThisTestStbd" runat="server"   CssClass="input"/></td>
                    
                    <td><asp:Literal ID="lblLOPressureBar" runat="server" Text="L.O. Pressure-Bar"></asp:Literal></td><td><eluc:Number ID="txtLOPBPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtLOPBStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtLOPBComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                    <td><asp:Literal ID="lblMGOConsPerDayBasedOnThisTest" runat="server" Text="MGO Cons per day based on this test - M3"></asp:Literal></td><td colspan="2"><eluc:Number ID="txtMGOConsPerDayPort" runat="server"   CssClass="input"/></td>
                    <td colspan="2"><eluc:Number ID="txtMGOConsPerDayStbd" runat="server"   CssClass="input"/></td>
                    
                    <td><asp:Literal ID="lblLoTempBeforeCoolerDeg" runat="server" Text="L.O. Temp Before Cooler-Deg C"></asp:Literal></td><td><eluc:Number ID="txtLOPBCPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtLOPBCStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtLOPBCComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                   <td colspan="6" rowspan="2"></td>
                    <td><asp:Literal ID="lblLOTempAfterCoolerDeg" runat="server" Text="L.O. Temp After Cooler-Deg C"></asp:Literal></td><td><eluc:Number ID="txtTACPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtTACStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtTACComments" runat="server"   CssClass="input"></asp:TextBox></td></tr><tr>
                   
                    <td><asp:Literal ID="lblFOPressureBar" runat="server" Text="F.O. Pressure-Bar"></asp:Literal></td><td><eluc:Number ID="txtFOPBPort" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtFOPBStbd" runat="server"   CssClass="input"/></td>
                     <td><asp:TextBox ID="txtFOPBComments" runat="server"   CssClass="input"></asp:TextBox></td></tr></table><table width="100%">
                        <caption>
                            <br />
                            <br />
                            <tr>
                                <td colspan="4">
                                    <asp:Literal ID="lblCheifEngineer" runat="server" Text="Chief Engineer"></asp:Literal></td><td colspan="4">
                                    <asp:literal ID="lblSecondEngineer" runat="server" Text="Second Engineeer"></asp:literal></td></tr></caption></table></ContentTemplate></asp:UpdatePanel></div></form></body></html>
