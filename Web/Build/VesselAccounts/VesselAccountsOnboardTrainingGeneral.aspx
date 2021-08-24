<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOnboardTrainingGeneral.aspx.cs"
    Inherits="VesselAccountsOnboardTrainingGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OnboardTrainingTopic" Src="~/UserControls/UserControlOnboardTrainingTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Onboard Training General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
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
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>

        <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="SaveTraining" />
        <eluc:TabStrip ID="MenuOnboardTraining" runat="server" OnTabStripCommand="MenuOnboardTraining_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%" CssClass="scrolpan">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />

            <table id="tblConfigureCrewOnboardTrainingList" width="100%">
                <tr>
                    <td width="30%">
                        <telerik:RadLabel ID="lblNameofthetraining" runat="server" Text="Training"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:OnboardTrainingTopic runat="server" ID="ucSubject" CssClass="input_mandatory"
                            Width="240px" AutoPostBack="true" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td width="30%">
                        <telerik:RadLabel ID="lblNameofthetrainingIfnotthereinthetraininglist" runat="server"
                            Text="Training (If not there in the training list)">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTraningName" Width="240px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="30%">
                        <telerik:RadLabel ID="lblDateFrom" runat="server" Text="Training Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtFromDate" CssClass="input_mandatory" />
                        <eluc:Date runat="server" ID="txtToDate" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td align="left" width="30%">
                        <telerik:RadLabel ID="lblDuration" runat="server" Text="Duration"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDuration" runat="server" CssClass="input_mandatory" DefaultZero="false"
                            DecimalPlace="1" />
                        <telerik:RadLabel ID="lblHrs" runat="server" Text="Hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="30%">
                        <telerik:RadLabel ID="lblTrainingConductedFor" runat="server" Text="Training For"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnOnBoardSelectedEmployee">
                            <telerik:RadTextBox ID="txtSelectedEmployeenameEdit" runat="server" CssClass="input_mandatory"
                                ReadOnly="true" Enabled="false" MaxLength="50" Width="49.6%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgOnBoardSelectedEmployeeEdit" ToolTip="Show Documents">
                                    <span class="icon"><i class="fas fas fa-list-alt"></i></span>
                            </asp:LinkButton>

                            <telerik:RadTextBox ID="txtSelectedEmployeeIdEdit" runat="server" CssClass="input" Width="1px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="30%">
                        <telerik:RadLabel ID="lblTrainingConductedBy" runat="server" Text="Training By"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnOnBoardEmployee">
                            <telerik:RadTextBox ID="txtEmployeenameEdit" runat="server" CssClass="input_mandatory" ReadOnly="true"
                                AutoPostBack="true" Enabled="false" MaxLength="50" Width="30%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtEmployeepRankEdit" runat="server" CssClass="input_mandatory"
                                ReadOnly="true" Enabled="false" MaxLength="50" Width="19%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgOnBoardEmployeeEdit" ToolTip="Show Documents">
                                    <span class="icon"><i class="fas fas fa-list-alt"></i></span>
                            </asp:LinkButton>

                            <telerik:RadTextBox ID="txtEmployeeIdEdit" runat="server" CssClass="input" Width="1px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="30%">
                        <telerik:RadLabel ID="lblTrainerNameIfnotthereintheTrainingConductedByList" runat="server"
                            Text="Training By (If not there in the Training By List)">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTrainerName" runat="server" Width="240px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="30%">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRemarks" Width="240px" TextMode="MultiLine" Height="50px"
                            CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>

            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
