<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormShaftEarthLog.aspx.cs"
    Inherits="StandardFormShaftEarthLog" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E16 Shaft Earth Log </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmShaftEarthLog" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Shaft Earth Log" ShowMenu="false"></eluc:Title>
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
                            <asp:Literal ID="lblE16" runat="server" Text="E16"></asp:Literal>
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
                            <asp:Literal ID="lbl604REV1" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h2>
                               <asp:Literal ID="lblShaftEarthLog" runat="server" Text="SHAFT EARTH LOG<br />"></asp:Literal>
                            </h2>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblVesselName" runat="server" Text="VESSEL NAME : "></asp:Literal></b>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtvesselName" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <b><asp:Literal ID="lblMonth" runat="server" Text="MONTH : "></asp:Literal></b>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="input">
                                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                <asp:ListItem Text="December" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="95%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                        <td align="center">
                           <asp:Literal ID="lblDate" runat="server" Text="DATE"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblATSEAPORT" runat="server" Text="AT SEA/PORT"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblRPM" runat="Server" Text="RPM"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblmv" runat="Server" Text="mV"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblAMP" runat="server" Text="AMP"></asp:Literal>
                        </td>
                        <td colspan="3" align="center">
                            <asp:Literal ID="lblRemarks" runat="server" Text="REMARKS"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate1" runat="server" Text="1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort1" runat="server" CssClass="input_mandatory" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm1" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV1" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp1" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark1" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate2" runat="server" Text="2"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort2" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm2" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV2" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp2" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark2" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate3" runat="server" Text="3"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort3" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm3" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV3" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp3" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark3" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate4" runat="server" Text="4"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort4" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm4" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV4" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp4" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark4" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate5" runat="server" Text="5"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort5" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm5" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV5" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp5" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark5" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate6" runat="server" Text="6"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort6" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm6" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV6" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp6" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark6" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate7" runat="server" Text="7"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort7" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm7" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV7" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp7" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark7" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate8" runat="server" Text="8"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort8" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm8" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV8" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp8" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark8" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate9" runat="server" Text="9"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort9" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm9" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV9" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp9" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark9" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate10" runat="server" Text="10"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort10" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm10" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV10" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp10" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark10" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate11" runat="server" Text="11"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort11" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm11" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV11" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp11" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark11" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate12" runat="server" Text="12"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort12" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm12" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV12" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp12" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark12" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate13" runat="server" Text="13"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort13" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm13" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV13" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp13" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark13" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate14" runat="server" Text="14"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort14" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm14" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV14" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp14" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark14" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate15" runat="server" Text="15"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort15" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm15" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV15" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp15" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark15" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate16" runat="server" Text="16"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort16" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm16" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV16" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp16" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark16" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate17" runat="server" Text="17"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort17" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm17" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV17" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp17" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark17" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate18" runat="server" Text="18"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort18" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm18" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV18" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp18" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark18" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate19" runat="server" Text="19"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort19" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm19" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV19" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp19" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark19" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate20" runat="server" Text="20"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort20" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm20" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV20" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp20" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark20" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate21" runat="server" Text="21"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort21" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm21" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV21" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp21" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark21" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate22" runat="server" Text="22"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort22" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm22" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV22" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp22" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark22" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate23" runat="server" Text="23"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort23" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm23" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV23" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp23" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark23" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate24" runat="server" Text="24"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort24" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm24" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV24" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp24" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark24" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate25" runat="server" Text="25"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort25" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm25" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV25" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp25" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark25" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate26" runat="server" Text="26"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort26" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm26" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV26" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp26" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark26" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate27" runat="server" Text="27"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort27" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm27" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV27" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp27" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark27" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate28" runat="server" Text="28"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort28" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm28" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV28" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp28" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark28" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate29" runat="server" Text="29"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort29" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm29" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV29" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp29" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark29" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate30" runat="server" Text="30"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort30" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm30" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV30" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp30" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark30" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate31" runat="server" Text="31"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeaPort31" runat="server" CssClass="input" Width="350px" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtRpm31" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtmV31" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmp31" runat="server" MaxLength="15" Width="80px" IsInteger="true"
                                CssClass="input" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark31" runat="server" CssClass="input" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" class="dummy">
                            <br />
                            <br />
                            <asp:TextBox ID="txtElectricalofficer" runat="server" CssClass="input" Width="350px"></asp:TextBox><br />
                            <asp:Literal ID="lblelectricalOfficer" runat="server" Text="ELECTRICAL OFFICER"></asp:Literal>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td colspan="3" align="center" class="dummy">
                            <br />
                            <br />
                            <asp:TextBox ID="txtchiefengineer" runat="server" CssClass="input" Width="350px"></asp:TextBox><br />
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
