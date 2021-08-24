<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDeficiencyAdd.aspx.cs"
    Inherits="InspectionDeficiencyAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register Src="../UserControls/UserControlQuick.ascx" TagName="Quick" TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionDeficiency" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuInspectionDeficiency" runat="server" OnTabStripCommand="InspectionDeficiency_TabStripCommand" Title="Add"></eluc:TabStrip>
            <table id="tblInspectionNC" width="100%">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDeficiencyType" runat="server" Text="Deficiency Type"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" colspan="3">
                        <telerik:RadRadioButtonList ID="rblDeficiencyType" runat="server" Direction="Horizontal"
                            OnTextChanged="DeficiencyType_TextChanged" AutoPostBack="true">
                            <Items>
                                <telerik:ButtonListItem Text="NC" Value="2" />
                                <telerik:ButtonListItem Text="Major NC" Value="1" />
                                <telerik:ButtonListItem Text="Observation" Value="3" />
                                <telerik:ButtonListItem Text="Hi Risk Observation" Value="4" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lbLVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:VesselByCompany ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="vessel_TextChanged" Width="270px" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Company ID="ucCompany" runat="server" Enabled="false" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            CssClass="input" AutoPostBack="true" AppendDataBoundItems="true" OnTextChangedEvent="company_TextChanged" Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblsource" runat="server" Text="Source"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlSchedule" runat="server" CssClass="input" Width="270px" AutoPostBack="true" OnSelectedIndexChanged="ddlSchedule_Changed" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblIssuedDate" runat="server" Text="Issued&nbsp; Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                            AutoPostBack="true" OnTextChangedEvent="ucIssueDateEdit_TextChanged" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDeficiencyCategory" runat="server" Text="Deficiency Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Quick ID="ucNonConformanceCategory" runat="server" AppendDataBoundItems="true"
                            Width="270px" CssClass="dropdown_mandatory" QuickTypeCode="47" Visible="true" />
                        <%--<eluc:Quick ID="ucRiskCategory" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="71" Visible="false" Width="250px"/>--%>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCheckListReferenceNo" runat="server" Text="CheckList Reference Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtChecklistRef" runat="server" CssClass="input" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblItem" runat="server" Text="Item"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtItem" runat="server" CssClass="input" Height="60px" TextMode="MultiLine"
                            Width="270px" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblPSCActionCode" runat="server" Text="PSC Action Code / VIR Condition"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtKey" runat="server" CssClass="input" Width="70px" onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtKeyName" runat="server" CssClass="readonlytextbox" Width="198px"
                            ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDescription" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtDesc" runat="server" CssClass="input_mandatory" Height="60px"
                            TextMode="MultiLine" Width="270px" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblinspectorComments" runat="server" Text="Inspector Comments"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtInspectorComments" runat="server" CssClass="input" Height="60px"
                            TextMode="MultiLine" Width="270px" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text="Office Remarks"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtOfficeRemarks" runat="server" CssClass="input" Height="60px"
                            TextMode="MultiLine" Width="270px" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCopyDetailstoCAR" runat="server" Text="Copy details to CAR"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkCopyCAR" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblRCAnotRequired" runat="server" Text="RCA not required"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadCheckBox ID="chkRCANotrequired" runat="server" AutoPostBack="true" OnCheckedChanged="chkRCANotrequired_CheckedChanged" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblRCATargetDate" runat="server" Text="RCA Target Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucRcaTargetDate" runat="server" CssClass="input" DatePicker="true"
                            COMMANDNAME="RCATARGETDATE" />
                    </td>
                    <td style="width: 15%">
                        <%--Master's Comments--%>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtMasterComments" runat="server" CssClass="input" Height="60px"
                            TextMode="MultiLine" Width="270px" Visible="false" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
