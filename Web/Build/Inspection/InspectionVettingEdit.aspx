<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionVettingEdit.aspx.cs" Inherits="InspectionVettingEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITopic" Src="~/UserControls/UserControlInspectionTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Checklist" Src="~/UserControls/UserControlInspectionChecklist.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersInspectionIncident" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionIncidentEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="ucTitle" Text="Details" ShowMenu="true"></eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuInspectionGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuInspectionGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuInspectionSchedule" runat="server" OnTabStripCommand="MenuInspectionSchedule_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureInspectionIncident" width="100%">
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference Number"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:TextBox runat="server" ID="txtRefNo" CssClass="readonlytextbox" MaxLength="200"
                                    Enabled="false"></asp:TextBox>
                            </td>
                            <td width="20%">
                               <asp:Literal ID="lblLastDoneDate" runat="server" Text="Last Done Date"></asp:Literal>
                            </td>
                            <td width="30%">
                                <%--<asp:TextBox ID="txtLastDoneDate" runat="server" CssClass="input_mandatory" MaxLength="200"
                                    Width="128px"></asp:TextBox>--%>
                                <%--<eluc:Date ID="txtLastDoneDate" runat="server" CssClass="input_mandatory" />--%>
                                <eluc:Date ID="txtLastDoneDate" runat="server" Width="90px" CssClass="input" DatePicker="true" />
                                <%--<asp:TextBox ID="txtLastDoneDate" runat="server" Width="90px" CssClass="input_mandatory"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ceRecivedDate" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtLastDoneDate" PopupPosition="TopRight">
                                </ajaxToolkit:CalendarExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <eluc:Hard runat="server" ID="ucInspectionType" CssClass="dropdown_mandatory" Visible="false"
                                    AppendDataBoundItems="true" HardTypeCode="148" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                               <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Hard ID="ucInspectionCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    CssClass="dropdown_mandatory" HardTypeCode="144" OnTextChangedEvent="InspectionType_Changed" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblLastPortOfVetting" runat="server" Text="Last Port of Vetting"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Port ID="ucLastPort" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    Width="280px" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblVetting" runat="server" Text="Vetting"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:DropDownList ID="ddlInspectionShortCodeList" runat="server" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblDueDate" runat="server" Text="Due Date"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Hard ID="ucDoneType" runat="server" CssClass="input" HardTypeCode="5" Visible="false" />
                                <eluc:Date ID="txtDueDate" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                               <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    CssClass="dropdown_mandatory" VesselsOnly="true" />
                            </td>
                            <td width="20%">
                               <asp:Literal ID="lblWindowPeriod" runat="server" Text="Window Period ( in days )"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Number ID="txtWindowperiod" runat="server" CssClass="input" MaxLength="3" IsPositive="true"
                                    Width="45px"></eluc:Number>
                                <eluc:Hard ID="Hard1" Visible="false" runat="server" AppendDataBoundItems="false"
                                    CssClass="input" HardTypeCode="7" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" Rows="2" TextMode="MultiLine"
                                    Width="240px"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="146" ShortNameFilter="ASG,CMP" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr style="height: -15px" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblPlannedDate" runat="server" Text="Planned Date"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Date ID="txtDateRangeFrom" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblPortofAuditInspection" runat="server" Text="Port of Audit / Inspection"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Port ID="ucPort" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    Width="70%" />
                                &nbsp;<asp:CheckBox ID="chkatsea" runat="server" AutoPostBack="true" OnCheckedChanged="chkatsea_CheckedChanged"
                                    Text="At Sea" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblCompletionDate" runat="server" Text="Completion Date"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Date ID="txtCompletionDate" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                            <td width="20%">
                                &nbsp;
                            </td>
                            <td width="30%">
                                <asp:RadioButtonList ID="rblLocation" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="At Berth" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="At Anchorage" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td width="10%">
                               <asp:Literal ID="lblFromPort" runat="server" Text="From Port"></asp:Literal>
                            </td>
                            <td width="25%">
                                <eluc:Port ID="ucFromPort" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Width="280px" Enabled="false" />
                            </td>
                            <td width="10%">
                                <asp:Literal ID="lblToPort" runat="server" Text="To Port"></asp:Literal>
                            </td>
                            <td width="10%">
                                <eluc:Port ID="ucToPort" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                    Width="280px" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                               <asp:Literal ID="lblETA" runat="server" Text="ETA"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Date ID="txtETA" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                            <td width="20%">
                               <asp:Literal ID="lbLETD" runat="server" Text="ETD"></asp:Literal>
                            </td>
                            <td width="30%">
                                <eluc:Date ID="txtETD" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" colspan="4">
                                <asp:Panel ID="Internal" runat="server" GroupingText="Internal Auditor / Inspector"
                                    Width="100%">
                                    <table>
                                        <tr>
                                            <td width="21%">
                                               <asp:Literal ID="lblNameDesignation" runat="server" Text="Name &amp; Designation"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                &nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlInspectorName" runat="server" CssClass="input" AutoPostBack="true"
                                                    Width="280px" />
                                            </td>
                                            <td width="29%">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Literal ID="lblOrganisatin" runat="server" Text="Organization"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                <asp:TextBox ID="txtOrganization" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                     Width="90px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" colspan="4">
                                <asp:Panel ID="External" runat="server" GroupingText="External Auditor / Inspector"
                                    Width="100%">
                                    <table>
                                        <tr>
                                            <td width="21%">
                                                <asp:Literal ID="lblNameDesignation1" runat="server" Text="Name &amp; Designation"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                &nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlExternalInspectorName" runat="server" CssClass="input" AutoPostBack="true"
                                                    OnTextChanged="ExtrenalInspector" Width="280px" />
                                            </td>
                                            <td width="29%">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:literal ID="lblOrganization" runat="server" Text="Organization"></asp:literal>
                                            </td>
                                            <td width="30%">
                                                <asp:TextBox ID="txtExternalOrganisationName" runat="server" CssClass="readonlytextbox"
                                                    ReadOnly="true" Width="90px"></asp:TextBox>
                                                <asp:Label ID="lblExternalOrganisationId" runat="server" Visible="false" />
                                                <asp:DropDownList ID="ddlExternalOrganisation" runat="server" CssClass="input" AutoPostBack="true"
                                                    Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="18%">
                                                <asp:Literal ID="lblInternalAuditor" runat="server" Text="Internal Auditor"></asp:Literal>
                                            </td>
                                            <td width="30%">
                                                &nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlAuditorName" runat="server" CssClass="input" AutoPostBack="true"
                                                    Width="280px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
                <div div id="divGrid" style="width: 100%; overflow-x: scroll;">
                    <asp:GridView ID="gvDeficiency" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvDeficiency_RowCommand" OnRowDataBound="gvDeficiency_ItemDataBound"
                        OnRowDeleting="gvDeficiency_RowDeleting" OnRowCreated="gvDeficiency_RowCreated"
                        OnRowEditing="gvDeficiency_RowEditing" OnRowCancelingEdit="gvDeficiency_RowCancelingEdit"
                        ShowFooter="true" OnRowUpdating="gvDeficiency_RowUpdating" OnSelectedIndexChanging="gvDeficiency_SelectedIndexChanging"
                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDDEFICIENCYID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="140px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblReferenceNoHeader" runat="server">Reference Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDeficiencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYID") %>'></asp:Label>
                                    <asp:Label ID="lblScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULEID") %>'></asp:Label>
                                    <asp:Label ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblChapterHeader" runat="server">Chapter</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblChapter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERNAME") %>'></asp:Label>
                                    <asp:Label ID="lblChapterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:IChapter runat="server" ID="ucChapterEdit" CssClass="input" AppendDataBoundItems="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:IChapter runat="server" ID="ucChapterAdd" CssClass="input" AppendDataBoundItems="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDeficiencyTypeHeader" runat="server">Deficiency Type</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYTYPE") %>'></asp:Label>
                                    <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlTypeEdit" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                        Enabled="false" CssClass="dropdown_mandatory">
                                        <asp:ListItem Text="Major NC" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NC" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Observation" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTypeAdd" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                        CssClass="dropdown_mandatory">
                                        <asp:ListItem Text="Major NC" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NC" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Observation" Value="3" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDeficiencyCategory" runat="server" >Deficiency Category</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYCATEGORY") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucNCCategoryEdit" runat="server" AppendDataBoundItems="true" Width="250px"
                                        CssClass="dropdown_mandatory" QuickTypeCode="47" Visible="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ucNCCategoryAdd" runat="server" AppendDataBoundItems="true" Width="250px"
                                        CssClass="dropdown_mandatory" QuickTypeCode="47" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblIssudDateHeader" runat="server" > Issued Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDISSUEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucDateEdit" runat="server" DatePicker="true" CssClass="input_mandatory"
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDISSUEDDATE")) %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="ucDateAdd" runat="server" DatePicker="true" CssClass="input_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblCheckListHeader" runat="server"> Checklist<br />Reference<br /> Number
                                    
                                    
                                    
                                   </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblChecklistRef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKLISTREFERENCENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtChecklistRefEdit" CssClass="input_mandatory" runat="server" Width="90px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKLISTREFERENCENUMBER") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtChecklistRefAdd" CssClass="input_mandatory" Width="90px" runat="server"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDescriptionHeader" runat="server">Description</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescEdit" CssClass="input_mandatory" runat="server" Width="150px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtDescAdd" CssClass="input_mandatory" Width="150px" runat="server"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblStatusHeader" runat="server">Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblStatusEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></asp:Label>
                                    <eluc:Hard ID="ucStatusEdit" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        Visible="false" HardTypeCode="146" ShortNameFilter="OPN,CLD,CMP" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                     Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle" style="z-index: -1000;">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
