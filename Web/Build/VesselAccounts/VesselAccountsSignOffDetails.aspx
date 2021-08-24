<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsSignOffDetails.aspx.cs"
    Inherits="VesselAccountsSignOffDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Off Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }       
        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuSignOffBow" runat="server" OnTabStripCommand="MenuSignOffBow_TabStripCommand"></eluc:TabStrip>

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" Enabled="false"
                            Width="200px">
                        </telerik:RadTextBox>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="500px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" CssClass="fon"
                            HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto"
                            Text="">
                            <telerik:RadLabel ID="lblNote" runat="server" Text="Note:"></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblAfterthesignoffdateisenteredorremoved" runat="server" Text="- After the sign off date is entered or removed"></telerik:RadLabel>
                            <br />
                            &nbsp;&nbsp;<telerik:RadLabel ID="lblPleaseclickthecalculatoricontorecalculatethebalanceofwage"
                                runat="server" Text="Please click the calculator icon to recalculate the balance of wage.">
                            </telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblForseeingBowReportentersignoffdateandclickBOWbutton" runat="server"
                                Text="- For seeing Bow Report enter sign off date and click BOW button.">
                            </telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblBeforeclicksavebuttonconfirmyourselftheenteredallotmentcashamountisright"
                                runat="server" Text="- Before click save button confirm yourself the entered allotment/cash amount is">
                            </telerik:RadLabel>
                            <br />
                            &nbsp;&nbsp;<telerik:RadLabel ID="lblOncetheallotmentcashisenteredandsavedyoucantchangeit"
                                runat="server" Text="Once the allotment/cash is entered and saved, you can't change it,">
                            </telerik:RadLabel>

                            &nbsp;&nbsp;&nbsp;&nbsp;<telerik:RadLabel ID="lblthereafteranychangesmadeinsignoffdateCurrentbalanceroundoffamountwillappearaccordingtothenewdatechanges"
                                runat="server" Text="there after any changes made in sign off date, Current balance/round
                            off amount will appear according to the new date changes">
                            </telerik:RadLabel>
                            <table>
                                <tr>
                                    <td colspan="4" style="color: red; font-weight: normal;">
                                        <telerik:RadLabel ID="lblBeforesavethesignoffdetailspleasecompletetheappraisal" runat="server"
                                            Text="- Before save the signoff details please complete the appraisal.">
                                        </telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadToolTip>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmpName" runat="server" CssClass="readonlytextbox" Width="200px"
                            Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignoffDate" runat="server" Text="Sign off"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtSignOffDate" runat="server" CssClass="input_mandatory" />
                        &nbsp;
                         <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdSignOffDateDelete" ToolTip="Delete Sign Off Date" OnClick="cmdSignOffDateDelete_Click">
                                    <span class="icon"> <i class="fas fa-backspace"></i></span>
                         </asp:LinkButton>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffPort" runat="server" Text="Sign Off Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ddlSeaPort" runat="server" CssClass="input_mandatory" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignoffReason" runat="server" Text="Sign off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOffReason runat="server" ID="ddlReason" Width="200px" CssClass="input_mandatory"
                            AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPayablewgaes" runat="server" Text="Payable wages"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="chbxlstPaidWages" RepeatDirection="Vertical" runat="server"
                            Width="200px" AutoPostBack="true" OnSelectedIndexChanged="chbxlstPaidWages_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Text="Sign Off Allowance" Value="1" />
                                <telerik:ButtonListItem Text="Performance Bonus" Value="1" />
                            </Items>
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrentBalance" runat="server" Text="Current Balance"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtBalance" runat="server" CssClass="readonlytextbox" Width="200px"
                            ReadOnly="true" />
                        <asp:LinkButton runat="server" AlternateText="Calculate" ID="cmdCalculateWage" ToolTip="Calculate Wage"
                            OnClick="cmdCalculateWage_Click">
                                    <span class="icon">  <i class="fas fa-calculator"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPayasCashAdvance" runat="server" Text="Pay as Cash Advance"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmountCash" runat="server" CssClass="input" DefaultZero="false"
                            AutoPostBack="true" OnTextChangedEvent="txtCashAmount_TextChanged" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPayasAllottment" runat="server" Text="Pay as Allotment"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmountAllot" runat="server" CssClass="input" DefaultZero="false"
                            AutoPostBack="true" OnTextChangedEvent="txtAllotAmount_TextChanged" OnblurScript="balance()"
                            Width="200px" />
                        &nbsp;<asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                            ID="cmdDelete" ToolTip="Delete Sign Off Allotment" OnClick="cmdDelete_Click"></asp:ImageButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBank" runat="server" Text="Bank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:BankAccount ID="ddlBankAccount" runat="server" Width="200px" CssClass="input"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOffremarks" runat="server" Text="Sign Off remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="200px" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRoundOffAmount" runat="server" Text="Round Off Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtRoundOff" runat="server" CssClass="readonlytextbox" Width="200px"
                            ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRecoveryYN" runat="server" Text="Office RecoverYN"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkRecoveryYn" runat="server" />
                    </td>
                </tr>

            </table>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
