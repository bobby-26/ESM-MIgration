<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormAuxiliaryEngineConnectingRodInspection.aspx.cs"
    Inherits="AuxiliaryEngineConnectingRodInspection" %>

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
    <form id="frmAuxiliaryEngineDecarbonization" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Auxiliary Engine Decarbonization" ShowMenu="false">
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
                            <asp:Literal ID="lblE7" runat="server" Text="E 7 (A)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                          <%--  <b><asp:Literal ID="lblSingapore" runat="server" Text="SINGAPORE"></asp:Literal></b>--%>
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl0714Rev1" runat="server" Text="(07/14 Rev1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h3>
                                <asp:Literal ID="lblAEConnectiongRODInspectionReport" runat="server" Text="A/E CONNECTING ROD INSPECTION REPORT<"></asp:Literal>br />
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
                           <asp:Literal ID="lblDate" runat="server" Text=" Date :"></asp:Literal>
                        </td>
                        <td>
                           <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="7" align="center">
                                        <asp:Literal ID="lblConnectingRODInspection" runat="server" Text="CONNECTING ROD INSPECTION"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:literal ID="lblChiefEngineer" runat="server" Text="Chief Engineer :"></asp:literal>
                                        <asp:TextBox ID="txtChiefEngineer" runat="server" CssClass="input" Width="50%"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:literal ID="lblSecondEngineer" runat="server" Text="Second Engineer :"></asp:literal>
                                        <asp:TextBox ID="txtSecondEngineer" runat="server" CssClass="input" Width="50%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:literal ID="lblAuxiliaryEngineNo" runat="server" Text="AUXILIARY ENGINE NO."></asp:literal>
                                        <asp:TextBox ID="txtAuxEngine" runat="server" CssClass="input" Width="60%"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:Literal ID="lblEngineType" runat="server" Text="ENGINE TYPE:"></asp:Literal>
                                        <asp:TextBox ID="txtEngineTye" runat="server" CssClass="input" Width="60%"></asp:TextBox>
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
                                    <td colspan="2">
                                        <asp:Literal ID="lblCylNo" runat="server" Text="Cyl No."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblCyno1" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblCyno2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblCylNo3" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblCylNo4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td align="center">
                                       <asp:Literal ID="lblCylNo5" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblCylNo6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblCylNo7" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblCylNo8" runat="server" Text="8"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblCylNo9" runat="server" Text="9"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblConnRodIdentNo" runat="server" Text="Conn. Rod Ident. No."></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox8" runat="server" CssClass="input"></asp:TextBox>
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
                                    <td>
                                        <asp:TextBox ID="TextBox13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblSerialNo" runat="server" Text="Serial No."></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox16" runat="server" CssClass="input"></asp:TextBox>
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
                                    <td>
                                        <asp:TextBox ID="TextBox21" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox22" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox23" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblRunningHoursSinceLast" runat="server" Text="Running Hours since last
                                        <br />
                                        renewal/recondition."></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox24" runat="server" CssClass="input"></asp:TextBox>
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
                                    <td>
                                        <asp:TextBox ID="TextBox29" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox30" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox31" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox32" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="5">
                                        <asp:Image ID="img1" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization.PNG" />
                                    </td>
                                    <td>
                                       <asp:Literal ID="lblA" runat="server" Text="A"></asp:Literal>
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
                                    <td>
                                        <asp:TextBox ID="TextBox41" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblB" runat="server" Text="B"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox42" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox43" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox44" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox45" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox46" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox47" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox48" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox49" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox50" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblC" runat="server" Text="C"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox51" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox52" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox53" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox54" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox55" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox56" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox57" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox58" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox59" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblD" runat="server" Text="D"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox60" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox61" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox62" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox63" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox64" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox65" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox66" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox67" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox68" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblE" runat="server" Text="E"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox69" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox70" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox71" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox72" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox73" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox74" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox75" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox76" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox77" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Literal ID="lblMaxOvality" runat="server" Text="Max. ovality"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox78" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox79" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox80" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox81" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox82" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox83" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox84" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox85" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox86" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblConnRodRenewed" runat="server" Text="Conn. Rod Renewed"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblYesNo1" runat="server" Text="&nbsp;&nbsp; YES / NO"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblYesNo2" runat="server" Text="&nbsp;&nbsp; YES / NO"></asp:Literal>
                                    </td>
                                    <td>
                                       
                                       <asp:literal ID="lblYesNo3" runat="server" text=" &nbsp;&nbsp; YES / NO"></asp:literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblYesno4" runat="server" Text="&nbsp;&nbsp; YES / NO"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblYesNo5" runat="server" Text="&nbsp;&nbsp; YES / NO"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblYenNo6" runat="server" Text="&nbsp;&nbsp;&nbsp; YES / NO"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblYesNo7" runat="server" Text="&nbsp;&nbsp; YES / NO"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblYesNo8" runat="server" Text="&nbsp;&nbsp; YES / NO"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblYesNo9" runat="server" Text="&nbsp;&nbsp; YES / NO"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Image ID="img2" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization3.PNG" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization1.PNG" />
                                        <br />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization2.PNG" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization1.PNG" />
                                        <br />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization2.PNG" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image5" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization1.PNG" />
                                        <br />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image6" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization2.PNG" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image7" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization1.PNG" />
                                        <br />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image8" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization2.PNG" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image9" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization1.PNG" />
                                        <br />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image10" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization2.PNG" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image11" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization1.PNG" />
                                        <br />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image12" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization2.PNG" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image13" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization1.PNG" />
                                        <br />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image14" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization2.PNG" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image15" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization1.PNG" />
                                        <br />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image16" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization2.PNG" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image17" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization1.PNG" />
                                        <br />
                                        <br />
                                        <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Image ID="Image18" runat="server" ImageUrl="~/StandardForm/Images/AuxEngineDecarbonization2.PNG" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblRemarks" runat="server" Text="REMARKS"></asp:Literal>
                        </td>
                        <td colspan="7">
                            <asp:Literal ID="lblremarksPositionforaboveMarking" runat="server" Text="POSITION FOR ABOVE MARKING, IF ANY, TO BE INDICATED ON THE CONCERNED UNIT CONROD
                            SERRATION."></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="7">
                            <asp:Literal ID="lblForMaxAllowanceOvalitySeeMeakersInstruction" runat="server" Text="FOR MAX. ALLOWANCE OVALITY, SEE MAKERS INSTRUCTION"></asp:Literal>
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
