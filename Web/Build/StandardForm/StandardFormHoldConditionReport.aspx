<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormHoldConditionReport.aspx.cs"
    Inherits="StandardFormHoldConditionReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                ShowMenu="false">
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
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT "></asp:Literal></b>
                        </td>
                        <td align="left">
                          
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPB12" runat="server" Text="PB12"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of2" runat="server" Text="Page 1 of 2"></asp:Literal>
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
                                <asp:Literal ID="lblHoldConditionReport" runat="server" Text="HOLD CONDITION REPORT"></asp:Literal><br />
                            </h2>
                            <asp:Literal ID="lblToBeSenttotheOfficeinHardCopywithMonthEndMail" runat="server" Text="(To be sent to the office in Hard Copy with month-end mail as and when holds are inspected)<br />HATCH COVER"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <b><asp:Literal ID="lblAllHoldstobeInspectedAtIntervalsNotExceeding12Months" runat="server" Text="- All holds to be inspected at intervals not exceeding 12 months"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselHoldNo" runat="server" Text="Vessel:<br /><br />Hold No :<br />"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="input"></asp:TextBox><br />
                            <br />
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="input"></asp:TextBox><br />
                        </td>
                        <td colspan="3" rowspan="2">
                            <table style="width: 99%">
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Literal ID="lblP" runat="server" Text="P"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LIteral ID="lblA" runat="server" Text="A"></asp:LIteral>
                                    </td>
                                    <td colspan="2">
                                        <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtA" runat="server" CssClass="input" TextMode="MultiLine" Height="54px"
                                                        Width="100px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtB" runat="server" CssClass="input" TextMode="MultiLine" Height="54px"
                                                        Width="100px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtC" runat="server" CssClass="input" TextMode="MultiLine" Height="54px"
                                                        Width="100px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtD" runat="server" CssClass="input" TextMode="MultiLine" Height="54px"
                                                        Width="100px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblF" runat="server" Text="F"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:LIteral ID="lblS" runat="server" Text="S"></asp:LIteral>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:literal ID="lblExaminedOn" runat="server" Text="EXAMINED ON:"></asp:literal>
                        </td>
                        <td>
                           <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="0px" cellspacing="0px">
                    <tr>
                        <td colspan="4" rowspan="4">
                            &nbsp;
                        </td>
                        <td style="border: 1px solid #000000;">
                            <br />
                            &nbsp;<asp:TextBox ID="txtP1" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;
                        </td>
                        <td rowspan="4" colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: none solid solid solid; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtp2" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: none solid solid solid; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtP3" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: none solid none solid; border-width: 1px; border-color: #000000"
                            class="style1">
                            <br />
                            &nbsp;<asp:TextBox ID="txtP4" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="border: 1px solid #000000;">
                            <br />
                            &nbsp;<asp:TextBox ID="txtA1" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="border-style: solid solid solid none; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtA2" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="border-style: solid solid solid none; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtA3" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="border-style: solid solid solid none; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtA4" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="border-style: solid solid solid none; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtA5" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="border-style: solid solid solid none; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtA6" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="border-style: solid solid solid none; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtA7" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="border-style: solid none solid none; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtA8" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="border: 1px solid #000000;">
                            <br />
                            &nbsp;<asp:TextBox ID="txtA9" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" rowspan="4">
                            &nbsp;
                        </td>
                        <td style="border-style: none solid solid solid; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtS1" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;
                        </td>
                        <td rowspan="4" colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: none solid solid solid; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtS2" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: none solid solid solid; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtS3" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: none solid solid solid; border-width: 1px; border-color: #000000">
                            <br />
                            &nbsp;<asp:TextBox ID="txtS4" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="54px" Width="100px"></asp:TextBox><br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Literal ID="lblRemarks" runat="server" Text="REMARKS: (Insert particulars of damage/corrosion found, remarks as to condition of paintwork on<br />plating, stiffeners, pipes and ladders, and any other points noted during examination)."></asp:Literal>                    </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Literal ID="lblAPhotoGraphs" runat="server" Text="A.) Photographs:"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Literal ID="lblAreAttachedInASeparateSheet" runat="server" Text="- Are attached in a separate sheet"></asp:Literal>
                        </td>
                         <td colspan="2">
                          
                           <asp:RadioButtonList ID="rbtnphotoattached" runat="server" RepeatDirection="Horizontal">
                           <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                           <asp:ListItem Text="No" Value="1"></asp:ListItem>
                           </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Literal ID="lblNoofPhotoGraphsAttachedAlongWithisReport" runat="server" Text="- Number of photographs attached along with this report :"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtNosPhos" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Literal ID="lblBIndicateonSketchDamagedArea" runat="server" Text="B.) Indicate on sketch damaged area and coating integrity by percentage with reference to Tank<br />coating breakdown extent diagram"></asp:Literal>  </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <asp:TextBox ID="txtCoating" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="108px" Width="952px"></asp:TextBox>
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                        <td>
                            <asp:literal ID="lblExaminedBy" runat="server" Text="Examined by:"></asp:literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExamiedBy" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank:"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRank" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9" align="right">
                            <asp:Literal ID="lblMaster" runat="server" Text="Master"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
