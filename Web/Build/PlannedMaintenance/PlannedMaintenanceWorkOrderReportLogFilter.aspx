<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReportLogFilter.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderReportLogFilter" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Maintenance Done Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            document.onkeydown = function (e) {
                var keyCode = (e) ? e.which : event.keyCode;
                if (keyCode == 13) {
                    __doPostBack('MenuWorkOrderReportLogFilter$dlstTabs$ctl00$btnMenu', '');
                }
            }
        </script>
    </telerik:RadCodeBlock>

    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>
</head>
<body>
    <form id="frmWorkOrderReportLogFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <%--   <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Maintenance Log Filter"></asp:Label>
            </div>
        </div>--%>
        <telerik:RadAjaxPanel ID="ajxpanel" runat="server">

            <eluc:TabStrip ID="MenuWorkOrderReportLogFilter" runat="server" OnTabStripCommand="MenuWorkOrderReportLogFilter_TabStripCommand"></eluc:TabStrip>


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWorkOrderNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtWorkOrderNumber" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWorkOrderTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtWorkOrderTitle" CssClass="input" MaxLength="100"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentNumber" runat="server" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                            <img alt="" id="img1" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />
                            <telerik:RadTextBox ID="txtTmpComponentName" runat="server" CssClass="input hidden"></telerik:RadTextBox>

                            <%--<telerik:RadImageButton id="img2" runat="server" onclick="return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx', true); "
                           Image-Url="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />--%>

                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="input hidden" Width="0px"></telerik:RadTextBox>
                        </span>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtComponentName" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                   <td>
                        Class Code
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtClassCode" runat="server" CssClass="input" MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWorkDoneBy" runat="server" Text="Work Done By"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Discipline ID="ucWorkDoneBy" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWorkDoneDateBetween" runat="server" Text="Work Done Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtWorkDoneDateFrom" CssClass="input" />
                        &nbsp;-&nbsp;
                    <eluc:Date runat="server" ID="txtWorkDoneDateTo" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJobClasses" runat="server" Text="Job Classes"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="dvClass" class="input" style="overflow: auto; height: 100px">
                            <telerik:RadCheckBoxList ID="chkClasses" runat="server" DataBindings-DataTextField="FLDQUICKNAME" DataBindings-DataValueField="FLDQUICKCODE"
                                RepeatDirection="Vertical" Height="100%" Columns="4">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>   
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCalenderFrequencyBetween" runat="server" Text="Calender Frequency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucFrequency" runat="server" AppendDataBoundItems="true" HardTypeCode="7" />

                        <telerik:RadMaskedTextBox runat="server" ID="txtFrequencyFrom" Mask="###" DisplayFormatPosition="Right" CssClass="input" Width="45px"></telerik:RadMaskedTextBox>
                        <%-- &nbsp;-&nbsp;
                    <telerik:RadMaskedTextBox runat="server" ID="txtFrequencyTo" Mask="###" DisplayFormatPosition="Right" CssClass="input" Width="45px"></telerik:RadMaskedTextBox>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRunHourFrequencyBetween" runat="server" Text="Run Hour Frequency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCounterType" runat="server" HardTypeCode="111" SelectedHard="498" />
                        <eluc:Decimal runat="server" ID="ucCounterFrequencyFrom" CssClass="input" Mask="99999" Width="60px" />
                        <%--  &nbsp;-&nbsp;
                        <eluc:Decimal runat="server" ID="ucCounterFrequencyTo" CssClass="input" Mask="99999" Width="60px" />--%>
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
                        <telerik:RadLabel ID="lblMaintenanceCause" runat="server" Text="Maintenance Claim"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainCause" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblOverdue" runat="server" Text="Overdue"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadCheckBox ID="chkPlanning" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPriority" CssClass="input" MaxLength="1" Width="45px"></telerik:RadTextBox>
                    </td>                    
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPostponed" runat="server" Text="Postponed"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadCheckBox ID="chkPostponed" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlJobType" runat="server">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--All--" />
                                <telerik:RadComboBoxItem Value="0" Text="Planned" />
                                <telerik:RadComboBoxItem Value="1" Text="Un Planned" />
                                <telerik:RadComboBoxItem Value="2" Text="Defect" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblRaJob" runat="server" Text="Risk Assessment"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadCheckBox ID="chkRaJob" runat="server" />
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Is Critical"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <telerik:RadCheckBox ID="chkCritical" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
