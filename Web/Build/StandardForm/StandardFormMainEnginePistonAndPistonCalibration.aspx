<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormMainEnginePistonAndPistonCalibration.aspx.cs" Inherits="StandardFormMainEnginePistonAndPistonCalibration" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Main Engine Pisten calibration</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
     <script language="javascript" type="text/javascript">
        function cmdPrint_Click()
        {            
            document.getElementById('cmdPrint').style.visibility = "hidden";
            window.print();
        }
    </script>
     <style type="text/css">
        .Sftblclass
        {
            border-collapse: collapse;
        }
        .Sftblclass tr td
        {
            border: 1px solid black;
        }
        .Sftblclass tr td input[type=text]
        {
            width: 95%;
        }       
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmmainEnginePistenCalibration" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Record Of Nox Verification" ShowMenu="false">
            </eluc:Title>
              <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>
        
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
            <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                <table width="95%">
                    <tr>
                        <td colspan="6" align="left">
                            <b><asp:literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT PTE LTD"></asp:literal></b>
                        </td>
                        <td align="left">
                               
                        </td>
                        <td align="right">
                            <asp:literal ID="lblE4" runat="server" Text="E4"></asp:literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <%--<b><asp:Literal ID="lblSingapore" runat="server" Text="SINGAPORE"></asp:Literal></b>--%>
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl0714Rev1" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h3>
                               <asp:Literal ID="lblMainEnginePiston" runat="server" Text="MAIN ENGINE PISTON AND PISTON RINGS CALIBRATION REPORT"></asp:Literal>
                            </h3>
                        </td>
                    </tr>
                  <tr>
                  <td colspan="2">
                      <asp:CheckBox ID="CheckBox1" runat="server" Text="Grooves Chrome Plated" />
                  </td>
                  <td>
                  &nbsp;
                  </td>
                   <td colspan="2">
                      <asp:CheckBox ID="CheckBox2" runat="server" Text="Grooves Equipped with Wear Rings" />
                  </td>
                  <td>
                  &nbsp;
                  </td>
                  <td colspan="2" align="center">
                  <asp:literal ID="lblAllMeasurementInmm" runat="server" Text="ALL MEASUREMENT IN mm"></asp:literal>
                  </td>
                  </tr>
                  <tr>
                  <td colspan="8">
                  <table class="Sftblclass" cellpadding="1" cellspacing="1" width="100%">
                  <tr>
                  <td colspan="8">
                  <asp:Literal ID="lblVessel" runat="server" Text="Vessel&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                  <asp:TextBox ID="txtvesselName" runat="server" CssClass="input" Enabled="false"  Width="60%"></asp:TextBox>
                  </td>
                  <td colspan="4">
                  <asp:literal ID="lblMainEngineType" runat="server" Text="Main Engine Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:literal>
                  <asp:TextBox ID="txtEngineTye" runat="server" CssClass="input_mandatory" Enabled="false"  Width="40%"></asp:TextBox>
                  </td>
                  <td colspan="4">
                  <asp:Literal ID="lblCylinderNo" runat="server" Text="Cylinder No.<br />
                      (From Ford.)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; "></asp:Literal>
                   <asp:TextBox ID="txtCYlinderNo" runat="server" CssClass="input" Enabled="false"  Width="60%"></asp:TextBox>                                                       
                  </td>
                  </tr>
                  <tr>
                  <td colspan="12" align="center">
                  <asp:Literal ID="lblPiston" runat="server" Text="PISTON"></asp:Literal>
                  </td>
                  <td colspan="4">
                  <asp:literal ID="lblDateofCalibration" runat="server" Text ="Date of<br />
                      Calibration &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; "></asp:literal>
                  <asp:TextBox ID="txtDate" runat="server"  CssClass="input" Width="70%"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtCalenderDate" PopupPosition="BottomRight" runat="server"  TargetControlID="txtDate">
                            </ajaxToolkit:CalendarExtender>                                              
                  </td>                  
                  </tr> 
                  <tr>
                  <td>
                 <asp:Literal ID="lblCrownandLands" runat="server" Text="Crown and Lands"></asp:Literal>
                  </td>
                  <td colspan="11">
                  <asp:TextBox ID="txtCrownLands" runat="server" CssClass="input" Width="50%"></asp:TextBox>
                 <asp:Literal ID="lblDateRenewed" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                  Date Renewed : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                  <eluc:Date ID="txtDate1" runat="server" CssClass="input" DatePicker="true" />
                  </td>
                  <td colspan="4">
                      <asp:literal ID="lblPortofCalibration" runat="server" Text="Port of Calibration &nbsp;&nbsp;&nbsp;"></asp:literal>
                  <asp:TextBox ID="txtPortCalibration" runat="server" CssClass="input" Width="50%"></asp:TextBox> 	
                  </td>                  
                  </tr>      
                  <tr>
                  <td>
                 <asp:Literal ID="lblGuideRing" runat="server" Text="Guide Ring"></asp:Literal>
                  </td>
                  <td colspan="11">
                  <asp:TextBox ID="txtGuideRing" runat="server" CssClass="input" Width="50%"></asp:TextBox>
                 <asp:Literal ID="lblDateRenew" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                  Date Renewed : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; "></asp:Literal>
                   <eluc:Date ID="txtDate2" runat="server" CssClass="input" DatePicker="true" />
                  </td>
                  <td colspan="4">
                  <asp:Literal ID="lblTotalEngineOperHrs" runat="server" Text="Total Engine Oper.<br />
                      Hrs. &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                  <asp:TextBox ID="txtTotEngineOp" runat="server" CssClass="input" Width="50%"></asp:TextBox> 	
                  </td>                  
                  </tr>      
                  <tr>
                  <td>
                  <asp:literal ID="lblSkirt" runat="server" Text="Skirt	"></asp:literal>
                  </td>
                  <td colspan="11">
                  <asp:TextBox ID="txtSkirt" runat="server" CssClass="input" Width="50%"></asp:TextBox>
                 <asp:Literal ID="lblRenewedDate" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                  Date Renewed : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; "></asp:Literal>
                  <eluc:Date ID="txtDate3" runat="server" CssClass="input" DatePicker="true" />
                  </td>
                  <td colspan="4">
                      <asp:Literal ID="lblPistonStamp" runat="server" Text="Piston Stamp &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                  <asp:TextBox ID="txtPisteonStamp" runat="server" CssClass="input" Width="50%"></asp:TextBox> 	
                  </td>                  
                  </tr>      
                  <tr>
                  <td>
                  <asp:Literal ID="lblRubblingBand" runat="server" Text="Rubbing Band"></asp:Literal>
                  </td>
                  <td colspan="11">
                  <asp:TextBox ID="txtRubbingBand" runat="server" CssClass="input" Width="50%"></asp:TextBox>
                 <asp:Literal ID="lblRenewDate" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                  Date Renewed : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; "></asp:Literal>
                   <eluc:Date ID="txtDate4" runat="server" CssClass="input" DatePicker="true" />
                  </td>
                  <td colspan="4">
                  <asp:Literal ID="lblDatePistonInstalled" runat="server" Text="Date Piston<br />
                      Installed &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                  <eluc:Date ID="txtDatePisteon" runat="server" CssClass="input" DatePicker="true" />                 
                   </td>                  
                  </tr> 
                  <tr>
                  <td rowspan="2">
                 <asp:Literal ID="lblGrooveNo" runat="server" Text="Groove No."></asp:Literal>
                  </td>
                  <td colspan="3" align="center">
                  <asp:Literal ID="lblMeasureMentH" runat="server" Text="Measurement ‘H’"></asp:Literal>
                  </td>
                  <td align="center" colspan="8" rowspan="2">
                  <asp:literal ID="lblCondition" runat="server" Text="CONDITION"></asp:literal>
                  </td>
                  <td colspan="4" rowspan="2">
                  <asp:Literal ID="lblServiceHrsOfPiston" runat="server" Text="Service Hrs. of<br />
                      Piston &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                  <asp:TextBox ID="txtPistonHrs" runat="server" CssClass="input" Width="50%"></asp:TextBox> 	
                  </td>
                  </tr> 
                  <tr>
                  <td align="center">
                  <asp:Literal ID="lblPresentMax" runat="server" Text="Present Maximum"></asp:Literal>
                  </td>
                  <td colspan="2" align="center">
                  <asp:literal ID="lblOriginalMax" runat="server" Text="Original/Max<br />
                  Allowed"></asp:Literal>
                  </td>
                  </tr>  
                  <tr>
                  <td align="center">
                  <asp:Literal ID="lblone" runat="server" Text="1"></asp:Literal>
                  </td>
                  <td>
                  <asp:TextBox ID="txtPercentMaker1" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="2">
                  <asp:TextBox ID="txtPercentMaker2" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="8">
                  <asp:TextBox ID="txtPercentMaker3" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="4" rowspan="2">
                <asp:Literal ID="lblHrsSinceLast" runat="server" Text="Hrs. Since Last<br />
                      Overhaul &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                  <asp:TextBox ID="txtOverhaulLast" runat="server" CssClass="input" Width="50%"></asp:TextBox> 	
                  </td>
                  </tr>  
                  <tr>
                  <td align="center">
                  <asp:Literal ID="lbltwo" runat="server" Text="2"></asp:Literal>
                  </td>
                  <td>
                  <asp:TextBox ID="txtPercentMaker21" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="2">
                  <asp:TextBox ID="txtPercentMaker22" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="8">
                  <asp:TextBox ID="txtPercentMaker23" runat="server" CssClass="input"></asp:TextBox> 
                  </td>                
                  </tr>  
                   <tr>
                  <td align="center">
                  <asp:Literal ID="lblthree" runat="server" Text="3"></asp:Literal>
                  </td>
                  <td>
                  <asp:TextBox ID="txtPercentMaker31" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="2">
                  <asp:TextBox ID="txtPercentMaker32" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="8">
                  <asp:TextBox ID="txtPercentMaker33" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="4" rowspan="2">
                      <asp:literal ID="lblCrownBurning" runat="server" Text="Crown Burning &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:literal>
                  <asp:TextBox ID="txtCrownBurning" runat="server" CssClass="input" Width="50%"></asp:TextBox> 	
                  </td>
                  </tr>  
                  <tr>
                  <td align="center">
                 <asp:Literal ID="lblfour" runat="server" Text="4"></asp:Literal>
                  </td>
                  <td>
                  <asp:TextBox ID="txtPercentMaker41" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="2">
                  <asp:TextBox ID="txtPercentMaker42" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="8">
                  <asp:TextBox ID="txtPercentMaker43" runat="server" CssClass="input"></asp:TextBox> 
                  </td>                
                  </tr>  
                   <tr>
                  <td align="center">
                 <asp:Literal ID="lblfive" runat="server" Text="5"></asp:Literal>
                  </td>
                  <td>
                  <asp:TextBox ID="txtPercentMaker51" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="2">
                  <asp:TextBox ID="txtPercentMaker52" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="8">
                  <asp:TextBox ID="txtPercentMaker53" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="4" rowspan="3">
                    <asp:Literal ID="lblReasonforOverhaul" runat="server" Text="Reason for<br />
                      Overhaul &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                  <asp:TextBox ID="txtReasonOverhaul" runat="server" CssClass="input" Width="50%"></asp:TextBox> 	
                  </td>
                  </tr>  
                  <tr>
                  <td align="center">
                  <asp:Literal ID="lblsix" runat="server" Text="6"></asp:Literal>
                  </td>
                  <td>
                  <asp:TextBox ID="txtPercentMaker61" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="2">
                  <asp:TextBox ID="txtPercentMaker62" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="8">
                  <asp:TextBox ID="txtPercentMaker63" runat="server" CssClass="input"></asp:TextBox> 
                  </td>                
                  </tr>  
                   <tr>
                  <td align="center">
                  <asp:Literal ID="lblseven" runat="server" Text="7"></asp:Literal>
                  </td>
                  <td>
                  <asp:TextBox ID="txtPercentMaker71" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="2">
                  <asp:TextBox ID="txtPercentMaker72" runat="server" CssClass="input"></asp:TextBox> 
                  </td>
                  <td colspan="8">
                  <asp:TextBox ID="txtPercentMaker73" runat="server" CssClass="input"></asp:TextBox> 
                  </td>                
                  </tr>
                  
                  <tr>
                  <td colspan="2" rowspan="12">
                  <asp:Image ID="img1" runat="server" ImageUrl="~/StandardForm/Images/PistonRingCalibration.PNG" Height="100%" Width="100%" />
                  </td>
                  <td colspan="2" rowspan="2" align="center">
                  <asp:Literal ID="lblRingsOldOrder" runat="server" Text="Rings Old Order"></asp:Literal>
                  </td>
                  <td colspan="4" align="center">
                  <asp:literal ID="lblAxialthickness" runat="server" Text="Axial Thickness"></asp:literal>
                  </td>
                  <td colspan="4" align="center">
                  <asp:Literal ID="lblradialThickness" runat="server" Text="Radial Thickness"></asp:Literal>
                  </td>
                  <td align="center" rowspan="3">
                  <asp:Literal ID="lblButtCirIn" runat="server" Text="Butt Clr in<br />
                  Liner/Max<br />
                  Allowed"></asp:Literal>
                  </td>
                  <td colspan="2" align="center">
                  <asp:Literal ID="lblRingsNewOrder" runat="server" Text="Rings New Order"></asp:Literal>
                  </td>
                  <td align="center" rowspan="3">
                  <asp:Literal ID="lblAxialCir" runat="server" Text="Axial Clr./<br />
                  Max<br />
                  Allowed"></asp:Literal>
                  </td>
                  </tr> 
                  <tr>
                  <td align="center" rowspan="2">
                  <asp:literal ID="lblNewMIn" runat="server" Text="New/Min<br />
                  Allowed"></asp:literal>
                  </td>
                  <td align="center" colspan="3">
                  <asp:Literal ID="lblNow" runat="server" Text="Now"></asp:Literal>
                  </td>
                       <td align="center" rowspan="2">
                  <asp:Literal ID="lblNewMinAllowed" runat="server" Text="New/Min<br />
                  Allowed"></asp:Literal>
                  </td>
                  <td align="center" colspan="3">
                  <asp:Literal ID="lblNow1" runat="server" Text="Now"></asp:Literal>
                  </td>                  
                  </tr> 
                  <tr>
                  <td align="center">
                  <asp:Literal ID="lblNo" runat="server" Text="No."></asp:Literal>
                  </td>
                  <td>
                  <asp:Literal ID="lblLHRII" runat="server" Text="L.H/R.II"></asp:Literal>
                  </td>
                  <td align="center">
                  <asp:Literal ID="lblA" runat="server" Text="A"></asp:Literal>
                  </td>
                   <td align="center">
                  <asp:Literal ID="lblB" runat="server" Text="B"></asp:Literal>
                  </td>
                   <td align="center">
                  <asp:Literal ID="lblC" runat="server" Text="C"></asp:Literal>
                  </td>
                   <td align="center">
                  <asp:Literal ID="lblA1" runat="server" Text="A"></asp:Literal>
                  </td>
                   <td align="center">
                  <asp:Literal ID="lblB1" runat="server" Text="B"></asp:Literal>
                  </td>
                   <td align="center">
                  <asp:Literal ID="lblC1" runat="server" Text="C"></asp:Literal>
                  </td>
                  <td align="center">
                  <asp:Literal ID="lblNo1" runat="server" Text="No."></asp:Literal>
                  </td>
                  <td align="center">
                  <asp:Literal ID="lblLHRII1" runat="server" Text="L.H/R.II"></asp:Literal>
                  </td>
                  </tr>
                  <tr>
                  <td align="center">
                  <asp:Literal ID="lblOne1" runat="server" Text="1"></asp:Literal>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder1" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder2" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder3" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder4" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder5" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder6" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder7" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder8" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder9" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder10" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder11" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder12" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder13" runat="server" CssClass="input"></asp:TextBox>
                  </td>           
                  </tr>  
                    <tr>
                  <td align="center">
                  <asp:Literal ID="lblTwo2" runat="server" Text="2"></asp:Literal>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder21" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder22" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder23" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder24" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder25" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder26" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder27" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder28" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder29" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder210" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder211" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder212" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder213" runat="server" CssClass="input"></asp:TextBox>
                  </td>           
                  </tr>  
                    <tr>
                  <td align="center">
                  <asp:Literal ID="lblThree3" runat="server" Text="3"></asp:Literal>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder31" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder32" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder33" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder34" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder35" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder36" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder37" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder38" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder39" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder310" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder311" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder312" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder313" runat="server" CssClass="input"></asp:TextBox>
                  </td>           
                  </tr>               
                    <tr>
                  <td align="center">
                  <asp:Literal ID="lblfour4" runat="server" Text="4"></asp:Literal>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder41" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder42" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder43" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder44" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder45" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder46" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder47" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder48" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder49" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder410" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder411" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder412" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder413" runat="server" CssClass="input"></asp:TextBox>
                  </td>           
                  </tr> 
                    <tr>
                  <td align="center">
                 <asp:Literal ID="lblFive5" runat="server" Text="5"></asp:Literal>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder51" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder52" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder53" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder54" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder55" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder56" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder57" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder58" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder59" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder510" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder511" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder512" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder513" runat="server" CssClass="input"></asp:TextBox>
                  </td>           
                  </tr> 
                    <tr>
                  <td align="center">
                  <asp:Literal ID="lblsix6" runat="server" Text="6"></asp:Literal>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder61" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder62" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder63" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder64" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder65" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder66" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder67" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder68" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder69" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder610" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder611" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder612" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder613" runat="server" CssClass="input"></asp:TextBox>
                  </td>           
                  
                  </tr> 
                    <tr>
                  <td align="center">
                  <asp:Literal ID="lblSeven7" runat="server" Text="7"></asp:Literal>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder71" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder72" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder73" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder74" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder75" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder76" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder77" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder78" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder79" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder710" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  <td>
                      <asp:TextBox ID="txtRingOlder711" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder712" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                   <td>
                      <asp:TextBox ID="txtRingOlder713" runat="server" CssClass="input"></asp:TextBox>
                  </td>           
                  </tr> 
                  <tr>
                  <td colspan="2" align="center">
                 <asp:Literal ID="lblGeneralRemarks" runat="server" Text="General<br />
                  Remarks on<br />
                  Cond of Piston<br />
                  Rings"></asp:Literal>
                  </td>
                  <td colspan="12">
                      <asp:TextBox ID="txtgenersalRemark" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  </tr>                   
                    <tr>
                    <td colspan="14" align="center">
                    <asp:Literal ID="lblPistonRod" runat="server" Text="PISTON ROD, STUFFING-BOX AND STUDS"></asp:Literal>
                    </td>
                    </tr> 
                    <tr>
                    <td colspan="2" align="center">
                    <asp:Literal ID="lblConditionofRod" runat="server" Text="Condition of Rod"></asp:Literal>
                    </td>
                    <td colspan="14">
                    <asp:TextBox ID="txtConditionRod" runat="server" CssClass="input"></asp:TextBox>
                    </td>
                    </tr>  
                    <tr>
                    <td colspan="2" align="center">
                    <asp:Literal ID="lblConditionofPistonStudsandNuts" runat="server" Text="Condition of Piston Studs & Nuts"></asp:Literal>
                    </td>
                    <td colspan="14">
                    <asp:TextBox ID="txtConditionPiston"  runat="server" CssClass="input"></asp:TextBox>
                    </td>
                    </tr> 
                    <tr>
                    <td align="center">
                    <asp:Literal ID="lblStuffingBox" runat="server" Text="Stuffing<br />
                    Box"></asp:Literal>
                    </td>
                    <td align="center">
                    <asp:Literal ID="lblOverhauled" runat="server" Text="Overhauled"></asp:Literal>
                    </td>                    
                     <td colspan="14">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:RadioButtonList runat="server" ID="rbtnOverhaul" RepeatDirection="Horizontal" Width="100%"  >
                    
                      <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                      <asp:ListItem Value="1" Text="No"></asp:ListItem>
                      
                     </asp:RadioButtonList>
                  
                    
                    
                    
                    </td>
                    </tr>                 
                    </td>
                   
                    </tr>      
                  </table>
                  </td>
                  </tr>
                  <tr>
                  <td colspan="8">
                  <br />
                  <br />
                  <br />
                  </td>
                  </tr>
                  <tr>
                  <td colspan="7">
                  &nbsp;
                  </td>
                  <td align="center">
                  <asp:TextBox ID="txtChiefEngineer" runat="server" CssClass="input"></asp:TextBox>
                  </td>
                  </tr>
                   <tr>
                  <td colspan="7">
                  &nbsp;
                  </td>
                  <td align="center">
                 <asp:Literal ID="lblchiefEngineer" runat="server" Text="Chief Engineer"></asp:Literal>
                  </td>
                  </tr>
                  
                                    
                </table>               
                <br />
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
