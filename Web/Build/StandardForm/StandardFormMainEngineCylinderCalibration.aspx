<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormMainEngineCylinderCalibration.aspx.cs"
    Inherits="StandardFormMainEngineCylinderCalibration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Main Engine Cylinder Calibration</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
        .Sftblclass tr td input
        {
            width: 96%;
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCylindercalibration" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Main Engine Cylinder Calibration" ShowMenu="false">
            </eluc:Title>
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>
       
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">           
            <ContentTemplate>
             <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false"/>
                <table width="95%">
                    <tr>
                        <td colspan="6" align="left">
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT"></asp:Literal></b>
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblE5" runat="server" Text="E5"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                           <%-- <b><asp:Literal ID="lblSingapore" runat="server" Text="SINGAPORE"></asp:Literal></b>--%>
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:literal>
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
                                <asp:Literal ID="lblMainEngineCylinderCalibrationReport" runat="server" Text="MAIN ENGINE CYLINDER CALIBRATION REPORT<br />"></asp:Literal>
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chPlateLinear" runat="server" Text="CHROME PLATED LINER" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chIronLinear" runat="server" Text="CAST-IRON LINER" />
                        </td>
                        <td colspan="5">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lblAllMeasurementsInMM" runat="server" Text="ALL MEASUREMENTS IN mm"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="4">
                                        <asp:Literal ID="lblVessel" runat="server" Text="&nbsp;&nbsp;Vessel: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                                        <asp:TextBox ID="txtvesselName" runat="server" CssClass="input" Width="60%" Enabled="false" ></asp:TextBox>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblMainEngineType" runat="server" Text="Main Engine Type"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblCylinderNo" runat="server" Text="Cylinder No. (From Ford.)"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblCalibrationPosition" runat="server" Text="Calibration Position"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblPresent" runat="server" Text="Present"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblPrevious" runat="server" Text="Previous"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblInitial" runat="server" Text="Initial"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblNo" runat="server" Text="No."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblmmFromTop" runat="server" Text="mm From Top"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFA" runat="server" Text="F / A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPS" runat="server" Text="P / S"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFA1" runat="server" Text="F / A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPS2" runat="server" Text="P / S"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFA2" runat="server" Text="F / A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPS3" runat="server" Text="P / S"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl1" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl3" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop20" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop21" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop22" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop23" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop24" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop25" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop26" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop31" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop32" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop33" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop34" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop35" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop36" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop37" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl5" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop41" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop42" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop43" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop44" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop45" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop46" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop47" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop51" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop52" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop53" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop54" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop55" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop56" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop57" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl7" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop61" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop62" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop63" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop64" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop65" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop66" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop67" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl8" runat="server" Text="8"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop71" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop72" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop73" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop74" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop75" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop76" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromTop77" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblLinearTemperature" runat="server" Text="Liner Temperature &degC"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtLinearTemp" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="TextBox58" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblLinearStampAndNameofLinearManufacturer" runat="server" Text="Liner Stamp and Name of<br />Liner Manufacturer:"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:literal ID="lblCylinderOilinUseatDateofCalibration" runat="server" Text="Cylinder Oil in Use at<br />Date of Calibration"></asp:literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCylinder1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCylinder2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCylinder3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblDateofCalibration" runat="server" Text="Date of Calibration"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDateCalibration1" runat="server" CssClass="input_mandatory"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDateCalibration2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblDateLinerInstalled" runat="server" Text="Date Liner<br />Installed:"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblPortOfCalibration" runat="server" Text="Port of Calibration"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtPortCalibration" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtPortCalibration1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:literal ID="lblMERunningHoursOnDateLinearInstalled" runat="server" Text="M/E Running Hours<br />on Date Liner<br />Installed"></asp:literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" rowspan="3">
                                        <asp:Literal ID="lblTotalRunningHours" runat="server" Text="Total<br />Running<br />Hours"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblOnLinear" runat="server" Text="On Liner<br />"></asp:Literal>
                                        <asp:TextBox ID="txtOnLinear" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2" rowspan="2">
                                        <asp:TextBox ID="txtOnLinear1" runat="server" CssClass="input"></asp:TextBox><br />
                                        <asp:TextBox ID="txtOnLinear2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2" rowspan="2">
                                        <asp:TextBox ID="txtOnLinear3" runat="server" CssClass="input"></asp:TextBox><br />
                                        <asp:TextBox ID="txtOnLinear4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblPresentCylOilConsumption" runat="server" Text="Present Cyl. Oil Consumption"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblGrmsBhpHr" runat="server" Text="Grms/Bhp/Hr"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblLtrsDay" runat="server" Text="Ltrs/Day"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblOnEngine" runat="server" Text="On Engine"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOnEngine1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOnEngine2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOnEngine3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOnEngine4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Literal ID="lblRunningHours" runat="server" Text="Running Hours<br />(Since New)"></asp:Literal>
                                        <eluc:Number ID="txtNewRunningHours" runat="server" CssClass="input" />
                                    </td>
                                    <td colspan="4">
                                        <asp:Literal ID="lblRunningHoursSincePreviousCalibration" runat="server" Text="Running Hours<br />(Since Previous Calibration)"></asp:Literal>
                                        <eluc:Number ID="txtPreviousRunningHours" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <b><asp:Literal ID="lblWearSinceNew" runat="server" Text="Wear Since New"></asp:Literal></b>
                                    </td>
                                    <td colspan="4" align="center">
                                        <b><asp:Literal ID="lblWearSincePrevioursCalibration" runat="server" Text="Wear Since Previous Calibration"></asp:Literal></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                       <asp:Literal ID="lblMaximum" runat="server" Text="Maximum<br />Wear<br />mm"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblMaximumWearRatemm" runat="server" Text="Maximum<br />Wear Rate<br />Mm/1000 Hrs"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:literal ID="lblMaximumWearRate" runat="server" Text="Maximum<br />Wear Rate<br />Mm/1000 Hrs"></asp:literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblMaximumWearRatemm1000hrs" runat="server" Text="Maximum<br /> Wear Rate<br />Mm/1000 Hrs"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:TextBox ID="txtMaxWear" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:TextBox ID="txtMaxRate" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:TextBox ID="txtMaxWear1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:TextBox ID="txtMaxRate1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8" align="center">
                                        <b><asp:Literal ID="lblDescriptionOfLinearParts" runat="server" Text="Description of Liner Parts (Deposits and Condition)"></asp:Literal></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lblWalls" runat="server" Text="Walls"></asp:Literal>
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="TextBox79" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblPorts" runat="server" Text="Ports"></asp:Literal>
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtPort" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblOilQuils" runat="server" Text="Oil Quills"></asp:Literal>
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtOilQuils" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblOilGrooves" runat="server" Text="Oil Grooves"></asp:Literal>
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtOilGrooves" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblReasonFor" runat="server" Text="Reason For<br />Opening-Up<br />Cylinder"></asp:Literal>
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtReason" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <b><asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal></b>
                                        <br />
                                        <br />
                                    </td>
                                    <td colspan="7">
                                        <asp:Literal ID="lblMaxLinearWearAllowed" runat="server" Text="Max. liner wear allowed : &nbsp;&nbsp;&nbsp;"></asp:Literal>
                                        <asp:TextBox ID="txtMaxLinear" runat="server" CssClass="input" Width="30%"></asp:TextBox>
                                        <asp:Literal ID="lblMaxOvalityALlowed" runat="server" Text="&nbsp;&nbsp;&nbsp; Max. ovality allowed : &nbsp;&nbsp;&nbsp;"></asp:Literal>
                                        <asp:TextBox ID="txtMaxOvality" runat="server" CssClass="input" Width="30%"></asp:TextBox>
                                        <br />
                                        <br />
                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                          
                        </td>
                        <td colspan="6">
                          
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
                            <asp:Literal ID="lblChiefEngineer" runat="server" Text="Chief Engineer"></asp:Literal>
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
