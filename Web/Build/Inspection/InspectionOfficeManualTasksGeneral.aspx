<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOfficeManualTasksGeneral.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="InspectionOfficeManualTasksGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubDepartment" Src="~/UserControls/UserControlSubDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAR</title>
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
            function setScrollPosition(divname, hdnname) {
                var div = $get(divname);
                var hdn = $get(hdnname);
                hdn.value = div.scrollTop;
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            }
            function EndRequestHandler(sender, args) {

                listBox = $get('<%= divVesselType.ClientID %>');
                hdn = $get('<%= hdnVesselTypeScroll.ClientID %>');
                listBox.scrollTop = hdn.value;

                listBox = $get('<%= divVessel.ClientID %>');
                hdn = $get('<%= hdnVesselScroll.ClientID %>');
                listBox.scrollTop = hdn.value;
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <eluc:Title ID="ucTitle" runat="server" Text="Office Manual Task" ShowMenu="true" Visible="false" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuOfficeTasksGeneral" runat="server" OnTabStripCommand="MenuOfficeTasksGeneral_TabStripCommand"></eluc:TabStrip>
        <div id="divDetails" runat="server">
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel runat="server" ID="lblPreventiveTask" Text="Preventive Task"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="300px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportedBy" runat="server" Text="Reported By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReportedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="300px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlTaskStatus" runat="server" AppendDataBoundItems="true" Width="300px"
                            CssClass="input_mandatory" CommandName="TASKSTATUS" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Open" Value="0" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Completed" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Closed" Value="4"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblReportedDate" runat="server" Text="Reported Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtReportedDate" runat="server" DatePicker="true" CssClass="readonlytextbox"
                            ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTaskCategory" runat="server" Text="Task Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            Width="300px" OnSelectedIndexChanged="ddlCategory_Changed" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTaskSubCategory" runat="server" Text="Task Subcategory"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubcategory" runat="server" Width="300px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDepartmentAssignedTo" runat="server" Text="Department ( Assigned to )"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Department ID="ucDept" runat="server" AppendDataBoundItems="true"
                            Width="300px" AutoPostBack="true" OnTextChangedEvent="ucDept_TextChangedEvent" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblSubDepartment" runat="server" Text="Sub Department"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:SubDepartment ID="ucSubDept" runat="server" AppendDataBoundItems="true"
                            Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblPreventiveAction" runat="server" Text="Preventive Action"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPreventiveAction" runat="server" CssClass="input_mandatory" Height="70px"
                            Rows="4" TextMode="MultiLine" Width="300px" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblControlActionNeeds" runat="server" Text="Control Action Needs"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCAN" runat="server" CssClass="input" Height="70px" Rows="4" TextMode="MultiLine"
                            Width="300px" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="paTargetDate" runat="server" Text="Target Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucpaTargetDate" runat="server" CssClass="input_mandatory" COMMANDNAME="TARGETDATE"
                            DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                        &nbsp;&nbsp;&nbsp;&nbsp 
                                    <asp:LinkButton ID="lnkcomment" runat="server" ToolTip="Comments" Visible="false">
                                                    <span class="icon"><i class="fa fa-comments"></i></span>
                                    </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" valign="top">
                        <telerik:RadLabel ID="lblApprovalRemarks" runat="server" Text="Approval Remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <telerik:RadTextBox ID="txtApprovalRemarks" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="300px" Height="70px" Resize="Both" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="bottom">
                        <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td valign="bottom" colspan="2">
                        <telerik:RadTextBox ID="txtApprovedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="300px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblApprovedDate" runat="server" Text="Approved Date"></telerik:RadLabel>
                    </td>
                    <td valign="top" colspan="2">
                        <eluc:Date ID="txtApprovedDate" runat="server" CssClass="readonlytextbox" DatePicker="true"
                            ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lblComletionRemarks" runat="server" Text="Completion Remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtCompletionRemarks" runat="server" CssClass="input" Height="70px" Resize="Both"
                            CommandName="COMPLETIONREMARKS" Rows="4" TextMode="MultiLine" Width="300px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%" valign="bottom">
                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucCompletionDate" runat="server" DatePicker="true"
                            CommandName="COMPLETIONDATE" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblCompletedBy" runat="server" Text="Completed By"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtCompletedByName" runat="server" Width="150px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCompletedByDesignation" runat="server" Width="143px" CssClass="readonlytextbox"
                            Enabled="false" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td rowspan="2" style="width: 15%;" valign="top">
                        <telerik:RadLabel ID="lblCloseOutRemarks" runat="server" Text="Close Out remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2" style="width: 35%">
                        <telerik:RadTextBox ID="txtCloseOutRemarks" runat="server" CssClass="input" Height="70px" Resize="Both"
                            CommandName="CLOSEOUTREMARKS" Rows="4" TextMode="MultiLine" Width="300px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 15%" valign="bottom">
                        <telerik:RadLabel ID="lblCloseOutDate" runat="server" Text="Close Out Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="bottom" colspan="2">
                        <eluc:Date ID="ucCloseOutDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblCloseOutBy" runat="server" Text="Close Out by"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%" valign="top" colspan="2">
                        <telerik:RadTextBox ID="txtCloseOutByName" runat="server" Width="150px" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCloseOutByDesignation" runat="server" Width="143px" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lbltaskgenerate" runat="server" Text="Are Corrective tasks to be generated"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chktaskgenerate" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel runat="server" ID="lblCorrectiveTask" Text="Corrective Task"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="Corrective Action"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCorrectiveAction" runat="server" CssClass="input_mandatory" Height="70px"
                            Rows="4" TextMode="MultiLine" Width="300px" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblDeficiencyDetails" runat="server" Text="Deficiency Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDeficiencyDetails" runat="server" CssClass="input_mandatory" Height="70px" Rows="4" TextMode="MultiLine" Width="300px" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucTargetDate" runat="server" CssClass="input_mandatory" COMMANDNAME="TARGETDATE"
                            DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVerificationLevel" runat="server" Text="Verification Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucVerficationLevel" runat="server" AppendDataBoundItems="true"
                            Width="300px" HardTypeCode="195" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel runat="server" ID="lblVesselType" Text="Vessel Type"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel runat="server" ID="lblVessels" Text="Vessel"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="divVesselType" runat="server" class="input" onscroll="javascript:setScrollPosition('divVesselType','hdnVesselTypeScroll');"
                            style="overflow: auto; height: 200px;">
                            <asp:HiddenField ID="hdnVesselTypeScroll" runat="server" />
                            &nbsp;<telerik:RadCheckBox ID="chkVesselTypeAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkVesselTypeAll_Changed"
                                Text="---SELECT ALL---" />
                            <telerik:RadCheckBoxList ID="chkVesselType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkVesselType_Changed"
                                Columns="2" Direction="Vertical">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td colspan="2">
                        <div id="divVessel" runat="server" class="input" onscroll="javascript:setScrollPosition('divVessel','hdnVesselScroll');"
                            style="overflow: auto; height: 200px">
                            <asp:HiddenField ID="hdnVesselScroll" runat="server" />
                            <telerik:RadCheckBoxList ID="chkVessel" runat="server" Columns="3" Direction="Vertical" AutoPostBack="false">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
