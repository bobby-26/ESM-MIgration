<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerBugEdit.aspx.cs"
    Inherits="DefectTrackerBugEdit" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.DefectTracker" %>
<%@ Register TagPrefix="eluc" TagName="ModuleList" Src="~/UserControls/UserControlSepModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugTypes" Src="~/UserControls/UserControlSepBugType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugSeverity" Src="~/UserControls/UserControlSepBugSeverity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugPriority" Src="~/UserControls/UserControlSepBugPriority.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugStatus" Src="~/UserControls/UserControlSEPBugStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCode" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPTeamMembers" Src="~/UserControls/UserControlSEPTeamMembers.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title>Edit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewLicenceEntry">
        <ContentTemplate>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <div id="divHeading" class="divFloatLeft">
                        <eluc:Title runat="server" ID="ucTitle" Text="Bug Edit" ShowMenu="false"></eluc:Title>
                    </div>
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuBugComment" runat="server" OnTabStripCommand="MenuBugComment_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div class="subHeader">
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuBugEdit" runat="server" OnTabStripCommand="MenuBugEdit_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div style="position: relative">
                    <table width="100%">
                        <tr>
                            <td colspan="6">
                                <font color="blue" size="0"><b>On Change Requests/Requirements</b>
                                    <li>Users other than persons-in-charge of the requirements can log only Change Requests</li>
                                    <li>Persons-in-charge of the requirements are required to "Approve" to make it as Requirement.
                                        Only Requirements will be scheduled for development</li>
                                    <li>Issues logged as Bugs that require change in functionality will be re-classified
                                        as Change Request by Development team</li>
                                    <li>Change request logged along with bugs will be re-classified as Change request by
                                        Development team</li>
                                </font>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Project
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProject" runat="server" CssClass="input" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" MaxLength="100" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Created On
                            </td>
                            <td>
                                <asp:Label ID="lblCreatedOn" runat="server"></asp:Label>
                            </td>
                            <td>
                                Completed On
                            </td>
                            <td>
                                <asp:Label ID="lblCompletedOn" runat="server"></asp:Label>
                            </td>
                            <td>
                                Closed On
                            </td>
                            <td>
                                <asp:Label ID="lblClosedOn" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                For Vessel
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVesselCode" runat="server" CssClass="input" MaxLength="100" 
                                    Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Module
                            </td>
                            <td>
                                <eluc:ModuleList ID="ddlModuleList" runat="server" MaxLength="100" Width="360px"
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </td>
                            <td valign="top">
                                Status
                            </td>
                            <td>
                                <eluc:BugStatus ID="ddlSEPBugStatus" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Issue Type
                            </td>
                            <td>
                                <eluc:BugTypes ID="ddlBugType" runat="server" MaxLength="100" Width="360px" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" />
                            </td>
                            <td>
                                Severity
                            </td>
                            <td>
                                <eluc:BugSeverity ID="ddlBugSeverity" runat="server" MaxLength="100" Width="360px"
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </td>
                            <td>
                                Priority
                            </td>
                            <td>
                                <eluc:BugPriority ID="ddlBugPriority" runat="server" MaxLength="100" Width="360px"
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Subject
                                <br />
                                <font color="blue" size="0">(Do not edit; information will not be saved)</font>
                            </td>
                            <td valign="top" colspan="3">
                                <asp:TextBox ID="txtSubject" runat="server" MaxLength="200" CssClass="input_mandatory"
                                    Width="95%"></asp:TextBox>
                            </td>
                            <td valign="top">
                                Reported By
                            </td>
                            <td valign="top" colspan="3">
                                <asp:TextBox ID="txtReportedBy" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Description
                                <br />
                                <font color="blue" size="0">(Do not edit; information will not be saved)</font>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" TextMode="MultiLine"
                                    Rows="8" Columns="140" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%">
                    <asp:GridView ID="gvDefectAssign" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvDefectAssign_RowCommand" OnRowDataBound="gvDefectAssign_ItemDataBound"
                        OnRowCancelingEdit="gvDefectAssign_RowCancelingEdit" OnRowDeleting="gvDefectAssign_RowDeleting"
                        OnRowUpdating="gvDefectAssign_RowUpdating" OnRowEditing="gvDefectAssign_RowEditing"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnSorting="gvDefectAssign_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Abbreviation">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Bug Assign
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                <b>Task</b>
                                            </td>
                                            <td colspan="5">
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Assigned To:</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDTKeyDisplay" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                                <asp:Label ID="lblAssignedTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDTO") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>Assigned Date:</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAssignedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDDATE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>Planned effort:</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPlannedEffort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNEDEFFORT")%>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Start Date: </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>Finish Date: </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFinishDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINISHDATE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>Deploy Date: </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDeployDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPLOYEDDATE")%>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Actual Start Date: </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActualStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALSTARTDATE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>Actual Finish Date:</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActualFinishDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")%>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>Actual effort:</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblActualEffort" runat="server" MaxLength="1" DecimalPlace="0" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALEFFORT")%>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                <b>Task: </b>
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtRemarksEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                                    CssClass="input" MaxLength="500" Width="720px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Assigned To: </b>
                                            </td>
                                            <td>
                                                <eluc:SEPTeamMembers ID="ucDeveloperNameEdit" AppendDataBoundItems="True" runat="server"
                                                    TeamMembersList='<%#PhoenixDefectTracker.DeveloperNameList() %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDTO") %>'
                                                    CssClass="input"></eluc:SEPTeamMembers>
                                            </td>
                                            <td>
                                                <b>Assigned Date:</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAssignedDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDDATE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>Planned effort:</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPlannedEffortEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNEDEFFORT")%>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Start Date: </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStartDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>Finish Date: </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFinishDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINISHDATE") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <b>Deploy Date: </b>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucDeployDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPLOYEDDATE") %>' />
                                                <asp:TextBox ID="ucDeployTimeEdit" runat="server" CssClass="input" Width="40px" />
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedDeployTimeEdit" runat="server" AcceptAMPM="false"
                                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                    TargetControlID="ucDeployTimeEdit" UserTimeFormat="TwentyFourHour" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Actual Start Date: </b>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucActualStartDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALSTARTDATE") %>' />
                                                <asp:TextBox ID="ucActualStartTimeEdit" runat="server" CssClass="input" Width="40px" />
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedActualStartTimeEdit" runat="server" AcceptAMPM="false"
                                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                    TargetControlID="ucActualStartTimeEdit" UserTimeFormat="TwentyFourHour" />
                                            </td>
                                            <td>
                                                <b>Actual Finish Date: </b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                                <asp:TextBox ID="txtAssignedTo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDTO") %>'
                                                    CssClass="gridinput_mandatory" MaxLength="100"></asp:TextBox>
                                                <eluc:Date ID="ucActualFinishDateEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE") %>' />
                                                <asp:TextBox ID="ucActualFinishTimeEdit" runat="server" CssClass="input" Width="40px" />
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedActualFinishTimeEdit" runat="server" AcceptAMPM="false"
                                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                    TargetControlID="ucActualFinishTimeEdit" UserTimeFormat="TwentyFourHour" />
                                            </td>
                                            <td>
                                                <b>Actual effort:</b>
                                            </td>
                                            <td>
                                                <eluc:Number ID="txtActualEffort" MaxLength="5" DecimalPlace="2" Width="30px" runat="server"
                                                    CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALEFFORT")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                <b>Task </b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRemarksAdd" runat="server" CssClass="input" MaxLength="500" Width="180px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <b>Assigned To: </b>
                                            </td>
                                            <td>
                                                <eluc:SEPTeamMembers ID="ucDeveloperNameAdd" AppendDataBoundItems="True" runat="server"
                                                    TeamMembersList='<%#PhoenixDefectTracker.DeveloperNameList() %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDTO") %>'
                                                    CssClass="input"></eluc:SEPTeamMembers>
                                            </td>
                                            <td>
                                                <b>Planned effort: </b>
                                            </td>
                                            <td>
                                                <eluc:Number ID="txtPlannedEffortAdd" MaxLength="5" DecimalPlace="2" Width="30px"
                                                    runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNEDEFFORT")%>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Start Date: </b>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucStartDateFooter" runat="server" CssClass="input_mandatory" />
                                                <asp:TextBox ID="ucStartTimeAdd" runat="server" CssClass="input_mandatory" Width="40px" />
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedStartTimeAdd" runat="server" AcceptAMPM="false"
                                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                    TargetControlID="ucStartTimeAdd" UserTimeFormat="TwentyFourHour" />
                                            </td>
                                            <td>
                                                <b>Finish Date: </b>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucFinishDateFooter" runat="server" CssClass="input_mandatory" />
                                                <asp:TextBox ID="ucFinishTimeAdd" runat="server" CssClass="input_mandatory" Width="40px" />
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedFinishTimeAdd" runat="server" AcceptAMPM="false"
                                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                    TargetControlID="ucFinishTimeAdd" UserTimeFormat="TwentyFourHour" />
                                            </td>
                                            <td>
                                                <b>Deploy Date: </b>
                                            </td>
                                            <td>
                                                <eluc:Date ID="ucDeployDateAdd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPLOYEDDATE") %>' />
                                                <asp:TextBox ID="ucDeployTimeAdd" runat="server" CssClass="input" Width="40px" />
                                                <ajaxToolkit:MaskedEditExtender ID="MaskedDeployTimeAdd" runat="server" AcceptAMPM="false"
                                                    ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                    TargetControlID="ucDeployTimeAdd" UserTimeFormat="TwentyFourHour" />
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                        Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
