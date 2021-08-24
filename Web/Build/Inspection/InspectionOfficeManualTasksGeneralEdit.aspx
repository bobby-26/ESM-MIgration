<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOfficeManualTasksGeneralEdit.aspx.cs"
    Inherits="Inspection_InspectionOfficeManualTasksGeneralEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
<head id="Head1" runat="server">
    <title>CAR</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="divInspectionIncidentCriticalFactor" runat="server">
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
        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <telerik:RadFormDecorator ID="RadFormDecorator" DecorationZoneID="pnlInspectionIncidentCriticalFactor" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <asp:UpdatePanel runat="server" ID="pnlInspectionIncidentCriticalFactor">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" Text="" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title ID="ucTitle" runat="server" Text="Office Manual Task" ShowMenu="false" />
                            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
                        </div>
                    </div>
                    <eluc:TabStrip ID="MenuOfficeTasksGeneral" runat="server" OnTabStripCommand="MenuOfficeTasksGeneral_TabStripCommand"></eluc:TabStrip>
                    <div id="divDetails" runat="server">
                        <table cellpadding="2" cellspacing="2" width="100%">
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <b>
                                        <asp:Literal runat="server" ID="lblPreventiveTask" Text="Preventive Task"></asp:Literal></b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference No."></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReferenceNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="200px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblReportedBy" runat="server" Text="Reported By"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReportedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTaskStatus" runat="server" AppendDataBoundItems="true" Width="150px"
                                        CssClass="input_mandatory" CommandName="TASKSTATUS" AutoPostBack="true">
                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Open" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Closed" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td valign="top">
                                    <asp:Literal ID="lblReportedDate" runat="server" Text="Reported Date"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtReportedDate" runat="server" DatePicker="true" CssClass="readonlytextbox"
                                        ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblTaskCategory" runat="server" Text="Task Category"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                                        Width="300px" OnSelectedIndexChanged="ddlCategory_Changed">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Literal ID="lblTaskSubCategory" runat="server" Text="Task Subcategory"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubcategory" runat="server" CssClass="input" Width="300px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%">
                                    <asp:Literal ID="lblDepartmentAssignedTo" runat="server" Text="Department ( Assigned to )"></asp:Literal>
                                </td>
                                <td style="width: 35%">
                                    <eluc:Department ID="ucDept" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        Width="200px" AutoPostBack="true" OnTextChangedEvent="ucDept_TextChangedEvent" />
                                </td>
                                <td style="width: 15%">
                                    <asp:Literal ID="lblSubDepartment" runat="server" Text="Sub Department"></asp:Literal>
                                </td>
                                <td style="width: 35%">
                                    <eluc:SubDepartment ID="ucSubDept" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="paTargetDate" runat="server" Text="Target Date"></asp:Literal>
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
                                <td valign="top">
                                    <asp:Literal ID="lblPreventiveAction" runat="server" Text="Preventive Action"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPreventiveAction" runat="server" CssClass="input_mandatory" Height="50px"
                                        Rows="4" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                </td>
                                <td valign="top">
                                    <asp:Literal ID="lblControlActionNeeds" runat="server" Text="Control Action Needs"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCAN" runat="server" CssClass="input" Height="50px" Rows="4" TextMode="MultiLine"
                                        Width="80%"></asp:TextBox>
                                </td>
                                <div id="divApprovedDetails" runat="server" visible="false">
                                    <tr>
                                        <td rowspan="2" valign="top">
                                            <asp:Literal ID="lblApprovalRemarks" runat="server" Text="Approval Remarks"></asp:Literal>
                                        </td>
                                        <td rowspan="2">
                                            <asp:TextBox ID="txtApprovalRemarks" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                Width="80%" Height="50px" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                        <td valign="bottom">
                                            <asp:Literal ID="lblApprovedBy" runat="server" Text="Approved By"></asp:Literal>
                                        </td>
                                        <td valign="bottom" colspan="2">
                                            <asp:TextBox ID="txtApprovedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Literal ID="lblApprovedDate" runat="server" Text="Approved Date"></asp:Literal>
                                        </td>
                                        <td valign="top" colspan="2">
                                            <eluc:Date ID="txtApprovedDate" runat="server" CssClass="readonlytextbox" DatePicker="true"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                            </tr>
                    </div>
                    <tr>
                        <td rowspan="2" style="width: 15%;" valign="top">
                            <asp:Literal ID="lblComletionRemarks" runat="server" Text="Completion Remarks"></asp:Literal>
                        </td>
                        <td rowspan="2" style="width: 35%">
                            <asp:TextBox ID="txtCompletionRemarks" runat="server" CssClass="input" Height="50px"
                                CommandName="COMPLETIONREMARKS" Rows="4" TextMode="MultiLine" Width="97%"></asp:TextBox>
                        </td>
                        <td style="width: 15%" valign="bottom">
                            <asp:Literal ID="lblCompletionDate" runat="server" Text="Completion Date"></asp:Literal>
                        </td>
                        <td style="width: 35%" valign="bottom" colspan="2">
                            <eluc:Date ID="ucCompletionDate" runat="server" DatePicker="true" CssClass="input"
                                CommandName="COMPLETIONDATE" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%" valign="top">
                            <asp:Literal ID="lblCompletedBy" runat="server" Text="Completed By"></asp:Literal>
                        </td>
                        <td style="width: 35%" valign="top" colspan="2">
                            <asp:TextBox ID="txtCompletedByName" runat="server" Width="150px" CssClass="readonlytextbox"
                                Enabled="false" ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="txtCompletedByDesignation" runat="server" Width="130px" CssClass="readonlytextbox"
                                Enabled="false" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td rowspan="2" style="width: 15%;" valign="top">
                            <asp:Literal ID="lblCloseOutRemarks" runat="server" Text="Close Out remarks"></asp:Literal>
                        </td>
                        <td rowspan="2" style="width: 35%">
                            <asp:TextBox ID="txtCloseOutRemarks" runat="server" CssClass="input" Height="50px"
                                CommandName="CLOSEOUTREMARKS" Rows="4" TextMode="MultiLine" Width="97%"></asp:TextBox>
                        </td>
                        <td style="width: 15%" valign="bottom">
                            <asp:Literal ID="lblCloseOutDate" runat="server" Text="Close Out Date"></asp:Literal>
                        </td>
                        <td style="width: 35%" valign="bottom" colspan="2">
                            <eluc:Date ID="ucCloseOutDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%" valign="top">
                            <asp:Literal ID="lblCloseOutBy" runat="server" Text="Close Out by"></asp:Literal>
                        </td>
                        <td style="width: 35%" valign="top" colspan="2">
                            <asp:TextBox ID="txtCloseOutByName" runat="server" Width="150px" CssClass="readonlytextbox"
                                Enabled="false"></asp:TextBox>
                            <asp:TextBox ID="txtCloseOutByDesignation" runat="server" Width="130px" CssClass="readonlytextbox"
                                Enabled="false"></asp:TextBox>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    </tr>
                <div id="divCorrectiveTask" runat="server" visible="false">
                    <tr>
                        <td valign="top">
                            <asp:Literal ID="lbltaskgenerate" runat="server" Text="Are Corrective tasks to be generated"></asp:Literal>
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
                                <asp:Literal runat="server" ID="lblCorrectiveTask" Text="Corrective Task"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Literal ID="lblCorrectiveAction" runat="server" Text="Corrective Action"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCorrectiveAction" runat="server" CssClass="input_mandatory" Height="50px"
                                Rows="4" TextMode="MultiLine" Width="80%"></asp:TextBox>
                        </td>
                        <td valign="top">
                            <asp:Literal ID="lblDeficiencyDetails" runat="server" Text="Deficiency Details"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDeficiencyDetails" runat="server" CssClass="input_mandatory"
                                Height="50px" Rows="4" TextMode="MultiLine" Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTargetDate" runat="server" Text="Target Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucTargetDate" runat="server" CssClass="input_mandatory" COMMANDNAME="TARGETDATE"
                                DatePicker="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblVerificationLevel" runat="server" Text="Verification Level"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucVerficationLevel" runat="server" AppendDataBoundItems="true" CssClass="input"
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
                                <asp:Literal runat="server" ID="lblVesselType" Text="Vessel Type"></asp:Literal></b>
                        </td>
                        <td colspan="2">
                            <b>
                                <asp:Literal runat="server" ID="lblVessels" Text="Vessel"></asp:Literal></b>
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
                                &nbsp;<asp:CheckBox ID="chkVesselTypeAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkVesselTypeAll_Changed"
                                    Text="---SELECT ALL---" />
                                <asp:CheckBoxList ID="chkVesselType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkVesselType_Changed"
                                    RepeatDirection="Horizontal" RepeatColumns="2">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td colspan="2">
                            <div id="divVessel" runat="server" class="input" onscroll="javascript:setScrollPosition('divVessel','hdnVesselScroll');"
                                style="overflow: auto; height: 200px">
                                <asp:HiddenField ID="hdnVesselScroll" runat="server" />
                                <asp:CheckBoxList ID="chkVessel" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </div>
                    </table>
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
