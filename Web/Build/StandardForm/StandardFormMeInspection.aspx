<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormMeInspection.aspx.cs"
    Inherits="StandardFormMeInspection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>26 E25 Me Inspection Through Scavenge Ports </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmMeInspection" runat="server" autocomplete="off">
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
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT "></asp:Literal></b>
                        </td>
                        <td align="left">
                          
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblE25" runat="server" Text="E25"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="left">
                            &nbsp;
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
                        <td colspan="7" align="center">
                            <h2 style="margin-bottom: 0px">
                               <asp:Literal ID="lblMEInspectionThroughScavengePorts" runat="server" Text="M/E INSPECTION THROUGH SCAVENGE PORTS<br />"></asp:Literal>
                            </h2>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:literal ID="lblInspectionDate" runat="server" Text="Inspection Date"></asp:literal>
                        </td>
                        <td>
                             <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                        <td>
                           <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPort" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                           <asp:Literal ID="lblChiefEngineer" runat="server" Text="Chief Engineer"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChiefEngineer" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table width="95%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Literal ID="lblEnginePart" runat="server" Text="&nbsp;Engine Part"></asp:Literal>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                           <asp:Literal ID="lblCondition" runat="server" Text="&nbsp;Condition"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblsimbol" runat="server" Text="&nbsp;Simbol"></asp:Literal>
                        </td>
                        <td colspan="8" align="center">
                            <asp:Literal ID="lblcylinderNumber" runat="server" Text="Cylinder Number"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lblCylinderNo1" runat="server" Text="&nbsp;1"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblCylinderNo2" runat="server" Text="&nbsp;2"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblCylinderNo3" runat="server" Text="&nbsp;3"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblCylinderNo4" runat="server" Text="&nbsp;4"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblCylinderNo5" runat="server" Text="&nbsp;5"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblCylinderNo6" runat="server" Text="&nbsp;6"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblCylinderNo7" runat="server" Text="&nbsp;7"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblCylinderNo8" runat="server" Text="&nbsp;8"></asp:Literal>
                        </td>
                    </tr>
                    <%--Header--%>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPiston" runat="server" Text="&nbsp;Piston"></asp:Literal>
                            <br />
                            <br />
                            <asp:Literal ID="lblCrown" runat="server" Text="&nbsp;Crown"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrown0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td rowspan="2">
                            <asp:Literal ID="lblCarbonBurningLeakageOilLeakageWaterCarbon" runat="server" Text="&nbsp;Carbon<br />
                            &nbsp;Burning<br />
                            &nbsp;Leakage Oil<br />
                            &nbsp;Leakage Water<br />
                            &nbsp;Carbon<br />
                            &nbsp;Polished Carbon<br />"></asp:Literal>
                        </td>
                        <td rowspan="2">
                            <asp:Literal ID="lblCBuLoLwCPc" runat="server" Text="&nbsp;C<br />
                            &nbsp;BU<br />
                            &nbsp;LO<br />
                            &nbsp;LW<br />
                            &nbsp;C<br />
                            &nbsp;PC<br />"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPisten1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPisten2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPisten3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPisten4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPisten5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPisten6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPisten7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPisten8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblTopland" runat="server" Text="&nbsp;Topland"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTop0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTop1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTop2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTop3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTop4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTop5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTop6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTop7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTop8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRing" runat="server" Text="&nbsp;Ring 1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing10" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td rowspan="4">
                            <asp:Literal ID="lblIntactBrokenOppositeRingGap" runat="server" Text="&nbsp;Intact<br />
                            &nbsp;Broken opposite ring gap<br />
                            &nbsp;Broken near Gap<br />
                            &nbsp;Several pieces<br />
                            &nbsp;Entirely missing"></asp:Literal>
                            <br />
                            <br />
                            <br />
                        </td>
                        <td rowspan="4">
                            <asp:Literal ID="lblIBOBNSPM" runat="server" Text="&nbsp;I<br />
                            &nbsp;BO<br />
                            &nbsp;BN<br />
                            &nbsp;SP<br />
                            &nbsp;M
                            <br />
                            <br />
                            <br />"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing11" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing12" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing13" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing14" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing15" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing16" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing17" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing18" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRing2" runat="server" Text="&nbsp;Ring 2"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing21" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing22" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing23" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing24" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing25" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing26" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing27" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing28" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing29" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRing3" runat="server" Text="&nbsp;Ring 3"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing30" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing31" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing32" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing33" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing34" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing35" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing36" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing37" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsRing38" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRing4" runat="server" Text="&nbsp;Ring 4"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRings40" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRings41" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRings42" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRings43" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRings44" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRings45" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRings46" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRings47" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRings48" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRIng1" runat="server" Text="&nbsp;Ring 1<br />"></asp:Literal>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing10" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td rowspan="4">
                            <asp:Literal ID="lblLooseSluggishStickingBlackRunning" runat="server" Text="&nbsp;Loose<br />
                            &nbsp;Sluggish<br />
                            &nbsp;Sticking<br />
                            &nbsp;Black running<br />
                            &nbsp;Surface, overall<br />
                            &nbsp;Black running<br />
                            &nbsp;Surface, partly"></asp:Literal>
                        </td>
                        <td rowspan="4">
                            <asp:Literal ID="lblSymbolLSLST" runat="server" Text="&nbsp;L<br />
                            &nbsp;SL<br />
                            &nbsp;ST<br />
                            <br />
                            &nbsp;B<br />
                            <br />
                            &nbsp;(B)"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing11" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing12" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing13" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing14" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing15" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing16" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing17" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing18" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRingTwo2" runat="server" Text="&nbsp;Ring 2<br />"></asp:Literal>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing20" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing21" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing22" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing23" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing24" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing25" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing26" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing27" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing28" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRIngTwo3" runat="server" Text="&nbsp;Ring 3<br />"></asp:Literal>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing30" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing31" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing32" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing33" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing34" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing35" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing36" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing37" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing38" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblringfour" runat="server" Text="&nbsp;Ring 4"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing40" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing41" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing42" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing43" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing44" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing45" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing46" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing47" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txttRing48" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lbltopLandtwo" runat="server" Text="&nbsp;Topland"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTopland0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td rowspan="11">
                            <asp:Literal ID="lblCleanSmoothScratchesMicroSeizures" runat="server" Text="&nbsp;Clean,<br />
                            <br />
                            &nbsp;Smooth Scratches<br />
                            <br />
                            &nbsp;(Vertical)<br />
                            <br />
                            &nbsp;Micro-seizures<br />
                            <br />
                            &nbsp;(Local)<br />
                            <br />
                            &nbsp;Micro-seizures<br />
                            <br />
                            &nbsp;(All over)<br />
                            <br />
                            &nbsp;Micro-seizures<br />
                            &nbsp;(Still Active)<br />
                            &nbsp;Old MZ<br />
                            &nbsp;Wear Rings at top of liner<br />
                            &nbsp;Wear ridges near Scav. Port<br />
                            <br />
                            &nbsp;Corrosion<br />
                            <br />
                            <br />"></asp:Literal>
                        </td>
                        <td rowspan="11">
                            <asp:Literal ID="lblsymbolCSMZMZMAZ" runat="server" Text="&nbsp;C<br />
                            <br />
                            <br />
                            &nbsp;S<br />
                            <br />
                            <br />
                            &nbsp;mz<br />
                            <br />
                            <br />
                            &nbsp;MZ<br />
                            <br />
                            <br />
                            &nbsp;MAZ<br />
                            <br />
                            &nbsp;OZ<br />
                            <br />
                            &nbsp;WR 1<br />
                            <br />
                            <br />
                            &nbsp;WR 2<br />
                            <br />
                            <br />
                            <br />
                            &nbsp;CO"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTopland1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTopland2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTopland3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTopland4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTopland5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTopland6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTopland7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTopland8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRingLands" runat="server" Text="&nbsp;Ringlands"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRinglands0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRinglands1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRinglands2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRinglands3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRinglands4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRinglands5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRinglands6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRinglands7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRinglands8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblRingOne" runat="server" Text="&nbsp;Ring 1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing10" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing11" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing12" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing13" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing14" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing15" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing16" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing17" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing18" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblRingTwo" runat="server" Text="&nbsp;Ring 2"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing20" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing21" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing22" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing23" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing24" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing25" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing26" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing27" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing28" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRingthree3" runat="server" Text="&nbsp;Ring 3"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing30" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing31" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing32" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing33" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing34" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing35" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing36" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing37" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing38" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblRingtwo4" runat="server" Text="&nbsp;Ring 4"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing40" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing41" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing42" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing43" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing44" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing45" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing46" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing47" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlRing48" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPistonSkirttwo" runat="server" Text="&nbsp;Piston Skirt"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpSkirt0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpSkirt1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpSkirt2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpSkirt3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpSkirt4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpSkirt5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpSkirt6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpSkirt7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpSkirt8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPistonRodtwo" runat="server" Text="&nbsp;Piston Rod"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRod0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRod1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRod2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRod3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRod4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRod5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRod6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRod7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRod8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblCylLinder" runat="server" Text="&nbsp; Cyl. Liner"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLiner0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLiner1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLiner2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLiner3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLiner4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLiner5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLiner6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLiner7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLiner8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblcylLinderBelowRings" runat="server" Text="&nbsp;Cyl. Liner below
                            <br />
                            &nbsp;Rings"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinerBelow0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinerBelow1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinerBelow2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinerBelow3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinerBelow4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinerBelow5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinerBelow6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinerBelow7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinerBelow8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCylinderNearTop" runat="server" Text="&nbsp;Cyl. Liner<br />
                            &nbsp;near top<br />
                            <br />"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinernear0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinernear1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinernear2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinernear3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinernear4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinernear5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinernear6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinernear7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcLinernear8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRingfourone" runat="server" Text="&nbsp;Ring 1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td rowspan="7">
                           <asp:Literal ID="lblOptimalTooMuchOilSlightlyDry" runat="server" Text="&nbsp;Optimal<br />
                            <br />
                            &nbsp;Too much oil<br />
                            <br />
                            &nbsp;Slightly dry<br />
                            <br />
                            &nbsp;Very dry<br />
                            <br />
                            &nbsp;Clover – leaf wear<br />
                            <br />
                            <br />"></asp:Literal>
                        </td>
                        <td rowspan="7">
                            <asp:Literal ID="lblSymbolODDOCL" runat="server" Text="&nbsp;OP<br />
                            <br />
                            &nbsp;O<br />
                            <br />
                            &nbsp;D<br />
                            <br />
                            &nbsp;DO<br />
                            <br />
                            &nbsp;CL<br />
                            <br />
                            <br />"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblRingfour2" runat="server" Text="&nbsp;Ring 2"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing20" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing21" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing22" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing23" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing24" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing25" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing26" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing27" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing28" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRingfour3" runat="server" Text="&nbsp;Ring 3"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRinyg30" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRying30" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing31" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing32" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing33" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing34" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing36" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing37" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing38" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblringfour4" runat="server" Text="&nbsp;Ring 4"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing40" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRig41" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing42" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing43" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing44" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing45" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing46" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing47" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRing48" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblPistonSkirt" runat="server" Text="&nbsp;Piston Skirt"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSkirt0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSkirt1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSkirt2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSkirt3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSkirt4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSkirt5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSkirt6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSkirt7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSkirt8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPistonRod" runat="server" Text="&nbsp;Piston Rod"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRod0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRod1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRod2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRod3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRod4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRod5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRod6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRod7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRRod8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCylinderLinear" runat="server" Text="&nbsp;Cylinder Liner"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRLiner0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRLiner1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRLiner2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRLiner3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRLiner4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRLiner5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRLiner6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRLiner7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRLiner8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lbLScavBox" runat="server" Text="&nbsp;Scav. Box"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaBox0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLittleSludge" runat="server" Text="&nbsp;Little sludge"></asp:Literal>
                        </td>
                        <td>
                           <asp:Literal ID="lblLS" runat="Server" Text="&nbsp;LS"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaBox1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaBox2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaBox3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaBox4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaBox5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaBox6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaBox7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaBox8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblScavReceiver" runat="server" Text="&nbsp;Scav. Receiver"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaReceiver0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMuchSludge" runat="server" Text="&nbsp;Much sludge"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblMS" runat="server" Text="&nbsp;MS"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaReceiver1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaReceiver2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaReceiver3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaReceiver4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaReceiver5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaReceiver6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaReceiver7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtScaReceiver8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFlagsandNonReturnValves" runat="server" Text="&nbsp;Flaps and non-<br />
                            &nbsp;return valves"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlaps0" runat="server" Width="100px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMovable" runat="server" Text="&nbsp;Movable"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblM" runat="server" Text="&nbsp;M"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlaps1" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlaps2" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlaps3" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlaps4" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlaps5" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlaps6" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlaps7" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlaps8" runat="server" Width="70px" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
