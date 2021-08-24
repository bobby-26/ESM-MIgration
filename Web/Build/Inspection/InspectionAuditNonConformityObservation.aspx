<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditNonConformityObservation.aspx.cs" Inherits="InspectionAuditNonConformityObservation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc"  TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="InspectionDepartment" Src="~/UserControls/UserControlInspectionDepartment.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection Observation</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="InspectionObservationComments" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">         
        </script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionObservation" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInspectionObservation">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuInspectionObsComments" runat="server" OnTabStripCommand="MenuInspectionObsComments_TabStripCommand">
                    </eluc:TabStrip>
                </div>                
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblInspectionNC" width="100%">
                        <tr>
                            <td align="left" style="width: 20%">
                                <asp:Literal ID="lblChecklistReferenceNo" runat="server" Text="Checklist Reference Number"></asp:Literal></td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtChecklistRefNo" runat="server" CssClass="readonlytextbox" Width="80%"></asp:TextBox>
                            </td>  
                            <td align="left" style="width: 20%">
                                <asp:Literal ID="lblRiskCategory" runat="server" Text="Risk Category"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <eluc:Quick ID="ucRiskCategory" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="71" />
                            </td>                            
                        </tr>
                        <tr>
                            <td align="left" style="width: 20%">
                                <asp:Literal ID="lblObservation" runat="server" Text="Observation"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtObservation" runat="server" CssClass="input_mandatory" Height="40px"
                                    TextMode="MultiLine" Width="80%"></asp:TextBox>
                                <asp:Label ID="lblObservationid" runat="server" CssClass="input" Visible="false"></asp:Label>
                            </td> 
                            <td align="left" style="width: 20%">
                                <asp:Literal ID="lblInspectorComments" runat="server" Text="Inspector Comments"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtComments" runat="server" CssClass="input_mandatory" Height="40px"
                                    TextMode="MultiLine" Width="80%"></asp:TextBox>
                            </td>                            
                        </tr>
                        <tr>
                            <td align="left" style="width: 20%">
                               <asp:Literal ID="lblOperatorComments" runat="server" Text="Operator Comments"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtOperatorComments" runat="server" CssClass="input" Height="40px"
                                    TextMode="MultiLine" Width="80%"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Literal ID="lblMasterComments" runat="server" Text="Master Comments"></asp:Literal>
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtOwnerComments" runat="server" CssClass="input" Height="40px"
                                    TextMode="MultiLine" Width="80%"></asp:TextBox>
                            </td> 
                        </tr>
                        <tr>                                                                                
                            <td align="left" style="width: 20%">
                                <asp:Literal ID="lblOperatorCommentsSubmittedOn" runat="server" Text="Operator Comments Submitted on"></asp:Literal>
                            </td>
                            <td style="width: 30%">                                
                                <eluc:Date ID="txtOperatorCommentsDate" runat="server" DatePicker="true" CssClass="input" />
                            </td> 
                            <td width="20%">
                                <asp:Literal ID="lblStatusHeader" runat="server" Text="Status"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="txtStatus" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                            </td>                  
                        </tr>
                        <tr>
                            <td valign="baseline" style="width: 12%">
                               <asp:Literal ID="lblVerificationLevel" runat="server" Text="Verification Level"></asp:Literal>
                            </td>
                            <td style="width: 25%" valign="baseline">
                                <eluc:Hard ID="ucVerficationLevel" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    HardTypeCode="195" />
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4"><hr /></td>
                        </tr>
                        <tr>
                            <td valign="baseline" style="width: 20%">
                                <asp:Literal ID="lblVerificationBySuperintendent" runat="server" Text="Verification by Superintendent"></asp:Literal>
                            </td>
                            <td valign="baseline" style="width: 30%">
                                <span id="spnPickListSupt">
                                    <asp:TextBox ID="txtSuptName" runat="server" CssClass="input" Enabled="false"
                                        MaxLength="200" Width="35%"></asp:TextBox>
                                    <asp:TextBox ID="txtSuptDesignation" runat="server" CssClass="input" Enabled="false"
                                        MaxLength="50" Width="25%"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgShowSupt" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" />
                                    <asp:TextBox runat="server" ID="txtSupt" Text="" CssClass="input"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtSuptEmailHidden" Text="" CssClass="input" Width="10px"></asp:TextBox>
                                </span>
                            </td>
                            <td valign="baseline" style="width: 20%">
                                <asp:Literal ID="lblVerificationDate" runat="server" Text="Verification Date"></asp:Literal>
                            </td>
                            <td valign="baseline" style="width: 30%">
                                <eluc:Date runat="server" ID="txtVerificationDate" CssClass="input" DatePicker="true" />
                            </td>
                        </tr>                        
                    </table>
                    <br />
                    <b><asp:Literal ID="lblActiontobeTaken" runat="server" Text="Action to be Taken"></asp:Literal></b>
                    <br />
                    <div id="divGridPreventiveAction" style="position: relative; z-index: 2; overflow-x: scroll;
                    overflow-y: hidden; position: static">
                    <asp:GridView ID="gvPreventiveAction" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvPreventiveAction_RowCommand"
                        OnRowDataBound="gvPreventiveAction_ItemDataBound" OnRowCancelingEdit="gvPreventiveAction_RowCancelingEdit"
                        OnRowDeleting="gvPreventiveAction_RowDeleting" OnRowCreated="gvPreventiveAction_RowCreated"
                        OnRowEditing="gvPreventiveAction_RowEditing" ShowFooter="true" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true" OnSelectedIndexChanging="gvPreventiveAction_SelectedIndexChanging"
                        OnRowUpdating="gvPreventiveAction_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="200px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPreventiveActionHeader" runat="server">
                                                    Action To Be Taken
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInsPreventActid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPREVENTIVEACTIONID") %>'></asp:Label>
                                    <asp:Label ID="lblPreventiveAction" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblInsPreventActidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPREVENTIVEACTIONID") %>'></asp:Label>
                                    <asp:TextBox ID="txtPreventActEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION") %>'
                                        CssClass="gridinput_mandatory" MaxLength="500" TextMode="MultiLine" Rows="2"
                                        Width="200px"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPreventActAdd" runat="server" CssClass="gridinput_mandatory"
                                        TextMode="MultiLine" Rows="2" MaxLength="500" Width="200px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" Width="100px" />
                                <HeaderTemplate>
                                    <asp:Label runat="server" ID="lblDepartmentHeader">Department
                                    <br />
                                    (Assigned to)</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDept" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:InspectionDepartment ID="ucDeptEdit" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        OnTextChangedEvent="departmentEdit_Changed" SelectedDepartment='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENT") %>'
                                        AutoPostBack="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:InspectionDepartment ID="ucDeptAdd" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        AutoPostBack="true" OnTextChangedEvent="departmentAdd_Changed" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" Width="120px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPICHeader" runat="server">
                                                    PIC
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDPERSONNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPICEdit">
                                        <asp:TextBox ID="txtPICNameEdit" runat="server" CssClass="input" Width="100px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDPERSONNAME") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtPICDesignationEdit" runat="server" CssClass="hidden" Width="0px"></asp:TextBox>
                                        <img id="imgPICEdit" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                            style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                                        <asp:TextBox runat="server" ID="txtPICEdit" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDPERSON")%>'></asp:TextBox>
                                        <asp:TextBox runat="server" ID="txtPICEmailHiddenEdit" CssClass="hidden" Width="0px"></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnPICAdd">
                                        <asp:TextBox ID="txtPICNameAdd" runat="server" CssClass="input" Width="100px"></asp:TextBox>
                                        <asp:TextBox ID="txtPICDesignationAdd" runat="server" CssClass="hidden" Width="0px"></asp:TextBox>
                                        <img id="imgPICAdd" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                            style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                                        <asp:TextBox ID="txtPICAdd" runat="server" CssClass="hidden" MaxLength="50" Width="0px"></asp:TextBox>
                                        <asp:TextBox runat="server" ID="txtPICEmailHiddenAdd" CssClass="hidden" Width="0px"></asp:TextBox>
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="120px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblCategoryHeader" runat="server">Category</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'
                                        Width="120px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucCategoryEdit" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        Width="120px" QuickTypeCode="72" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ucCategoryAdd" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        QuickTypeCode="72" Width="120px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="120px" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSubcategroyHeader" runat="server">Subcategory</asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'
                                        Width="120px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Quick ID="ucSubCategoryEdit" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        Width="120px" QuickTypeCode="74" SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORY") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Quick ID="ucSubCategoryAdd" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        Width="120px" QuickTypeCode="74" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="90px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPATargetDateHeader" runat="server">
                                                    Target Date
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPATargetDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucPATargetDateEdit" CssClass="input" runat="server" Enabled="false"
                                        Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>' DatePicker="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Date ID="ucPATargetDateAdd" CssClass="input_mandatory" runat="server" DatePicker="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="200px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPACompletionDateHeader" runat="server">
                                                    Completion <br />Date
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPACompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'
                                        Width="80px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucPACompletionDateEdit" runat="server" CssClass="input" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'
                                        DatePicker="true" Width="80px" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                <FooterStyle Wrap="False" HorizontalAlign="Left" Width="200px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPAExtensionRequiredHeader" runat="server">
                                                    Reschedule<br /> Required
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPAExtensionRequired" runat="server" Checked="false" Enabled="false"
                                        Width="80px" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkPAExtensionRequiredEdit" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkPAExtensionRequiredEdit_checkedChanged" Width="80px" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="200px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblRescheduleReasonHeader" runat="server">
                                                    Reschedule Reason
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPAExtensionReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTENSIONREASON") %>'
                                        Width="200px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPAExtensionReasonEdit" runat="server" Width="200px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTENSIONREASON") %>'
                                        CssClass="input" TextMode="MultiLine" Enabled="false" MaxLength="500"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="100px" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Reschedule Reason" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>"
                                        CommandName="RescheduleReason" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="cmdReschedule" ToolTip="Reschedule Reason"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
