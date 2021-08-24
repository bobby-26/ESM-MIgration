StandardFormExhaustValveOverhul.aspx<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormExhaustValveOverhul.aspx.cs"
    Inherits="StandardFormExhaustValveOverhul" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Exhaust Valve Overhul</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmExhaust" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Exhaust Valve Overhul" ShowMenu="false">
            </eluc:Title>
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>       
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
            <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                <table width="95%">
                    <tr>
                        <td colspan="2" align="left">
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT"></asp:Literal></b>
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblE23" runat="server" Text="E23"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Literal ID="lbl0714Rev1" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblvessel" runat="server" Text="VESSEL"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselName" runat="server" CssClass="input" Enabled="false" ></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblDate" runat="server" Text="DATE:"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Literal ID="lblMainEngineType" runat="server" Text="MAIN ENGINE TYPE -"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <h3 style="margin-bottom: 0px">
                               <asp:Literal ID="lblMainEngineExhaustValuesOverHaulRecord" runat="server" Text="MAIN ENGINE EXHAUST VALVES OVERHAUL RECORD"></asp:Literal>
                                <br />
                            </h3>
                            <asp:Literal ID="lblTobesenttotheOfficeEveryMonth" runat="server" Text="(To be sent to the office every month)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblExhaustValueBodyNo" runat="server" Text="EXHAUST VALVE BODY NUMBER"></asp:Literal>
                                    </td>                                        
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl1" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl3" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl5" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl7" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl8" runat="server" Text="8"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl9" runat="server" Text="9"></asp:Literal>
                                    </td>
                                </tr>
                                 <tr>
                                     <td colspan="3">
                                        <asp:Literal ID="lblPresentPositionOfTheValue" runat="server" Text="PRESENT POSITION OF THE VALVE"></asp:Literal>
                                     </td>
                                    <td>
                                        <asp:TextBox ID="txtpov1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpov2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpov3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpov4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpov5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpov6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpov7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpov8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpov9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                <td colspan="3">
                                    <asp:Literal ID="lblRunningHoursSinceNew" runat="server" Text="RUNNING HOURS SINCE NEW (approximate)"></asp:Literal>
                                </td>                                          
                                    <td>
                                        <asp:TextBox ID="txtRunNew1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunNew2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunNew3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunNew4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunNew5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunNew6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunNew7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunNew8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunNew9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblRunningHrsSinceLastOhaul" runat="server" Text="RUNNING HRS SINCE LAST O'HAUL"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunLast1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunLast2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunLast3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunLast4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunLast5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunLast6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunLast7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunLast8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunLast9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblDateInstalled" runat="server" Text="DATE INSTALLED"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateInstall1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateInstall2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateInstall3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateInstall4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateInstall5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateInstall6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateInstall7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateInstall8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateInstall9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblValueSpindleNumber" runat="server" Text="VALVE SPINDLE NUMBER"></asp:Literal>
                                     </td>                                                                 
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo1" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo2" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo3" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                        <br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo5" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo6" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo7" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo8" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo9" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">                                      
                                        <asp:Literal ID="lblrunningHoursSinceNewRunningHoursSinceLastRecord" runat="server" Text="RUNNING HOURS SINCE NEW RUNNING HOURS SINCE LAST RECOND"></asp:Literal>                                              
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo21" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo22" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo23" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo24" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo25" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo26" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo27" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo28" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo29" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblRunningHoursSinceLastRecond" runat="server" Text="RUN'G HOURS SINCE LAST RECOND"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo31" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo32" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo33" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo34" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo35" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo36" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo37" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo38" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo39" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblNoOfTimesReconditioned" runat="server" Text="NO OF TIMES RECONDITIONED "></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo41" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo42" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo43" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo44" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo45" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo46" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo47" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo48" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValveSpNo49" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblValueSeaatNo" runat="server" Text="VALVE SEAT NUMBER"></asp:Literal>
                                    </td>                                                                                
                                                                      
                                    <td>
                                        <asp:TextBox ID="txtSeatNo1" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo2" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo3" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                        <br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo5" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo6" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo7" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo8" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo9" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblRunningHoursSinceNew1" runat="server" Text="RUNNING HOURS SINCE NEW"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo21" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo22" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo23" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo24" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo25" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo26" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo27" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo28" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo29" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblRunningHrsSinceLastRecond" runat="server" Text="RUNNING HRS SINCE LAST RECOND"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo31" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo32" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo33" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo34" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo35" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo36" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo37" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo38" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo39" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                         <asp:Literal ID="lblNoOfTimesReconditioned1" runat="server" Text="NO OF TIMES RECONDITIONED"></asp:Literal>
                                    </td>                                    
                                    <td>
                                        <asp:TextBox ID="txtSeatNo41" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo42" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo43" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo44" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo45" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo46" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo47" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo48" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeatNo49" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblBushForSpindleGuide" runat="server" Text="BUSH FOR<br />SPINDLE GUIDE"></asp:Literal>
                                        
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblTop" runat="server" Text="TOP"></asp:Literal>
                                    </td>
                                    <td>
                                       <asp:Literal ID="lbl404mm" runat="server" Text="40.4mm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuide1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuide2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuide3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuide4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuide5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuide6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuide7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuide8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuide9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:Literal ID="lblBTM" runat="server" Text="BTM"></asp:Literal>
                                    </td>
                                    <td>
                                       <asp:Literal ID="lbl418mm" runat="server" Text="41.8mm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuideBtm1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuideBtm2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuideBtm3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuideBtm4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuideBtm5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuideBtm6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuideBtm7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuideBtm8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpGuideBtm9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="5">
                                        <asp:Literal ID="lblBurnAwayOfValueSpindleMeasuredAt4Sides" runat="server" Text="BURN AWAY OF<br /><br />VALVE SPINDLE<br /><br />MEASURED AT<br /><br />4 SIDES (max)"></asp:Literal>      </td>
                                    <td>
                                        <asp:Literal ID="lblA" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl60mm" runat="server" Text="6.0mm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurn1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurn2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurn3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurn4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurn5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurn6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurn7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurn8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurn9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblB" runat="server" Text="B"></asp:Literal>
                                    </td>
                                    <td>
                                       <asp:Literal ID="lbl600mm" runat="server" Text="6.0mm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnB1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnB2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnB3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnB4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnB5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnB6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnB7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnB8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnB9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblC" runat="server" Text="C"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl600mm1" runat="server" Text="6.0mm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnC1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnC2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnC3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnC4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnC5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnC6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnC7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnC8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnC9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblD" runat="server" Text="D"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl600mm2" runat="server" Text="6.0mm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnD1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnD2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnD3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnD4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnD5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnD6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnD7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnD8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnD9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblE" runat="server" Text="E"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl600mm3" runat="server" Text="6.0mm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnE1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnE2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnE3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnE4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnE5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnE6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnE7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnE8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBurnE9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblMinDIAOfValueSpindle" runat="server" Text="MIN. DIA OF<br />VALVE SPINDLE<br />"></asp:Literal>                         </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lbl398mm" runat="server" Text="39.8mm<br />"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiaSpindile1" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiaSpindile2" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiaSpindile3" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiaSpindile4" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiaSpindile5" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiaSpindile6" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiaSpindile7" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiaSpindile8" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiaSpindile9" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3">
                                        <asp:Literal ID="lblValueSpindle" runat="server" Text="VALVE SPIINDLE<br />/ SEAT<br />GRINDING STEM"></asp:Literal>                              </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblG120mm" runat="server" Text="G1 = 2.0 mm (max)"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblG22mm" runat="server" Text="G2 = 2 mm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat11" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat12" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat13" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat14" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat15" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat16" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat17" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat18" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat19" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblG3mm" runat="server" Text="G3 = 3 mm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat31" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat32" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat33" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat34" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat35" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat36" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat37" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat38" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpindileSeat39" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblOilCylDiameter" runat="server" Text="OIL CYL DIAMETER"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lbl502mmMax" runat="server" Text="50.2mm max"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOilCy1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOilCy2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOilCy3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOilCy4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOilCy5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOilCy6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOilCy7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOilCy8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOilCy9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblPistonRings" runat="server" Text="PISTON RINGS"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lbl20mmMIn" runat="server" Text="2.0mm min"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPistonRing1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPistonRing2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPistonRing3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPistonRing4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPistonRing5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPistonRing6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPistonRing7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPistonRing8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPistonRing9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblRemarks" runat="server" Text="REMARKS"></asp:Literal><br />
                                    </td>
                                    <td colspan="2">
                                        <br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark1" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark2" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark3" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark4" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark5" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark6" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark7" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark8" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark9" runat="server" CssClass="input" Width="70px"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="4">
                                        <asp:Literal ID="lblOverHaulDetails" runat="server" Text="OVERHAUL<br />DETAILS"></asp:Literal>                               </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblDate1" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblDate2" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul21" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul22" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul23" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul24" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul25" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul26" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul27" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul28" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul29" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                       <asp:Literal ID="lblDate3" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul31" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul32" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul33" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul34" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul35" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul36" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul37" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul38" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul39" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblDate4" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul41" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul42" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul43" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul44" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul45" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul46" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul47" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul48" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOverhul49" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="6">
                                        <asp:Literal ID="lblSparesUsed" runat="server" Text="SPARES USED"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed3" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed4" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed7" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed8" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed9" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed21" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed22" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed23" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed24" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed25" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed26" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed27" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpareUsed28" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed29" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpareUsed31" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparsUsed22" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpareUsed23" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpareUsed24" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpareUsed25" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpareUsed26" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpareUsed27" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed28" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpareUsed29" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed31" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed32" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed33" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed34" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed35" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed36" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed37" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed38" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed39" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed41" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed42" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed43" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed44" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed45" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed46" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed47" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed48" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed49" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed51" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed52" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed53" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed54" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed55" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed56" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed57" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed58" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSparesUsed59" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Literal ID="lblChiefEngineer" runat="server" Text="CHIEF ENGINEER&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                            <asp:TextBox ID="TextBox316" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                           <asp:Literal ID="lblNote" runat="server" Text="Note :"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                           <asp:Literal ID="lblClearancesDepictedAbovewillVary" runat="server" Text="Clearances depicted above will vary depending on the engine make and type. Please
                            fill in values as instructed by Maker."></asp:Literal>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
