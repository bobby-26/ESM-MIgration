<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewMedicalRequestUpdate.aspx.cs"
    Inherits="Crew_CrewMedicalRequestUpdate" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Clinic" Src="~/UserControls/UserControlClinic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Medical Request Update</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewMedical" runat="server" OnTabStripCommand="CrewMedical_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblguidanceText" runat="server" Text=" Note: Liver Function Test(LFT) required for seafarers signed off from all oil chemical
                            vessels">
                        </telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtName" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRankName" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCDCNumber" runat="server" Text="CDC Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCDC" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbldob" runat="server" Text="D.O.B."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtDOB" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofBirth" runat="server" Text="Place of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPlaceofbirth" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequest" runat="server" Text="Request Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblRequest" runat="server" Layout="Flow" Columns="2" Direction="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblRequest_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Full medical request" />
                                <telerik:ButtonListItem Value="2" Text="LFT request" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="52%"
                            AutoPostBack="true" OnTextChanged="VesselChanged" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td width="20%">
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Zone ID="ddlZone" runat="server" AppendDataBoundItems="true" CssClass="input" Width="60%"
                            AutoPostBack="true" OnTextChangedEvent="ClinicAddress" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClinic" runat="server" Text="Clinic"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Clinic ID="ddlClinic" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="52%"/>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppointment" runat="server" Text="Appointment"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                        <telerik:RadMaskedTextBox runat="server" ID="txtTime" CssClass="input_mandatory" Width="50px" Mask="##:##"></telerik:RadMaskedTextBox>
                        <telerik:RadLabel ID="lblhrs" runat="server" Text="(hrs)"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBudgetCode" runat="server" ReadOnly="true"
                            Width="21%">
                        </telerik:RadTextBox>
                        <telerik:RadComboBox ID="ddlBudgetCode" runat="server" CssClass="dropdown_mandatory" Width="31%"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="OnBudgetChange" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME" OnTextChanged="ddlAccountDetails_TextChanged"
                            DataValueField="FLDACCOUNTID" Width="60%" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlOwnerBudgetCode" runat="server" CssClass="dropdown_mandatory" Width="52%"
                            AppendDataBoundItems="false" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentBy" runat="server" Text="Payment By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblFee" runat="server" Layout="Flow" Columns="2" Direction="Horizontal">
                            <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                        </telerik:RadRadioButtonList>               
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lbl1" runat="server" Text="Please examine the following persons for M.V/M.T"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Font-Bold="true"></telerik:RadLabel>
                        <telerik:RadLabel ID="lbl2" runat="server" Text="as per the requirement below"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <b>
                            <telerik:RadLabel ID="lblMedicalFlag" runat="server" Text="Medical Examination for Flags"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="cblFlagMedical" runat="server" RepeatDirection="Horizontal" Columns="4">
                            <DataBindings DataTextField="FLDFLAGNAME" DataValueField="FLDCOUNTRYCODE" />
                        </telerik:RadCheckBoxList>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblpreexamination" runat="server" Text="Pre Employment Medical Examination"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="cblPreMedical" runat="server" RepeatDirection="Horizontal" Columns="3">
                            <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblTest" runat="server" Text="Test"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblMedicalTest" runat="server" RepeatDirection="Horizontal" Columns="4">
                            <DataBindings DataTextField="FLDNAMEOFMEDICAL" DataValueField="FLDDOCUMENTMEDICALID" />
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblVaccination" runat="server" Text="Vaccination"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="4">
                        <telerik:RadCheckBoxList ID="cblVaccination" runat="server" RepeatDirection="Horizontal" Columns="6">
                            <DataBindings DataTextField="FLDNAMEOFMEDICAL" DataValueField="FLDDOCUMENTMEDICALID" />
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblDeclaration" runat="server" Text="Declaration"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="4">
                        <telerik:RadCheckBoxList ID="cblDeclaration" runat="server" RepeatDirection="Horizontal" Columns="6">
                            <DataBindings DataTextField="FLDNAMEOFDECLARATION" DataValueField="FLDMEDICALDECLARATIONID" />
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblOther" runat="server" Text="Others(Please Specify)"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOthers" TextMode="MultiLine" runat="server"
                            Height="30px" Width="360px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
