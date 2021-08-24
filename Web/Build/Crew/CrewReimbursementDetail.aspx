<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReimbursementDetail.aspx.cs"
    Inherits="CrewReimbursementDetail" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Budget" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ReimbursementRecovery" Src="~/UserControls/UserControlReimbursementRecovery.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reimbursment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }

        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .checkRtl {
            direction: rtl;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewList" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <eluc:TabStrip ID="MenuDetail" runat="server" OnTabStripCommand="MenuDetail_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="92%" CssClass="scrolpan">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1" runat="server" id="tblSefarer">
                <tr>
                    <td style="width: 11%">
                        <telerik:RadLabel ID="lblSeafarer" runat="server" Text="Seafarer File No."></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox runat="server" ID="txtFileNo" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="ImgBtnValidFileno" ToolTip="Verify Entered File Number" OnClick="ImgBtnValidFileno_Click">
                                    <span class="icon"><i class="fas fa-search"></i></span>
                        </asp:LinkButton>
                        <font color="blue">Note: Please verify the entered file number by clicking search icon
                                next to the File number textbox</font>
                    </td>
                </tr>
                <tr>
                    <td style="width: 11%">
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 22%">
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 11%">
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 22%">
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" Enabled="false" Width="180px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td style="width: 11%">
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 22%">
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" Enabled="false" Width="180px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" Width="180px" Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" Width="180px" Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 25%">
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td style="width: 25%">
                        <telerik:RadTextBox ID="txtEmpFileNo" runat="server" CssClass="readonlytextbox" Enabled="false" Width="280px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td style="width: 25%">
                        <telerik:RadLabel ID="lblEmployee" runat="server" Text="Employee"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%">
                        <telerik:RadTextBox ID="txtEmployee" runat="server" CssClass="readonlytextbox" Enabled="false" Width="280px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrentVessel" Text="Current Vessel" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" Enabled="false" Width="280px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" CssClass="readonlytextbox" Enabled="false" Width="280px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRefno" runat="server" Text="Reference No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtId" runat="server" CssClass="readonlytextbox" Enabled="false" Width="280px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReimbursementRecovery" runat="server" Text="Reimbursement/ Recovery"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" ID="ddlEarDed" runat="server" EnableLoadOnDemand="True"
                            OnSelectedIndexChanged="ddlEarDed_SelectedIndexChanged" Width="280px"
                            EmptyMessage="Type to select Reimbursement/ Recovery" Filter="Contains" AutoPostBack="true" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--All--" Value="" />
                                <telerik:RadComboBoxItem Text="Reimbursement(B.O.C)" Value="1" />
                                <telerik:RadComboBoxItem Text="Reimbursement(Monthly)" Value="2" />
                                <telerik:RadComboBoxItem Text="Reimbursement(E.O.C)" Value="3" />
                                <telerik:RadComboBoxItem Text="Recovery(B.O.C)" Value="-1" />
                                <telerik:RadComboBoxItem Text="Recovery(Monthly)" Value="-2" />
                                <telerik:RadComboBoxItem Text="Recovery(E.O.C)" Value="-3" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClaim" runat="server" Text="Claim Submission Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" Width="150px" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblPurpose" Text="Purpose" runat="server"></telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:ReimbursementRecovery ID="ddlPurpose" runat="server" CssClass="input_mandatory"
                            AppendDataBoundItems="true" OnTextChangedEvent="ddlPurpose_TextChangedEvent" AutoPostBack="true" Width="280px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" Text="Currency" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ddlCurrency" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            OnTextChangedEvent="ddlCurrency_TextChangedEvent" AppendDataBoundItems="true" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDescription" Text="Description" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDesc" runat="server" CssClass="input_mandatory" Width="280px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBudgetCode" Text="Budget Code" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Budget ID="ddlBudgetCode" runat="server" CssClass="input_mandatory" HardList='<%#PhoenixRegistersBudget.ListBudget() %>'
                            AppendDataBoundItems="true" Width="150px" Visible="false" />
                        <telerik:RadTextBox ID="txtBudgetCode" runat="server" ReadOnly="true" CssClass="readonlytextbox" with="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                        <b>Reimbursement/Recovery Details</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblpaymentmode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlPaymentModeAdd" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                            HardTypeCode="142" AutoPostBack="true" OnTextChangedEvent="ddlPaymentModeAdd_TextChangedEvent" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBankaccount" runat="server" Text="Bank Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:BankAccount ID="ddlBankAccount" runat="server" Width="280px"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblAccountof" Text="Account of" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                            VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="ddlVessel_TextChangedEvent" Entitytype="VSL"
                            Width="150px" AssignedVessels="true" />
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="350px" ShowEvent="onmouseover" CssClass="fon"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto"
                            Text="*Note:<b>Account Of</b> column to selected based on the reimbursement being made in relation to expenses made for the Last vessel,Present vessel(if onboard only),Next Vessel(ifplanned)">
                        </telerik:RadToolTip>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLVessel" runat="server" Text="Last Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblLastVessel" runat="server" CssClass="readonlytextbox" Enabled="false" Width="280px" ReadOnly="true" Font-Bold="true"></telerik:RadTextBox>

                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChargeable" Text="Chargeable" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVesselChargeable" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                            VesselsOnly="true" Entitytype="VSL"  AssignedVessels="true" Width="150px" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblPvessel" runat="server" Text="Present Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblPresentVessel" runat="server" CssClass="readonlytextbox" Enabled="false" Width="280px" ReadOnly="true" Font-Bold="true"></telerik:RadTextBox>

                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblRecovery" runat="server" Text="Recovery to be Completed in Current Contact Only"></telerik:RadLabel>

                        <asp:CheckBox ID="chk_CurrentContract" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNVessel" runat="server" Text="Next Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="lblNextVessel" runat="server" CssClass="readonlytextbox" Enabled="false" Width="280px" ReadOnly="true" Font-Bold="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPaymentcurrency" Text="Vessel Currency" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucPaymentcurrency" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            AppendDataBoundItems="true" OnTextChangedEvent="ucPaymentcurrency_TextChangedEvent" Width="150px" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExchange" Text="Exchange Rate" runat="server"></telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Number ID="txtExchangeRate" runat="server" CssClass="input_mandatory" DecimalPlace="17" MaxLength="25" Width="150px" />
                        <%--ReadOnly="true"--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApprovedamount" Text="Approved Amount" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtApprovedAmount" runat="server" CssClass="input" Width="150px"
                            AutoPostBack="true" OnTextChangedEvent="txtApprovedAmount_TextChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblApprovedClaimAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtClaimAmount" runat="server" CssClass="readonlytextbox txtNumber"
                            ReadOnly="true" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUnapproved" runat="server" Text="Unapproved Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtUnApprovedAmount" runat="server" CssClass="input" Width="150px" />
                        <asp:ImageButton runat="server" AlternateText="Calculate" ImageUrl="<%$ PhoenixTheme:images/Cal.png %>"
                            CommandName="SAVE" ID="cmdCalculate" ToolTip="Calculate" Style="cursor: pointer; vertical-align: top"
                            OnClick="cmdCalculateWage_Click"></asp:ImageButton>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblunApprovedClaimAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtunClaimAmount" runat="server" CssClass="readonlytextbox txtNumber"
                            ReadOnly="true" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReason" Text="Reason for Unapproved Amount" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUnApprovedReason" runat="server" CssClass="input" Width="280px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNoofInstallment" runat="server" Text="Number of Installment"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtNoofInstallment" runat="server" CssClass="input_mandatory" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCreatedby" runat="server" Text="Created By : "></telerik:RadLabel>
                        <asp:Label ID="txtCreatedBy" runat="server" Font-Bold="true"></asp:Label>
                        <telerik:RadLabel ID="lblCreatedDate" runat="server" Text=" On "></telerik:RadLabel>
                        <asp:Label ID="txtCreatedDate" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By : "></telerik:RadLabel>
                        <asp:Label ID="txtApprovedBy" runat="server" Font-Bold="true"></asp:Label>
                        <telerik:RadLabel ID="lblApprovedDate" runat="server" Text=" On"></telerik:RadLabel>
                        <asp:Label ID="txtApprovedDate" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApprovalRemark" runat="server" Text="Approval Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtApprovalRemark" runat="server" CssClass="input" MaxLength="500"
                            TextMode="MultiLine" Width="280px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                

            </table>

            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
