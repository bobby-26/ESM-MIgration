<%@ Page Language="C#" AutoEventWireup="true" Inherits="StandardFormMeggerTestReport"
    CodeFile="StandardFormMeggerTestReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E9 Megger Test Report</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmAuxEngine" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="MEGGER TEST REPORT" ShowMenu="false">
            </eluc:Title>
               <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>        
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>           

                <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                <table width="95%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="3">
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT PTE LTD"></asp:Literal></b>
                        </td>
                        <td align="center">
                          
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblE9" runat="server" Text="E9"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                           <%-- <b><asp:Literal ID="lblSingapore" runat="server" Text="SINGAPORE"></asp:Literal></b>--%>
                        </td>
                        <td align="center">
                          
                        </td>
                        <td align="right">
                           <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="5">
                            <asp:Literal ID="lbl303Rev0" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="5">
                            <h3>
                                <asp:Literal ID="lblMeggerTestReport" runat="server" Text="MEGGER TEST REPORT"></asp:Literal></h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselName" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2" align="right">
                            <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td align="center">
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                </table>
                <br />
                <table width="95%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                        <td width="35%" align="center">
                           <asp:Literal ID="lblUnitMeasured" runat="server" Text="Unit Measured"></asp:Literal>
                        </td>
                        <td align="center" width="20%">
                            <asp:Literal ID="lblLastOverHauled" runat="server" Text="Last Overhauled"></asp:Literal>
                        </td>
                        <td align="center" width="30%">
                            <asp:Literal ID="lblReadingTakenofBetween" runat="server" Text="Reading Taken of / Between"></asp:Literal>
                        </td>
                        <td align="center" width="15%">
                            <asp:Literal ID="lblMegOhms" runat="server" Text="Meg. Ohms"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUnitMeasured1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastOverhaull" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReading1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMegOhms1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUnitMeasured2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastOverhaul2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReading2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMegOhms2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="input"></asp:TextBox>
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
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server" CssClass="input"></asp:TextBox>
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
                    </tr>
                    <tr>
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
                        <td>
                            <asp:TextBox ID="TextBox41" runat="server" CssClass="input"></asp:TextBox>
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
                    </tr>
                    <tr>
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
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TextBox49" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox50" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox51" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox52" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
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
                        <tr>
                            <td>
                                <asp:TextBox ID="TextBox57" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox58" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox59" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox60" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <tr>
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
                                <tr>
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
                                    <tr>
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
                                    </tr>
                                    <tr>
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
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="TextBox77" runat="server" CssClass="input"></asp:TextBox>
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
                                    </tr>
                                    <tr>
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
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="TextBox85" runat="server" CssClass="input"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox86" runat="server" CssClass="input"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox87" runat="server" CssClass="input"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox88" runat="server" CssClass="input"></asp:TextBox>
                                        </td>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox89" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox90" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox91" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox92" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox93" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox94" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox95" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox96" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox97" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox98" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox99" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox100" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox101" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox102" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox103" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox104" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox105" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox106" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox107" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox108" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox109" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox110" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox111" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox112" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox113" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox114" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox115" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox116" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="TextBox117" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox118" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox119" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox120" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </tr>
                                    </tr>
                                </tr>
                            </tr>
                        </tr>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <table width="95%">
                    <tr>
                        <td align="center" width="35%">
                            <asp:TextBox ID="txtElectrician" runat="server" CssClass="gridinput"></asp:TextBox><br />
                            <asp:Literal ID="lblElectrician" runat="server" Text="Electrician"></asp:Literal>
                        </td>
                        <td width="30%">
                            &nbsp;
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtChiefEngineer" runat="server" CssClass="gridinput"></asp:TextBox><br />
                            <asp:Literal ID="lblChiefENgineer" runat="server" Text="Chief Engineer"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
