<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormPistonRingGap.aspx.cs"
    Inherits="StandardFormPistonRingGap" %>

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
                           <asp:Literal ID="lblE24" runat="server" Text="E24"></asp:Literal>
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
                            <asp:Literal ID="lbl0714Rev1" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <asp:Literal ID="lblMainEnginePistonRingGapMeasurement" runat="server" Text="<b>MAIN ENGINE PISTON RING GAP MEASUREMENT</b><br />
                           
                            <br />"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td>
                           <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                        <td>
                           <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPort" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblChiefEngineer" runat="server" Text="Chief Engineer"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtchiefEngineer1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="95%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                        <td colspan="4">
                            <br />
                            <asp:Literal ID="lblTotalBunningHours" runat="server" Text="&nbsp; TOTAL RUNNING HOURS:"></asp:Literal>
                            <eluc:Number ID="txtTotalRunningHours" runat="server" CssClass="input" />
                            <asp:Literal ID="lblHours" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Hours:"></asp:Literal>
                            <eluc:Number ID="txtHours" runat="server" CssClass="input" />
                            <asp:Literal ID="lblMinutes" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Minutes:"></asp:Literal>
                            <eluc:Number ID="txtMinutes" runat="server" CssClass="input" />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCylinderNo" runat="server" Text="&nbsp; Cylinder No.<br />
                            &nbsp; Ring No."></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblRingGapImprint" runat="server" Text="&nbsp;Piston Ring Gap Imprint"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:Literal ID="lblRingGapandRunningHoursOfTheRing" runat="server" Text="&nbsp;Ring Gap and running hours of the Ring"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%">
                            <eluc:Number ID="txtCylinderRingNo1" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtRingGapImprint1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRingGapRunningHours1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <eluc:Number ID="txtCylinderRingNo2" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtRingGapImprint2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRingGapRunningHours2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <eluc:Number ID="txtCylinderRingNo3" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtRingGapImprint3" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRingGapRunningHours3" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <eluc:Number ID="txtCylinderRingNo4" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtRingGapImprint4" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRingGapRunningHours4" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <eluc:Number ID="txtCylinderRingNo5" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtRingGapImprint5" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRingGapRunningHours5" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <eluc:Number ID="txtCylinderRingNo6" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtRingGapImprint6" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRingGapRunningHours6" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Literal ID="lblRemarks" runat="server" Text="&nbsp;Remarks: Record made for only top most accessible Rings of individual units."></asp:Literal>
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table width="95%">
                    <tr>
                        <td width="70%">
                            &nbsp;
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtChiefEngineer2" runat="server" CssClass="input"></asp:TextBox><br />
                            <asp:Literal ID="lblChiefEngineer2" runat="server" Text="Chief Engineer"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
