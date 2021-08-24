<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormCrankwebsDeflection.aspx.cs"
    Inherits="StandardFormCrankwebsDeflection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crankwebs Deflection</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmCrankwebsDeflectionx" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Crankwebs Deflection" ShowMenu="false">
            </eluc:Title>
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>        
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
             <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                <table width="95%">
                    <tr>
                        <td colspan="5" align="left">
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT"></asp:Literal></b>
                        </td>
                        <td align="left">
                          
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblE2" runat="server" Text="E2"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="left">
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" align="right">
                            <asp:Literal ID="lbl0714Rev1" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h3>
                               <asp:Literal ID="lblCargoTankInspectionReport" runat="server" Text="CRANKWEBS DEFLECTION"></asp:Literal>
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtVesselName" runat="server" CssClass="input" Enabled="false" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPort" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblDate" runat="server" Text=" Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                           <asp:Literal ID="lbltypeEngine" runat="server" Text="Type Engine"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtEnginType" runat="server" CssClass="input" ></asp:TextBox>
                        </td>
                        <td colspan="2">
                           <asp:Literal ID="lblMainEngineAuxEngineNo" runat="server" Text="Main Engine/Aux Engine No"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainEngineAuxEngine" runat="server" CssClass="input" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                           <asp:Literal ID="lbLDateLastDeflectionTaken" runat="server" Text="Date last deflection taken"></asp:Literal>
                        </td>
                        <td colspan="4">
                             <asp:TextBox ID="txtLastDeflection" runat="server"  CssClass="input"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" PopupPosition="BottomRight" runat="server"  TargetControlID="txtLastDeflection">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lbLDraft" runat="server" Text="DRAFT:"></asp:Literal>
                        </td>
                        <td>
                           <asp:Literal ID="lblFord" runat="server" Text="Ford"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFord" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblAft" runat="server" Text="Aft"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAft" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblTrim" runat="server" Text="Trim"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTrim" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Literal ID="lblStartWithCrankatBDC" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1. Start with crank at B.D.C."></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Literal ID="lblMoveCranktoPositionA" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2. Move crank to position ‘A’ and fit gauge between the webs, set dial to zero."></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Literal ID="lbl3ContinueTurningEngineinDirectionShown" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3. Continue turning engine in direction shown (ahead direction) and read gauge at<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;position ‘B’, ‘C’, ‘D’, and ‘E’ of crankpin, without stopping movement of<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;crankshaft."></asp:Literal>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Literal ID="lbl4OpeningofWebstobeRecorded" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 4. Opening of webs to be recorded as +."></asp:Literal>
                        </td>
                        <td colspan="4" rowspan="2" align="center">
                            <asp:Image ID="img1" runat="server" ImageUrl="~/StandardForm/Images/CrankwebDeflectionDiagram1.PNG" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Literal ID="lbl5ClosingOfWebsToBeRecordedAs" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 5. Closing of webs to be recorded as --."></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <asp:Image ID="img2" runat="server"  ImageUrl="~/StandardForm/Images/CrankwebDeflectionDiagram2.PNG"
                                Width="80%" Height="454px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:literal ID="lblEngineStroke" runat="server" Text="Engine stroke"></asp:literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEngineStoke" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblmm" runat="server" Text="mm"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblCrankcaseTemperature" runat="server" Text="Crankcase temperature"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemperature" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="3">
                            &degC<br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblCrankpinPosition" runat="server" Text="Crankpin<br /> Position"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAFT1" runat="server" Text="AFT"></asp:Literal>
                                    </td>
                                    <td colspan="7" align="center">
                                        <asp:Literal ID="lblCylinderNo" runat="server" Text="Cylinder No. as per Classification Society"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFORD1"  runat="server" Text="FORD"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lbl9" runat="server" Text="9"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl8" runat="server" Text="8"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl7" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td align="center">
                                       <asp:Literal ID="lbl6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td align="center">
                                       <asp:Literal ID="lbl5" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl3" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td align="center">
                                     <asp:Literal ID="lbl1" runat="server" Text="1"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblA" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinA1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinA2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinA3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinA4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinA5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinA6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinA7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinA8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinA9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lblB" runat="server" Text="B"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinB1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinB2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinB3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinB4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinB5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinB6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinB7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinB8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinB9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblC" runat="server" Text="C"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinC1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinC2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinC3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinC4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinC5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinC6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinC7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinC8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinC9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblD" runat="Server" Text="D"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinD1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinD2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinD3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinD4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinD5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinD6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinD7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinD8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinD9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblE" runat="server" Text="E"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinE1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinE2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinE3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinE4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinE5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinE6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinE7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinE8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinE9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lblMaxDeflection" runat="server" Text="Max<br />Deflection"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinMax1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinMax2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinMax3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinMax4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinMax5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinMax6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinMax7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinMax8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtCrankpinMax9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                          
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtChiefEngineer" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                          
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
