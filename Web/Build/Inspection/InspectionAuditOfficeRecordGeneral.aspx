<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditOfficeRecordGeneral.aspx.cs"
    Inherits="InspectionAuditOfficeRecordGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
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
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Audit/Inspection</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            } 
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersInspectionIncident" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="ucTitle" Text="Office Audit/Inspection" ShowMenu="false" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuInspectionScheduleGeneral" runat="server" OnTabStripCommand="MenuInspectionScheduleGeneral_TabStripCommand"></eluc:TabStrip>
            <div id="divFind" style="position: relative;">
                <table id="tblConfigureInspectionIncident" width="100%">
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                        </td>
                        <td class="style2" width="30%">
                            <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" MaxLength="200"
                                Width="200px" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                        <td width="20%">
                            <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadTextBox ID="txtCompany" runat="server" Width="200px" CssClass="readonlytextbox"
                                Enabled="false">
                            </telerik:RadTextBox>
                            <telerik:RadLabel ID="lblCompanyId" Visible="false" runat="server" CssClass="readonlytextbox"
                                Enabled="false">
                            </telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadTextBox ID="txtCategory" runat="server" Width="200px" CssClass="readonlytextbox"
                                Enabled="false">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtCategoryid" runat="server" Visible="false" CssClass="readonlytextbox"
                                Enabled="false">
                            </telerik:RadTextBox>
                        </td>
                        <td style="width: 20%">
                            <telerik:RadLabel ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                        </td>
                        <td style="width: 30%">
                            <telerik:RadTextBox ID="txtInspection" runat="server" Width="200px" CssClass="readonlytextbox"
                                Enabled="false">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblDateofInspection" runat="server" Text="Date of Inspection"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <eluc:Date ID="txtCompletedDate" runat="server" DatePicker="true"
                                AutoPostBack="true" OnTextChangedEvent="txtCompletedDate_Changed" />
                        </td>
                        <td width="20%">
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                HardTypeCode="146" ShortNameFilter="ASG,CMP,REV,CLD" />
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Rows="2" TextMode="MultiLine" Resize="Both"
                                Width="200px">
                            </telerik:RadTextBox>
                        </td>
                        <td width="20%">
                            <telerik:RadLabel ID="lblNILDeficiencies" runat="server" Text="NIL Deficiencies"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadCheckBox ID="chkNILDeficiencies" runat="server" AutoPostBack="true" OnCheckedChanged="chkNILDeficiencies_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <b>
                                <telerik:RadLabel ID="lblInternalAuditorInspector" runat="server" Text="Internal Auditor / Inspector"></telerik:RadLabel></b>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblNameDesignation" runat="server" Text="Name &amp; Designation"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadComboBox ID="ddlInspectorName" runat="server" AutoPostBack="true"
                                Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                        <td width="20%">
                            <telerik:RadLabel ID="lblOrganization" runat="server" Text="Organization"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadTextBox ID="txtOrganization" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="100px" Visible="false">
                            </telerik:RadTextBox>
                            <telerik:RadComboBox ID="ddlOrganization" runat="server"  Width="200px"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <b>
                                <telerik:RadLabel ID="lblExternalAuditorInspector" runat="server" Text="Extenal Auditor / Inspector"></telerik:RadLabel></b>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblNameDesignation1" runat="server" Text="Name &amp; Designation"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadTextBox ID="txtExternalInspectorName" runat="server" CssClass="input_mandatory"
                                Width="110px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtExternalInspectorDesignation" runat="server" CssClass="input"
                                Width="80px">
                            </telerik:RadTextBox>
                        </td>
                        <td width="20%">
                            <telerik:RadLabel ID="lblOrganization1" runat="server" Text="Organization"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadTextBox ID="txtExternalOrganisationName" runat="server" CssClass="input" Width="100px" Visible="false"></telerik:RadTextBox>
                            <telerik:RadComboBox ID="ddlExternalOrganizationName" runat="server" Width="200px"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%">
                            <telerik:RadLabel ID="lblAttendingSupt" runat="server" Text="Attending Supt"></telerik:RadLabel>
                        </td>
                        <td width="30%">
                            <telerik:RadComboBox ID="ddlAuditorName" runat="server" AutoPostBack="true"
                                Width="200px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCompany1" runat="server" Text="Company"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlCompany" runat="server" Width="200px"
                                EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </div>
            <%--<div id="divGrid" style="position: relative; z-index: +1">--%>
            <div div id="divGrid" style="width: 100%; overflow-x: scroll;">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvDeficiency" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" Width="100%" OnItemCommand="gvDeficiency_ItemCommand" OnItemDataBound="gvDeficiency_ItemDataBound"
                    ShowFooter="true" OnNeedDataSource="gvDeficiency_NeedDataSource" ShowHeader="true" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDDEFICIENCYID" TableLayout="Fixed">
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Ref No">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDeficiencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Chapter" HeaderStyle-Width="136px">
                                <ItemStyle HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <FooterStyle HorizontalAlign="Left" Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblChapter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblChapterId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHAPTERID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:IChapter runat="server" ID="ucChapterEdit" AppendDataBoundItems="true"
                                        Width="120px" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:IChapter runat="server" ID="ucChapterAdd" AppendDataBoundItems="true"
                                        Width="120px" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Checklist Reference">
                                <ItemStyle HorizontalAlign="Left" Width="60px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblChecklistRef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKLISTREFERENCENUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtChecklistRefEdit" CssClass="input_mandatory" runat="server" Width="98%"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKLISTREFERENCENUMBER") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtChecklistRefAdd" CssClass="input_mandatory" Width="98%" runat="server"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PSC/VIR Code">
                                <ItemStyle HorizontalAlign="Left" Width="60px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblKeyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKEYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtKeyEdit" runat="server" CssClass="input" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKEY") %>'
                                        onkeypress="return isNumberKey(event)">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtKeyAdd" runat="server" CssClass="input" Width="60px" onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Deficiency Type" HeaderStyle-Width="136px">
                                <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblType" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="ddlTypeEdit" runat="server" RepeatDirection="Horizontal" Width="98%"
                                        RepeatLayout="Flow" Enabled="false" CssClass="dropdown_mandatory" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Major NC" Value="1"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="NC" Value="2"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Observation" Value="3"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadComboBox ID="ddlTypeAdd" runat="server" RepeatDirection="Horizontal" Width="98%"
                                        RepeatLayout="Flow" CssClass="dropdown_mandatory" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                        <Items>
                                        <telerik:RadComboBoxItem Text="Major NC" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="NC" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Observation" Value="3"></telerik:RadComboBoxItem>
                                             </Items>
                                    </telerik:RadComboBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblDeficiencyCategoryHeader" runat="Server">Deficiency Category</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYCATEGORY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucNCCategoryEdit" runat="server" AppendDataBoundItems="true" Width="250px"
                                        Visible="true" CssClass="dropdown_mandatory" QuickTypeCode="47" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ucNCCategoryAdd" runat="server" AppendDataBoundItems="true" Width="250px"
                                        Visible="true" CssClass="dropdown_mandatory" QuickTypeCode="47" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="173px">
                                <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <FooterStyle HorizontalAlign="Left" Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblItem" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDITEMTEXT").ToString().Length > 120 ? (DataBinder.Eval(Container, "DataItem.FLDITEMTEXT").ToString().Substring(0, 120) + "...") : DataBinder.Eval(Container, "DataItem.FLDITEMTEXT").ToString()%>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMTEXT") %>'
                                        Width="150px" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtItemEdit" CssClass="input" runat="server" Width="150px" TextMode="MultiLine" Resize="Both"
                                        Rows="4" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMTEXT") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtItemAdd" CssClass="input" Width="150px" TextMode="MultiLine" Resize="Both"
                                        Rows="4" runat="server"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="173px">
                                <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <FooterStyle HorizontalAlign="Left" Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString().Length > 200 ? (DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString().Substring(0, 200) + "...") : DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString()%>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                        Width="300px" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtDescEdit" CssClass="input_mandatory" runat="server" Width="150px" Resize="Both"
                                        TextMode="MultiLine" Rows="4" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtDescAdd" CssClass="input_mandatory" Width="150px" TextMode="MultiLine" Resize="Both"
                                        Rows="4" runat="server"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblIssuedDateHeader" runat="server">Issued Date</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <eluc:Date ID="ucDateEdit" runat="server" DatePicker="true" CssClass="input_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="ucDateAdd" runat="server" DatePicker="true" CssClass="input_mandatory" />
                                </FooterTemplate>--%>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="RCA Due" HeaderStyle-Width="150px">
                                <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRCADate" runat="server" Width="80px" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRCATARGETDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucRCADateEdit" runat="server" Width="98%" DatePicker="true"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDTARGETDATE") %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblStatusName" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                         <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave"
                                        ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel"
                                        ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" Width="60px" />
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Add" ID="cmdAdd" CommandName="ADD" ToolTip="Add New" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                    </asp:LinkButton>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </div>
        <asp:Button ID="ucConfirm" runat="server" OKText="Yes" CancelText="No" OnClick="ucConfirm_Click" />
        <%--<asp:Button ID="ucConfirmDelete" runat="server" Text="ConfirmDelete" OnClick="btnConfirmDelete_Click" />--%>
    </form>
</body>
</html>
