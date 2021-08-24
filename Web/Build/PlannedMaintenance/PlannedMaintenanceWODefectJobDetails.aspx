<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWODefectJobDetails.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceWODefectJobDetails" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Component" Src="~/UserControls/UserControlMultiColumnComponents.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselJob" Src="~/UserControls/UserControlMultiColumnWorkOrder.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlMaskNumber.ascx" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseModelWindow() {
                    var radwindow = $find('<%=modalPopup.ClientID %>');
            radwindow.close();
        }
           
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="FormWOWorkRequest" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
        <eluc:TabStrip ID="MenuNavigate" runat="server" OnTabStripCommand="MenuNavigate_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuWOWorkRequest" runat="server" OnTabStripCommand="MenuWOWorkRequest_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
            <asp:Panel ID="pnlDefect" runat="server" GroupingText="Defect Job">
                <table width="100%">

                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%;">
                            <telerik:RadLabel runat="server" ID="lblComponent" Text="Component"></telerik:RadLabel>
                        </td>
                        <td style="width:25%;">
                            <eluc:Component ID="ucComponent" runat="server" Width="300px" />
                        </td>
                        <td style="width:10%;">
                            <telerik:RadLabel runat="server" ID="lblTitle" Text="Title"></telerik:RadLabel>
                        </td>
                        <td style="width:25%;">
                            <telerik:RadTextBox runat="server" ID="txtTitle" Width="300px" CssClass="input_mandatory"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblDueDate" Text="Due Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtPlannedStartDate" CssClass="input_mandatory" Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Discipline ID="ucDiscipline" runat="server" AppendDataBoundItems="true" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucMaintClass" runat="server" AppendDataBoundItems="true" AutoPostBack="true" 
                                OnTextChangedEvent="ucMaintClass_TextChangedEvent" Width="300px" CssClass="input_mandatory" />
                        </td>

                        <td>
                            <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucMainType" runat="server" AppendDataBoundItems="true" Width="300px" CssClass="input_mandatory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblWorkDetails" runat="server" Text="Defect Details"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtWorkDetails" runat="server" TextMode="MultiLine" Resize="Both"
                                Height="70px" Width="300px">
                            </telerik:RadTextBox>

                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAction" runat="server" Text="Interim steps taken for the period when equipment is defective"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtActionTaken" runat="server" TextMode="MultiLine" Height="70px" Width="300px" Resize="Both"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblReason" runat="server" Text="Possible reason for defect"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtReason" runat="server" TextMode="MultiLine" Height="70px" Width="300px" Resize="Both"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblActionRequired" runat="server" Text="Action Required"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtActionRequired" runat="server" TextMode="MultiLine" Height="70px" Width="300px" Resize="Both"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                            <telerik:RadLabel runat="server" ID="lblLinkedToExistingJob" Text="Link to Existing Job" Visible="false"></telerik:RadLabel>
                            <asp:ImageButton ID="ImgLink" runat="server" ImageUrl="~/css/Theme1/images/add_task.png" OnClick="ImgLink_Click" Visible="false" />
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPriority" runat="server" Width="60px" ReadOnly="true"></telerik:RadTextBox>
                            <telerik:RadCheckBoxList runat="server" ID="chkLinkedJobs" Visible="false"></telerik:RadCheckBoxList>
                            <asp:HiddenField ID="hdnLinkedJobs" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVIQCode" runat="server" Text="VIQ Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Decimal runat="server" ID="ucVIQCode" DecimalDigits="2" MaxValue="99.99" />   
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                </table>
                <table width="100%" cellpadding="2px">
                    <tr>
                        <td style="width:10%;">
                            <telerik:RadLabel runat="server" ID="lblAvailable" Text="Are Spares/Stores Available Onboard"></telerik:RadLabel>
                        </td>
                        <td style="width:15%;text-align:center;">
                            <telerik:RadRadioButtonList ID="rbtSpare" runat="server" Direction="Horizontal" 
                                OnSelectedIndexChanged="rbtSpare_SelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                            <asp:LinkButton ID="lnkRequisitionCreate" runat="server" Text="Requisition" OnClick="lnkRequisitionCreate_Click"></asp:LinkButton>
                            
                            <%--<asp:LinkButton ID="lnkRequisitionMap" runat="server" Text="Requisition Map" OnClick="lnkRequisitionMap_Click"></asp:LinkButton>--%>
                        </td>
                        <td style="width:10%;">
                            <telerik:RadLabel runat="server" ID="lblIsDefectAffecting" Text="Is it Significant defect affecting Navigation, Safety, or Pollution prevention"></telerik:RadLabel>
                        </td>
                        <td style="width:15%; text-align:center;">
                            <telerik:RadRadioButtonList ID="rbtAffectNavigation" runat="server" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblIsEquipmentRequired" Text="Is Equipment required to be taken out of service"></telerik:RadLabel>
                        </td>
                        <td style="width:15%;text-align:center;">
                            <telerik:RadRadioButtonList ID="rbtEquipmentTaken" runat="server" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblIsRARequired" Text="RA Required"></telerik:RadLabel>
                        </td>
                        <td style="width:15%;text-align:center;">
                            <telerik:RadRadioButtonList ID="rbtRaRequired" runat="server" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbtRaRequired_SelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                            <asp:LinkButton ID="lnkRaCreate" runat="server" Text="RA Create" OnClick="lnkRaCreate_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <telerik:RadLabel ID="lblIsShoreAssistanceReq" runat="server" Text="Is Shore Assistance Required?"></telerik:RadLabel>
                        </td>
                        <td style="width:15%;text-align:center;">
                            <telerik:RadRadioButtonList ID="rbtShoreAssistant" runat="server" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblDryDockJob" Text="Is the Job to be added to Dry Dock List"></telerik:RadLabel>
                        </td>
                        <td style="width:15%;text-align:center;">
                            <telerik:RadRadioButtonList ID="rbtDrydock" runat="server" Direction="Horizontal" OnSelectedIndexChanged="rbtDrydock_SelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblIncident" Text="Do you want to raise Machinery Incident"></telerik:RadLabel>
                        </td>
                        <td style="width:15%;text-align:center;">
                            <telerik:RadRadioButtonList ID="rbtIncident" runat="server" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbtIncident_SelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                            <asp:LinkButton ID="lnkIncidentCreate" runat="server" Text="Incident Create" OnClick="lnkIncidentCreate_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblIsInternal" Text="Is Internal Purpose"></telerik:RadLabel>
                        </td>
                        <td style="width:15%;text-align:center;">
                            <telerik:RadRadioButtonList ID="rbtInternalDefect" runat="server" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1" />
                                    <telerik:ButtonListItem Text="No" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    </table>
                    <table width="100%" cellpadding="2px">
                    <tr>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblRequisitionNo" Text="Requisition No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRequisitionNo" CssClass="input" Enabled="false"></telerik:RadTextBox>
                            <asp:HiddenField ID="hdnRequisitionID" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblRANumber" Text="RA Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRANumber" CssClass="input" Enabled="false"></telerik:RadTextBox>
                            <asp:HiddenField ID="hdnRAID" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblIncidentNo" Text="Incident"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtIncident" RenderMode="Lightweight" Enabled="false"></telerik:RadTextBox>
                            <asp:HiddenField ID="hdnIncidentID" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblWorkOrder" Text="Work Order"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtWorkOrder" RenderMode="Lightweight" Enabled="false"></telerik:RadTextBox>
                            <asp:HiddenField ID="hdnGroupID" runat="server" />
                            <asp:HiddenField ID="hdnWorkOrderID" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlDryDock" runat="server" GroupingText="Docking Details">
                <table width="100%" runat="server" id="tblExtraQuestion">
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel runat="server" ID="lblWorktobeSurveyedBy" Text="Surveyed by"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <div style="height: 100px; width: 85%; overflow: auto;" class="input">
                                <telerik:RadCheckBoxList runat="server" ID="cblWorktobeSurveyedBy" Width="85%"></telerik:RadCheckBoxList>
                            </div>
                        </td>
                        <td width="20%">
                            <telerik:RadLabel runat="server" ID="lblMaterial" Text="Material"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <div style="height: 100px; width: 85%; overflow: auto;" class="input">
                                <telerik:RadCheckBoxList runat="server" ID="cblMaterial"></telerik:RadCheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblEnclosed" Text="Enclosed"></telerik:RadLabel>
                        </td>
                        <td>
                            <div style="height: 100px; width: 85%; overflow: auto;" class="input">
                                <telerik:RadCheckBoxList runat="server" ID="cblEnclosed"></telerik:RadCheckBoxList>
                            </div>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlJobType" runat="server" AppendDataBoundItems="true" Width="85%">
                                <Items>
                                    <telerik:DropDownListItem Value="" Text="--Select--" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="Left" width="20%">
                            <telerik:RadLabel ID="lblPTWApproval" runat="server" Text="PTW Approval"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucWTOApproval" runat="server" HardTypeCode="117"
                                AppendDataBoundItems="true" DataBoundItemName="None" />
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblInclude" Text="Include"></telerik:RadLabel>
                        </td>
                        <td>
                            <div style="height: 100px; width: 85%; overflow: auto;" class="input">
                                <telerik:RadCheckBoxList ID="cblInclude" runat="server">
                                </telerik:RadCheckBoxList>
                            </div>
                        </td>
                    </tr>
                    
                </table>
            </asp:Panel>
        </telerik:RadAjaxPanel>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="700px" Height="450px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
             VisibleStatusbar="false" KeepInScreenBounds="true">            
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
