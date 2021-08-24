
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationComparePrint.aspx.cs" Inherits="Purchase_PurchaseQuotationComparePrint" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation Compare</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script language="javascript" type="text/javascript">
        function cmdPrint_Click()
        {
            //document.getElementById('Title1_lblTitle').style.display = "none";
            document.getElementById('cmdPrint').style.display = "none";
            window.print();
        }
    </script>
    <style type="text/css">
        .Sftblclass{
            border-collapse: collapse;
        }
        .Sftblclass tr td
        {
            border: 1px solid black;
        }

        .table {
                border: 1px solid black;
                font-family: Tahoma, Arial, Helvetica, Verdana, sans-serif;
		        font-size: 11px;
            }

            .table td,th {
                border-collapse: collapse;
            }
        .textbox{
            margin: 0;
            background: 0;
            border: 0;
            border-bottom: 1px solid black;
            outline: 0;
            
        }
    </style>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmQuotationCompare" runat="server" autocomplete="off">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />    
    <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
               <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server"/>
                <table width="100%">
                    <tr>
                        <td style="font-size:medium">
                           <telerik:RadLabel RenderMode="Lightweight" ID="lblEXECUTIVESHIPMANAGEMENT" runat="server" Text="EXECUTIVE SHIP MANAGEMENT"></telerik:RadLabel>
                        </td>
                        <td align="right" style="font-size:small">
                           <telerik:RadLabel RenderMode="Lightweight" ID="lblTA012" runat="server" Text="TA 012"></telerik:RadLabel>
                                                       
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right" style="font-size:small">
                           <telerik:RadLabel RenderMode="Lightweight" ID="lblPage1of1" runat="server" Text="Page 1 of 1"></telerik:RadLabel>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right" style="font-size:small">
                           <telerik:RadLabel RenderMode="Lightweight" ID="lbl1210Rev1" runat="server" Text="(08/17 Rev 3)"></telerik:RadLabel>
                            
                        </td>
                    </tr>
                </table>
                <table class="table" width="100%">
                    <tr>
                        <td align="center" colspan="2" >
                           <h2><b><telerik:RadLabel RenderMode="Lightweight" ID="lblQuotationCompare" runat="server" Text="Quotation Compare"></telerik:RadLabel>
                            </b></h2>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="3px">
                                <tr>    
                                    <td style="width:20%; font-size:medium" align="left" >
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblContentVESSEL" runat="server" Text="VESSEL:-"></telerik:RadLabel>
                                    </td>
                                    <td align="left" style="width:30%; font-size:medium">
                                        <asp:TextBox ID="lblVessel" runat="server" CssClass="textbox" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="width:20%; font-size:medium" align="left">
                                        <asp:Literal ID="lblContentDATE" runat="server" Text="DATE:-"></asp:Literal>
                                        
                                    </td>
                                    <td align="left" style="width:30%; font-size:medium">
                                        <asp:TextBox ID="lblDate" runat="server" CssClass="textbox" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%; font-size:medium" align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblREFNO" runat="server" Text="REF NO:-"></telerik:RadLabel>
                                        
                                    </td>
                                    <td align="left" style="width:30%; font-size:medium">
                                        <asp:TextBox ID="txtRefNo" runat="server" CssClass="textbox"></asp:TextBox>
                                    </td>
                                    <td style="width:20%; font-size:medium" align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblContentPONO" runat="server" Text="P.O. NO:-"></telerik:RadLabel>
                                        
                                    </td>
                                    <td align="left" style="width:30%; font-size:medium">
                                        <asp:TextBox ID="lblPoNo" runat="server" CssClass="textbox" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%; font-size:medium" align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDESCRIPTIONCaption" runat="server" Text="DESCRIPTION:- "></telerik:RadLabel>
                                       
                                    </td>
                                    <td colspan="3" align="left" style="width:30%; font-size:medium">
                                        <asp:TextBox ID="lblDescription" runat="server" CssClass="textbox" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1" style="font-size:medium">
                                <tr>
                                    <td align="left" style="width:30%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblSUPPLIERS" runat="server" Text="SUPPLIERS"></telerik:RadLabel>
                                        
                                    </td>
                                    <td align="left" style="width:20%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblSupplier1" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left" style="width:20%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblSupplier2" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left" style="width:20%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblSupplier3" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                                        
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPort1" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPort2" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPort3" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalinlocalcurrency" runat="server" Text="Total in local currency"></telerik:RadLabel>
                                        
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTotal1" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTotal2" runat="server"></telerik:RadLabel>                                    
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTotal3" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblExchangeRate" runat="server" Text="Exchange Rate"></telerik:RadLabel>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lbl1" runat="server" Text="(1$ =       )"></telerik:RadLabel>
                                        <br />
                                        
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblExchangeRate1" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblExchangeRate2" runat="server"></telerik:RadLabel>                                    
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblExchangeRate3" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalinUSD" runat="server" Text="Total in USD"></telerik:RadLabel>
                                        <b></b>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblUsdTotal1" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblUsdTotal2" runat="server"></telerik:RadLabel>                                    
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblUsdTotal3" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryTime" runat="server" Text="Delivery Time"></telerik:RadLabel>
                                        
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryTime1" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryTime2" runat="server"></telerik:RadLabel>                                    
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryTime3" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <%--Discount--%>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDiscount" runat="server" Text="Discount"></telerik:RadLabel>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDiscount1" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDiscount2" runat="server"></telerik:RadLabel>                                    
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDiscount3" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblNetPriceinUSD" runat="server" Text="Net Price in USD"></telerik:RadLabel>
                                        
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalPrice1" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalPrice2" runat="server"></telerik:RadLabel>                                    
                                    </td>
                                    <td align="left">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalPrice3" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                 
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                     <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1" style="font-size:medium">
                                <tr>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblSpares" runat="server" Text="Spares"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblStores" runat="server" Text="Stores"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblRepairs" runat="server" Text="Repairs & Maintenance"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblLubes" runat="server" Text="Lubes"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblNBSpares" runat="server" Text="Non budgeted spares"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblNBStores" runat="server" Text="Non budgeted stores"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblNBRepairs" runat="server" Text="Non budgeted repairs"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDryDock" runat="server" Text="Dry Dock"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="font-size:x-small;font-weight:bold">
                                (Tick the appropriate box)
                            </td>
                        </tr>
                    </table> 
                    <table style="width:100%;" >
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                     <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:TextBox ID="txtTecSup" runat="server" Text="" CssClass="textbox" ReadOnly="true" Width="80%" ></asp:TextBox>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtFleetMan" runat="server" Text="" CssClass="textbox" ReadOnly="true" Width="70%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-size:small; font-style:italic" valign="top">
                            Technical Superintendent
                            
                        </td>
                        <td align="center" style="font-size:small;">
                            <u>Manager - Fleet,<font style="font-style:italic">HSEQA, Vetting & Operations,
                                <br />
                                Assistant General Manager, Crew</font></u>
                                <br />
                                (for expenditure US$5,000-US$10,000)               
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                     <tr>
                        <td align="center" colspan="2">
                            <asp:TextBox ID="txtAsstTechDire" runat="server" Text="" CssClass="textbox" ReadOnly="true" Width="70%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="font-size:small">
                            <font style="font-style:italic"><u>Assistant Technical Director</u> / <u>General Manager,</u></font> HR & Crew <font style="font-style:italic">/ <u>General Manager HSEQA</u></font>
                                <br />(For expenditure above US$ 10,000 up to US$ 20,000)
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                     <tr>
                        <td align="center" colspan="2">
                            <asp:TextBox ID="txtMD" runat="server" Text="" CssClass="textbox" ReadOnly="true" Width="70%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="font-size:small">
                            <font style="font-style:italic"><u>Managing Director</u> / <u>Assistant Managing Director</u></font>
                                <br />(For expenditure exceeding US$ 20,000)
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" class="Sftblclass">
                                <tr>
                                    <td colspan="2" style="font-size: medium">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblRemark" runat="server" Text="Remarks:-"></telerik:RadLabel>
                                        <b></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="font-size: medium;width:30%;">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarksName" runat="server"></telerik:RadLabel>
                                    </td>
                                    <td align="left" style="font-size: medium;width:70%">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarks" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="font-size: medium">
                                         <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarksName2" runat="server" Text="&nbsp&nbsp"></telerik:RadLabel>
                                    </td>
                                    <td align="left" style="font-size: medium">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarks2" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="font-size: medium">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarksName3" runat="server" Text="&nbsp&nbsp"></telerik:RadLabel>
                                    </td>
                                    <td align="left" style="font-size: medium">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarks3" runat="server"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                </table>    
    </div>
    </form>
</body>
</html>
