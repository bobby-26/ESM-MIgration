<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderGeneral.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderGeneral" ValidateRequest="true" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HistoryTemplate" Src="~/UserControls/UserControlHistoryTemplate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detail</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderGeneral" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <%--<telerik:RadSkinManager ID="RadSkinManager1" runat="server" />--%>

        <telerik:RadAjaxPanel ID="Panel1" runat="server" EnableAJAX="true">

            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuWorkOrderGeneral" runat="server" OnTabStripCommand="MenuWorkOrderGeneral_TabStripCommand"></eluc:TabStrip>
            </div>

            <div id="general" runat="server" style="overflow-y: hidden">
                <br clear="all" />

                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtWorkOrderNumber" RenderMode="Lightweight" runat="server" Enabled="false" CssClass="input"
                                MaxLength="20" Width="120px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtJobId" RenderMode="Lightweight" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentJobId" RenderMode="Lightweight" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCreatedBy" runat="server" Text="Created By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCreatedBy" Enabled="false" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTitle" RenderMode="Lightweight" runat="server" CssClass="input" Width="300px" MaxLength="200"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblForEquipment" runat="server" Text="For Equipment"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPickListComponent">
                                <telerik:RadTextBox ID="txtComponentCode" RenderMode="Lightweight" runat="server" CssClass="input_mandatory readonlytextbox"
                                    MaxLength="20" Width="68px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtComponentName" RenderMode="Lightweight" runat="server" CssClass="input_mandatory readonlytextbox"
                                    MaxLength="20" Width="230px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtComponentId" RenderMode="Lightweight" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                            </span>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                        </td>
                        <td>
                            <span style="width: 200px">
                                <eluc:Discipline ID="ucDiscipline" runat="server" AppendDataBoundItems="true" />
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblUnplannedWork" runat="server" Text="Unplanned Work"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkUnexpected" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPTWApproval" runat="server" Text="PTW Approval"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucWTOApproval" runat="server" HardTypeCode="117"
                                AppendDataBoundItems="true" DataBoundItemName="None" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblStartedDate" runat="server" Text="StartedDate"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtStartedDate" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCompletedDate" runat="server" Text="Completed Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCoplitedDate" RenderMode="Lightweight" runat="server" CssClass="readonlytextbox" MaxLength="20"
                                Width="90px" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text="Last Done Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLastDone" RenderMode="Lightweight" runat="server" CssClass="input readonlytextbox" ReadOnly="true" Width="90px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRA" runat="server" Text="Risk Assessment"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnRA">
                                <telerik:RadTextBox ID="txtRANumber" runat="server" RenderMode="Lightweight" CssClass="input readonlytextbox"
                                    MaxLength="50" Width="100px" Text="">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtRA" runat="server" RenderMode="Lightweight" CssClass="input readonlytextbox"
                                    MaxLength="50" Width="200px" Text="">
                                </telerik:RadTextBox>
                                <asp:ImageButton ID="imgShowRA" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." OnClick="imgShowRA_Click" />
                                <telerik:RadTextBox ID="txtRAId" runat="server" RenderMode="Lightweight" CssClass="hidden" MaxLength="20" Width="0px"
                                    Text="">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtRaType" runat="server" RenderMode="Lightweight" CssClass="hidden" MaxLength="2" Width="0px"
                                    Text=''>
                                </telerik:RadTextBox>
                            </span>
                            &nbsp
                            <asp:ImageButton runat="server" AlternateText="Show RA Details" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>"
                                ID="cmdRA" ToolTip="Show PDF" OnClick="cmdRA_Click" Visible="false" />
                            &nbsp
                            <asp:LinkButton ID="lnkCreateRA" runat="server" Text="Create" OnClick="lnkCreateRA_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtFrequency" Mask="999" Enabled="false" CssClass="input" Width="45px"></telerik:RadTextBox>
                            <eluc:Hard ID="ucFrequency" runat="server" AppendDataBoundItems="false" Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="WindowDays" runat="server" Text="Window(Days)"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtWindow" RenderMode="Lightweight" runat="server" CssClass="input" Enabled="false"
                                Width="60px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDueDate" runat="server" Text="Due Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtDueDate" CssClass="input" Enabled="false" />
                            <span id="spnPickReason" runat="server" visible="false">
                                <asp:TextBox ID="txtReason" runat="server" Width="1px"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="cmdShowReason" ImageUrl="<%$ PhoenixTheme:images/reschedule-remark.png %>"
                                    ImageAlign="AbsMiddle" Text=".." ToolTip="Reschedule Remarks" />
                            </span>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOverdue" runat="server" Text="Overdue (Days)"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOverduedays" RenderMode="Lightweight" runat="server" Enabled="false" CssClass="input"
                                MaxLength="20" Width="60px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                            <telerik:RadLabel ID="lblRescheduleReason" runat="server" Text="Reschedule Reason"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ddlRescheduleReason" runat="server" QuickTypeCode="120" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPostPonedDate" runat="server" Text="PostPoned Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPosponeDate" RenderMode="Lightweight" runat="server" ReadOnly="true" CssClass="readonlytextbox" MaxLength="20" Width="90px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCounterFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucCounterType" runat="server" CssClass="input" AppendDataBoundItems="true" Enabled="false"
                                AutoPostBack="false" />
                            <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="ucCounterFrequency" CssClass="input" Enabled="false"
                                Width="60px">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="WindowHrs" runat="server" Text="Window (Hrs)"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtWindowsHrs" CssClass="input" Style="text-align: right;"
                                Width="60px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDueHrs" runat="server" Text="Due (Hrs)"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtDueHours" CssClass="input txtNumber" Width="90px" Enabled="false"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOverdueHrs" runat="server" Text="Overdue (Hrs)"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOverDueHrs" RenderMode="Lightweight" runat="server" Enabled="false" CssClass="input txtNumber"
                                MaxLength="20" Width="90px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration(Hrs)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtDuration" runat="server" CssClass="input" MaxLength="8" Width="90px" IsInteger="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtPriority" MaxLength="1" EnabledStyle-HorizontalAlign="Right" runat="server" CssClass="input" Width="60px" />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucMaintClass" runat="server" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMaintenanceClaim" runat="server" Text="Maintenance Claim"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucMainCause" runat="server" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucMainType" runat="server" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblHistoryTemplate" runat="server" Text="History Template" Visible="false"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:HistoryTemplate ID="ucHistory" runat="server" AppendDataBoundItems="true" CssClass="input" Visible="false" />
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
