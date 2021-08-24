<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAppraisalMidtenureactivity.aspx.cs"
    Inherits="CrewAppraisalMidtenureactivity" ValidateRequest="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProfileEvaluation" Src="~/UserControls/UserControlProfileEvaluationItem.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConductEvaluation" Src="~/UserControls/UserControlConductEvaluationItem.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Apprasial Form</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .RadCheckBox {
            width: 99% !important;
        }

        .rbText {
            text-align: left;
            width: 89% !important;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewAppraisalactivity" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewAppraisalactivity" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="AppraisalTabs" runat="server" OnTabStripCommand="AppraisalTabs_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="CrewAppraisal" runat="server" OnTabStripCommand="CrewAppraisal_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%" CssClass="scrolpan">
            <div id="divPrimarySection" runat="server">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <telerik:RadLabel ID="lblPrimaryDetails" runat="server" Text="Primary Details"></telerik:RadLabel>
                <table width="99%" cellspacing="0" cellpadding="1" border="1" rules="all">
                    <tr>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblName" runat="server" Text="Appraisee Name:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtEmployeeName" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                        </td>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text="MV/ MT:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                                CssClass="input_mandatory" AppendItemPreSea="true" VesselList='<%#PhoenixRegistersVessel.ListVessel() %>' />
                        </td>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="labelFrom" runat="server" Text="Report for period from:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" />
                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                            <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblAppraisalDate" runat="server" Text="Appraisal Date:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date runat="server" ID="txtdate" CssClass="input_mandatory" />
                        </td>

                    </tr>
                    <tr>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblOccassionForReport" runat="server" Text="Occasion For Report:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Occassion ID="ddlOccassion" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" ActiveYN="1" Enabled="false" />

                        </td>

                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblNumberofvisitstothedoctor" runat="server" Text="Number of visits to the doctor:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadDropDownList ID="ddlNoOfDocVisits" runat="server" CssClass="input_mandatory">
                                <Items>
                                    <telerik:DropDownListItem Value="DUMMY" Text="--Select--" />
                                    <telerik:DropDownListItem Value="0" Text="0" />
                                    <telerik:DropDownListItem Value="1" Text="1" />
                                    <telerik:DropDownListItem Value="2" Text="2" />
                                    <telerik:DropDownListItem Value="3" Text="3" />
                                    <telerik:DropDownListItem Value="Above 3" Text="4" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                        <div id="divSignondate" runat="server">
                            <td style="border-right: hidden">
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="Sign On:"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtSignOnDate" runat="server" CssClass="readonly_textbox" ReadOnly="true" Enabled="false" />
                            </td>
                        </div>
                    </tr>
                    <tr>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="Literal20" runat="server" Text="SIMS Batch:"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtBatch" runat="server" ReadOnly="true" Enabled="false" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>

                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblAttachedcopy" runat="server" Text="Attached Appraisal in lieu"></telerik:RadLabel>
                        </td>
                        <td style="border-right: hidden">
                            <asp:CheckBox ID="chkAttachedCopyYN" runat="server" />
                        </td>
                    </tr>

                </table>
            </div>
            <div id="divOtherSection" runat="server">

                <table cellspacing="0" cellpadding="1" border="1" style="width: 99%;" rules="all">
                    <tr>
                        <td width="4%" align="center">
                            <telerik:RadLabel ID="lbl109" runat="server" Text="(EE)"></telerik:RadLabel>
                        </td>
                        <td width="27%">
                            <b>
                                <telerik:RadLabel ID="lblExceeds" runat="server" Text="Exceeds Expectations-"></telerik:RadLabel>
                                &nbsp;</b>
                            <telerik:RadLabel ID="lblEE" runat="server" Text="a standard substantially exceeding the job requirements."></telerik:RadLabel>
                        </td>
                        <td width="4%" align="center" style="border-bottom: hidden">
                            <telerik:RadLabel ID="lbl87" runat="server" Text="(ME)"></telerik:RadLabel>
                        </td>
                        <td width="27%">
                            <b>
                                <telerik:RadLabel ID="ltMeetsExpectation" runat="server" Text="Meets Expectations-"></telerik:RadLabel>
                                &nbsp;</b>
                            <telerik:RadLabel ID="ltME" runat="server" Text="a standard fully meeting all the requirements of the job."></telerik:RadLabel>
                        </td>
                        <td width="4%" align="center" style="border-bottom: hidden">
                            <telerik:RadLabel ID="lbl65" runat="server" Text="(BE)"></telerik:RadLabel>
                        </td>
                        <td width="27%">
                            <b>
                                <telerik:RadLabel ID="lblBelowExpectations" runat="server" Text="Below Expectations-"></telerik:RadLabel>
                                &nbsp;</b>
                            <telerik:RadLabel ID="lblBE" runat="server" Text="a standard of limited effectiveness and needs improvement."></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td width="6%" align="center">
                            <b>
                                <telerik:RadLabel ID="lblAction" runat="server" Text="Action"></telerik:RadLabel>
                            </b>
                        </td>
                        <td width="27%">
                            <telerik:RadLabel ID="lblEEAction" runat="server" Text="No action required."></telerik:RadLabel>
                        </td>
                        <td width="6%" align="center"></td>
                        <td width="27%">
                            <telerik:RadLabel ID="lblMEAction" runat="server" Text="No action required."></telerik:RadLabel>
                        </td>
                        <td width="6%" align="center"></td>
                        <td width="27%">
                            <telerik:RadLabel ID="lblBEAction" runat="server" Text="Select one or multiple actions in Remarks"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <br />
                <table cellspacing="0" cellpadding="1" border="1" style="width: 99%;" rules="all">
                    <tr>
                        <td width="40%">
                            <telerik:RadLabel ID="Literal1" runat="server" Text="Vessel's Operational performance during the tenure"></telerik:RadLabel>
                        </td>
                        <td width="9%" align="center"></td>
                        <td width="51%">
                            <telerik:RadLabel ID="Literal2" runat="server" Text="If this is ticked as <b>&quot;BE&quot;</b>, Mention the action required in applicable key elements in below tables."></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <table width="99%" cellpadding="1" cellspacing="1">

                    <tr>
                        <td valign="top">
                            <telerik:RadGrid ID="gvmidturn" Width="100%" runat="server"
                                OnNeedDataSource="gvmidturn_NeedDataSource" OnItemDataBound="gvmidturn_ItemDataBound" EnableViewState="false" ShowFooter="true">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="KEY ELEMENTS">
                                            <HeaderStyle Width="40%" />
                                            <ItemStyle Width="80%" Wrap="true" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblJobrole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTION") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="RATING">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>

                                                <telerik:RadLabel ID="lblAppraisalQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALQUESTIONID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblAppraisalScoreid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPRAISALSCOREID") %>'></telerik:RadLabel>
                                                <telerik:RadDropDownList ID="ddlscore" runat="server" CssClass="input_mandatory" Width="80px">
                                                    <Items>
                                                        <telerik:DropDownListItem Value="0" Text="--Select--" />
                                                        <telerik:DropDownListItem Text="EE" Value="1" />
                                                        <telerik:DropDownListItem Text="ME" Value="2" />
                                                        <telerik:DropDownListItem Text="BE" Value="3" />
                                                    </Items>
                                                </telerik:RadDropDownList>
                                                <%--AutoPostBack="true" OnTextChanged="ddlscore_Changed"--%>

                                                <telerik:RadLabel ID="ddlScoreId" runat="server" Visible="false"></telerik:RadLabel>

                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ACTION REQUIRED">
                                            <HeaderStyle Width="50%" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadCheckBoxList runat="server" ID="chkRemarks" AutoPostBack="false" Width="100%">
                                                </telerik:RadCheckBoxList>

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
                <table width="99%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblComments" runat="server" Text="Seafarer Comments"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtSeafarerComment" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"
                                Width="300px" TextMode="MultiLine">
                            </telerik:RadTextBox>
                            <asp:HiddenField ID="hdnSeamen" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblHeadOfDept" Visible="false" runat="server" Text="Head of Dept."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtHeadOfDeptComment" Visible="false" CssClass="input" Width="300px"
                                TextMode="MultiLine">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMaster" runat="server" Visible="false" Text="Master"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtMasteComment" CssClass="input" Width="300px" Visible="false" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblsuperintendentcmd" runat="server" Visible="false" Text="Superintendent Comments"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtSuperintendentComment" CssClass="input" Visible="false" Width="300px" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="5" style="color: Blue; font-weight: bold;">
                            <telerik:RadLabel ID="lbl1Pleaseclicksavechangesbuttonontopofthescreenforsaveyourchanges" runat="server" Text="1. Please click ‘save changes’ button on top of the screen for save your changes."></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lbl2Kindlygettheseafarercommentsinseafarercommenttab" runat="server" Text="2. Please click &quot;Complete&quot; button to complete the appraisal."></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lblSeafarerComment" runat="server" Text="3. Kindly save seafarer comments in &quot;seafarer comment&quot; tab to finalize the appraisal."></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:HiddenField runat="server" ID="hdnappraisalcomplatedyn" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
