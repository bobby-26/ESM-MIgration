<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCtmRequestArrangementGeneral.aspx.cs" Inherits="AccountsCtmRequestArrangementGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CTM Request Arrangement General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
     <%--   <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>--%>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function sethidden() {
            document.getElementById('hdnagentyn').value = "1";

        } function setview() {
            document.getElementById('hdnagentyn').value = "0";

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
          <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
    
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
       <%-- <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
            <ContentTemplate>--%>
               
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                
                        <eluc:Status runat="server" ID="ucStatus" />
               
                
                        <eluc:TabStrip ID="MenuCTMMain" runat="server" OnTabStripCommand="MenuCTMMain_TabStripCommand"
                            TabStrip="true">
                        </eluc:TabStrip>
                
               <%--     <div class="subHeader">
                        <div style="position: absolute; right: 0px">--%>
                            <eluc:TabStrip ID="MenuCTM" runat="server" OnTabStripCommand="MenuCTM_TabStripCommand"></eluc:TabStrip>
                            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
                  <%--      </div>
                    </div>--%>

              <%--      <div id="divMain" runat="server" style="width: 99%; float: left; padding: 5px 5px 5px 5px;">--%>
                      
                        <table width="100%" style="line-height: 20px;">
                            <tr>
                                <td style="width: 13%;">
                                    <telerik:RadLabel ID="lblRequestDate" runat="server" Visible="false" Text="Request Date"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                                    <asp:HiddenField ID="hdnagentyn" runat="server" Value="" />
                                </td>
                                <td style="width: 22%;">
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false"></telerik:RadLabel>
                                    <eluc:Date ID="txtDate" runat="server" CssClass="input" Visible="false" ReadOnly="true" />
                                    <telerik:RadTextBox ID="txtPort" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="95%"></telerik:RadTextBox>
                                </td>
                                <td style="width: 8%;">
                                    <telerik:RadLabel ID="lblPortAgent" runat="server" Text="Port Agent"></telerik:RadLabel>
                                </td>
                                <td style="width: 20%;">
                                    <telerik:RadTextBox ID="txtSupplierName" runat="server" CssClass="readonlytextbox"
                                        ReadOnly="true" Width="95%"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1px" CssClass="input" Visible="false"></telerik:RadTextBox>
                                </td>
                                <td style="width: 14%;">Agent Email</td>
                                <td style="width: 24%;">
                                    <telerik:RadTextBox ID="txtPortAgentEmail" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="95%"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Arranged Via
                                </td>
                                <td>
                                    <telerik:RadRadioButtonList ID="rblOfficeUser" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chkOfficeUser_OnCheckedChanged">
                                        <Items>
                                        <telerik:ButtonListItem Text="Joiner" Value="1"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Text="Office Employee" Value="2"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Text="Agent" Value="3"></telerik:ButtonListItem>
                                       </Items>
                                    </telerik:RadRadioButtonList>
                                    <telerik:RadLabel ID="lblEmployees" runat="server" Visible="false" Text=""></telerik:RadLabel>
                                </td>

                                <td colspan="2">
                                    <span id="spnPickListFleetManager" style="width: 95%">
                                        <telerik:RadTextBox ID="txtuserDesignation" CssClass="input_mandatory" Width="20%" runat="server"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtMentorName" runat="server" CssClass="input_mandatory" Width="53%" MaxLength="100"></telerik:RadTextBox>
                                        <asp:ImageButton runat="server" ID="imguser" Style="cursor: pointer; vertical-align: top"
                                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="sethidden()" />
                                        <telerik:RadTextBox runat="server" ID="txtuserid" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                        <telerik:RadTextBox runat="server" ID="txtuserEmailHidden" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                        <telerik:RadTextBox runat="server" ID="txtDesignation" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                                    </span><%--CssClass="hidden"--%>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRemittedTo" runat="server" Text="Remitted To"></telerik:RadLabel>
                                </td>
                                <td>
                                    <span id="spnPickListBank" style="width: 95%">
                                        <telerik:RadTextBox ID="txtAccountNo" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                            Width="32%"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtBankName" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                            Width="55%"></telerik:RadTextBox>
                                        <asp:ImageButton runat="server" ID="imgBankPicklist" Style="cursor: pointer; vertical-align: top"
                                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="setview()" />
                                        <%-- <img id="imgBankPicklist" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" 
                                            style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;"   />--%>
                                        <telerik:RadTextBox ID="txtBankID" runat="server" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                                    </span>
                                </td>

                            </tr>

                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                                </td>
                                <td>
                                 <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                                     DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" Width="95%"
                                     EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    </telerik:RadComboBox>
                                   <%-- <asp:DropDownList ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                                        DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" Width="95%">
                                    </asp:DropDownList>--%>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPaidFrom" runat="server" Text="Paid From"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:UserControlCompany ID="ddlPaidFromCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                        CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRequestedAmount" runat="server" Text="Requested Amount/Currency" Width="150px"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtAmount" CssClass="readonlytextbox input" runat="server" Width="80px" MaxLength="8" />
                                    /
                                    <telerik:RadTextBox ID="txtcurrencycode" CssClass="readonlytextbox input" runat="server" Width="80px"></telerik:RadTextBox>
                                </td>

                            </tr>
                            <tr>


                                <td>
                                    <telerik:RadLabel ID="lblArrangedAmount" runat="server" Text="Arranged Amount"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtAmountArranged" CssClass="input" runat="server" Width="80px" MaxLength="8" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                                </td>
                                <td>
                                    <span id="spnBudget" style="width: 95%">
                                        <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="80%" CssClass="input_mandatory"
                                            Enabled="False"></telerik:RadTextBox>
                                        <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." OnClientClick="setview()" />
                                        <telerik:RadTextBox ID="txtBudgetName" runat="server" CssClass="input" Enabled="False"
                                            Width="0px"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    </span>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                                </td>
                                <td>
                                    <span id="spnOwnerBudget" style="width: 95%">
                                        <telerik:RadTextBox ID="txtOwnerBudgetCode" runat="server"
                                            MaxLength="20" CssClass="input_mandatory" Width="80%"></telerik:RadTextBox>
                                        <asp:ImageButton ID="btnShowOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." OnClientClick="setview()" />
                                        <telerik:RadTextBox ID="txtOwnerBudgetName" runat="server" Width="0px" CssClass="input"
                                            Enabled="False"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtOwnerBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtOwnerBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <hr />
                                    <b>Charges</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblDeliveredBy" runat="server" Text="Delivered By" Visible="false"></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtDeliveredBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"></telerik:RadTextBox>
                                    <telerik:RadLabel ID="lblCtmChargesLumpsum" Visible="false" runat="server" Text="Charges (Lumpsum)"></telerik:RadLabel>
                                    <eluc:Number ID="txtCharges" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false" Width="90px" />
                                    <eluc:Number ID="txtLumpsumCharges" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false" Width="90px" />
                                    <telerik:RadLabel ID="lblhandlingfee" runat="server" Text="Handling Fees(%)"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtHandlingFees" runat="server" CssClass="input" Width="90px" />
                                </td>

                                <td>
                                    <telerik:RadLabel ID="lblRemittanceCharges" runat="server" Text="Charges (%)"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtRemittanceCharges" runat="server" CssClass="input" Width="90px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRemittanceChargesLumpsum" runat="server" Text="Lumpsum"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtRemittanceLumpsumCharges" runat="server" CssClass="input" Width="90px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblTotalCharges" runat="server" Text="Total Charges"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtTotalCharges" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="90px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblChargesBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                                </td>
                                <td>
                                    <span id="spnChargesBudgetCode" style="width: 95%">
                                        <telerik:RadTextBox ID="txtChargesBudgetCode" runat="server" Width="80%" CssClass="input_mandatory"
                                            Enabled="False"></telerik:RadTextBox>
                                        <asp:ImageButton ID="btnShowChargesBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." OnClientClick="setview()" />
                                        <telerik:RadTextBox ID="txtChargesBudgetName" runat="server" CssClass="input" Enabled="False"
                                            Width="0px"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtChargesBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtChargesBudgetGroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    </span>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblChargesOwnerBudget" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                                </td>
                                <td>
                                    <span id="spnChargesOwnerBudget" style="width: 95%">
                                        <telerik:RadTextBox ID="txtChargesOwnerBudget" runat="server"
                                            MaxLength="20" CssClass="input_mandatory" Width="80%"></telerik:RadTextBox>
                                        <asp:ImageButton ID="btnShowChargesOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." OnClientClick="setview()" />
                                        <telerik:RadTextBox ID="txtChargesOwnerBudgetName" runat="server" Width="0px" CssClass="input"
                                            Enabled="False"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtChargesOwnerBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtChargesOwnerBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblRemittanceAmount" runat="server" Text="Remittance Amount"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtRemittanceAmount" runat="server" CssClass="input" Width="90px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblBalance" runat="server" Text="Balance"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtBalance" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="90px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text="Office Remarks"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine" Width="95%" Height="40px"></telerik:RadTextBox>
                                </td>
                            </tr>


                        </table>
                    
     </telerik:RadAjaxPanel>
    </form>
</body>
</html>
