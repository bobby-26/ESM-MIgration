<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormMainEngineStuffingBox.aspx.cs"
    Inherits="StandardFormMainEngineStuffingBox" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E4A Main Engine Stuffing Box </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmStuffingBox" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Main Engine Stuffing Box" ShowMenu="false">
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
                            <asp:literal ID="lblE4A" runat="server" Text="E4A"></asp:literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                          
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl0714REV1" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h2>
                               <asp:literal ID="lblMainEngineStuffingBoxReport" runat="server" Text="Main Engine Stuffing Box Report"></asp:literal><br />
                            </h2>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselName" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblUnitNo" runat="server" Text="Unit.No"></asp:Literal>
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtUnitNo" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblDatePortofOverHaul" runat="server" Text="Date/Port of Overhaul"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDatePort" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblRunningHoursSinceLastOverHaul" runat="server" Text="Running Hours Since Last Overhaul"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRunningHours" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="95%">
                    <tr>
                        <td width="20%">
                            <asp:Image ID="img1" runat="server" ImageUrl="~/StandardForm/Images/EngineStuffingBox.JPG"
                                Height="420px" Width="228px" BorderStyle="Solid" BorderWidth="1px" />
                        </td>
                        <td>
                            <table cellpadding="1" cellspacing="1" class="Sftblclass" width="100%">
                                <tr>
                                    <td align="center" colspan="2" rowspan="3">
                                        <asp:Literal ID="lblRingPosition" runat="server" Text="Ring Position"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="2" rowspan="3">
                                        <asp:Literal ID="lblClearance" runat="server" Text="Clearance
                                        <br />
                                        Between Ring
                                        <br />
                                        and Groove"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="4">
                                       <asp:Literal ID="lblTotalGapClearance" runat="server" Text="Total Gap Clearance"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Literal ID="lblupperSegment" runat="server" Text="Upper Segment"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="2">
                                       <asp:Literal ID="lblLowerSegment" runat="server" Text="Lower Segment"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblStandardupper" runat="server" Text="Standard"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPresentupper" runat="server" Text="Present"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblstandardLower" runat="server" Text="Standard"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPresentlower" runat="server" Text="Present"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:literal ID="lblUpperScraperRing" runat="server" Text="Upper Scraper Ring"></asp:literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA1" runat="server" Text="(A-1)"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblAequal" runat="server" Text="“A”&nbsp;&nbsp; ="></asp:Literal>
                                        <eluc:MaskNumber ID="MaskNumber1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:Literal ID="lblTighteningRing" runat="server" Text="Tightening Ring"></asp:Literal>                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAminus2" runat="server" Text="(A-2)"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblAone" runat="server" Text="“A1” ="></asp:Literal>
                                        <eluc:MaskNumber ID="MaskNumber3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lbltighteningRingB" runat="server" Text="Tightening Ring"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBminus1" runat="server" Text="(B-1)"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblB" runat="server" Text="“B”&nbsp;&nbsp; ="></asp:Literal>
                                        <eluc:MaskNumber ID="MaskNumber4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:literal ID="lblOilScraperRing" runat="server" Text="Oil Scraper Ring"></asp:literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBminus2" runat="server" Text="(B-2)"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblblB1equal" runat="server" Text="“B1” ="></asp:Literal>
                                        <eluc:MaskNumber ID="MaskNumber5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox19" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox20" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:literal ID="lblOilScraperRing2" runat="server" Text="Oil Scraper Ring"></asp:literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblOilScraper" runat="server" Text="(C-1)"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblCequal" runat="server" Text="“C”&nbsp;&nbsp; ="></asp:Literal>
                                        <eluc:MaskNumber ID="MaskNumber6" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox21" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox22" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox23" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox24" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblOilScaperRing3" runat="server" Text="Oil Scraper Ring"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblCminus2" runat="server" Text="(C-2)"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblC1equal" runat="server" Text="“C1” ="></asp:Literal>
                                        <eluc:MaskNumber ID="MaskNumber7" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox25" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox26" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox27" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox28" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <asp:literal ID="lblOilScraperRing4" runat="server" Text="Oil Scraper Ring"></asp:literal>
                                    </td>
                                    <td align="center" class="style1">
                                        <asp:Literal ID="lblCminus3" runat="server" Text="(C-3)"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblC2equal" runat="server" Text="“C2” ="></asp:Literal>
                                        <eluc:MaskNumber ID="MaskNumber8" runat="server" CssClass="input" />
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="TextBox29" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="TextBox30" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="TextBox31" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="TextBox32" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:literal ID="lblOilScraperRingC" runat="server" Text="Oil Scraper Ring"></asp:literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblC4" runat="server" Text="(C-4)"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblC3equal" runat="server" Text="“C3” ="></asp:Literal>
                                        <eluc:MaskNumber ID="MaskNumber9" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox33" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox34" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox35" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox36" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblOilScraperRings6" runat="server" Text="Oil Scraper Rings"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblcminus5" runat="server" Text="(C-5)"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblc4equal" runat="server" Text="“C4” ="></asp:Literal>
                                        <eluc:MaskNumber ID="MaskNumber10" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox37" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox38" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox39" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox40" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblReasonForOverhaul" runat="server" Text="Reason for Overhaul"></asp:Literal>
                                    </td>
                                    <td colspan="3">
                                        <asp:literal ID="lblRoutine" runat="server" Text="Routine"></asp:literal>
                                    </td>
                                    <td colspan="3">
                                        <asp:Literal ID="lblExcessiveOilLeakage" runat="server" Text="Excessive Oil Leakage"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:literal ID="lblConditionOfPistonRod" runat="server" Text="Condition of Piston Rod"></asp:literal>
                                    </td>
                                    <td colspan="6">
                                        <asp:TextBox ID="TextBox46" runat="server" Width="96%" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:literal ID="lblConditionofStuffingRings" runat="server" Text="Condition of Stuffing Box Rings &amp; Garter Springs When Dismantled<br />"></asp:literal>
                                        <asp:TextBox ID="TextBox47" runat="server" Width="96%" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:Literal ID="lblPartsRenewed" runat="server" Text="Parts Renewed (Specify)<br />"></asp:Literal>
                                        <asp:TextBox ID="TextBox48" runat="server" CssClass="input" Width="96%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <br />
                            <asp:TextBox ID="TextBox44" runat="server" CssClass="input"></asp:TextBox><br />
                            <asp:Literal ID="lblSecondEngineer" runat="server" Text="Second Engineer"></asp:Literal>
                        </td>
                        <td align="right">
                            <br />
                            <br />
                            <asp:TextBox ID="TextBox45" runat="server" CssClass="input"></asp:TextBox>
                            <asp:Literal ID="lblChiefEngineer" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                            Chief Engineer&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:literal ID="lblNote" runat="server" Text="Note:<br />
                            1) The above report shall be used for B & W engines only<br />
                            2) This report is to be sent along with form E4, unless stuffing box has been separately
                            overhauled"></asp:literal>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
