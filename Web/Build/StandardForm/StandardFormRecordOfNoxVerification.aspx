<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormRecordOfNoxVerification.aspx.cs"
    Inherits="StandardFormRecordOfNoxVerification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Record of Nox Verification</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmRecordofNox" runat="server" autocomplete="off">
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
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT PTE LTD"></asp:Literal></b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblFileRef" runat="server" Text="File Ref- Marpol Annex VI"></asp:Literal>
                        </td>
                        <td align="right">
                           <asp:Literal ID="lblE12" runat="server" Text="E12"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <b><asp:Literal ID="lblSingapore" runat="server" Text="SINGAPORE"></asp:Literal></b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblCE23" runat="server" Text="C/E 23"></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl605Rev1" runat="server" Text="(6/05 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h3>
                                <asp:Literal ID="lblRecordOfNoxVerificationMEAE" runat="server" Text="RECORD of Nox VERIFICATION-ME/AE"></asp:Literal><br />
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name :"></asp:Literal>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtvesselName" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblDate" runat="server" Text="Date :"></asp:Literal>
                        </td>
                        <td>
                           <eluc:Date ID="txtDate" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblengineMakeType" runat="server" Text="Engine Make & Type :"></asp:Literal>
                        </td>
                        <td colspan="7">
                            <asp:TextBox ID="txtEngineTyeMake" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblMainEngineAuxEngine" runat="server" Text="Main Engine / Aux. Engine :"></asp:Literal>
                        </td>
                        <td colspan="7">
                            <asp:TextBox ID="txtMainEngineAuxEngine" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <br />
                            <asp:Literal ID="lblPhysicalParameter" runat="server" Text="Physical parameter (components and setting)"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="95%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                        <td align="center">
                            <asp:literal ID="lblNo" runat="server" Text="No"></asp:literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblMakesPartNo" runat="server" Text="Maker’s Part No"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblNewMarkingOrSetting" runat="server" Text="New Marking or Setting*
                            <br />
                            See Note"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblPhysicalParameterDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblReasonforUpdate" runat="server" Text="Reason for update"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblNo1" runat="server" Text="1"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblFuelValueNozzle" runat="server" Text="Fuel valve nozzle (atomizer)"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate1" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:Literal ID="lblNo2" runat="server" Text="2"></asp:Literal>
                        </td>
                        <td>
                           <asp:Literal ID="lblFuelPumpPlunger" runat="server" Text="Fuel pump plunger"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate2" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:Literal ID="lblNo3" runat="server" Text="3"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblFuelPumpBarrel" runat="server" Text="Fuel pump barrel"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno3" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting3" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate3" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate3" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:Literal ID="lblNo4" runat="server" Text="4"></asp:Literal>
                        </td>
                        <td>
                           <asp:Literal ID="lblCylinderLiner" runat="server" Text="Cylinder liner"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno4" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting4" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate4" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate4" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblNo5" runat="server" Text="5"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblCylinderCover" runat="server" Text="Cylinder cover"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno5" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting5" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate5" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate5" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:Literal ID="lbl6" runat="server" Text="6"></asp:Literal>
                        </td>
                        <td>
                           <asp:Literal ID="lblPistonCrown" runat="server" Text="Piston crown"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno6" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting6" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate6" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate6" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblNo7" runat="server" Text="7"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblexhaustCam" runat="server" Text="Exhaust cam"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno7" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting7" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate7" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate7" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblNo8" runat="server" Text="8"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblFuelCam" runat="server" Text="Fuel cam"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno8" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting8" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate8" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate8" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblNo9" runat="server" Text="9"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblTurboCharger" runat="server" Text="Turbocharger"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno9" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting9" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate9" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate9" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lbl10" runat="server" Text="10"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblAirCooler" runat="server" Text="Air cooler"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno10" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting10" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate10" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate10" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lbl11" runat="server" Text="11"></asp:Literal>
                        </td>
                        <td>
                           <asp:Literal ID="lblCompressionShimThickness" runat="server" Text="Compression shim thickness"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkerspartno11" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewMarkingTesting11" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate11" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonforUpdate11" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Literal ID="lblNote" runat="server" Text="Note:<br />
                            New marking or setting column to be filled. In case of any new design or modification
                            done
                            <br />
                            on existing components or if any existing setting or timing is changed as compared
                            to
                            <br />
                            maker’s original specification."></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="input" Width="300px"></asp:TextBox><br />
                            <asp:Literal ID="lblNameSignature" runat="server" Text="Name / Signature of Chief Engineer&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
