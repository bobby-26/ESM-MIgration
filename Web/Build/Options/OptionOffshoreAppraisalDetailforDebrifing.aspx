<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionOffshoreAppraisalDetailforDebrifing.aspx.cs" Inherits="Options_OptionOffshoreAppraisalDetailforDebrifing" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
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

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seafarer's Comment</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div5" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body style="margin: 0; padding: 0px; text-align: center;">
    <div style="margin: 0 auto; width: 1024px; text-align: left; vertical-align: middle;">
        <form id="frmCrewAppraisalactivity" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCrewApprasial">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="Div1" style="width: 1024px; top: 30px;">
                    <div class="subHeader">
                        <eluc:Title runat="server" ID="Appraisalactivity" Text="Apprasial Form" ShowMenu="false">
                        </eluc:Title>
                        <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                    <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
                    <div id="divPrimarySection" runat="server">
                       <%-- <h3>
                            <asp:Literal ID="lblPrimaryDetails" runat="server" Text="Primary Details"></asp:Literal></h3>--%>
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                            <td>
                                <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblEmployeeNumber" runat="server" Text="File Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblVesselName" runat="server" Text="Vessel"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtVesselName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                    
                                   <%-- <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Visible="false"
                                        CssClass="input" AppendItemPreSea="true" Enabled="false" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>" />--%>
                                </td>
                                <td colspan="2">
                                    <asp:Panel ID="pnlDate" GroupingText="Period of appraisal" runat="server">
                                        <asp:Literal ID="labelFrom" runat="server" Text="From"></asp:Literal>
                                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" Enabled="false" />
                                        <asp:Literal ID="lblToDate" runat="server" Text="To"></asp:Literal>
                                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" Enabled="false" />
                                    </asp:Panel>
                                    <td>
                                        <asp:Literal ID="lblAppraisalDate" runat="server" Text="Appraisal Date"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date runat="server" ID="txtdate" CssClass="input" Enabled="false" />
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblOccassionForReport" runat="server" Text="Occasion For Report"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Occassion ID="ddlOccassion" runat="server" CssClass="dropdown" AppendDataBoundItems="true"
                                        ActiveYN="1" Enabled="false" />
                                    <%--<asp:TextBox runat="server" ID="txtoccassion" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>--%>
                                </td>
                                <div id="divSignondate" runat="server">
                                    <td>
                                        <asp:Literal ID="lblSignOnDate" runat="server" Text="SignOn Date"></asp:Literal>
                                    </td>
                                    <td>
                                     <eluc:Date runat="server" ID="txtSignOnDate" CssClass="input" Enabled="false" />
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblAttachedcopy" runat="server" Text="Attached Appraisal in lieu"></asp:Literal>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkAttachedCopyYN" runat="server" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divOtherSection" runat="server">
                        <hr />
                        <b>
                            <asp:Literal ID="lblGuidanceforAppraisers" runat="server" Text="Guidance for Appraisers:"></asp:Literal></b>
                        <br />
                        <span id="guidelines" visible="false" runat="server"></span>
                        <br />
                       
                        <table cellspacing="0" cellpadding="1" border="1" style="width: 85%;" rules="all">
                            <tr>
                                <td width="27%">
                                    <b>
                                        <asp:Literal ID="lblOutstanding" runat="server" Text="Outstanding(OS):"></asp:Literal>&nbsp;</b>
                                    <asp:Literal ID="lblAstandardrarelyachievedbyothers" runat="server" Text="A standard rarely achieved by others"></asp:Literal>
                                </td>
                                <td width="6%" align="center">
                                    <asp:Literal ID="lbl109" runat="server" Text="10-9"></asp:Literal>
                                </td>
                                <td width="27%">
                                    <b>
                                        <asp:Literal ID="lblVerygood" runat="server" Text="Very good(VG):"></asp:Literal>&nbsp;</b>
                                    <asp:Literal ID="lblAstandardsubstantiallyexceedingthejobrequirement" runat="server"
                                        Text="A standard substantially exceeding the job requirement"></asp:Literal>
                                </td>
                                <td width="6%" align="center">
                                    <asp:Literal ID="lbl87" runat="server" Text="8-7"></asp:Literal>
                                </td>
                                <td width="27%">
                                    <b>
                                        <asp:Literal ID="lblGood" runat="server" Text="Good(G):"></asp:Literal>&nbsp;</b>
                                    <asp:Literal ID="lblAstandardfullymeetingalltherequirementsofthejob" runat="server"
                                        Text="A standard fully meeting all the requirements of the job"></asp:Literal>
                                </td>
                                <td width="6%" align="center">
                                    <asp:Literal ID="lbl65" runat="server" Text="6-5"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td width="27%">
                                    <b>
                                        <asp:Literal ID="lblNeedsimprovement" runat="server" Text="Needs improvement(NI):"></asp:Literal>&nbsp;</b>
                                    <asp:Literal ID="lblAstandardoflimitedeffectiveness" runat="server" Text="A standard of limited effectiveness"></asp:Literal>
                                </td>
                                <td width="6%" align="center">
                                    <asp:Literal ID="lbl43" runat="server" Text="4-3"></asp:Literal>
                                </td>
                                <td width="27%">
                                    <b>
                                        <asp:Literal ID="lblNeedssignificantimprovement" runat="server" Text="Needs significant improvement(NSI):"></asp:Literal>&nbsp;</b>
                                    <asp:Literal ID="lblThebasicrequirementsofthejobhavenotbeenmet" runat="server" Text="The basic requirements of the job have not been met"></asp:Literal>
                                </td>
                                <td width="6%" align="center">
                                    <asp:Literal ID="lbl21" runat="server" Text="2-1"></asp:Literal>
                                </td>
                                <td width="27%">
                                    &nbsp;
                                </td>
                                <td width="6%" align="center">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <hr />
                        <h3>
                            <asp:Literal ID="lblSection1PersonalProfile" runat="server" Text="Section 1 Personal Profile"></asp:Literal></h3>
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td colspan="2" style="color: Blue; font-weight: bold;">
                                    <asp:Literal ID="lblForsavingthemarkspleaseusethesavebuttononeachitemorarrowkeys"
                                        runat="server" Text="For saving the marks please use the save button on each item or ↓ / ↑ arrow keys."></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <b>
                                        <asp:Literal ID="lblConduct" runat="server" Text="Conduct"></asp:Literal></b>
                                    <br />
                                    <div id="divGrid" style="position: relative;">
                                        <asp:GridView ID="gvCrewProfileAppraisalConduct" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                            Style="margin-bottom: 0px" EnableViewState="false" OnRowCreated="gvCrewProfileAppraisalConduct_RowCreated"
                                            ShowFooter="true" Enabled="false">
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                            <RowStyle Height="10px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Evaluation Item">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></asp:Label>
                                                        <asp:Label ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></asp:Label>
                                                        <asp:Label ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></asp:Label>
                                                        <asp:Label ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                            Visible="false"></asp:Label>
                                                        <eluc:Number runat="server" ID="ucRatingItem" CssClass="input" MaxLength="2" Width="50px"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                                <td valign="top">
                                    <b>Attitude</b>
                                    <br />
                                    <div id="div2" style="position: relative;">
                                        <asp:GridView ID="gvCrewProfileAppraisalAttitude" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                            Style="margin-bottom: 0px" EnableViewState="false" OnRowCreated="gvCrewProfileAppraisalAttitude_RowCreated"
                                            ShowFooter="true" Enabled="false">
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                            <RowStyle Height="10px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Evaluation Item">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></asp:Label>
                                                        <asp:Label ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></asp:Label>
                                                        <asp:Label ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></asp:Label>
                                                        <asp:Label ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                            Visible="false"></asp:Label>
                                                        <eluc:Number runat="server" ID="ucRatingItem" CssClass="input" MaxLength="2" Width="50px"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <b>
                                        <asp:Literal ID="lblLeadership" runat="server" Text="Leadership"></asp:Literal></b>
                                    <br />
                                    <div id="div3" style="position: relative;">
                                        <asp:GridView ID="gvCrewProfileAppraisalLeadership" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                            Style="margin-bottom: 0px" EnableViewState="false" OnRowCreated="gvCrewProfileAppraisalLeadership_RowCreated"
                                            Enabled="false" ShowFooter="true">
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                            <RowStyle Height="10px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Evaluation Item">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></asp:Label>
                                                        <asp:Label ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></asp:Label>
                                                        <asp:Label ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></asp:Label>
                                                        <asp:Label ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                            Visible="false"></asp:Label>
                                                        <eluc:Number runat="server" ID="ucRatingItem" CssClass="input" MaxLength="2" Width="50px"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                                <td valign="top">
                                    <b>
                                        <asp:Literal ID="lblJudgementandCommonSense" runat="server" Text="Judgement and Common Sense"></asp:Literal></b>
                                    <br />
                                    <div id="div4" style="position: relative;">
                                        <asp:GridView ID="gvCrewProfileAppraisalJudgementCommonSense" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewProfileAppraisal_RowDataBound"
                                            Style="margin-bottom: 0px" EnableViewState="false" OnRowCreated="gvCrewProfileAppraisalJudgementCommonSense_RowCreated"
                                            ShowFooter="true" Enabled="false">
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                            <RowStyle Height="10px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Evaluation Item">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProfileQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROFILEQUESTIONID") %>'></asp:Label>
                                                        <asp:Label ID="lblAppraisalProfileId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALPROFILEID")%>'></asp:Label>
                                                        <asp:Label ID="lblProfileCategoryId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILECATEGORYID")%>'></asp:Label>
                                                        <asp:Label ID="lblevaluationitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROFILEQUESTION")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                            Visible="false"></asp:Label>
                                                        <eluc:Number runat="server" ID="ucRatingItem" CssClass="input" MaxLength="2" Width="50px"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <h3>
                                        <asp:Literal ID="lblSection2ProfessionalConduct" runat="server" Text="Section 2 Professional Conduct"></asp:Literal></h3>
                                    <div id="divConduct" style="position: relative;">
                                        <asp:GridView ID="gvCrewConductAppraisal" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="80%" CellPadding="3" EnableViewState="false" OnRowDataBound="gvCrewConductAppraisal_RowDataBound"
                                            OnRowCreated="gvCrewConductAppraisal_RowCreated" ShowFooter="true" Enabled="false">
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                            <RowStyle Height="10px" />
                                            <Columns>
                                                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                                <asp:TemplateField>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:Literal ID="lblEvaluationItem" runat="server" Text="Evaluation Item"></asp:Literal>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppraisalConductId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALCONDUCTID")%>'></asp:Label>
                                                        <asp:Label ID="lblEvaluationQuestionId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCONDUCTQUESTIONID")%>'></asp:Label>
                                                        <asp:Label ID="lblEvaluation" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCONDUCTQUESTION")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                            Visible="false"></asp:Label>
                                                        <eluc:Number runat="server" ID="ucRatingItem" CssClass="input" MaxLength="2" Width="50px"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <%--<br />
                                <asp:Label runat="server" ID="lblGrandTotal" Text="Grand Total :" Font-Bold="true"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <h3>
                                        <asp:Literal ID="lblSection3Competence" runat="server" Text="Section 3 Skill, Experience, Knowledge, Competence"></asp:Literal></h3>
                                    <div id="div6" style="position: relative;">
                                        <asp:GridView ID="gvCrewCompetenceAppraisal" runat="server" AutoGenerateColumns="False"
                                            Font-Size="11px" Width="80%" CellPadding="3" EnableViewState="false" OnRowDataBound="gvCrewCompetenceAppraisal_RowDataBound"
                                            OnRowCreated="gvCrewCompetenceAppraisal_RowCreated" ShowFooter="true" Enabled="false">
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                            <RowStyle Height="10px" />
                                            <Columns>
                                                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                                <asp:TemplateField>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:Literal ID="lblEvaluationItem" runat="server" Text="Evaluation Item"></asp:Literal>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCrewAppraisalCompetenceId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREWAPPRAISALCOMPETENCEID")%>'></asp:Label>
                                                        <asp:Label ID="lblEvaluationQuestionId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYID")%>'></asp:Label>
                                                        <asp:Label ID="lblEvaluation" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCATEGORYNAME")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rating">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRange" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANGE")%>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRating" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>'
                                                            Visible="false"></asp:Label>
                                                        <eluc:Number runat="server" ID="ucRatingItem" CssClass="input" MaxLength="2" Width="50px"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATING") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderTemplate>
                                                        <asp:Literal ID="lblMandatory" runat="server" Text="Mandatory"></asp:Literal>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkMandatory" runat="server" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblActionHeader" runat="server" Text="Identify Training Need">                                                        
                                                        </asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" AlternateText="Complete" ImageUrl='<%$ PhoenixTheme:images/showlist.png%>'
                                                            CommandName="TRAINING" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdTraining"
                                                            ToolTip="Training Need"></asp:ImageButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
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
                        &nbsp;<b><asp:Literal ID="lblPromotion" runat="server" Text="Promotion"></asp:Literal></b>
                        <br />
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 40%">
                                    <asp:Literal ID="lblConsideringoverallappraisaldoyourecommendpromotion" runat="server"
                                        Text="Considering overall appraisal, do you recommend promotion"></asp:Literal>
                                </td>
                                <td style="width: 10%">
                                    <asp:DropDownList ID="ddlRecommendPromotion" AutoPostBack="true" runat="server" CssClass="input"
                                        Enabled="false">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="2">N/A</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CheckBox runat="server" ID="chkRecommendPromotion" AutoPostBack="true" Visible="false" />
                                    <asp:ImageButton ID="imgBtnPromotionDtls" runat="server" ToolTip="View Promotion ratings"
                                        ImageUrl="<%$ PhoenixTheme:images/edit-info.png%>" />
                                </td>
                                <td style="width: 40%">
                                    <asp:Literal ID="lblHastheofficerbeenexposedtodutiesandresponsibilitiesofhigherrank"
                                        runat="server" Text="Has the officer been exposed to duties and responsibilities of higher rank"></asp:Literal>
                                </td>
                                <td style="width: 10%">
                                    <asp:CheckBox runat="server" ID="chkExposedToDuties" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <hr />
                        &nbsp;<b><asp:Literal ID="lblTraining" runat="server" Text="Training" Visible="false"></asp:Literal></b>
                        <br />
                        <%-- <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td style="width: 40%">
                                <asp:Literal ID="lblAnyadditionaltrainingrecommendedtoimprovehisperformance" runat="server" Text="Any additional training recommended to improve his performance" Visible="false"></asp:Literal>
                            </td>
                            <td style="width: 10%">
                                <asp:CheckBox runat="server" ID="chkTrainingRequired" AutoPostBack="true" OnCheckedChanged="chkTrainingRequired_CheckedChanged" Visible="false" />
                            </td>
                            <td style="width: 10%">
                                <asp:Literal ID="lblIfsoinwhichfield" runat="server" Text="If so, in which field" Visible="false"></asp:Literal>
                            </td>
                            <td style="width: 40%">
                                <span id="spnCourse" runat="server" Visible="false">
                                    <asp:TextBox ID="txtCourseName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="400px" Wrap="true" Visible="false"></asp:TextBox>
                                    <img runat="server" id="imgShowCourse" style="cursor: pointer; vertical-align: top"
                                        src="<%$ PhoenixTheme:images/picklist.png %>" />
                                    <asp:TextBox ID="txtCourseId" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"
                                        Width="10px"></asp:TextBox>
                                </span>
                                <img runat="server" visible="false" id="imgShowOffshoreTraining" style="cursor: pointer; vertical-align: top"
                                        src="<%$ PhoenixTheme:images/picklist.png %>" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <hr />--%>
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 40%">
                                    <asp:Literal ID="lblAnyinstanceofenvironmentalnoncomplianceduringhistenure" runat="server"
                                        Text="Any instance of environmental non-compliance during his tenure"></asp:Literal>
                                </td>
                                <td style="width: 10%">
                                    <%--<asp:CheckBox runat="server" ID="rbEnvironment" AutoPostBack="true" />--%>
                                    <asp:DropDownList ID="ddlEnvironment" AutoPostBack="true" runat="server" CssClass="input"
                                        Enabled="false">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 10%">
                                    <asp:Literal ID="lblIfsospecify" runat="server" Text="If so, specify"></asp:Literal>
                                </td>
                                <td style="width: 40%">
                                    <asp:TextBox ID="txtEnvironment" runat="server" CssClass="input" Width="400px" TextMode="MultiLine"
                                        Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <%--<tr>
                            <td style="width: 40%">
                                <asp:Literal ID="lblNumberofvisitstothedoctor" runat="server" Text="Number of visits to the doctor"></asp:Literal>
                            </td>
                            <td style="width: 10%" colspan="3">
                                <asp:DropDownList ID="ddlNoOfDocVisits" runat="server" CssClass="input">
                                    <asp:ListItem Text="--Select--" Value="DUMMY"></asp:ListItem>
                                    <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Above 3" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>--%>
                        </table>
                        <br />
                        <hr />
                        &nbsp;<b><asp:Literal ID="lblRecommendations" runat="server" Text="Recommendations"></asp:Literal></b>&nbsp;&nbsp;<font
                            color="blue">
                            <asp:Literal ID="lblpleaseselectanyoneoptiononly" runat="server" Text="(please select any one option only)"></asp:Literal>
                        </font>
                        <br />
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 40%">
                                    <asp:Literal ID="lblFitforReemployment" runat="server" Text="Fit for Reemployment"></asp:Literal>
                                </td>
                                <td style="width: 60%" colspan="3">
                                    <asp:CheckBox runat="server" ID="rbReemployment" AutoPostBack="true" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <asp:Literal ID="lblWarningtobegivenpriornextassignment" runat="server" Text="Warning to be given prior next assignment"></asp:Literal>
                                </td>
                                <td style="width: 10%">
                                    <asp:CheckBox runat="server" ID="rbWarningToBeGiven" AutoPostBack="true" Enabled="false" />
                                </td>
                                <td style="width: 10%">
                                    <asp:Literal ID="lblWarningRemarks" runat="server" Text="Warning Remarks"></asp:Literal>
                                </td>
                                <td style="width: 40%">
                                    <asp:TextBox ID="txtWarningRemarks" runat="server" CssClass="input" Width="400px"
                                        TextMode="MultiLine" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <asp:Literal ID="lblNottobeReemployed" runat="server" Text="Not to be Reemployed"></asp:Literal>
                                </td>
                                <td style="width: 10%">
                                    <asp:CheckBox runat="server" ID="rbNTBR" AutoPostBack="true" Enabled="false" />
                                </td>
                                <td style="width: 10%">
                                    <asp:Literal ID="lblNTBRRemarks" runat="server" Text="NTBR Remarks"></asp:Literal>
                                </td>
                                <td style="width: 40%">
                                    <asp:TextBox ID="txtNtbrRemarks" runat="server" CssClass="input" Width="400px" TextMode="MultiLine"
                                        Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                                <td style="width: 10%">
                                    <asp:Literal ID="lblCategory" runat="server" Text="Reason for Ntbr/Warning"></asp:Literal>
                                </td>
                                <td style="width: 40%">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <asp:Label ID="lblWantToWorkAgainHOD" runat="server" Text="Would you like to work with him in Future?"></asp:Label>
                                </td>
                                <td style="width: 10%">
                                    <asp:DropDownList ID="ddlWantToWorkAgainHOD" runat="server" CssClass="input" Enabled="false">
                                        <asp:ListItem Text="--Select--" Value="DUMMY"></asp:ListItem>
                                        <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <asp:Label ID="lblWantToWorkAgainCrew" runat="server" Text="Would you like to sail with this officer/crew again?"></asp:Label>
                                </td>
                                <td style="width: 10%">
                                    <asp:DropDownList ID="ddlWantToWorkAgainCrew" runat="server" CssClass="input" Enabled="false">
                                        <asp:ListItem Text="--Select--" Value="DUMMY"></asp:ListItem>
                                        <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                </td>
                            </tr>
                        </table>
                        <br />
                        <hr />
                        &nbsp;<b><asp:Literal ID="lblComments" runat="server" Text="Comments"></asp:Literal></b>
                        <br />
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 20%">
                                    <asp:Literal ID="lblHeadOfDept" runat="server" Text="Head Of Dept."></asp:Literal>
                                </td>
                                <td style="width: 40%">
                                    <asp:TextBox runat="server" ID="txtHeadOfDeptComment" CssClass="input" Width="300px"
                                        TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblMaster" runat="server" Text="Master"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtMasteComment" CssClass="input" Width="300px" TextMode="MultiLine"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                          <%--  <tr>
                                <td style="width: 20%">
                                    <asp:Literal ID="lblSeafarersComment" runat="server" Text="Seafarer's Comment"></asp:Literal>
                                </td>
                                <td colspan="3" style="width: 90%">
                                    <asp:TextBox runat="server" ID="txtSeafarerComment" CssClass="input_mandatory" Style="width: 93%"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="4" style="color: Blue; font-weight: bold;">
                                    <%--<asp:Literal ID="lbl1Pleaseclicksavechangesbuttonontopofthescreenforsaveyourchanges"
                                        runat="server" Text="1. Please click ‘save changes’ button on top of the screen for save your changes."></asp:Literal>
                                    <br />
                                    <asp:Literal ID="lbl2Kindlygettheseafarercommentsinseafarercommenttab" runat="server"
                                        Text="2. Kindly get the seafarer comments in &quot;seafarer comment&quot; tab"></asp:Literal>
                                    <br />
                                    <asp:Literal ID="lbl3ThenclickFinalisebuttontocompletetheappraisal" runat="server"
                                        Text="3. Then click &quot;Complete&quot; button to enable &quot;Seafarer Comments&quot; tab"></asp:Literal>--%>
                                    <td width="20px">
                                        <input type="hidden" id="isouterpage" name="isouterpage" />
                                    </td>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <eluc:Status runat="server" ID="ucStatus" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
