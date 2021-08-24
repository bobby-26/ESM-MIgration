<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreAppraisalActivity.aspx.cs"
    Inherits="CrewOffshoreAppraisalActivity" %>

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
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appraisal Activity</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>


        <script type="text/javascript" language="javascript">
            function OpenChkboxPopup(obj, rank, prom, vessel, empid) {
                if (obj.checked && prom == "1" && vessel == "") {
                    javascript: parent.Openpopup('codehelp1', '', '../Crew/CrewAppraisalPromotion.aspx?rankid=' + rank); return false;
                }
                else if (obj.checked && prom != "1" && vessel != "" && empid != "") {
                    return showPickList('spnCourse', 'codehelp1', '', '../Common/CommonPickListCourse.aspx?rankid=' + rank + '&vessel=' + vessel + '&empid=' + empid + '', 'yes'); return true;
                }
            }

            var SelectedRow = null;
            var SelectedRowIndex = null;
            var UpperBound = null;
            var LowerBound = null;

            function SelectRow(CurrentRow, RowIndex, CellIndex, Gvname) {

                UpperBound = document.getElementById(Gvname).rows.length - 1;
                LowerBound = 0;
                SelectedRowIndex = -1;

                if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
                    return;

                if (CellIndex == null) {
                    SelectedRow = CurrentRow;
                    SelectedRowIndex = RowIndex;
                    return;
                }

                if (SelectedRow != null) {
                    try { SelectedRow.cells[CellIndex].getElementsByTagName('INPUT')[0].focus(); } catch (e) { return true; }
                }

                if (CurrentRow != null) {
                    try { CurrentRow.cells[CellIndex].getElementsByTagName('INPUT')[0].focus(); } catch (e) { return true; }
                }

                SelectedRow = CurrentRow;
                SelectedRowIndex = RowIndex;
                //setTimeout("SelectedRow.focus();",0);

            }

            function SelectSibling(e, Gvname) {
                var eleminscroll;
                e = e ? e : window.event;
                var KeyCode = e.which ? e.which : e.keyCode;
                if (e.target) eleminscroll = e.target;
                if (e.srcElement) eleminscroll = e.srcElement;

                try {
                    if (KeyCode == 40)
                        SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1, eleminscroll.parentNode.cellIndex, Gvname);
                    else if (KeyCode == 38)
                        SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1, eleminscroll.parentNode.cellIndex, Gvname);
                }
                catch (ex) {
                    return true;
                }
                return true;
            }

        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewAppraisalactivity" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
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
                                <telerik:RadTextBox ID="txtFirstName" runat="server" Width="150px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtMiddleName" runat="server" Width="150px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtLastName" runat="server" Width="150px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee File Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" Width="150px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox runat="server" ID="txtRank" Width="150px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" Width="150px" VesselsOnly="true"
                                    CssClass="input_mandatory" AppendItemPreSea="true" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                            </td>
                            <td colspan="2">
                                <asp:Panel ID="pnlDate" GroupingText="Period of appraisal" runat="server">
                                    <telerik:RadLabel ID="labelFrom" runat="server" Text="From"></telerik:RadLabel>
                                    <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" />
                                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                                    <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                                </asp:Panel>
                                <td>
                                    <telerik:RadLabel ID="lblAppraisalDate" runat="server" Text="Appraisal Date"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date runat="server" ID="txtdate" CssClass="input_mandatory" />
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOccassionForReport" runat="server" Text="Occasion For Report"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Occassion ID="ddlOccassion" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    ActiveYN="1" />

                            </td>
                            <div id="divSignondate" runat="server">
                                <td>
                                    <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="SignOn Date"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtSignOnDate" runat="server" CssClass="readonly_textbox" ReadOnly="true" />
                                </td>
                            </div>
                        </tr>
                        <tr>
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
                        <telerik:RadLabel ID="lblRatingsectionwillbebasedonascaleof10andPointstobereflectedonreportasfollows"
                            runat="server" Text="Rating section will be based on a scale of 10 and Points to be reflected on report as follows:">
                        </telerik:RadLabel>
                    </b>
                    <table cellspacing="0" cellpadding="1" border="1" style="width: 100%;" rules="all">
                        <tr>
                            <td width="6%" align="center">
                                <telerik:RadLabel ID="lbl109" runat="server" Text="(10-9)"></telerik:RadLabel>
                            </td>
                            <td width="27%">
                                <b>
                                    <telerik:RadLabel ID="lblOutstanding" runat="server" Text="Outstanding(OS):"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblAstandardrarelyachievedbyothers" runat="server" Text="A standard rarely achieved by others"></telerik:RadLabel>
                            </td>
                            <td width="6%" align="center">
                                <telerik:RadLabel ID="lbl87" runat="server" Text="(8-7)"></telerik:RadLabel>
                            </td>
                            <td width="27%">
                                <b>
                                    <telerik:RadLabel ID="lblVerygood" runat="server" Text="Very good(VG):"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblAstandardsubstantiallyexceedingthejobrequirement" runat="server"
                                    Text="A standard substantially exceeding the job requirement">
                                </telerik:RadLabel>
                            </td>
                            <td width="6%" align="center">
                                <telerik:RadLabel ID="lbl65" runat="server" Text="(6-5)"></telerik:RadLabel>
                            </td>
                            <td width="27%">
                                <b>
                                    <telerik:RadLabel ID="lblGood" runat="server" Text="Good(G):"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblAstandardfullymeetingalltherequirementsofthejob" runat="server"
                                    Text="A standard fully meeting all the requirements of the job">
                                </telerik:RadLabel>
                            </td>

                        </tr>
                        <tr>
                            <td width="6%" align="center">
                                <telerik:RadLabel ID="lbl43" runat="server" Text="(4-3)"></telerik:RadLabel>
                            </td>
                            <td width="27%">
                                <b>
                                    <telerik:RadLabel ID="lblNeedsimprovement" runat="server" Text="Needs improvement(NI):"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblAstandardoflimitedeffectiveness" runat="server" Text="A standard of limited effectiveness"></telerik:RadLabel>
                            </td>
                            <td width="6%" align="center">
                                <telerik:RadLabel ID="lbl21" runat="server" Text="(2-0)"></telerik:RadLabel>
                            </td>
                            <td width="27%">
                                <b>
                                    <telerik:RadLabel ID="lblNeedssignificantimprovement" runat="server" Text="Needs significant improvement(NSI):"></telerik:RadLabel>
                                    &nbsp;</b>
                                <telerik:RadLabel ID="lblThebasicrequirementsofthejobhavenotbeenmet" runat="server" Text="The basic requirements of the job have not been met"></telerik:RadLabel>
                            </td>
                            <td width="6%" align="center">&nbsp;
                            </td>
                            <td width="27%">&nbsp;
                            </td>

                        </tr>
                    </table>
                    <hr />
                    <h3>
                        <telerik:RadLabel ID="lblSection1PersonalProfile" runat="server" Text="Section 1 Personal Profile"></telerik:RadLabel>
                    </h3>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td colspan="2" style="color: Blue; font-weight: bold;">
                                <telerik:RadLabel ID="lblForsavingthemarkspleaseusethesavebuttononeachitemorarrowkeys"
                                    runat="server" Text="For saving the marks please use the save button on each item or ↓ / ↑ arrow keys.">
                                </telerik:RadLabel>
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
                                    Style="margin-bottom: 0px" EnableViewState="false" OnRowCreated="gvCrewProfileAppraisalConduct_RowCreated"
                                    ShowFooter="true">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />--%>
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewProfileAppraisalConduct" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                            CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource"
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
                                                    <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                        <ItemStyle Wrap="true" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
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
                                    </telerik:RadAjaxPanel>
                                </div>
                            </td>
                            <td valign="top">
                                <b>Attitude</b>
                                <br />
                                <div id="div2" style="position: relative;">
                                    <%-- <asp:GridView ID="gvCrewProfileAppraisalAttitude" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                    Style="margin-bottom: 0px" EnableViewState="false" OnRowCreated="gvCrewProfileAppraisalAttitude_RowCreated"
                                    ShowFooter="true">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />--%>
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewProfileAppraisalAttitude" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                            CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewProfileAppraisalAttitude_NeedDataSource"
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
                                                    <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                        <ItemStyle Wrap="true" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
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
                                    </telerik:RadAjaxPanel>
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
                                    <%--<asp:GridView ID="gvCrewProfileAppraisalLeadership" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                    Style="margin-bottom: 0px" EnableViewState="false" OnRowCreated="gvCrewProfileAppraisalLeadership_RowCreated"
                                    ShowFooter="true">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />--%>
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server">
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewProfileAppraisalLeadership" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                            CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewProfileAppraisalLeadership_NeedDataSource"
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
                                                    <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                        <ItemStyle Wrap="true" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
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
                                    </telerik:RadAjaxPanel>
                                </div>
                            </td>
                            <td valign="top">
                                <b>
                                    <telerik:RadLabel ID="lblJudgementandCommonSense" runat="server" Text="Judgement and Common Sense"></telerik:RadLabel>
                                </b>
                                <br />
                                <div id="div4" style="position: relative;">
                                    <%--<asp:GridView ID="gvCrewProfileAppraisalJudgementCommonSense" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                    Style="margin-bottom: 0px" EnableViewState="false" OnRowCreated="gvCrewProfileAppraisalJudgementCommonSense_RowCreated"
                                    ShowFooter="true">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />--%>
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat="server">
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewProfileAppraisalJudgementCommonSense" runat="server" Width="100%" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                            CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewProfileAppraisalJudgementCommonSense_NeedDataSource"
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
                                                    <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                        <ItemStyle Wrap="true" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
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
                                    </telerik:RadAjaxPanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h3>
                                    <telerik:RadLabel ID="lblSection2ProfessionalConduct" runat="server" Text="Section 2 Professional Conduct"></telerik:RadLabel>
                                </h3>
                                <div id="divConduct" style="position: relative;">
                                    <%-- <asp:GridView ID="gvCrewConductAppraisal" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="3" EnableViewState="false" OnRowDataBound="gvCrewConductAppraisal_RowDataBound"
                                    OnRowCreated="gvCrewConductAppraisal_RowCreated" ShowFooter="true">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />--%>
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel5" runat="server">
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewConductAppraisal" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                            CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewConductAppraisal_NeedDataSource"
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
                                                    <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                                    <telerik:GridTemplateColumn>
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="88%"></HeaderStyle>
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lblEvaluationItem" runat="server" Text="Evaluation Item"></telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemStyle Wrap="true" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblAppraisalConductId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALCONDUCTID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblEvaluationQuestionId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCONDUCTQUESTIONID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblEvaluation" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCONDUCTQUESTION")%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></HeaderStyle>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
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
                                    </telerik:RadAjaxPanel>
                                </div>
                                <%--<br />
                                <asp:Label runat="server" ID="lblGrandTotal" Text="Grand Total :" Font-Bold="true"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h3>

                                    <telerik:RadLabel ID="lblSection3Competence" runat="server" Text="Section 3 Skill, Experience, Knowledge, Competence"></telerik:RadLabel>
                                </h3>
                                <div id="div6" style="position: relative;">
                                    <%--<asp:GridView ID="gvCrewCompetenceAppraisal" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="3" EnableViewState="false" OnRowDataBound="gvCrewCompetenceAppraisal_RowDataBound"
                                    OnRowCreated="gvCrewCompetenceAppraisal_RowCreated" ShowFooter="true">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />--%>
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel6" runat="server">
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCompetenceAppraisal" runat="server" Height="" Width="100%" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                            CellSpacing="0" GridLines="None" ShowFooter="true" OnNeedDataSource="gvCrewCompetenceAppraisal_NeedDataSource"
                                            OnItemDataBound="gvCrewCompetenceAppraisal_ItemDataBound"
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
                                                    <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                                    <telerik:GridTemplateColumn>
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80%"></HeaderStyle>
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lblEvaluationItem" runat="server" Text="Evaluation Item"></telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemStyle Wrap="true" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblCrewAppraisalCompetenceId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREWAPPRAISALCOMPETENCEID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblEvaluationQuestionId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYID")%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblEvaluation" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME")%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn>
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lblMandatory" runat="server" Text="Mandatory"></telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemStyle Wrap="true" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkMandatory" runat="server" Enabled="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn>
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Identify Training Need">
                                                            </telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" AlternateText="Complete"
                                                                CommandName="TRAINING" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdTraining"
                                                                ToolTip="Training Need">
                                                                <span class ="icon"><i class="fas fa-book"></i></span>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
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
                                    </telerik:RadAjaxPanel>
                                </div>
                                <br />
                                <asp:Label runat="server" ID="lblGrandTotal" Text="Grand Total :" Font-Bold="true"></asp:Label>
                                <br />
                                <asp:Label ID="lblNote" runat="server" Style="color: Blue">
                                     Note: If Rating is 4 or less, then Identify Training Need is mandatory.
                                </asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    &nbsp;<b></b>
                    <hr />
                    &nbsp;<b><telerik:RadLabel ID="lblPromotion" runat="server" Text="Promotion"></telerik:RadLabel>
                    </b>
                    <br />
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblConsideringoverallappraisaldoyourecommendpromotion" runat="server"
                                    Text="Considering overall appraisal, do you recommend promotion">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadComboBox ID="ddlRecommendPromotion" AutoPostBack="true" runat="server" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                        <telerik:RadComboBoxItem Value="1" Text="Yes" />
                                        <telerik:RadComboBoxItem Value="0" Text="No" />
                                        <telerik:RadComboBoxItem Value="2" Text="N/A" />
                                    </Items>

                                </telerik:RadComboBox>
                                <asp:CheckBox runat="server" ID="chkRecommendPromotion" AutoPostBack="true" Visible="false" />
                            </td>
                            <td style="width: 40%"><asp:LinkButton ID="lnkpendingcbt" runat="server" ToolTip="Pending CBT">
                                                     <span class ="icon" style="color: red;"><i  class="fas fa-book"></i></span>
                                                   </asp:LinkButton></td>
                            <td style="width: 10%"></td>
                        </tr>
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblHastheofficerbeenexposedtodutiesandresponsibilitiesofhigherrank"
                                    runat="server" Text="Has the officer been exposed to duties and responsibilities of higher rank">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <asp:CheckBox runat="server" ID="chkExposedToDuties" />
                            </td>
                            <td style="width: 40%"></td>
                            <td style="width: 10%"></td>
                        </tr>
                    </table>
                    <br />
                    <hr />
                    &nbsp;<b><telerik:RadLabel ID="lblTraining" runat="server" Text="Training" Visible="false"></telerik:RadLabel>
                    </b>
                    <br />

                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblAnyinstanceofenvironmentalnoncomplianceduringhistenure" runat="server"
                                    Text="Any instance of environmental non-compliance during his tenure">
                                </telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <%--<asp:CheckBox runat="server" ID="rbEnvironment" AutoPostBack="true" />--%>
                                <telerik:RadComboBox ID="ddlEnvironment" AutoPostBack="true" runat="server" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                    </Items>

                                </telerik:RadComboBox>
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
                    <br />
                    <hr />
                    &nbsp;<b><telerik:RadLabel ID="lblRecommendations" runat="server" Text="Recommendations"></telerik:RadLabel>
                    </b>&nbsp;&nbsp;<font
                        color="blue">
                        <telerik:RadLabel ID="lblpleaseselectanyoneoptiononly" runat="server" Text="(please select any one option only)"></telerik:RadLabel>
                    </font>
                    <br />
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblFitforReemployment" runat="server" Text="Fit for Reemployment"></telerik:RadLabel>
                            </td>
                            <td style="width: 60%" colspan="3">
                                <telerik:RadCheckBox runat="server" ID="rbReemployment" AutoPostBack="true" OnCheckedChanged="Recommendations_Changed" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%">
                                <telerik:RadLabel ID="lblWarningtobegivenpriornextassignment" runat="server" Text="Warning to be given prior next assignment"></telerik:RadLabel>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadCheckBox runat="server" ID="rbWarningToBeGiven" AutoPostBack="true" OnCheckedChanged="Recommendations_Changed" />
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
                                <telerik:RadCheckBox runat="server" ID="rbNTBR" AutoPostBack="true" OnCheckedChanged="Recommendations_Changed" />
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
                        <tr>
                            <td colspan="2"></td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblCategory" runat="server" Text="Reason for Ntbr/Warning"></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <telerik:RadComboBox ID="ddlCategory" runat="server">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%">
                                <asp:Label ID="lblWantToWorkAgainHOD" runat="server" Text="Would you like to work with him in Future?"></asp:Label>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadComboBox ID="ddlWantToWorkAgainHOD" runat="server" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                                        <telerik:RadComboBoxItem Text="YES" Value="1" />
                                        <telerik:RadComboBoxItem Text="NO" Value="0" />
                                    </Items>

                                </telerik:RadComboBox>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td style="width: 40%">
                                <asp:Label ID="lblWantToWorkAgainCrew" runat="server" Text="Would you like to sail with this officer/crew again?"></asp:Label>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadComboBox ID="ddlWantToWorkAgainCrew" runat="server" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                                        <telerik:RadComboBoxItem Text="YES" Value="1" />
                                        <telerik:RadComboBoxItem Text="NO" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td colspan="2"></td>
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
                                <telerik:RadTextBox runat="server" ID="txtHeadOfDeptComment" CssClass="input_mandatory" Width="400px"
                                    TextMode="MultiLine">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblMaster" runat="server" Text="Master"></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <telerik:RadTextBox runat="server" ID="txtMasteComment" CssClass="input_mandatory" Width="400px" TextMode="MultiLine"></telerik:RadTextBox>
                            </td>
                            <td colspan="2">
                                <asp:HiddenField ID="hdnSeamen" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblhodname" runat="server" Text="Head Of Dept. Name"></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <telerik:RadTextBox runat="server" ID="txthodname" CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
                            </td>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblmastername" runat="server" Text="Master Name"></telerik:RadLabel>
                            </td>
                            <td style="width: 40%">
                                <telerik:RadTextBox runat="server" ID="txtMastername" CssClass="readonlytextbox" Width="150px"></telerik:RadTextBox>
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
                                <telerik:RadLabel ID="lbl1Pleaseclicksavechangesbuttonontopofthescreenforsaveyourchanges"
                                    runat="server" Text="1. Please click ‘save changes’ button on top of the screen for save your changes.">
                                </telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lbl2Kindlygettheseafarercommentsinseafarercommenttab" runat="server"
                                    Text="2. Kindly get the seafarer comments in &quot;seafarer comment&quot; tab">
                                </telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="lbl3ThenclickFinalisebuttontocompletetheappraisal" runat="server"
                                    Text="3. Then click &quot;Complete&quot; button to enable &quot;Seafarer Comments&quot; tab">
                                </telerik:RadLabel>
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
