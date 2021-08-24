<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAppraisalActivity.aspx.cs"
    Inherits="CrewAppraisalActivity" ValidateRequest="false" %>

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
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appraisal Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

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
                return showPickList('spnCourse', 'codehelp1', '', 'Common/CommonPickListCourse.aspx?rankid=' + rank + '&vessel=' + vessel + '&empid=' + empid + '', 'yes'); return true;
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
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewAppraisalactivity" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmCrewAppraisalactivity" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <eluc:TabStrip ID="AppraisalTabs" runat="server" OnTabStripCommand="AppraisalTabs_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="CrewAppraisal" runat="server" OnTabStripCommand="CrewAppraisal_TabStripCommand" Title="Appraisal Form"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div id="divPrimarySection" runat="server">

                <telerik:RadLabel ID="lblPrimaryDetails" runat="server" Text="Primary Details" Font-Bold="true" Font-Size="Small"></telerik:RadLabel>
                <table width="100%" cellspacing="0" cellpadding="1" border="1" style="width: 99%;" rules="all">
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
                    </tr>
                    <tr>
                        <td style="border-right: hidden">
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text="MV/ MT:"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Entitytype="VSL"
                                ActiveVesselsOnly="true" CssClass="input_mandatory" AppendItemPreSea="true" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
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
                            <eluc:Occassion ID="ddlOccassion" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" ActiveYN="1" />
                            <%--<telerik:RadTextBox runat="server" ID="txtoccassion" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>--%>
                        </td>
                        <div id="divSignondate" runat="server">
                            <td style="border-right: hidden">
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="Sign On:"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtSignOnDate" runat="server" CssClass="readonly_textbox" ReadOnly="true" Enabled="false" />
                            </td>
                            <td style="border-right: hidden">
                                <telerik:RadLabel ID="lblAttachedcopy" runat="server" Text="Attached Appraisal in lieu"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkAttachedCopyYN" runat="server" />
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
                    </tr>

                </table>
            </div>
            <div id="divOtherSection" runat="server">
                <b>
                    <telerik:RadLabel ID="lblRatingsectionwillbebasedonascaleof10andPointstobereflectedonreportasfollows" runat="server" Text="Scoring in this report will be based on a scale of 10 as per below guidelines:"></telerik:RadLabel>
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
                <table width="99%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td colspan="2" style="color: Blue; font-weight: bold;">
                            <telerik:RadLabel ID="Literal25" runat="server" Text="For saving the marks please use the save button on each item."></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <telerik:RadDockZone ID="RadDockZone2" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                    <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock1" runat="server" Title="Section 1: PERSONAL PROFILE – LEADERSHIP" EnableDrag="false" EnableAnimation="true"
                        EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                        <Commands>
                            <telerik:DockExpandCollapseCommand />
                        </Commands>
                        <ContentTemplate>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td valign="top" width="50%">
                                        <telerik:RadLabel ID="lblConduct" runat="server" Text=""></telerik:RadLabel>
                                        <telerik:RadGrid ID="gvCrewProfileAppraisalConduct" Width="100%" runat="server"
                                            OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource" OnItemDataBound="gvCrewProfileAppraisalConduct_ItemDataBound" ShowFooter="true">
                                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                                AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="A. Conduct">
                                                        <HeaderStyle Width="70%" />
                                                        <ItemStyle Width="80%" Wrap="true" />
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
                                                    <telerik:GridTemplateColumn HeaderText="Max Score">
                                                        <HeaderStyle Width="12%" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblMaxscore" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMAXMARK")%>' Visible="true"></telerik:RadLabel>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblMaxscoreFOOTER" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle Width="18%" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>' Visible="false"></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>' Visible="false"></telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblgvid" runat="server" Visible="false" Text="gvCrewProfileAppraisalConduct"></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
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
                                    <td valign="top" width="50%">
                                        <telerik:RadLabel ID="lblJudgementandCommonSense" runat="server" Text=""></telerik:RadLabel>
                                        <telerik:RadGrid ID="gvCrewProfileAppraisalJudgementCommonSense" Width="100%" runat="server" ShowFooter="true"
                                            OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource" OnItemDataBound="gvCrewProfileAppraisalConduct_ItemDataBound">
                                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                                AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="C. Judgement and Common Sense">
                                                        <HeaderStyle Width="70%" />
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
                                                    <telerik:GridTemplateColumn HeaderText="Max Score">
                                                        <HeaderStyle Width="12%" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblMaxscore" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMAXMARK")%>' Visible="true"></telerik:RadLabel>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblMaxscoreFOOTER" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle Width="18%" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>' Visible="false"></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>' Visible="false"></telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblgvid" runat="server" Visible="false" Text="gvCrewProfileAppraisalJudgementCommonSense"></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
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
                                <tr>
                                    <td valign="top" width="50%">
                                        <b>
                                            <telerik:RadLabel ID="lblatt" runat="server" Text=""></telerik:RadLabel>
                                        </b>
                                        <telerik:RadGrid ID="gvCrewProfileAppraisalAttitude" Width="100%" runat="server" ShowFooter="true"
                                            OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource" OnItemDataBound="gvCrewProfileAppraisalConduct_ItemDataBound">
                                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                                AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="B. Attitude">
                                                        <HeaderStyle Width="70%" />
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
                                                    <telerik:GridTemplateColumn HeaderText="Max Score">
                                                        <HeaderStyle Width="12%" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblMaxscore" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMAXMARK")%>' Visible="true"></telerik:RadLabel>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblMaxscoreFOOTER" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle Width="18%" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>' Visible="false"></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>' Visible="false"></telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblgvid" runat="server" Visible="false" Text="gvCrewProfileAppraisalAttitude"></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
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
                                    <td valign="top" width="50%">
                                        <b>
                                            <telerik:RadLabel ID="lblLeadership" runat="server" Text=""></telerik:RadLabel>
                                        </b>
                                        <telerik:RadGrid ID="gvCrewProfileAppraisalLeadership" Width="100%" runat="server" ShowFooter="true"
                                            OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource" OnItemDataBound="gvCrewProfileAppraisalConduct_ItemDataBound">
                                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                                AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="D. Personnel Management">
                                                        <HeaderStyle Width="70%" />
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
                                                    <telerik:GridTemplateColumn HeaderText="Max Score">
                                                        <HeaderStyle Width="12%" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblMaxscore" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMAXMARK")%>' Visible="true"></telerik:RadLabel>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblMaxscoreFOOTER" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Rating">
                                                        <HeaderStyle Width="18%" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>' Visible="false"></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>' Visible="false"></telerik:RadLabel>
                                                            <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <telerik:RadLabel ID="lblgvid" runat="server" Visible="false" Text="gvCrewProfileAppraisalLeadership"></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                                        </FooterTemplate>
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
                                    <tr>
                                        <td colspan="4">
                                            <table cellspacing="0" cellpadding="1" border="1" style="width: 100%;" rules="all">
                                                <tr bgcolor="#cccccc">
                                                    <td style="border-right: hidden">
                                                        <telerik:RadLabel runat="server" ID="lblPersonalProfile" Text="Total" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                    <td align="right">
                                                        <telerik:RadLabel runat="server" ID="lblPersonalProfileTotal" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </tr>

                            </table>
                        </ContentTemplate>
                    </telerik:RadDock>
                </telerik:RadDockZone>
                <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                    <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock2" runat="server" Title="Section 2: PROFESSIONAL PERFORMANCE " EnableDrag="false" EnableAnimation="true"
                        EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                        <Commands>
                            <telerik:DockExpandCollapseCommand />
                        </Commands>
                        <ContentTemplate>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewConductAppraisal" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false"
                                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewConductAppraisal_ItemCommand" OnItemDataBound="gvCrewConductAppraisal_ItemDataBound" EnableHeaderContextMenu="true"
                                ShowFooter="true" ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvCrewConductAppraisal_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldName="FLDCATEGORYDESC" FieldAlias="Details" SortOrder="Ascending" />
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="FLDCATEGORYDESC" SortOrder="Ascending" />
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
                                    <Columns>
                                        <telerik:GridTemplateColumn Visible="false" UniqueName="Data" DataField="FLDCATEGORYDESC">
                                            <ItemStyle Wrap="False" Width="5%" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblcategoryDesc" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYDESC")%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="">
                                            <HeaderStyle Width="80%" />
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblcategory" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORY")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblAppraisalConductId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALCONDUCTID")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblEvaluationQuestionId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCONDUCTQUESTIONID")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblshortcode" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSHORTCODE")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblEvaluation" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCONDUCTQUESTION")%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Max Score">
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblMaxscore" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMAXMARK")%>' Visible="true"></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblMaxscoreFOOTER" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Rating">
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>' Visible="false"></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>' Visible="false"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadLabel ID="lblcurrentscore" Visible="false" runat="server" Text="Current Score:" Font-Bold="true"></telerik:RadLabel>
                                                <br />
                                                <eluc:Number runat="server" ID="ucRatingItem" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                <asp:LinkButton runat="server" AlternateText="View KPI score" Visible="false" ID="imgkpiscore" ToolTip="View KPI score">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                                </asp:LinkButton>
                                                <asp:LinkButton runat="server" AlternateText="Refresh score"
                                                    CommandName="REFRESH" CommandArgument="<%# Container.DataSetIndex %>" ID="imgkpiscorerefresh"
                                                    ToolTip="Refresh score" Visible="false">
                                <span class="icon"><i class="fas fa-redo"></i></span>
                                                </asp:LinkButton>
                                                <br />
                                                <telerik:RadLabel ID="lbloldscore" Visible="false" runat="server" Text="Signoff Score:" Font-Bold="true"></telerik:RadLabel>
                                                <br />
                                                <eluc:Number runat="server" ID="ucoldscore" CssClass="input_mandatory" MaxLength="2" Width="50px"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDOLDRATING") %>' />
                                                <asp:LinkButton runat="server" AlternateText="View Old KPI score" Visible="false" ID="imgoldscore" ToolTip="View Old KPI score">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                                </asp:LinkButton>


                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                            </FooterTemplate>
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="false" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="false" SaveScrollPosition="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            <table cellspacing="0" cellpadding="1" border="1" style="width: 98%;" rules="all">
                                <tr>
                                    <td>
                                        <b>
                                            <telerik:RadLabel runat="server" ID="lblpercentage" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </b>
                                    </td>

                                    <td>
                                        <b>
                                            <telerik:RadLabel runat="server" ID="lblGrandTotal" Text="Final Total Score achieved -" Font-Bold="true"></telerik:RadLabel>
                                        </b>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </telerik:RadDock>
                </telerik:RadDockZone>
                <telerik:RadDockZone ID="RadDockZone3" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                    <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock3" runat="server" Title="Section 3: TRAINING, PROMOTION, ETC." EnableDrag="false" EnableAnimation="true"
                        EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                        <Commands>
                            <telerik:DockExpandCollapseCommand />
                        </Commands>
                        <ContentTemplate>
                            <table cellspacing="0" cellpadding="1" border="1" style="width: 99%;" rules="all">
                                <tr>
                                    <td width="4%" align="center">
                                        <b>
                                            <telerik:RadLabel ID="Literal1" runat="server" Text="A."></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadLabel ID="Literal3" runat="server" Text="Any training and development recommendation to improve knowledge and/ or skills (If Yes, please include details in the below section)"></telerik:RadLabel>
                                    </td>
                                    <td width="27%">
                                        <telerik:RadCheckBox runat="server" ID="chkTrainingRequired" AutoPostBack="true" OnCheckedChanged="chkTrainingRequired_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <b>
                                            <telerik:RadLabel ID="Literal4" runat="server" Text="List of Training and Development Recommended Courses:"></telerik:RadLabel>
                                        </b>
                                        <br />
                                        <telerik:RadLabel ID="Literal2" runat="server" Text="(Guidelines for the recommendation, when any of the scores is 1 – 2: Classroom based course; 3 – 4: EPSS based course; 5 or above: Not required)"></telerik:RadLabel>
                                        <br />
                                    </td>
                                    <td><span id="spnCourse">
                                        <telerik:RadTextBox ID="txtCourseName" runat="server" Width="50%" Wrap="true">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton runat="server" ID="imgShowCourse" ToolTip="Show Course">
                                    <span class="icon"><i class="fas fas fa-list-alt"></i></span>
                                        </asp:LinkButton>

                                        <telerik:RadTextBox ID="txtCourseId" runat="server" ReadOnly="true" Width="10px">
                                        </telerik:RadTextBox>
                                    </span>
                                        <img runat="server" visible="false" id="imgShowOffshoreTraining" style="cursor: pointer; vertical-align: middle"
                                            src="<%$ PhoenixTheme:images/picklist.png %>" />

                                    </td>
                                </tr>
                                <tr>
                                    <td width="4%" align="center">
                                        <b>
                                            <telerik:RadLabel ID="Literal14" runat="server" Text=""></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadLabel ID="Literal15" runat="server" Text="Has the training done since last employment been effective?"></telerik:RadLabel>
                                    </td>
                                    <td width="27%" style="vertical-align: top">
                                        <asp:LinkButton runat="server" AlternateText="View courses done since last employment"
                                            ID="imgBtnCourseDone"
                                            ToolTip="View courses done since last employment"><span class="icon"><i class="fas fa-user-graduate"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadDropDownList ID="ddlAddTrainingYN" AutoPostBack="true" runat="server" CssClass="input_mandatory" Style="vertical-align: middle" OnTextChanged="ddlAddTrainingYN_Changed">
                                            <Items>
                                                <telerik:DropDownListItem Value="" Text="--Select--" />
                                                <telerik:DropDownListItem Value="1" Text="Yes" />
                                                <telerik:DropDownListItem Value="0" Text="No" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                        <telerik:RadTextBox ID="txtTrainingRemarks" runat="server" CssClass="input" Width="250px" Style="vertical-align: middle" TextMode="MultiLine"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="4%" align="center">
                                        <b>
                                            <telerik:RadLabel ID="Literal5" runat="server" Text="B."></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadLabel ID="Literal6" runat="server" Text="Has the Appraisee been exposed to duties and responsibilities of higher rank."></telerik:RadLabel>
                                    </td>
                                    <td width="27%">
                                        <telerik:RadCheckBox runat="server" ID="chkExposedToDuties" AutoPostBack="true" OnCheckedChanged="chkExposedToDuties_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="4%" align="center">
                                        <telerik:RadLabel ID="Literal7" runat="server" Text=""></telerik:RadLabel>
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadLabel ID="Literal8" runat="server" Text="If above is yes, do you recommend for his promotion."></telerik:RadLabel>
                                    </td>
                                    <td width="27%">
                                        <telerik:RadDropDownList ID="ddlRecommendPromotion" Enabled="false" AutoPostBack="true" runat="server" CssClass="input_mandatory">
                                            <Items>
                                                <telerik:DropDownListItem Value="" Text="--Select--" />
                                                <telerik:DropDownListItem Value="1" Text="Yes" />
                                                <telerik:DropDownListItem Value="0" Text="No" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                        <telerik:RadCheckBox runat="server" ID="chkRecommendPromotion" AutoPostBack="true" Visible="false" />
                                        <asp:ImageButton ID="imgBtnPromotionDtls" runat="server" ToolTip="View Promotion ratings"
                                            ImageUrl="<%$ PhoenixTheme:images/edit-info.png%>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="4%" align="center">
                                        <b>
                                            <telerik:RadLabel ID="Literal9" runat="server" Text="C."></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadLabel ID="Literal10" runat="server" Text="Any instance of environmental non-compliance during his tenure"></telerik:RadLabel>
                                        <br />
                                        <telerik:RadLabel ID="Literal11" runat="server" Text="If so, specify"></telerik:RadLabel>
                                        <br />
                                        <telerik:RadTextBox ID="txtEnvironment" runat="server" CssClass="input_mandatory" Width="350px"
                                            TextMode="MultiLine">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td width="27%">
                                        <telerik:RadCheckBox runat="server" ID="rbEnvironment" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="4%" align="center">
                                        <b>
                                            <telerik:RadLabel ID="Literal12" runat="server" Text="D."></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadLabel ID="Literal13" runat="server" Text="Number of visits to the doctor:"></telerik:RadLabel>
                                    </td>
                                    <td width="27%">
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
                                </tr>
                            </table>
                        </ContentTemplate>
                    </telerik:RadDock>
                </telerik:RadDockZone>
                <telerik:RadDockZone ID="RadDockZone4" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                    <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock4" runat="server" Title="Section 4: RECOMMENDATIONS & COMMENTS" EnableDrag="false" EnableAnimation="true"
                        EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                        <Commands>
                            <telerik:DockExpandCollapseCommand />
                        </Commands>
                        <ContentTemplate>
                            <table cellspacing="0" cellpadding="1" border="1" style="width: 99%;" rules="all">
                                <tr>
                                    <td width="35%" align="center">
                                        <telerik:RadCheckBox runat="server" ID="rbReemployment" AutoPostBack="true" OnCheckedChanged="Recommendations_Changed" Text="Fit for Reemployment" />

                                    </td>
                                    <td width="35%" colspan="2">
                                        <telerik:RadCheckBox runat="server" ID="rbWarningToBeGiven" AutoPostBack="true" OnCheckedChanged="Recommendations_Changed" Text="To be counselled prior next assignment" />

                                    </td>
                                    <td width="30%">
                                        <telerik:RadCheckBox runat="server" ID="rbNTBR" AutoPostBack="true" OnCheckedChanged="Recommendations_Changed" Text="Not to be Re-employed" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="35%" align="center"></td>
                                    <td width="35%" colspan="2">
                                        <telerik:RadTextBox ID="txtWarningRemarks" runat="server" CssClass="input_mandatory" Width="350px"
                                            TextMode="MultiLine">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td width="30%">
                                        <telerik:RadTextBox ID="txtNtbrRemarks" runat="server" CssClass="input_mandatory" Width="350px"
                                            TextMode="MultiLine">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <telerik:RadLabel ID="Literal22" runat="server" Text="Reason For NTBR/Warning"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadDropDownList ID="ddlCategory" runat="server">
                                        </telerik:RadDropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <telerik:RadLabel ID="lblWantToWorkAgainHOD" runat="server" Text="Would you like to work with him in Future?"></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblWantToWorkAgainCrew" runat="server" Text="Would you like to work with him in Future?"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadDropDownList ID="ddlWantToWorkAgainHOD" runat="server" CssClass="input_mandatory">
                                            <Items>
                                                <telerik:DropDownListItem Value="DUMMY" Text="--Select--" />
                                                <telerik:DropDownListItem Value="1" Text="Yes" />
                                                <telerik:DropDownListItem Value="0" Text="No" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                        <telerik:RadDropDownList ID="ddlWantToWorkAgainCrew" runat="server" CssClass="input_mandatory">
                                            <Items>
                                                <telerik:DropDownListItem Value="DUMMY" Text="--Select--" />
                                                <telerik:DropDownListItem Value="1" Text="Yes" />
                                                <telerik:DropDownListItem Value="0" Text="No" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <b>
                                            <telerik:RadLabel ID="Literal21" runat="server" Text="Comments by Appraisee, if any: "></telerik:RadLabel>
                                        </b>
                                        <br />
                                        <telerik:RadLabel ID="Literal17" runat="server" Text="(This report must be discussed with the appraisee and signed by him. If the appraisee is in disagreement with any part of his assessment, then his remarks may be reflected accordingly in the comments box below from the seafarer comment)."></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <telerik:RadTextBox runat="server" ID="txtSeafarerComment" CssClass="readonlytextbox" ReadOnly="true" Width="100%" Resize="Vertical"
                                            TextMode="MultiLine">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <b>
                                            <telerik:RadLabel ID="Literal18" runat="server" Text="Comments by Appraiser: "></telerik:RadLabel>
                                        </b>
                                        <br />
                                        <telerik:RadLabel ID="Literal19" runat="server" Text="If an Officer requires improvement in a particular work area, Assessing Officers should prepare some constructive suggestions which may include Training and development Recommendations."></telerik:RadLabel>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <telerik:RadLabel ID="txtmastername" runat="server" Text="Master: "></telerik:RadLabel>
                                        <br />
                                        <br />
                                        <telerik:RadTextBox runat="server" ID="txtMasteComment" Width="100%" Resize="Vertical" TextMode="MultiLine"></telerik:RadTextBox>
                                        <asp:HiddenField ID="hdnSeamen" runat="server" />

                                    </td>
                                    <td colspan="4">
                                        <telerik:RadLabel ID="txthodname" runat="server" Text="Head of the Department: "></telerik:RadLabel>
                                        <br />
                                        <br />
                                        <telerik:RadTextBox runat="server" ID="txtHeadOfDeptComment" Width="100%" Resize="Vertical"
                                            TextMode="MultiLine">
                                        </telerik:RadTextBox>

                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </telerik:RadDock>
                </telerik:RadDockZone>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblsuperintendentcmd" Visible="false" runat="server" Text="Superintendent Comments"></telerik:RadLabel>
                        </td>
                        <td style="width: 40%">
                            <telerik:RadTextBox runat="server" ID="txtSuperintendentComment" Visible="false" CssClass="input" Width="350px" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>
                        <td style="width: 10%">
                            <telerik:RadLabel ID="lblSeafarersComment" runat="server" Visible="false" Text="Seafarer's Comment"></telerik:RadLabel>
                        </td>
                        <td style="width: 40%"></td>
                    </tr>
                    <tr>
                        <td colspan="6" style="color: Blue; font-weight: bold;">
                            <telerik:RadLabel ID="lbl1Pleaseclicksavechangesbuttonontopofthescreenforsaveyourchanges" runat="server" Text="1. Please click ‘save changes’ button on top of the screen for save your changes."></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lbl2Kindlygettheseafarercommentsinseafarercommenttab" runat="server" Text="2. Please click &quot;Complete&quot; button to complete the appraisal."></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel ID="lbl3ThenclickFinalisebuttontocompletetheappraisal" runat="server" Text="3. Kindly save seafarer comments in &quot;seafarer comment&quot; tab to finalize the appraisal."></telerik:RadLabel>
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
