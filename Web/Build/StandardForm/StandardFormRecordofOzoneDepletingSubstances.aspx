<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormRecordofOzoneDepletingSubstances.aspx.cs"
    Inherits="StandardForm_StandardFormRecordofOzoneDepletingSubstances" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Record of Ozone Depleting Substances </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="FrmRecordofOzoneDepletingSubstances" runat="server">
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
                    <b><asp:Literal ID="lblexecutiveOffshore" runat="server" Text="EXECUTIVE OFFSHORE"></asp:Literal></b>
                </td>
                <td align="left">
                    <asp:Literal ID="lblFileRef1504" runat="server" Text="File Ref – 150.4"></asp:Literal>
                </td>
                <td align="right" colspan="2">
                    <asp:Literal ID="lblE28" runat="server" Text="E28"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="5" align="left">
                </td>
                <td align="left">
                    <asp:Literal ID="lblCE23" runat="server" Text="C/E 23"></asp:Literal>
                </td>
                <td align="right" colspan="2">
                   <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="8" align="right">
                    <asp:Literal ID="lbl813Rev1"  runat="server" Text="(8/13 Rev 1)"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td colspan="8" align="center">
                    <h2>
                       <asp:Literal ID="lblRecordofOzoneDepletingSubstances" runat="server" Text="Record of Ozone Depleting Substances / Global Warming Potential substances"></asp:Literal>
                    </h2>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblvesselName" runat="server" Text="Vessel Name"></asp:Literal>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtGovernorMakeType" runat="server" CssClass="input"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                </td>
                <td colspan="2">
                    <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
        </table>
    </div>
    <div>
    <table class="Sftblclass" cellpadding="1" cellspacing="1">
        <tr>
            <td rowspan="2">
                <asp:Literal ID="lblsrNo" runat="server" Text="Sr No"></asp:Literal>
            </td>
            <td rowspan="2">
                <asp:Literal ID="lblDate1" runat="server" Text="Date"></asp:Literal>
            </td>
            <td rowspan="2">
               <asp:Literal ID="lblLocationOnboard" runat="server" Text="Location onboard"></asp:Literal>
            </td>
            <td colspan="2">
                <asp:Literal ID="lblChooseGWP" runat="server" Text="Choose GWP by inserting 'X' underneath"></asp:Literal>
            </td>
            <td rowspan="2">
                <asp:Literal ID="lblRefrigerantName" runat="server" Text="Refrigerant name"></asp:Literal>
            </td>
            <td rowspan="2">
                <asp:Literal ID="lblODSCategory" runat="server" Text="ODS category?( Note 'Y'  or 'N' in this column)"></asp:Literal>
            </td>
            <td rowspan="2">
                <asp:Literal ID="lbLCAPKG" runat="server" Text="Cap KG"></asp:Literal>
            </td>
            <td rowspan="2">
                <asp:Literal ID="lblNameofEquipment" runat="server" Text="Name of Equipment"></asp:Literal>
            </td>
            <td colspan="5">
                <asp:Literal ID="lblChooseAppropriate" runat="server" Text="Choose appropriate occasion by inserting 'X' underneath"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblBelow3500" runat="server" Text="Below 3500"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="lblBelow1890" runat="server" Text="Below 1890"></asp:Literal>
            </td>
            <td>
                <asp:literal ID="lblRecharge" runat="server" Text="Recharge(Note full or partial)"></asp:literal>
            </td>
            <td>
                <asp:Literal ID="lblRepairMaintenance" runat="server" Text="Repair / Maintenance"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="lblDischargeToATM" runat="server" Text="Discharge to atm(note delibrate or non-deliberate"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="lblDischargetoReceiptionFacility" runat="server" Text="Discharge to receiption facility"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="lblSupplytoShip" runat="server" Text="Supply to ship"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblNo1" runat="server" Text="1"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate1" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard1" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP35001" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP18901" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname1" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory1" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG1" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment1" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge1" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair1" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm1" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption1" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship1" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblNo2" runat="server" Text="2"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate2" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard2" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP35002" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP18902" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname2" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory2" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG2" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment2" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge2" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair2" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm2" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption2" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship2" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblNo3" runat="server" Text="3"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate3" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard3" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP35003" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP18903" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname3" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory3" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG3" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment3" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge3" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair3" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm3" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption3" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship3" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblNo4" runat="server" Text="4"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate4" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard4" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP35004" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP18904" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname4" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory4" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG4" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment4" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge4" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair4" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm4" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption4" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship4" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
              <asp:Literal ID="lblNo5" runat="server" Text="5"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate5" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard5" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP35005" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP18905" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname5" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory5" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG5" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment5" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge5" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair5" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm5" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption5" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship5" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblNo6" runat="server" Text="6"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate6" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard6" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP35006" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP18906" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname6" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory6" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG6" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment6" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge6" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair6" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm6" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption6" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship6" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
               <asp:Literal ID="lblNo7" runat="server" Text="7"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate7" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard7" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP35007" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP18907" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname7" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory7" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG7" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment7" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge7" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair7" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm7" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption7" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship7" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
               <asp:Literal ID="lblNo8" runat="server" Text="8"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate8" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard8" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP35008" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP18908" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname8" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory8" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG8" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment8" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge8" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair8" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm8" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption8" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship8" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblNo9" runat="server" Text="9"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate9" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard9" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP35009" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP18909" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname9" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory9" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG9" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment9" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge9" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair9" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm9" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption9" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship9" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
               <asp:Literal ID="lblNo10" runat="server" Text="10"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtDate10" runat="server" CssClass="input" DatePicker="true" />
            </td>
            <td>
                <asp:TextBox ID="txtLocationOnBoard10" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP350010" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtGWP189010" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRefrigerantname10" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtODSCategory10" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <eluc:Number ID="txtCapKG10" runat="server" CssClass="input" />
            </td>
            <td>
                <asp:TextBox ID="txtNameOfEquipment10" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRecharge10" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtRepair10" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoatm10" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtDischargetoreceiption10" runat="server" CssClass="input"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtSupplytoship10" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        
    </table>
   <table>
   <tr><td >  </td>  </tr>
   <tr>
   <td ><br /><br />
   <asp:Label id="lblNameSignatureofChiefEngineer" Text="Name / Signature of Chief Engineer" runat="server"></asp:Label></td></tr>
   <tr>
   
   <td colspan="2"><br />
    <asp:Label ID="lblNote" Text="Note : Executive Honour & Executive Pride refrigerants R 134A, 404A & R507 are Not falling under Ozone Depleting substances. Their GWP is less than 3500
Executive Valour & Executive Courage refrigerants R407C is not falling under Ozone depleting substance. Its GWP is less than 1890." runat="server"></asp:Label>
 </td> </tr>
   </table>
     </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
