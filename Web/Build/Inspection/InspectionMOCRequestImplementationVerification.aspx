<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCRequestImplementationVerification.aspx.cs"
    Inherits="InspectionMOCRequestImplementationVerification" %>

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
            function Impconfirm(args) {
                if (args) {
                    __doPostBack("<%=Impconfirm.UniqueID %>", "");
                }
            }
            function Vericonfirm(args) {
                if (args) {
                    __doPostBack("<%=Vericonfirm.UniqueID %>", "");
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
        <eluc:TabStrip ID="MenuMOCVerification" runat="server" OnTabStripCommand="MenuMOCVerification_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <br />
        <b>
            <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="Implementation">
            </telerik:RadLabel>
        </b>
        <hr />
        <table id="tblimplementation" runat="server">
            <tr>
                <td width="50%">
                    <telerik:RadLabel ID="lblImplementaionQ1" runat="server" Text="1. Date of Implementation of change (When last of the action items has been closed out): ">
                    </telerik:RadLabel>
                </td>
                <td width="10%">
                </td>
                <td>
                    <eluc:Date ID="dteImplementationDate" runat="server" CssClass="input" DatePicker="true"
                        AutoPostBack="true" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td >
                    <telerik:RadLabel ID="lblImplementationQ2" runat="server" Text="2. Has the intermediate verification been carried out to ensure progress of the MOC as planned in Section C? If No, provide details in comments below">
                    </telerik:RadLabel>
                </td>
                <td >
                    <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rbnImpQ2" runat="server"
                        Direction="Horizontal" DropDownHeight="80px" AutoPostBack="true">
                        <Items>
                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtImpQ2" runat="server" CssClass="gridinput" Width="80%"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <br />
        <table id="tblconfirmation" runat="server" width="100%">
            <tr>
                <td width="50%">
                    <telerik:RadLabel runat="server" ID="lblintermediateveri" Text="I confirm that the change has been implemented as identified above. ">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox runat="server" ID="chkconfirmation">
                    </telerik:RadCheckBox>
                </td>
            </tr>           
            <tr>
                <td>
                    <telerik:RadLabel ID="lblimplementationperson" runat="server" Text="Responsible Person (Name &amp; Rank)">
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
                        <asp:LinkButton runat="server" ID="imgPersonInChargeOffice" ><span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>                        
                        <telerik:RadTextBox runat="server" ID="txtPersonInChargeOfficeId" CssClass="hidden"
                            Width="0px" MaxLength="20">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtPersonInChargeOfficeEmail" CssClass="hidden"
                            Width="0px" MaxLength="20">
                        </telerik:RadTextBox>
                    </span>
                </td>
            </tr>
        </table>
        <br />
        <b>
            <telerik:RadLabel ID="lblverification" runat="server" Text="Verification on completion of Change">
            </telerik:RadLabel>
            <hr />
        </b>
        <br />
        <table id="tblverification" runat="server">
            <tr>
                <td width="50%">
                    <telerik:RadLabel ID="lblverifiQ1" runat="server" Text="1. Was the Management of Change effective? ">
                    </telerik:RadLabel>
                </td>
                <td width="10%">
                    <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rbnverifiQ1" runat="server"
                        Direction="Horizontal" DropDownHeight="80px" AutoPostBack="true">
                        <Items>
                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtverifiQ1Remarks" runat="server" CssClass="gridinput" Width="80%"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td >
                    <telerik:RadLabel ID="lblverifiQ2" runat="server" Text="2. Any feedback, recommendations and suggestions for improvement to proposer /responsible person /Senior management out of this MOC? (If yes provide details below)">
                    </telerik:RadLabel>
                </td>
                <td >
                    <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rbnverifiQ2" runat="server"
                        Direction="Horizontal" DropDownHeight="80px" AutoPostBack="true">
                        <Items>
                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Text="No" Value="2" Selected="true"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtVerifiQ2Remarks" runat="server" CssClass="gridinput" Width="80%"
                        TextMode="MultiLine">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel runat="server" ID="lblconfirmation" Text="I confirm that the Management of Change has been completed.">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox runat="server" ID="chkMOCConfirmation">
                    </telerik:RadCheckBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="rdlVerifiedby" runat="server" Text="Verified by (Name & Rank):">
                    </telerik:RadLabel>
                </td>
                <td colspan="2">
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
        <asp:Button ID="Impconfirm" runat="server" Text="confirm" OnClick="Impconfirm_Click" />
        <asp:Button ID="Vericonfirm" runat="server" Text="confirm" OnClick="Vericonfirm_Click" />
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
