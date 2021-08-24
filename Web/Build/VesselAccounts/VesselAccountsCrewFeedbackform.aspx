<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCrewFeedbackform.aspx.cs"
    Inherits="VesselAccountsCrewFeedbackform" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title>Feedback questions</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmFeedbackQuestion" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
            runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlFeedbackQuestions">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="ucTitle" Text="Crew Joiner Feedback" ShowMenu="false"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuCrewFeedback" runat="server" OnTabStripCommand="MenuCrewFeedback_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table width="80%" cellspacing="2">
                        <tr>
                            <td>
                                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" ReadOnly="true" runat="server" CssClass="readonlytextbox"
                                    Width="200"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRank" ReadOnly="true" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblJoinedVessel" runat="server" Text="Joined Vessel"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVessel" ReadOnly="true" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblJoinedDate" runat="server" Text="Joined Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="txtSignonDate" ReadOnly="true" runat="server" CssClass="readonlytextbox" />
                            </td>
                        </tr>
                        <table cellpadding="1" cellspacing="1" width="100%" style="border-style: none; color: blue;"
                            runat="server" id="tblNote">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblNotes" runat="server" Text="Notes :"></asp:Literal>
                                    <br />
                                    <asp:Literal ID="lbl1Allquestionstobeanswered" runat="server" Text="1. All questions to be answered."></asp:Literal>
                                    <br />
                                    <asp:Literal ID="lbl2IffeedbackisNoremarksismandatory" runat="server" Text="2. If feedback is 'No' remarks is mandatory."></asp:Literal>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <div id="divGrid" style="position: relative; z-index: 0">
                                <asp:GridView ID="gvFeedback" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    GridLines="None" Width="100%" CellPadding="3" EnableViewState="False" AllowSorting="true" OnRowDataBound="gvFeedback_RowDataBound"
                                    ShowFooter="true">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <RowStyle Height="10px" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Order No.">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblFEEDBACKFORMFORJOININGFORMALITIES" runat="server" Text="FEEDBACK FORM FOR JOINING FORMALITIES"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td></b><asp:Label ID="lblQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'></asp:Label>
                                                            <asp:Label ID="lblOrderNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERNO") %>'
                                                                Font-Bold="true"></asp:Label>
                                                            <asp:Label ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONNAME") %>'
                                                                Font-Bold="true"></asp:Label>
                                                            <asp:Label ID="lblIsObjectiveTypeYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISOBJECTIVETYPEYN") %>'></asp:Label>
                                                            <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                                            <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                                            <asp:Label ID="lblSignonoffId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'></asp:Label>
                                                            <asp:Label ID="lblFeedbackId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKID") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Literal ID="lblFeedback" runat="server" Text="Feedback"></asp:Literal>
                                                            <asp:RadioButtonList ID="rblAnswermode" runat="server" RepeatDirection="Horizontal"
                                                                AutoPostBack="true" OnSelectedIndexChanged="rblAnswermode_SelectedIndexChanged">
                                                                <asp:ListItem Text="Yes" Value="1"><b>Yes</b></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"><b>No</b></asp:ListItem>
                                                                <asp:ListItem Text='NA" ' Value="2"><b>NA</b></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>
                                                                <asp:Literal ID="lblRemarksIfNo" runat="server" Text="Remarks If No"></asp:Literal></b>
                                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="input" TextMode="MultiLine"
                                                                Height="60" Width="500" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWERDESCRIPTION")%>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <eluc:Status ID="ucStatus" runat="server" Visible="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
