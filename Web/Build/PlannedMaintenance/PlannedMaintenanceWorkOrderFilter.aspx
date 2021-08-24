<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderFilter.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Maintenance Due</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

    <script type="text/javascript">
        document.onkeydown = function (e) {
            var keyCode = (e) ? e.which : event.keyCode;
            if (keyCode == 13) {
                __doPostBack('MenuWorkOrderFilter$dlstTabs$ctl00$btnMenu', '');
            }
        }
    </script>
</head>
<body>
    <form id="frmComponentFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="ajxpanel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <eluc:TabStrip ID="MenuWorkOrderFilter" runat="server" OnTabStripCommand="MenuWorkOrderFilter_TabStripCommand"></eluc:TabStrip>
            </telerik:RadCodeBlock>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr id="wrinfo" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWorkOrderNumber" runat="server" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr id="compinfo" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentNumber" RenderMode="Lightweight" runat="server" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                            <img id="img1" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" class="input hidden" style="cursor: pointer; vertical-align: top" />
                            <telerik:RadTextBox ID="txtTmpComponentName" RenderMode="Lightweight" runat="server" CssClass="input hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentId" RenderMode="Lightweight" runat="server" CssClass="input hidden" Width="0px"></telerik:RadTextBox>
                        </span>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input" MaxLength="50"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClassCode" runat="server" Text="Class Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtClassCode" runat="server" CssClass="input" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentCategory" runat="server" Text="Component Category"></telerik:RadLabel>
                    </td>
                    <td>
                         <eluc:Quick ID="ddlComponentCategory" runat="server" AppendDataBoundItems="true" QuickTypeCode="166" />
                    </td>
                </tr>
                <tr id="respinfo" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="Div3" class="input" style="overflow: auto; width: 70%; height: 100px">
                            <telerik:RadCheckBoxList ID="cblResponsibility" runat="server" DataBindings-DataTextField="FLDDISCIPLINENAME"
                                DataBindings-DataValueField="FLDDISCIPLINEID"
                                RepeatDirection="Vertical" Height="100%">
                            </telerik:RadCheckBoxList>
                            <telerik:RadComboBox ID="ddlResponsibility" DataTextField="FLDDISCIPLINENAME" DataValueField="FLDDISCIPLINEID" AppendDataBoundItems="true"
                                runat="server">
                            </telerik:RadComboBox>
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDueBetweenDate" runat="server" Text="Due"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtDateFrom" CssClass="input" />
                        &nbsp;&nbsp;
                    <eluc:Date runat="server" ID="txtDateTo" CssClass="input" />
                        <telerik:RadComboBox ID="ddlDueDays" runat="server">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--All--" />
                                <telerik:RadComboBoxItem Value="0" Text="Over Due" />
                                <telerik:RadComboBoxItem Value="15" Text="15 Days" />
                                <telerik:RadComboBoxItem Value="30" Text="30 Days" />
                                <telerik:RadComboBoxItem Value="60" Text="60 Days" />
                                <telerik:RadComboBoxItem Value="90" Text="90 Days" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblJobClasses" runat="server" Text="Job Classes"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="dvClass" class="input" style="overflow: auto; width: 70%; height: 100px">
                            <telerik:RadCheckBoxList ID="chkClasses" runat="server" DataBindings-DataTextField="FLDQUICKNAME"
                                DataBindings-DataValueField="FLDQUICKCODE" Columns="4"
                                RepeatDirection="Vertical" Height="100%">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPlanning" runat="server" Text="Planning"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <div runat="server" id="dvPlaning" class="input" style="overflow: auto; width: 70%; height: 100px">
                            <telerik:RadCheckBoxList ID="ckPlaning" runat="server" DataBindings-DataTextField="FLDHARDNAME"
                                DataBindings-DataValueField="FLDHARDCODE"
                                RepeatDirection="Vertical" Height="100%">
                            </telerik:RadCheckBoxList>
                        </div>
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
                        <telerik:RadLabel ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMaintClass" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceClaim" runat="server" Text="Maintenance Claim"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainCause" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUnplannedWork" runat="server" Text="Unplanned Work"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox RenderMode="Lightweight" ID="chkUnexpected" runat="server" />
                    </td>
                </tr>
                <tr id="statusinfo" runat="server">
                    <td valign="top" rowspan="2">
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <div runat="server" id="dvStatus" class="input" style="overflow: auto; width: 70%; height: 100px">
                            <telerik:RadCheckBoxList ID="chkStatus" runat="server" DataBindings-DataTextField="FLDHARDNAME" DataBindings-DataValueField="FLDHARDCODE"
                                RepeatDirection="Vertical" Height="100%">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <eluc:Decimal runat="server" ID="txtPriority" Mask="9" CssClass="input" Width="60px" />
                    </td>
                </tr>
                <tr id="definfo" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblDefectList" runat="server" Text="Defect List"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkDefect" runat="server" />
                        <telerik:RadComboBox ID="ddlDefect" runat="server">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--All--" />
                                <telerik:RadComboBoxItem Value="0" Text="Planned" />
                                <telerik:RadComboBoxItem Value="1" Text="Un Planned" />
                                <telerik:RadComboBoxItem Value="2" Text="Defect" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr id="rainfo" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblRaRequired" runat="server" Text="RA Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkRaRequired" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRaPending" runat="server" Text="RA Approval Pending"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkRaPendingApproval" runat="server" />
                    </td>

                </tr>
                <tr id="woinfo" runat="server">
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Planned In Work Order"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPlannedInWo" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobCategory" runat="server" Text="Job Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucJobCategory" runat="server" QuickTypeCode="165" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
