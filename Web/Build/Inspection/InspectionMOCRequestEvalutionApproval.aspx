<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCRequestEvalutionApproval.aspx.cs"
    Inherits="InspectionMOCRequestEvalutionApproval" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RootCause" Src="~/UserControls/UserControlImmediateMainCause.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubRootCause" Src="~/UserControls/UserControlImmediateSubCause.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BasicCause" Src="~/UserControls/UserControlBasicMainCause.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubBasicCause" Src="~/UserControls/UserControlBasicSubCause.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAR</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <eluc:TabStrip ID="MenuMOCStatus" runat="server" OnTabStripCommand="MenuMOCStatus_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuMOCApproveStatus" runat="server" OnTabStripCommand="MenuMOCApproveStatus_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <br />
        <table id="TableEvaluation" width="100%">
            <tr>
                <td width="20%">
                    <telerik:RadLabel ID="lblevalperson" runat="server" Text="Person Responsible for Change (Name &amp; Rank)">
                    </telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPersonInCharge" runat="server">
                        <telerik:RadTextBox ID="txtCrewName" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="150px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCrewRank" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="150px">
                        </telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="imgPersonInCharge"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtCrewId" runat="server" CssClass="hidden" MaxLength="20"
                            Width="10px">
                        </telerik:RadTextBox>
                    </span><span id="spnPersonInChargeOffice" runat="server">
                        <telerik:RadTextBox ID="txtOfficePersonName" runat="server" CssClass="input_mandatory"
                            Enabled="false" MaxLength="50" Width="150px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtOfficePersonDesignation" runat="server" CssClass="input_mandatory"
                            Enabled="false" MaxLength="50" Width="150px">
                        </telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="imgPersonInChargeOffice"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox runat="server" ID="txtPersonInChargeOfficeId" CssClass="hidden"
                            Width="0px" MaxLength="20">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtPersonInChargeOfficeEmail" CssClass="hidden"
                            Width="0px" MaxLength="20">
                        </telerik:RadTextBox>
                    </span>
                    <asp:Label ID="lblDtkey" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
        <table id="Table1" width="100%">
            <tr>
                <td width="20%">
                    <telerik:RadLabel ID="lblApprover" runat="server" Text="Approved By">
                    </telerik:RadLabel>
                </td>
                <td>
                    <span id="spnApprovedBy" runat="server">
                        <telerik:RadTextBox ID="txtApprovalPerson" runat="server" CssClass="input_mandatory"
                            Enabled="false" MaxLength="50" Width="150px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtApprovalDesignation" runat="server" CssClass="input_mandatory"
                            Enabled="false" MaxLength="50" Width="150px">
                        </telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="imgPersonInChargeApproval"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox runat="server" ID="txtPersonInChargeApproverId" CssClass="hidden"
                            Width="0px" MaxLength="20">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtPersonInChargeApproverEmail" CssClass="hidden"
                            Width="0px" MaxLength="20">
                        </telerik:RadTextBox>
                    </span>
                    <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
        <br>
        <b>
            <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="&nbsp;Evaluation of the Proposal ">
            </telerik:RadLabel>            
        </b>
        <hr />
        <br />
        <table id="tblEvaluation" runat="server">
            <tr>
                <td width="50%">
                    <telerik:RadLabel ID="lblEvaluationQuestion1" runat="server" Text="1. Is the change necessary/beneficial ?">
                    </telerik:RadLabel>
                </td>
                <td width="10%">
                    <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rbnnecessary" runat="server"
                        Direction="Horizontal" OnSelectedIndexChanged="rdnnecessary_SelectedIndexChanged"
                        DropDownHeight="80px" AutoPostBack="true">
                        <Items>
                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtremarks" runat="server" CssClass="gridinput" Width="80%"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEvaluationQuestion2" runat="server" Text="2. Details of the change requested, justification, Risk Assessment (if any) and items listed in Section A and B above were reviewed and found adequate?">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rbnrequested" runat="server"
                        Direction="Horizontal" OnSelectedIndexChanged="rdnrequested_SelectedIndexChanged"
                        DropDownHeight="100px" AutoPostBack="true">
                        <Items>
                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtremarks1" runat="server" CssClass="gridinput" Width="80%"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblIntermediate" runat="server" Text="3. What is the frequency of intermediate verification to be carried out to ensure progress of the MOC as planned?">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="rcbIntermediateFrequency" Width="80%" runat="server" Filter="Contains"
                        EmptyMessage="Type to Select" CssClass="input_mandatory" OnSelectedIndexChanged="rcbIntermediateFrequency_SelectedIndexChanged">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtremarks2" runat="server" CssClass="gridinput" Width="80%"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <br />
        <b>
            <telerik:RadLabel ID="lblApproval" runat="server" Text="Approval for the Change">
            </telerik:RadLabel>          
            <hr />
        </b>
        <br />
        <table id="tblApproval" runat="server">
            <tr>
                <td width="50%">
                    <telerik:RadLabel ID="lblApprovalQuestion1" runat="server" Text="1. Were all the MOC process documents including Risk assessment submitted by Responsible person(s) reviewed & found adequate?">
                    </telerik:RadLabel>
                </td>
                <td width="10%">
                    <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rdnApprovalQ1" runat="server"
                        Direction="Horizontal" OnSelectedIndexChanged="rdnApprovalQ1_SelectedIndexChanged"
                        DropDownHeight="80px" AutoPostBack="true">
                        <Items>
                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtApprovalQ1Remarks" runat="server" CssClass="gridinput"
                        Width="80%" TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblApprovalQuestion2" runat="server" Text="2. Is the overall estimated cost, if applicable; commensurate to the benefit?">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rdnApprovalQ2" runat="server"
                        Direction="Horizontal" OnSelectedIndexChanged="rdnApprovalQ2_SelectedIndexChanged"
                        DropDownHeight="80px" AutoPostBack="true">
                        <Items>
                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtApprovalQ2Remarks" runat="server" CssClass="gridinput"
                        Width="80%" TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="3. Is the Target Date acceptable?">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rdnApprovalQ3" runat="server"
                        Direction="Horizontal" OnSelectedIndexChanged="rdnApprovalQ3_SelectedIndexChanged"
                        DropDownHeight="80px" AutoPostBack="true">
                        <Items>
                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtApprovalQ3Remarks" runat="server" CssClass="gridinput"
                        Width="80%" TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMOCCommittee" runat="server" Text="4. MOC Committee Involved?">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rbnMOCCommitte" runat="server"
                        Direction="Horizontal" DropDownHeight="80px" AutoPostBack="true" OnSelectedIndexChanged="rbnMOCCommitte_SelectedIndexChanged">
                        <Items>
                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <br />
        <table id="tblMOCCommitee" runat="server" visible="false">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblmocMeeting" runat="server" Text="Date of MOC Meeting">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucMOCMeetingDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMOCMember" runat="server" Text="Name and Designation of other MOC Committee Members:">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtMOCMember" runat="server" CssClass="gridinput" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblauthority" runat="server" Text="Approving Authority (Name & Rank)*:">
                    </telerik:RadLabel>
                </td>
                <td>
                    <span id="spnApprovalPersonOffice" runat="server">
                        <telerik:RadTextBox ID="txtApprovalPersonName" runat="server" CssClass="input_mandatory"
                            Enabled="false" MaxLength="50" Width="150px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtApprovalPersonRank" runat="server" CssClass="input_mandatory"
                            Enabled="false" MaxLength="50" Width="150px">
                        </telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="lnkimage"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox runat="server" ID="txtApprovalPersonOfficeId" CssClass="hidden"
                            Width="0px" MaxLength="20">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtApprovalPersonOfficeEmail" CssClass="hidden"
                            Width="0px" MaxLength="20">
                        </telerik:RadTextBox>
                    </span>
                </td>
            </tr>
        </table>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
