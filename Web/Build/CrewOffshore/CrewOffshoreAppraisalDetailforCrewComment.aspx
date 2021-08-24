<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreAppraisalDetailforCrewComment.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreAppraisalDetailforCrewComment" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

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
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seafarer's Comment</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>

    <form id="frmCrewAppraisalactivity" runat="server" autocomplete="off">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <div hidden="hidden">
                <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
            <eluc:TabStrip ID="AppraisalTabs" runat="server" OnTabStripCommand="AppraisalTabs_TabStripCommand"></eluc:TabStrip>

            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />

            <table width="90%" cellpadding="1" cellspacing="1">
                <tr style="text-align: left; padding-left: 30px">
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
                <tr style="text-align: left; padding-left: 30px">
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr style="text-align: left; padding-left: 30px">
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtVesselName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>

                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Visible="false"
                            AppendItemPreSea="true" Enabled="false" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />
                    </td>
                    <td colspan="2">
                        <asp:Panel ID="pnlDate" GroupingText="Period of appraisal" runat="server">
                            <telerik:RadLabel ID="labelFrom" runat="server" Text="From"></telerik:RadLabel>
                            <eluc:Date ID="txtFromDate" runat="server" Enabled="false" />
                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To"></telerik:RadLabel>
                            <eluc:Date ID="txtToDate" runat="server" Enabled="false" />
                        </asp:Panel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppraisalDate" runat="server" Text="Appraisal Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtdate" Enabled="false" />
                    </td>
                </tr>
                <tr style="text-align: left; padding-left: 30px">
                    <td>
                        <telerik:RadLabel ID="lblOccassionForReport" runat="server" Text="Occasion For Report"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Occassion ID="ddlOccassion" runat="server" CssClass="dropdown" AppendDataBoundItems="true"
                            ActiveYN="1" Enabled="false" />
                        <%--<telerik:radtextbox runat="server" ID="txtoccassion" CssClass="readonlytextbox" ReadOnly="true"></telerik:radtextbox>--%>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblSignOnDate" runat="server" Text="SignOn Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtSignOnDate" Enabled="false" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAttachedcopy" runat="server" Text="Attached Appraisal in lieu"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAttachedCopyYN" runat="server" Enabled="false" />
                    </td>
                </tr>
            </table>

            <hr />
            <b>
                <telerik:RadLabel ID="lblGuidanceforAppraisers" runat="server" Text="Guidance for Appraisers:"></telerik:RadLabel>
            </b>
            <br />
            <span id="guidelines" visible="false" runat="server"></span>
            <br />

            <table cellspacing="0" cellpadding="1" border="1" style="width: 85%;" rules="all">
                <tr>
                    <td width="27%">
                        <b>
                            <telerik:RadLabel ID="lblOutstanding" runat="server" Text="Outstanding(OS):"></telerik:RadLabel>
                            &nbsp;</b>
                        <telerik:RadLabel ID="lblAstandardrarelyachievedbyothers" runat="server" Text="A standard rarely achieved by others"></telerik:RadLabel>
                    </td>
                    <td width="6%" align="center">
                        <telerik:RadLabel ID="lbl109" runat="server" Text="10-9"></telerik:RadLabel>
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
                        <telerik:RadLabel ID="lbl87" runat="server" Text="8-7"></telerik:RadLabel>
                    </td>
                    <td width="27%">
                        <b>
                            <telerik:RadLabel ID="lblGood" runat="server" Text="Good(G):"></telerik:RadLabel>
                            &nbsp;</b>
                        <telerik:RadLabel ID="lblAstandardfullymeetingalltherequirementsofthejob" runat="server"
                            Text="A standard fully meeting all the requirements of the job">
                        </telerik:RadLabel>
                    </td>
                    <td width="6%" align="center">
                        <telerik:RadLabel ID="lbl65" runat="server" Text="6-5"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td width="27%">
                        <b>
                            <telerik:RadLabel ID="lblNeedsimprovement" runat="server" Text="Needs improvement(NI):"></telerik:RadLabel>
                            &nbsp;</b>
                        <telerik:RadLabel ID="lblAstandardoflimitedeffectiveness" runat="server" Text="A standard of limited effectiveness"></telerik:RadLabel>
                    </td>
                    <td width="6%" align="center">
                        <telerik:RadLabel ID="lbl43" runat="server" Text="4-3"></telerik:RadLabel>
                    </td>
                    <td width="27%">
                        <b>
                            <telerik:RadLabel ID="lblNeedssignificantimprovement" runat="server" Text="Needs significant improvement(NSI):"></telerik:RadLabel>
                            &nbsp;</b>
                        <telerik:RadLabel ID="lblThebasicrequirementsofthejobhavenotbeenmet" runat="server" Text="The basic requirements of the job have not been met"></telerik:RadLabel>
                    </td>
                    <td width="6%" align="center">
                        <telerik:RadLabel ID="lbl21" runat="server" Text="2-1"></telerik:RadLabel>
                    </td>
                    <td width="27%">&nbsp;
                    </td>
                    <td width="6%" align="center">&nbsp;
                    </td>
                </tr>
            </table>
            <hr />
            <h3>
                <telerik:RadLabel ID="lblSection1PersonalProfile" runat="server" Text="Section 1 Personal Profile"></telerik:RadLabel>
            </h3>
            <table width="90%" cellpadding="1" cellspacing="1">
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

                        <telerik:RadGrid ID="gvCrewProfileAppraisalConduct" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnItemCreated="gvCrewProfileAppraisalConduct_ItemCreated"
                            CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemDataBound="gvCrewProfileAppraisal_ItemDataBound"
                            CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true"
                            OnNeedDataSource="gvCrewProfileAppraisalConduct_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">

                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Auto" AlternatingItemStyle-HorizontalAlign="Left" AlternatingItemStyle-BackColor="White">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>

                                <ItemStyle HorizontalAlign="Left" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75%"></HeaderStyle>
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
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:Number runat="server" ID="ucRatingItem" MaxLength="2" Width="50px" ReadOnly="true"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>

                            </MasterTableView>
                        </telerik:RadGrid>

                    </td>
                    <td valign="top">
                        <b>Attitude</b>
                        <br />

                        <telerik:RadGrid ID="gvCrewProfileAppraisalAttitude" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnItemCreated="gvCrewProfileAppraisalAttitude_ItemCreated"
                            CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemDataBound="gvCrewProfileAppraisal_ItemDataBound"
                            CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true"
                            OnNeedDataSource="gvCrewProfileAppraisalAttitude_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">

                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Auto" AlternatingItemStyle-HorizontalAlign="Left" AlternatingItemStyle-BackColor="White">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>

                                <ItemStyle HorizontalAlign="Left" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75%"></HeaderStyle>
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
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:Number runat="server" ID="ucRatingItem" MaxLength="2" Width="50px" ReadOnly="true"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>

                            </MasterTableView>
                        </telerik:RadGrid>

                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblLeadership" runat="server" Text="Leadership"></telerik:RadLabel>
                        </b>
                        <br />

                        <telerik:RadGrid ID="gvCrewProfileAppraisalLeadership" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnItemCreated="gvCrewProfileAppraisalLeadership_ItemCreated"
                            CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemDataBound="gvCrewProfileAppraisal_ItemDataBound"
                            CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true"
                            OnNeedDataSource="gvCrewProfileAppraisalLeadership_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">

                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Auto" AlternatingItemStyle-HorizontalAlign="Left" AlternatingItemStyle-BackColor="White">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>

                                <ItemStyle HorizontalAlign="Left" />

                                <Columns>

                                    <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75%"></HeaderStyle>
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
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:Number runat="server" ID="ucRatingItem" MaxLength="2" Width="50px" ReadOnly="true"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>

                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblJudgementandCommonSense" runat="server" Text="Judgement and Common Sense"></telerik:RadLabel>
                        </b>
                        <br />

                        <telerik:RadGrid ID="gvCrewProfileAppraisalJudgementCommonSense" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnItemCreated="gvCrewProfileAppraisalJudgementCommonSense_ItemCreated"
                            CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemDataBound="gvCrewProfileAppraisal_ItemDataBound"
                            CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true"
                            OnNeedDataSource="gvCrewProfileAppraisalJudgementCommonSense_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">

                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Auto" AlternatingItemStyle-HorizontalAlign="Left" AlternatingItemStyle-BackColor="White">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>

                                <ItemStyle HorizontalAlign="Left" />

                                <Columns>

                                    <telerik:GridTemplateColumn HeaderText="Evaluation Item">
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75%"></HeaderStyle>
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
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:Number runat="server" ID="ucRatingItem" MaxLength="2" Width="50px" ReadOnly="true"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>

                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h3>
                            <telerik:RadLabel ID="lblSection2ProfessionalConduct" runat="server" Text="Section 2 Professional Conduct"></telerik:RadLabel>
                        </h3>

                        <telerik:RadGrid ID="gvCrewConductAppraisal" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnItemCreated="gvCrewConductAppraisal_ItemCreated"
                            CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" ShowFooter="true"
                            OnItemDataBound="gvCrewConductAppraisal_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            OnNeedDataSource="gvCrewConductAppraisal_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false">

                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Auto" AlternatingItemStyle-HorizontalAlign="Left" AlternatingItemStyle-BackColor="White">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>

                                <ItemStyle HorizontalAlign="Left" />

                                <Columns>

                                    <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                    <telerik:GridTemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75%"></HeaderStyle>
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
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:Number runat="server" ID="ucRatingItem" MaxLength="2" Width="50px" ReadOnly="true"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10000" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <h3>
                            <telerik:RadLabel ID="lblSection3Competence" runat="server" Text="Section 3 Skill, Experience, Knowledge, Competence"></telerik:RadLabel>
                        </h3>

                        <telerik:RadGrid ID="gvCrewCompetenceAppraisal" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnItemCreated="gvCrewCompetenceAppraisal_ItemCreated"
                            CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" ShowFooter="true"
                            OnItemDataBound="gvCrewCompetenceAppraisal_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            OnNeedDataSource="gvCrewCompetenceAppraisal_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false">

                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Auto" AlternatingItemStyle-HorizontalAlign="Left" AlternatingItemStyle-BackColor="White">
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>

                                <ItemStyle HorizontalAlign="Left" />

                                <Columns>

                                    <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />

                                    <telerik:GridTemplateColumn HeaderText="Mandatory">

                                        <ItemTemplate>
                                            <telerik:RadCheckBox ID="chkMandatory" runat="server" Enabled="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Identify Training Need">

                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Complete" CommandName="TRAINING" ID="cmdTraining" ToolTip="Training Need">
                                            <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Evaluation Item">

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

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:Number runat="server" ID="ucRatingItem" MaxLength="2" Width="50px" ReadOnly="true"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10000" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                        </telerik:RadGrid>

                        <br />
                        <telerik:RadLabel runat="server" ID="lblGrandTotal" Text="Grand Total :" Font-Bold="true"></telerik:RadLabel>
                        <br />
                        <telerik:RadLabel ID="lblNote" runat="server" Style="color: Blue">
                            Note: If Rating is 4 or less, then Identify Training Need is mandatory.
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            &nbsp;<b></b>
            <hr />
            &nbsp;<b><telerik:RadLabel ID="lblPromotion" runat="server" Text="Promotion"></telerik:RadLabel>
            </b>
            <br />
            <table width="75%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblConsideringoverallappraisaldoyourecommendpromotion" runat="server" Text="Considering overall appraisal, do you recommend promotion">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlRecommendPromotion" AutoPostBack="true" runat="server" Enabled="false">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Yes" />
                                <telerik:RadComboBoxItem Value="0" Text="No" />
                                <telerik:RadComboBoxItem Value="2" Text="N/A" />
                            </Items>
                        </telerik:RadComboBox>
                        <telerik:RadCheckBox runat="server" ID="chkRecommendPromotion" AutoPostBack="true" Visible="false" />
                        <asp:LinkButton ID="imgBtnPromotionDtls" runat="server" ToolTip="View Promotion ratings">
                              <span class="icon"><i class="fas fa-edit"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHastheofficerbeenexposedtodutiesandresponsibilitiesofhigherrank"
                            runat="server" Text="Has the officer been exposed to duties and responsibilities of higher rank">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkExposedToDuties" Enabled="false" />
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            &nbsp;<b><telerik:RadLabel ID="lblTraining" runat="server" Text="Training" Visible="false"></telerik:RadLabel>
            </b>
            <br />

            <table width="75%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAnyinstanceofenvironmentalnoncomplianceduringhistenure" runat="server"
                            Text="Any instance of environmental non-compliance during his tenure">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadCheckBox runat="server" ID="rbEnvironment" AutoPostBack="true" />--%>
                        <telerik:RadComboBox ID="ddlEnvironment" AutoPostBack="true" runat="server" Enabled="false">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Yes" />
                                <telerik:RadComboBoxItem Value="0" Text="No" />
                            </Items>
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIfsospecify" runat="server" Text="If so, specify"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEnvironment" runat="server" Width="400px" TextMode="MultiLine" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>

            </table>
            <br />
            <hr />
            &nbsp;<b><telerik:RadLabel ID="lblRecommendations" runat="server" Text="Recommendations   (please select any one option only)"></telerik:RadLabel>
            </b>

            <br />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFitforReemployment" runat="server" Text="Fit for Reemployment"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="rbReemployment" AutoPostBack="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWarningtobegivenpriornextassignment" runat="server" Text="Warning to be given prior next assignment"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="rbWarningToBeGiven" AutoPostBack="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWarningRemarks" runat="server" Text="Warning Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWarningRemarks" runat="server" Width="400px"
                            TextMode="MultiLine" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNottobeReemployed" runat="server" Text="Not to be Reemployed"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="rbNTBR" AutoPostBack="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNTBRRemarks" runat="server" Text="NTBR Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNtbrRemarks" runat="server" Width="400px" TextMode="MultiLine"
                            Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Reason for Ntbr/Warning"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" Enabled="false">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWantToWorkAgainHOD" runat="server" Text="Would you like to work with him in Future?"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlWantToWorkAgainHOD" runat="server" Enabled="false">
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
                    <td>
                        <telerik:RadLabel ID="lblWantToWorkAgainCrew" runat="server" Text="Would you like to sail with this officer/crew again?"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlWantToWorkAgainCrew" runat="server" Enabled="false">
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
                    <td>
                        <telerik:RadLabel ID="lblHeadOfDept" runat="server" Text="Head Of Dept."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtHeadOfDeptComment" Width="300px"
                            TextMode="MultiLine" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMaster" runat="server" Text="Master"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMasteComment" Width="300px" TextMode="MultiLine"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeafarersComment" runat="server" Text="Seafarer's Comment"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtSeafarerComment" CssClass="input_mandatory"  Width="95%"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="color: Blue; font-weight: bold;">

                        <input type="hidden" id="isouterpage" name="isouterpage" />

                    </td>
                </tr>
            </table>

            <eluc:Status runat="server" ID="ucStatus" />

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
