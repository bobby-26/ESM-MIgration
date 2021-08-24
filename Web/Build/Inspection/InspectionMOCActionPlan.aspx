<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCActionPlan.aspx.cs" Inherits="InspectionMOCActionPlan" MaintainScrollPositionOnPostback ="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MOCActionPlan List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="MOCActionPlanlink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
     <form id="frmMOCActionPlan" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMOCActionPlan">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Action Plan (By Proposer)"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuMOCRequest" runat="server" OnTabStripCommand="MOCRequest_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGridMOCActionPlan">
                            <asp:GridView ID="gvMOCActionPlan" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                DataKeyNames="FLDMOCActionPlanID" EnableViewState="false" Font-Size="11px" OnRowCancelingEdit="gvMOCActionPlan_RowCancelingEdit"
                                OnRowCommand="gvMOCActionPlan_RowCommand" OnRowDataBound="gvMOCActionPlan_ItemDataBound"
                                OnRowDeleting="gvMOCActionPlan_RowDeleting" OnRowEditing="gvMOCActionPlan_RowEditing"
                                onrowcreated="gvMOCActionPlan_RowCreated"                                                                                                                                               
                                OnRowUpdating="gvMOCActionPlan_RowUpdating" ShowFooter="true" ShowHeader="true"
                                Width="100%">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:ButtonField CommandName="Edit" Text="DoubleClick" Visible="false" />
                                    <asp:TemplateField HeaderText="Abbreviation">
                                        <Itemstyle wrap="true" horizontalalign="left"></itemstyle>
                                        <HeaderTemplate>
                                            Action Plan
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table cellpadding="1" cellspacing="1" width="100%">
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Actions to be taken</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblActionToBeTaken" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONTOBETAKEN") %>'></asp:Label>
                                                        <asp:Label ID="lblMOCActionPlanid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCACTIONPLANID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblMOC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                            Visible="false"></asp:Label>
                                                         <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Department</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTID") %>'
                                                            Visible="false"></asp:Label>
                                                         <asp:Label ID="lbldepartmentname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Person In Charge</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'></asp:Label>
                                                        -
                                                        <asp:Label ID="lblPICRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICRANK") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Remarks</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblremarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Documents to be uploaded as evidence</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDocuments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTATTACHMENT") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Target date</b>
                                                    </td>
                                                    <td width = "40%">
                                                        <asp:Label ID="lblTargetdate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></asp:Label>
                                                    </td>
                                                     <td width = "20%">
                                                        <b>Completion date</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCompletiondate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Reschedule Date</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblrescheduledate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRESCHEDULEDATE")) %>'></asp:Label>
                                                    </td>
                                                    <td width = "20%">
                                                        <b>Closed date</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblcloseddate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE")) %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Reschedule Remarks</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblrescheduleremarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESCHEDULEREMARKS") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Status</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <table cellpadding="1" cellspacing="1" width="100%">
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Actions to be taken</b>
                                                    </td>
                                                    <td colspan ="3">
                                                        <asp:Label ID="lblMOCActionPlanid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCACTIONPLANID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblMOCid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txtActionToBeTakenEdit" runat="server" CssClass="gridinput_mandatory"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONTOBETAKEN") %>'
                                                            Width = "47%" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Department</b>
                                                    </td>
                                                    <td colspan = "3">
                                                        <asp:Label ID="lbldepartmentid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTID") %>'
                                                            Visible="false"></asp:Label>
                                                        <eluc:Department ID="ucDepartmentedit" runat="server" CssClass="gridinput_mandatory" DepartmentList='<%#PhoenixRegistersDepartment.Listdepartment(1,null)%>' Width = "47%"
                                                        OnTextChangedEvent="ucDepartmentEdit_SelectedIndexChanged"  AutoPostBack ="true" AppendDataBoundItems="true" />
                                                    </td>
                                                </tr>
                                                <tr id ="actionplancrewedit" runat ="server">
                                                <td width = "20%">
                                                    <b>Person In Charge</b>
                                                </td>
                                                <td colspan = "3">
                                                    <span id="spnPersonInChargeactionplanEdit">
                                                        <asp:TextBox ID="txtCrewNameEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'
                                                             MaxLength="50" Width="23%" Enabled = "false"></asp:TextBox>
                                                        <asp:TextBox ID="txtCrewRankEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICRANK") %>'
                                                             MaxLength="50" Width="23%" Enabled = "false"></asp:TextBox>
                                                        <asp:ImageButton runat="server" ID="imgPersonInChargeEdit" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                            ImageAlign="Top" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                                       <asp:TextBox ID="txtCrewIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                                </td>
                                                </tr>
                                                 <tr id ="actionplanofficeedit" runat ="server">
                                                    <td width = "20%">
                                                        <b>Person In Charge</b>
                                                    </td>
                                                    <td colspan ="3">
                                                        <span id="spnOfficeEdit">
                                                            <asp:TextBox ID="txtNameEdit" runat="server" CssClass="input"
                                                                MaxLength="50" Width="23%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'></asp:TextBox>
                                                            <asp:TextBox ID="txtRankEdit" runat="server" CssClass="input"
                                                                MaxLength="50" Width="23%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICRANK") %>'></asp:TextBox>
                                                            <asp:ImageButton ID="imgOfficeEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                                Style="cursor: pointer; vertical-align: top" CommandArgument="<%# Container.DataItemIndex %>"  />
                                                            <asp:TextBox ID="txtOfficeIdEdit" runat="server" MaxLength="20" Width="0px"
                                                                CssClass="hidden"></asp:TextBox>
                                                            <asp:TextBox ID="txtOfficeEmailEdit" runat="server" MaxLength="20" Width="0px"
                                                                CssClass="hidden"></asp:TextBox>
                                                        </span>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td width = "20%">
                                                        <b>Remarks</b>
                                                    </td>
                                                    <td colspan = "3">
                                                        <asp:TextBox ID="txtremarksedit" runat="server" CssClass="gridinput" Width = "47%" TextMode="SingleLine"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Documents to be uploaded as evidence</b>
                                                    </td>
                                                    <td colspan ="3">
                                                        <asp:TextBox ID="txtDocumentsEdit" runat="server" CssClass="gridinput"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTATTACHMENT") %>' Width = "47%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Target date</b>
                                                    </td>
                                                    <td width = "40%"> 
                                                        <eluc:Date ID="txtTargetdateEdit" runat="server" CssClass="input_mandatory" DatePicker="true"
                                                            Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>' />
                                                    </td>
                                                    <td width = "20%">
                                                        <b>Completion date</b>
                                                    </td>
                                                    <td>
                                                        <eluc:Date ID="txtCompletiondateEdit" runat="server" CssClass="input_mandatory" DatePicker="true"
                                                            Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>' Enabled = "false" ReadOnly = "true"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Reschedule date</b>
                                                    </td>
                                                    <td width = "40%">
                                                       <eluc:Date ID="txtRescheduledateEdit" runat="server" CssClass="input_mandatory" DatePicker="true"
                                                            Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRESCHEDULEDATE")) %>' Enabled = "false" ReadOnly = "true"/>
                                                    </td>
                                                    <td width = "20%">
                                                        <b>Closed date</b>
                                                    </td>
                                                    <td>
                                                        <eluc:Date ID="txtcloseddateEdit" runat="server" CssClass="input_mandatory" DatePicker="true"
                                                            Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE")) %>' Enabled = "false" ReadOnly = "true"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Reschedule Remarks</b>
                                                    </td>
                                                    <td colspan = "3">
                                                          <asp:TextBox runat="server" ID="txtRescheduleremarksEdit" Width="47%" ReadOnly="true" 
                                                          CssClass="readonlytextbox" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESCHEDULEREMARKS") %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Status</b>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>
                                                        <asp:Label ID="lblcompletedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDBY") %>' Visible ="false"></asp:Label>
                                                        <asp:Label ID="lblclosedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEDBY") %>' Visible = "false"></asp:Label>
                                                        <asp:Label ID="lblverfication" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFICATIONLEVEL") %>' Visible = "false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <table cellpadding="1" cellspacing="1" width="100%">
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Actions to be taken</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtActionToBeTakenAdd" runat="server" CssClass="gridinput_mandatory" Width = "47%" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Department</b>
                                                    </td>
                                                    <td>
                                                        <eluc:Department ID="ucDepartmentAdd" runat="server" CssClass="gridinput_mandatory" DepartmentList='<%#PhoenixRegistersDepartment.Listdepartment(1,null)%>' Width = "47%"
                                                        OnTextChangedEvent="ucDepartmentAdd_SelectedIndexChanged" AutoPostBack ="true" AppendDataBoundItems="true"  />
                                                    </td>
                                                </tr>
                                                <tr id ="actionplancrew" runat ="server">
                                                <td width = "20%">
                                                    <b>Person In Charge</b>
                                                </td>
                                                <td>
                                                    <span id="spnPersonInChargeactionplan">
                                                        <asp:TextBox ID="txtCrewName" runat="server" CssClass="input" Enabled = "false"
                                                            MaxLength="50" Width="23%"></asp:TextBox>
                                                        <asp:TextBox ID="txtCrewRank" runat="server" CssClass="input" Enabled = "false"
                                                            MaxLength="50" Width="23%"></asp:TextBox>
                                                        <asp:ImageButton runat="server" ID="imgPersonInCharge" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                            ImageAlign="Top" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                                        <asp:TextBox ID="txtCrewId" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                                    </span>
                                                </td>
                                                </tr>
                                                <tr id ="actionplanoffice" runat ="server">
                                                    <td width = "20%">
                                                        <b>Person In Charge</b>
                                                    </td>
                                                    <td>
                                                        <span id="spnOffice">
                                                            <asp:TextBox ID="txtName" runat="server" CssClass="input" 
                                                                MaxLength="50" Width="23%"></asp:TextBox>
                                                            <asp:TextBox ID="txtRank" runat="server" CssClass="input"
                                                                MaxLength="50" Width="23%"></asp:TextBox>
                                                            <asp:ImageButton ID="imgOffice" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                                Style="cursor: pointer; vertical-align: top"  CommandArgument="<%# Container.DataItemIndex %>"/>
                                                            <asp:TextBox ID="txtOfficeId" runat="server" MaxLength="20" Width="0px" CssClass="hidden"></asp:TextBox>
                                                            <asp:TextBox ID="txtOfficeEmail" runat="server" MaxLength="20" Width="0px"
                                                                CssClass="hidden"></asp:TextBox>
                                                         </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Remarks</b>
                                                    </td>
                                                    <td>
                                                       <asp:TextBox ID="txtremarksadd" runat="server" CssClass="gridinput" Width = "47%" TextMode="SingleLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Documents to be uploaded as evidence</b>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDocumentsAdd" runat="server" CssClass="gridinput" Width = "47%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width = "20%">
                                                        <b>Target date</b>
                                                    </td>
                                                    <td>
                                                        <eluc:Date ID="txtTargetdateAdd" runat="server" CssClass="input_mandatory" DatePicker="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderTemplate>
                                            <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataItemIndex %>"
                                                CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>" ToolTip="Edit" />
                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                                CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                                ToolTip="Attachment"></asp:ImageButton>
                                            <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                                                CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                                CommandName="Update" ImageUrl="<%$ PhoenixTheme:images/save.png %>" ToolTip="Save" />
                                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton ID="cmdCancel" runat="server" AlternateText="Cancel" CommandArgument="<%# Container.DataItemIndex %>"
                                                CommandName="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Cancel" />
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:ImageButton ID="cmdAdd" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                                CommandName="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>" ToolTip="Add New" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvMOCActionPlan" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
