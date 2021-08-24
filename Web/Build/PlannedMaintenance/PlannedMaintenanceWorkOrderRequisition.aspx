<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderRequisition.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderRequisition" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Component" Src="~/UserControls/UserControlMultiColumnComponents.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Job" Src="~/UserControls/UserControlMultiColumnJob.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Machinery / Equipment failure</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderRequisition" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuWorkOrderRequestionTop" runat="server" OnTabStripCommand="MenuWorkOrderRequestionTop_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuWorkOrderRequestion" runat="server" OnTabStripCommand="MenuWorkOrderRequestion_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel Visible="false" runat="server" ID="lblWorkOrderRequisitionID"></telerik:RadLabel>
                    <telerik:RadLabel Visible="false" runat="server" ID="lblWorkOrderID"></telerik:RadLabel>
                    <telerik:RadTextBox ID="txtWorkOrderNumber" runat="server"  Enabled="false"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input_mandatory" Width="300px"></telerik:RadTextBox>
                </td>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblInclude" runat="server" Text="Include"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListComponent">
                        <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                            Enabled="false" Width="60px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="20"
                            Enabled="false" Width="210px">
                        </telerik:RadTextBox>
                        <img id="imgComponent" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />
                        <telerik:RadTextBox ID="txtComponentId" runat="server"  Width="10px"></telerik:RadTextBox>
                    </span>&nbsp;
                            <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClick="cmdComponentClear_Click" />
                    <eluc:Component ID="ucComponent" runat="server" CssClass="input_mandatory"
                        Width="290px" Visible="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblCreated" runat="server" Text="Created"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCreatedDate" runat="server" CssClass="readonlytextbox" Width="90px"
                        Enabled="false">
                    </telerik:RadTextBox>
                </td>
                <td rowspan="9" valign="top">
                    <telerik:RadListBox ID="cblInclude" runat="server" Direction="Vertical" CheckBoxes="true">
                    </telerik:RadListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListJob">
                        <telerik:RadTextBox ID="txtJobCode" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                            ReadOnly="false" Width="60px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtJobName" runat="server" CssClass="input readonlytextbox" MaxLength="20"
                            ReadOnly="false" Width="210px">
                        </telerik:RadTextBox>
                        <img id="imgJob" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>" style="cursor: pointer; vertical-align: top" />
                        <telerik:RadTextBox ID="txtJobId" runat="server"  Width="10px"></telerik:RadTextBox>
                    </span>&nbsp;
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClick="cmdJobClear_Click" />
                    <eluc:Job ID="ucJob" runat="server" CssClass="input readonlytextbox"
                        Width="290px" Visible="false" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Discipline ID="ucDiscipline" runat="server"  AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration(Hrs)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal runat="server" ID="txtDuration" Mask="99999999"  Width="60px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblUnplannedWork" runat="server" Text="UnplannedWork"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkUnexpected" runat="server" OnCheckedChanged="chkUnplanned_CheckedChanged" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPlannedStart" runat="server" Text="Planned Start"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtPlannedStartDate"  />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtPriority"  Width="60px" Text="3" MaxLength="1" EnabledStyle-HorizontalAlign="Right">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <telerik:RadLabel ID="lblWorkDetails" runat="server" Text="Work Details"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtJobDescription" runat="server" Width="300px" TextMode="MultiLine"
                        Height="50px" >
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDefectList" runat="server" Text="Defect List"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox runat="server" ID="chkIsDefect" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblWorktobesurveyedby" runat="server" Text="Work to be surveyed by"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadListBox ID="cblWorkSurvey" runat="server" Direction="Horizontal" CheckBoxes="true">
                    </telerik:RadListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMaterial" runat="server" Text="Material"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadListBox ID="cblMaterial" runat="server" Direction="Horizontal" CheckBoxes="true">
                    </telerik:RadListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEnclosed" runat="server" Text="Enclosed"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadListBox ID="cblEnclosed" runat="server" Direction="Horizontal" CheckBoxes="true">
                    </telerik:RadListBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList ID="ddlJobType" runat="server"  AppendDataBoundItems="true">
                        <Items>
                            <telerik:DropDownListItem Value="" Text="--Select--" />
                        </Items>
                    </telerik:RadDropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPTWApproval" runat="server" Text="PTW Approval"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucWTOApproval" runat="server"  HardTypeCode="117"
                        AppendDataBoundItems="true" DataBoundItemName="None" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucMaintClass" runat="server" AppendDataBoundItems="true" AutoPostBack="true"  OnTextChangedEvent="ucMaintClass_TextChangedEvent" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucMainType" runat="server" AppendDataBoundItems="true"  />
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="5">
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDateDone" runat="server" Text="Date Done"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtWorkDoneDate" runat="server"  />
                </td>
                <td>
                    <telerik:RadLabel ID="lblTotalManHours" runat="server" Text="Total Man Hours"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtWorkDuration" runat="server"  Mask="999.99" Width="60px" Text="" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
