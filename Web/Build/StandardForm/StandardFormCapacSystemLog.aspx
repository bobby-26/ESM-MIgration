<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormCapacSystemLog.aspx.cs"
    Inherits="StandardFormCapacSystemLog" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E17 Capac System Log</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmCapacSystemLog" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Capac System Log" ShowMenu="false">
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
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT PTE LTD"></asp:Literal></b>
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:LIteral ID="E17" runat="server" Text="E17"></asp:LIteral>
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
                                <asp:Literal id="lblCapacSystemLog" runat="server" Text="CAPAC SYSTEM LOG<br />AUTO CONTROLLER POWER SUPPLY"></asp:Literal>
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h4>
                                <asp:Literal ID="lblLocationOfCapacSystem" runat="server" Text="LOCATION OF CAPAC SYSTEM<br />AFT / FORE"></asp:Literal>
                                
                            </h4>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselName" runat="server" Text="VESSEL NAME"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselName" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblVoyageNo" runat="server" Text="VOYAGE NO:"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtVoyageNO" runat="server" CssClass="input"></asp:TextBox>&nbsp;
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblFrom" runat="server" Text="FROM"></asp:Literal>
                        </td>
                        <td align="center">
                           <eluc:Date ID="txtDateFrom" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblTo" runat="server"  Text="TO">
                            </asp:Literal>
                        </td>
                        <td align="center">
                           <eluc:Date ID="txtDateTO" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                       
                        <td colspan="2" align="left">
                            <b><asp:Literal ID="lblAFT" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;AFT"></asp:Literal></b> &nbsp;
                        </td>
                        <td>
                        </td>
                        <td align="left">
                            &nbsp; <b><asp:Literal ID="lblFORE" runat="server" Text="&nbsp;&nbsp;&nbsp;FORE"></asp:Literal></b>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <table width="95%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                        <td rowspan="3" colspan="2">
                            <asp:Literal ID="lblDateNoon" runat="server" Text="DATE NOON"></asp:Literal>
                        </td>
                        <td align="center" rowspan="3" colspan="2">
                            <asp:Literal ID="lblShipLocation" runat="server" Text="SHIP&#39;S <br />LOCATION"></asp:Literal>                       </td>
                        <td align="center" rowspan="2" colspan="2">
                            <asp:Literal ID="lblAutoContSetting" Runat="server" Text="AUTO CONT.<br />  SETTING"></asp:Literal>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Literal ID="lblRefCellCheck" runat="server" Text="REF. CELL CHECK"></asp:Literal>
                        </td>
                        <td align="center" rowspan="2" colspan="2">
                            <asp:Literal ID="lblAnodeCurrent1" runat="server" Text="ANODE<br />CURRENT"></asp:Literal>
                        </td>
                        <td align="center" rowspan="2" colspan="2">
                            <asp:Literal ID="lblAutoContSetting1" runat="server" Text="AUTO<br />CONT.<br />SETTING"></asp:Literal>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Literal ID="lblRefCellCheck1" runat="server" Text="REF. CELL CHECK"></asp:Literal>
                        </td>
                        <td align="center" rowspan="2" colspan="2">
                            <asp:Literal ID="lblAnodeCurrent2" runat="server" Text="ANODE <br /> CURRENT"></asp:Literal>
                        </td>
                        <td align="center" rowspan="3" colspan="2">
                            <asp:Literal ID="lblShaftHull" runat="server" Text="SHAFT HULL<br />&nbsp;mV METER"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblB" runat="server" Text="B"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblC1" runat="server" Text="C"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblBB" runat="server" Text="B"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblC2" runat="server" Text="C"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Literal ID="lblA2" runat="server" Text="A"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblCont" runat="server" Text="CONT"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lbLAVR" runat="server" Text="AVR"></asp:Literal>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Literal ID="lblD" runat="server" Text="D"></asp:Literal>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Literal ID="lblA1" runat="server" Text="A"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblCont2" runat="server" Text="CONT"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblAvr1" runat="server" Text="AVR"></asp:Literal>
                        </td>
                        <td align="center" colspan="2">
                           <asp:Literal ID="lblD1" runat="server" Text="D"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto11" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont11" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr11" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode11" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto21" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont21" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr21" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode21" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto12" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont12" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr12" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode12" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto22" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont22" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr22" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode22" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon3" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation3" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto13" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont13" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr13" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode13" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto23" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont23" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr23" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode23" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull3" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon4" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation4" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto14" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont14" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr14" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode14" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto24" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont24" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr24" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode24" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull4" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon5" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation5" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto15" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox3" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox4" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox6" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox7" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox8" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon6" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation6" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto16" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont16" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr16" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode16" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto26" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont26" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr26" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode26" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull226" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon7" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation7" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto17" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont17" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr17" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode17" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto27" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont27" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr27" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode27" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull7" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon8" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation8" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto18" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont18" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr18" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode18" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto28" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont28" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr28" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode28" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull8" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon9" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation9" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto19" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont19" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr19" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode19" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto29" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont29" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr29" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode29" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull9" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon10" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation10" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto110" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont110" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr110" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode110" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto210" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont210" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr210" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode210" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull10" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon11" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation11" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto111" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont111" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr111" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode111" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto211" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont211" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr211" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode211" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull11" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon12" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation12" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto112" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont112" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr112" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode112" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto212" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont212" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr212" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode212" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull12" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon13" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation13" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto113" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont113" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr113" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode113" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto213" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont213" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr213" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode213" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull13" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon14" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation14" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto114" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont114" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr114" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode114" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto214" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont214" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr214" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode214" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull14" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon15" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation15" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto115" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont115" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr115" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode115" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto215" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont215" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr215" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode215" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull15" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon16" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation16" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto116" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont116" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr116" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode116" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto216" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont216" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr216" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode216" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull16" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon17" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation17" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto117" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont117" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr117" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode117" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto217" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont217" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr217" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode217" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull17" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon18" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation18" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto118" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont118" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr118" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode118" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto218" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont218" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr218" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode218" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull18" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="Date2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox19" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox20" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox21" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox22" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox23" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox24" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox25" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox26" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox27" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextBox28" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon20" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation20" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto120" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont120" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr120" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode1201" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto2201" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont2201" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr2201" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode2201" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull20" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon21" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation21" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto121" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont121" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr121" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode121" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto221" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont221" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr221" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode221" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull21" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon22" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation22" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto122" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont122" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr122" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode122" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto222" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont222" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr222" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode222" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull22" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon23" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation23" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto123" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont123" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr123" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode123" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto223" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont223" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr223" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode223" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull23" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon24" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation24" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto124" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont124" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr124" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode124" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto224" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont224" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr224" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode224" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull24" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon25" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation25" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto125" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont125" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr125" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode125" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto225" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont225" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr225" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode225" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull25" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon26" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation26" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto126" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont126" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr126" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode126" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto226" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont226" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr226" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode226" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull26" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon27" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation27" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto127" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont127" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr127" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode127" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto227" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont227" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr227" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode227" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull27" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon28" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation28" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto128" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont128" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr128" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode128" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto228" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont228" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr228" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode228" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull28" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon29" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation29" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto129" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont129" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr129" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode129" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto229" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont229" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr229" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode229" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull29" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <eluc:Date ID="txtdatenoon30" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtlocation30" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto130" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont130" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr130" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode130" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtauto230" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcont230" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtavr230" runat="server" CssClass="input" Width="70px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtanode230" runat="server" CssClass="input" Width="80px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txthull30" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center" class="dummy">
                            <br />
                            <br />
                            <asp:TextBox ID="txtElectricalofficer" runat="server" CssClass="input"></asp:TextBox><br />
                            <asp:Literal ID="lblElectricalOfficer" runat="server" Text="ELECTRICAL OFFICER"></asp:Literal>
                        </td>
                        <td colspan="4">
                            &nbsp;
                        </td>
                        <td colspan="8" align="center" class="dummy">
                            <br />
                            <br />
                            <asp:TextBox ID="txtchiefengineer" runat="server" CssClass="input"></asp:TextBox><br />
                            <asp:Literal ID="lblChiefEngineer" runat="server" Text="CHIEF ENGINEER"></asp:Literal>
                        </td>
                    </tr>
                    
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
