<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddressApprovalForm.aspx.cs" Inherits="Registers_RegistersAddressApprovalForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Approval Form</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
        .Sftblclass tr td
        {
            border: 1px solid black;
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAePerformane" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Approval Form" ShowMenu="false">
            </eluc:Title>
        </div>  
        <%--<div class="navSelectHeader" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuSupplierApproval" runat="server" OnTabStripCommand="MenuSupplierApproval_TabStripCommand">
            </eluc:TabStrip>
        </div>  --%>   
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>            
               <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server"/>
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="lblEXECUTIVESHIPMANAGEMENT" runat="server" Text=""></asp:Literal>
                        </td>
                        <td colspan="2" align="right">
                            <asp:Literal ID="lblTA003A" runat="server" Text="TA-003A"></asp:Literal>                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Literal ID="lbl1210Rev3" runat="server" Text="(12/10 Rev 3)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <h2><b><asp:Literal ID="lblSTORESSPARESSUPPLIERWORKSHOP" runat="server" Text="STORES/SPARES SUPPLIER/WORKSHOP"></asp:Literal></b></h2>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                           <h2><b><asp:Literal ID="lblINITIALAPPROVALFORM" runat="server" Text="INITIAL APPROVAL FORM"></asp:Literal></b></h2>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <asp:Literal ID="lblDate" runat="server" Text="Date:"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" CssClass="input" />
                        </td> 
                    </tr>
                    <tr>
                        <td style=" width:10%">
                        </td>
                        <td>
                            <asp:Literal ID="lblNameoftheCompany" runat="server" Text="Name of the Company:"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNameoftheCompany" runat="server" CssClass="input" Width="600px"></asp:TextBox>
                        </td>
                        <td style=" width:10%">
                        </td>
                    </tr>
                    <tr>
                        <td style=" width:10%">
                        </td>
                        <td>
                            <asp:Literal ID="lblAddress" runat="server" Text="Address:"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="input" Width="600px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td style=" width:10%"></td>
                    </tr>
                    <tr>
                        <td style=" width:10%"></td>
                        <td>
                            <asp:Literal ID="lblTel" runat="server" Text="Tel:"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="input" Width="600px"></asp:TextBox>
                        </td>
                        <td style=" width:10%"></td>
                    </tr>
                    <tr>
                         <td style=" width:10%"></td>
                        <td>
                            <asp:Literal ID="lblFax" runat="server" Text="Fax:"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" CssClass="input" Width="600px"></asp:TextBox>
                        </td>
                         <td style=" width:10%"></td>
                    </tr>
                    <tr>
                         <td style=" width:10%"></td>
                        <td>
                            <asp:Literal ID="lblEmail" runat="server" Text="Email:"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="input" Width="600px"></asp:TextBox>
                        </td>
                         <td style=" width:10%"></td>
                    </tr>
                    <tr>
                         <td style=" width:10%"></td>
                        <td>
                            <asp:Literal ID="lblPIC" runat="server" Text="PIC:"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPic" runat="server" CssClass="input" Width="600px"></asp:TextBox>
                        </td>
                         <td style=" width:10%"></td>
                    </tr>
                    <tr>
                        <td style=" width:10%"></td>
                        <td colspan="3">
                            <h4><u><asp:Literal ID="lblPleasecompletethebelowQuestionnaire" runat="server" Text="Please complete the below Questionnaire"></asp:Literal></u></h4>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr >
                        <td colspan="4" align="center">
                            <table width="80%" class="Sftblclass" cellpadding="1" cellspacing="1" >
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name:"></asp:Literal>
                                    </td>
                                    <td align="left">
                                        <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="lblProposedBy" runat="server" Text="Proposed By:"></asp:Literal>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtProposedBy" runat="server" CssClass="input" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="lblHSEQuestionnairefilledYN" runat="server" Text="HSE Questionnaire filled Y/N"></asp:Literal> <br />
                                        <asp:Literal ID="lblRemarksDateHSESenttoSupplier" runat="server" Text="(Remarks / Date HSE Sent to Supplier) :"></asp:Literal> 
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtQuestionnaire" runat="server" CssClass="input" TextMode="MultiLine" Height="55px" Width="600px"></asp:TextBox>                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="lblSupplierTypeSparesStoresService" runat="server" Text="Supplier Type (Spares/Stores/Service)"></asp:Literal>
                                        <br />
                                        <asp:Literal ID="lblIfMakerAuthorizedAgent" runat="server" Text="If Maker / Authorized Agent?"></asp:Literal>
                                        <br />
                                        <asp:Literal ID="lblPlsattachsupportingdoc" runat="server" Text="(Pls attach supporting doc) :"></asp:Literal>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtSuplier" runat="server" CssClass="input" TextMode="MultiLine" Height="85px" Width="600px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="lblItemProductType" runat="server" Text="Item/Product Type"></asp:Literal> <br />
                                        <asp:Literal ID="lblandDescription" runat="server" Text="and Description :"></asp:Literal>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtProduct" runat="server" CssClass="input" TextMode="MultiLine" Width="600px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="lblReasonforintroducingthis" runat="server" Text="Reason for introducing this"></asp:Literal> <br /> <asp:Literal ID="lblSupplier" runat="server" Text="Supplier :"></asp:Literal>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtReason" runat="server" CssClass="input" TextMode="MultiLine" Height="40px" Width="600px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="lblOtherAlternativesifany" runat="server" Text="Other Alternatives if any :"></asp:Literal>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtOther" runat="server" CssClass="input" Width="600px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="lblRiskAssociatedifany" runat="server" Text="Risk Associated if any :"></asp:Literal>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtRisk" runat="server" CssClass="input" Width="600px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Literal ID="lblRemarksbySuperintendent" runat="server" Text="Remarks by Superintendent"></asp:Literal> <br /><asp:Literal ID="lblRemarksFleetManager" runat="server" Text="Fleet Manager: /"></asp:Literal>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine" Height="40px" Width="600px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:TextBox ID="TextBox6" runat="server" Visible="false"></asp:TextBox>
                            <br />
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center" colspan="4">
                            <table width="75%">
                                <tr align="center">
                                    <td align="center" style="width:25%">
                                        <br />
                                    </td>
                                    <td align="center" style="width:25%">
                                        <br />
                                    </td>
                                    <td align="center" style="width:25%">
                                         <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center" colspan="4">
                            <table width="75%">
                                <tr>
                                    <td align="center" style="width:25%">
                                        <asp:Literal ID="lblPurchaseExecutive" runat="server" Text="Purchase Executive"></asp:Literal>
                                    </td>
                                    <td align="center" style="width:25%">
                                        <asp:Literal ID="lblPurchaseTechnicalSupdt" runat="server" Text="Purchase/Technical Supdt"></asp:Literal>
                                    </td>
                                    <td align="center" style="width:25%">
                                        <asp:Literal ID="lblFleetManager" runat="server" Text="Fleet Manager"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center" colspan="4">
                            <table width="100%">
                                <tr>
                                    <td align="center" style="width: 66%">
                                        
                                        <br />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>    
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center" colspan="4">
                            <table width="100%">
                                <tr>
                                    <td align="center" style=" width:66%">
                                        <asp:Literal ID="lblDirectorHRCrewDirectorTechnicalDirectorOperations" runat="server" Text="Director, HR & Crew/ Director, Technical/ Director, Operations"></asp:Literal>
                                    </td>
                                    <td align="left" rowspan="4" style="font-size:small">
                                        <asp:CheckBoxList ID="chkApproval" BorderStyle="Solid" BorderColor="Black" BorderWidth="1" runat="server">
                                            <asp:ListItem Value="0">Approved</asp:ListItem>
                                            <asp:ListItem Value="1">Conditionally Approved</asp:ListItem>
                                            <asp:ListItem Value="2">One Time Use</asp:ListItem>                                            
                                            <asp:ListItem Value="3">Not Approved</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" rowspan="4">
                                        <%--<asp:RadioButtonList ID="rblType" runat="server" BorderStyle="Solid" BorderColor="Black" BorderWidth="1" Width="50%">
                                            <asp:ListItem Value="0">Agent/charterers</asp:ListItem>
                                            <asp:ListItem Value="1">Regulatory Body</asp:ListItem>
                                            <asp:ListItem Value="2">Owners preferred Vendor</asp:ListItem>                                            
                                            <asp:ListItem Value="3">New Vendor</asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                        <table width="100%">
                                            <tr>
                                                <td style="width:50%">
                                                </td>
                                                <td style="font-size:small">
                                                    <asp:CheckBoxList ID="chkType" runat="server" BorderStyle="Solid" BorderColor="Black"
                                                        BorderWidth="1" Width="85%">
                                                        <asp:ListItem Value="0">Agent/charterers</asp:ListItem>
                                                        <asp:ListItem Value="1">Regulatory Body</asp:ListItem>
                                                        <asp:ListItem Value="2">Owners preferred Vendor</asp:ListItem>
                                                        <asp:ListItem Value="3">New Vendor</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
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
