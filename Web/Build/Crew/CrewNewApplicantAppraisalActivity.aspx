<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantAppraisalActivity.aspx.cs"
    Inherits="CrewNewApplicantAppraisalActivity" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProfileEvaluation" Src="~/UserControls/UserControlProfileEvaluationItem.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConductEvaluation" Src="~/UserControls/UserControlConductEvaluationItem.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appraisal Activity</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

    <script type="text/javascript">
        function OpenChkboxPopup(obj, rank, prom, vessel, empid) {
            if (obj.checked && prom == "1" && vessel == "") {
                javascript: parent.Openpopup('codehelp1', '', '../Crew/CrewAppraisalPromotion.aspx?rankid=' + rank); return false;
            }
            else if (obj.checked && prom != "1" && vessel != "" && empid != "") {
                return showPickList('spnCourse', 'codehelp1', '', '../Common/CommonPickListCourse.aspx?rankid=' + rank + '&vessel=' + vessel + '&empid=' + empid + '', 'yes'); return true;
            }
        }
    </script>
</head>
<body>
    <form id="frmCrewAppraisalactivity" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


                <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <eluc:TabStrip ID="AppraisalTabs" runat="server" OnTabStripCommand="AppraisalTabs_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>


                <eluc:TabStrip ID="CrewAppraisal" runat="server" OnTabStripCommand="CrewAppraisal_TabStripCommand"></eluc:TabStrip>

                <div id="divPrimarySection" runat="server">
                    <h3>
                        <telerik:RadLabel ID="lblPrimaryDetails" runat="server" Text="Primary Details"></telerik:RadLabel>
                    </h3>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" EntityType="VSL" ActiveVessels="true"
                                    CssClass="input_mandatory" AppendItemPreSea="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAppraisalDate" runat="server" Text="Appraisal Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date runat="server" ID="txtdate" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblOccassionForReport" runat="server" Text="Occassion For Report"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Occassion ID="ddlOccassion" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                                <%--<telerik:RadTextBox runat="server" ID="txtoccassion" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>--%>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblAttachedcopy" runat="server" Text="Attached Appraisal in lieu"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkAttachedCopyYN" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divOtherSection" runat="server">
                    <hr />
                    <b>
                        <telerik:RadLabel ID="lblGuidanceforAppraisers" runat="server" Text="Guidance for Appraisers:"></telerik:RadLabel>
                    </b>
                    <br />
                    <span id="guidelines" runat="server"></span>
                    <br />
                    <b>
                        <telerik:RadLabel ID="lblRatingsectionwillbebasedonascaleof10andPointstobereflectedonreportasfollows" runat="server" Text="Rating section will be based on a scale of 10 and Points to be reflected on report as follows:"></telerik:RadLabel>
                    </b>
                    <table cellspacing="0" cellpadding="1" border="1" style="width: 99%;" rules="all">
                        <tr>
                            <td width="4%" align="center">
                                <telerik:RadLabel ID="lbl109" runat="server" Text="(10-9)"></telerik:RadLabel>
                            </td>
                            <td width="27%">
                                <b>
                                    <telerik:RadLabel ID="lblOutstanding" runat="server" Text="Outstanding:"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblAstandardrarelyachievedbyothers" runat="server" Text="A standard rarely achieved by others"></telerik:RadLabel>
                            </td>
                            <td width="4%" align="center" style="border-bottom: hidden">
                                <telerik:RadLabel ID="lbl87" runat="server" Text="(6-5)"></telerik:RadLabel>
                            </td>
                            <td width="27%" style="border-bottom: hidden">
                                <b>
                                    <telerik:RadLabel ID="lblVerygood" runat="server" Text="Good:"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblAstandardsubstantiallyexceedingthejobrequirement" runat="server" Text="A standard fully meeting all the requirements of the job"></telerik:RadLabel>
                            </td>
                            <td width="4%" align="center">
                                <telerik:RadLabel ID="lbl65" runat="server" Text="(4-3)"></telerik:RadLabel>
                            </td>
                            <td width="27%">
                                <b>
                                    <telerik:RadLabel ID="lblGood" runat="server" Text="Needs improvement:"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblAstandardfullymeetingalltherequirementsofthejob" runat="server" Text="A standard of limited effectiveness"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td width="6%" align="center">
                                <telerik:RadLabel ID="lbl43" runat="server" Text="(8-7)"></telerik:RadLabel>
                            </td>
                            <td width="27%">
                                <b>
                                    <telerik:RadLabel ID="lblNeedsimprovement" runat="server" Text="Very good:"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblAstandardoflimitedeffectiveness" runat="server" Text="A standard substantially exceeding the job requirement."></telerik:RadLabel>
                            </td>
                            <td width="6%" align="center">&nbsp;
                            </td>
                            <td width="27%">&nbsp;
                            </td>
                            <td width="6%" align="center">
                                <telerik:RadLabel ID="lbl21" runat="server" Text="(2-1)"></telerik:RadLabel>
                            </td>
                            <td width="27%">
                                <b>
                                    <telerik:RadLabel ID="lblNeedssignificantimprovement" runat="server" Text="Needs significant improvement:"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblThebasicrequirementsofthejobhavenotbeenmet" runat="server" Text="The basic requirements of the job have not been met"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <h3>
                        <telerik:RadLabel ID="lblSection1" runat="server" Text="Section 1"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;<telerik:RadLabel ID="lblPersonalProfile" runat="server" Text="Personal Profile"></telerik:RadLabel>
                    </h3>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td colspan="2" style="color: Blue; font-weight: bold;">
                                <telerik:RadLabel ID="lblForsavingthemarkspleaseusethesavebuttononeachitemorarrowkeys" runat="server" Text="For saving the marks please use the save button on each item or ↓ / ↑ arrow keys."></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <b>
                                    <telerik:RadLabel ID="lblConduct" runat="server" Text="Conduct"></telerik:RadLabel>
                                </b>
                                <br />
                                <div id="divGrid" style="position: relative;">
                                    <%-- <asp:GridView ID="gvCrewProfileAppraisalConduct" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                        OnRowDeleting="gvCrewProfileAppraisal_RowDeleting" OnRowEditing="gvCrewProfileAppraisal_RowEditing"
                                        Style="margin-bottom: 0px" EnableViewState="false" OnRowCommand="gvCrewProfileAppraisal_RowCommand"
                                        OnRowCancelingEdit="gvCrewProfileAppraisal_RowCancelingEdit" OnRowUpdating="gvCrewProfileAppraisal_RowUpdating"
                                        OnRowCreated="gvCrewProfileAppraisal_RowCreated">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />--%>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewProfileAppraisalConduct" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                        CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource"
                                        OnItemDataBound="gvCrewProfileAppraisalConduct_ItemDataBound"
                                        OnItemCommand="gvCrewProfileAppraisalConduct_ItemCommand"
                                        AutoGenerateColumns="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                                            <HeaderStyle Width="102px" />
                                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number runat="server" ID="ucRatingEdit" Width="100%" CssClass="input_mandatory" MaxLength="2"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                                            Action                                           
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                            ToolTip="Edit">
                                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save"
                                                            CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                            ToolTip="Save">
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                            ToolTip="Cancel">
                                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </div>
                            </td>
                            <td valign="top">
                                <b>
                                    <telerik:RadLabel ID="lblAttitude" runat="server" Text="Attitude"></telerik:RadLabel>
                                </b>
                                <br />
                                <div id="div2" style="position: relative;">
                                    <%--<asp:GridView ID="gvCrewProfileAppraisalAttitude" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                        OnRowDeleting="gvCrewProfileAppraisal_RowDeleting" OnRowEditing="gvCrewProfileAppraisal_RowEditing"
                                        Style="margin-bottom: 0px" EnableViewState="false" OnRowCommand="gvCrewProfileAppraisal_RowCommand"
                                        OnRowCancelingEdit="gvCrewProfileAppraisal_RowCancelingEdit" OnRowUpdating="gvCrewProfileAppraisal_RowUpdating"
                                        OnRowCreated="gvCrewProfileAppraisal_RowCreated">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />--%>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewProfileAppraisalAttitude" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                        CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource"
                                        OnItemDataBound="gvCrewProfileAppraisalConduct_ItemDataBound"
                                        OnItemCommand="gvCrewProfileAppraisalConduct_ItemCommand"
                                        AutoGenerateColumns="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                                            <HeaderStyle Width="102px" />
                                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number runat="server" ID="ucRatingEdit" Width="100%" CssClass="input_mandatory" MaxLength="2"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                                            Action                                           
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                            ToolTip="Edit">
                                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save"
                                                            CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                            ToolTip="Save">
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                            ToolTip="Cancel">
                                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <b>
                                    <telerik:RadLabel ID="lblLeadership" runat="server" Text="Leadership"></telerik:RadLabel>
                                </b>
                                <br />
                                <div id="div3" style="position: relative;">
                                    <%-- <asp:GridView ID="gvCrewProfileAppraisalLeadership" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                        OnRowDeleting="gvCrewProfileAppraisal_RowDeleting" OnRowEditing="gvCrewProfileAppraisal_RowEditing"
                                        Style="margin-bottom: 0px" EnableViewState="false" OnRowCommand="gvCrewProfileAppraisal_RowCommand"
                                        OnRowCancelingEdit="gvCrewProfileAppraisal_RowCancelingEdit" OnRowUpdating="gvCrewProfileAppraisal_RowUpdating"
                                        OnRowCreated="gvCrewProfileAppraisal_RowCreated">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />--%>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewProfileAppraisalLeadership" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                        CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource"
                                        OnItemDataBound="gvCrewProfileAppraisalConduct_ItemDataBound"
                                        OnItemCommand="gvCrewProfileAppraisalConduct_ItemCommand"
                                        AutoGenerateColumns="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                                            <HeaderStyle Width="102px" />
                                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number runat="server" ID="ucRatingEdit" Width="100%" CssClass="input_mandatory" MaxLength="2"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                                            Action                                           
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                            ToolTip="Edit">
                                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save"
                                                            CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                            ToolTip="Save">
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                            ToolTip="Cancel">
                                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </div>
                            </td>
                            <td valign="top">
                                <b>
                                    <telerik:RadLabel ID="lblJudgementandCommonSense" runat="server" Text="Judgement and Common Sense"></telerik:RadLabel>
                                </b>
                                <br />
                                <div id="div4" style="position: relative;">
                                    <%-- <asp:GridView ID="gvCrewProfileAppraisalJudgementCommonSense" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                        OnRowDeleting="gvCrewProfileAppraisal_RowDeleting" OnRowEditing="gvCrewProfileAppraisal_RowEditing"
                                        Style="margin-bottom: 0px" EnableViewState="false" OnRowCommand="gvCrewProfileAppraisal_RowCommand"
                                        OnRowCancelingEdit="gvCrewProfileAppraisal_RowCancelingEdit" OnRowUpdating="gvCrewProfileAppraisal_RowUpdating"
                                        OnRowCreated="gvCrewProfileAppraisal_RowCreated">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />--%>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewProfileAppraisalJudgementCommonSense" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                        CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource"
                                        OnItemDataBound="gvCrewProfileAppraisalConduct_ItemDataBound"
                                        OnItemCommand="gvCrewProfileAppraisalConduct_ItemCommand"
                                        AutoGenerateColumns="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                                            <HeaderStyle Width="102px" />
                                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number runat="server" ID="ucRatingEdit" Width="100%" CssClass="input_mandatory" MaxLength="2"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                                            Action                                           
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                            ToolTip="Edit">
                                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save"
                                                            CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                            ToolTip="Save">
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                            ToolTip="Cancel">
                                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h3>
                                    <telerik:RadLabel ID="lblSection2" runat="server" Text="Section 2"></telerik:RadLabel>
                                    &nbsp;&nbsp;&nbsp;<telerik:RadLabel ID="lblProfessionalConduct" runat="server" Text="Professional Conduct"></telerik:RadLabel>
                                </h3>
                                <div id="divConduct" style="position: relative;">
                                    <%-- <asp:GridView ID="gvCrewConductAppraisal" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="80%" CellPadding="3" EnableViewState="false" OnRowCancelingEdit="gvCrewConductAppraisal_RowCancelingEdit"
                                        OnRowCommand="gvCrewConductAppraisal_RowCommand" OnRowDataBound="gvCrewConductAppraisal_RowDataBound"
                                        OnRowDeleting="gvCrewConductAppraisal_RowDeleting" OnRowEditing="gvCrewConductAppraisal_RowEditing"
                                        OnRowUpdating="gvCrewConductAppraisal_RowUpdating" OnRowCreated="gvCrewConductAppraisal_RowCreated">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />--%>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewConductAppraisal" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                        CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewConductAppraisal_NeedDataSource"
                                        OnItemCommand="gvCrewConductAppraisal_ItemCommand"
                                        OnItemDataBound="gvCrewProfileAppraisalConduct_ItemDataBound"
                                        AutoGenerateColumns="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                                            <HeaderStyle Width="102px" />
                                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>

                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblEvaluationItem" runat="server" Text="Evaluation Item"></telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAppraisalConductId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALCONDUCTID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblEvaluationQuestionId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCONDUCTQUESTIONID")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblEvaluation" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCONDUCTQUESTION")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number runat="server" ID="ucRatingEdit" Width="100%" CssClass="input_mandatory" MaxLength="2"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                                            Action
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                            ToolTip="Edit">
                                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save"
                                                            CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                            ToolTip="Save">
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                            ToolTip="Cancel">
                                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    &nbsp;<b></b>
                    <br />
                    <hr />
                    &nbsp;<b><telerik:RadLabel ID="lblPromotion" runat="server" Text="Promotion"></telerik:RadLabel>
                    </b>
                    <br />
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblConsideringoverallappraisaldoyourecommendpromotion" runat="server" Text="Considering overall appraisal, do you recommend promotion"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <asp:CheckBox runat="server" ID="chkRecommendPromotion" AutoPostBack="true" />
                                <asp:ImageButton ID="imgBtnPromotionDtls" runat="server" ToolTip="View Promotion ratings"
                                    ImageUrl="<%$ PhoenixTheme:images/edit-info.png%>" />
                            </td>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblHastheofficerbeenexposedtodutiesandresponsibilitiesofhigherrank" runat="server" Text="Has the officer been exposed to duties and responsibilities of higher rank"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <asp:CheckBox runat="server" ID="chkExposedToDuties" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <hr />
                    &nbsp;<b><telerik:RadLabel ID="lblTraining" runat="server" Text="Training"></telerik:RadLabel>
                    </b>
                    <br />
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblAnyadditionaltrainingrecommendedtoimprovehisperformance" runat="server" Text="Any additional training recommended to improve his performance"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <asp:CheckBox runat="server" ID="chkTrainingRequired" AutoPostBack="true" OnCheckedChanged="chkTrainingRequired_CheckedChanged" />
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblIfsoinwhichfield" runat="server" Text="If so, in which field"></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <span id="spnCourse" runat="server">
                                    <telerik:RadTextBox ID="txtCourseName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="400px" Wrap="true">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" ID="imgShowCourse" Style="cursor: pointer; vertical-align: top">
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtCourseId" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="10px">
                                    </telerik:RadTextBox>
                                </span>
                                <asp:LinkButton runat="server" Visible="false" ID="imgShowOffshoreTraining" Style="cursor: pointer; vertical-align: top">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <hr />
                    &nbsp;<b><telerik:RadLabel ID="lblEnvironmentalnoncompliance" runat="server" Text="Environmental non-compliance"></telerik:RadLabel>
                    </b>
                    <br />
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblAnyinstanceofenvironmentalnoncomplianceduringhistenure" runat="server" Text="Any instance of environmental non-compliance during his tenure"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <asp:CheckBox runat="server" ID="rbEnvironment" AutoPostBack="true" />
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblIfsospecify" runat="server" Text="If so, specify"></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <telerik:RadTextBox ID="txtEnvironment" runat="server" CssClass="input_mandatory" Width="400px"
                                    TextMode="MultiLine">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    &nbsp;<b><telerik:RadLabel ID="lblRecommendations" runat="server" Text="Recommendations"></telerik:RadLabel>
                    </b>&nbsp;&nbsp;<font color="blue">
                        <telerik:RadLabel ID="lblpleaseselectanyoneoptiononly" runat="server" Text="(please select any one option only)"></telerik:RadLabel>
                    </font>
                    <br />
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblFitforReemployment" runat="server" Text="Fit for Reemployment"></telerik:RadLabel>
                            </td>
                            <td style="width: 60%" colspan="3">
                                <asp:CheckBox runat="server" ID="rbReemployment" AutoPostBack="true" OnCheckedChanged="Recommendations_Changed" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblWarningtobegivenpriornextassignment" runat="server" Text="Warning to be given prior next assignment"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <asp:CheckBox runat="server" ID="rbWarningToBeGiven" AutoPostBack="true" OnCheckedChanged="Recommendations_Changed" />
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblWarningRemarks" runat="server" Text="Warning Remarks"></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <telerik:RadTextBox ID="txtWarningRemarks" runat="server" CssClass="input_mandatory" Width="400px"
                                    TextMode="MultiLine">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblNottobeReemployed" runat="server" Text="Not to be Reemployed"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <asp:CheckBox runat="server" ID="rbNTBR" AutoPostBack="true" OnCheckedChanged="Recommendations_Changed" />
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblNTBRRemarks" runat="server" Text="NTBR Remarks"></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <telerik:RadTextBox ID="txtNtbrRemarks" runat="server" CssClass="input_mandatory" Width="400px"
                                    TextMode="MultiLine">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <hr />
                    &nbsp;<b><telerik:RadLabel ID="lblComments" runat="server" Text="Comments"></telerik:RadLabel>
                    </b>
                    <br />
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblHeadOfDept" runat="server" Text="Head Of Dept."></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <telerik:RadTextBox runat="server" ID="txtHeadOfDeptComment" CssClass="input" Width="400px"
                                    TextMode="MultiLine">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblMaster" runat="server" Text="Master"></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <telerik:RadTextBox runat="server" ID="txtMasteComment" CssClass="input" Width="400px" TextMode="MultiLine"></telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:HiddenField ID="hdnSeamen" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblSeafarersComment" runat="server" Text="Seafarer's Comment"></telerik:RadLabel>
                            </td>
                            <td colspan="3" style="width: 90%">
                                <telerik:RadTextBox runat="server" ID="txtSeafarerComment" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="965px" TextMode="MultiLine">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="color: Blue; font-weight: bold;">
                                <telerik:RadLabel ID="lbl1Pleaseclicksavechangesbuttonontopofthescreenforsaveyourchanges" runat="server" Text="1. Please click ‘save changes’ button on top of the screen for save your changes."></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lbl2Kindlygettheseafarercommentsinseafarercommenttab" runat="server" Text="2. Kindly get the seafarer comments in &quot;seafarer comment&quot; tab"></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lbl3ThenclickFinalisebuttontocompletetheappraisal" runat="server" Text="3. Then click &quot;Finalise&quot; button to complete the appraisal"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </div>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
