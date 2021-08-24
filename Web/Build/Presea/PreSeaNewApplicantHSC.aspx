<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantHSC.aspx.cs"
    Inherits="PreSeaNewApplicantHSC" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Sex" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ECNR" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Relation" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="../UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaQualificaiton" Src="~/UserControls/UserControlPreSeaQualification.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Institution" Src="~/UserControls/UserControlPreSeaMultiColAddress.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea NewApplicant Academic Detail</title>
  <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaNewApplicantAcademicDetail" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaAcademicDetail">
        <ContentTemplate>
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div>
                    <table cellpadding="2" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                Qualification
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlQualification" runat="server" DataTextField="FLDQUALIFICATION"
                                    CssClass="input_mandatory" DataValueField="FLDQUALIFICATIONID" AutoPostBack="true"
                                    OnSelectedIndexChanged="Qualification_Changed">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Board
                            </td>
                            <td>
                                <eluc:Quick ID="ucAcademicBoard" runat="server" CssClass="dropdown_mandatory" QuickTypeCode="101"
                                    AppendDataBoundItems="true" Width="300" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td>
                                            State
                                        </td>
                                        <td>
                                            <eluc:State ID="ucState" CssClass="input_mandatory" runat="server" AppendDataBoundItems="true"
                                                AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" />
                                        </td>
                                        <td>
                                            City
                                        </td>
                                        <td>
                                            <eluc:City ID="ddlPlace" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                                AutoPostBack="true" OnTextChangedEvent="ddlPlace_TextChanged" />
                                        </td>
                                        <td>
                                            School
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ucInstitution" runat="server" CssClass="input_mandatory" DataValueField="FLDADDRESSCODE"
                                                AutoPostBack="true" OnSelectedIndexChanged="ucInstitution_SelectedIndexChanged"
                                                DataTextField="FLDDISPLAYNAME" OnDataBound="ddlInstitution_DataBound">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            If Other, Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInstituteName" runat="server" CssClass="input_mandatory"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Exam Roll No.
                            </td>
                            <td>
                                <asp:TextBox ID="txtExamRollno" runat="server" CssClass="input" MaxLength="200" Width="200"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="rollno" runat="server">
                            <td>
                                Years of Education : From
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYearFrom" runat="server" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
                            </td>
                            <td>
                                To
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYearTo" runat="server" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Year of Passing
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYearPass" runat="server" CssClass="dropdown_mandatory">
                                </asp:DropDownList>
                            </td>
                            <td>
                                First Attempt
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFirstAttemptYN" runat="server" CssClass="input">
                                    <asp:ListItem Text="-select-" Value="Dummy"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                Still awaiting for the exam result :&nbsp;
                                <asp:CheckBox ID="chkResultYN" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <h4>
                                    Institute Address</h4>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" style="width: 65%;">
                                <table style="width: 98%">
                                    <tr>
                                        <td>
                                            <eluc:CommonAddress ID="ucAddress" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2" style="width: 35%;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <asp:Label ID="lblMarks" runat="server" Text="Marks"></asp:Label></b>
                                <asp:RadioButtonList ID="rblMarksGrade" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rblMarksGrade_Changed">
                                    <asp:ListItem Text="Marks" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Grade" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
                            <td valign="top" colspan="4" style="width: 80%;">                                
                                    <asp:GridView ID="gvMarks" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                                        ShowFooter="false" OnRowDataBound="gvMarks_RowDataBound" Width="95%" CellPadding="3"
                                        EnableViewState="false">
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Subject/Sem">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMarksId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKSID") %>'></asp:Label>
                                                    <asp:Label ID="lblAcademicType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACADEMICTYPE") %>'></asp:Label>
                                                    <asp:Label ID="lblAcademicId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACADEMICID") %>'></asp:Label>
                                                    <asp:Label ID="lblSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                                    <asp:Label ID="lblSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Marks obtained">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMarks" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKS") %>'></asp:Label>
                                                    <eluc:Number ID="txtMarksEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKS") %>'
                                                        Width="100px" MaxLength="6" DecimalPlace="0" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max Marks">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <eluc:Number ID="txtOutOffEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOUTOFF") %>'
                                                        Width="100px" MaxLength="6" DecimalPlace="0" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlGradeEdit" runat="server" CssClass="dropdown_mandatory"
                                                        DataTextField="FLDGRADE" DataValueField="FLDGRADEPOINT" AutoPostBack="true" OnSelectedIndexChanged="ddlGrade_TextChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade Point">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <eluc:Number ID="txtGradePointEdit" CssClass="readonlytextbox" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRADEPOINT") %>'
                                                        Width="100px" MaxLength="2" Enabled="false" IsInteger="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Percentage">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                    <eluc:Number ID="txtPercentageEdit" CssClass="readonlytextbox" ReadOnly="true" runat="server"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>' Width="100px"
                                                        MaxLength="6" Visible="false" />
                                                    <asp:Label ID="lblPercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></asp:Label>
                                                    <asp:Label ID="lblTotalMarks" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALMARKS") %>'></asp:Label>
                                                    <asp:Label ID="lblAvg" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGE") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    Action
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                                        ToolTip="Edit"></asp:ImageButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                                        ToolTip="Save"></asp:ImageButton>
                                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                        width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                        ToolTip="Cancel"></asp:ImageButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>                             
                                <br />
                                <table style="width: 95%; float: right;">
                                    <tr>
                                        <td>
                                            Total
                                        </td>
                                        <td>
                                            <eluc:Number ID="txtTotal" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Overall Percentage/Average
                                        </td>
                                        <td>
                                            <eluc:Number ID="txtPercentage" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                            %
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <asp:Button ID="btnBasicSave" runat="server" CommandName="HSC" Text="Save" CssClass="DataGrid-HeaderStyle"
                                    OnClick="Application_Save" Font-Bold="true" BorderWidth="0" Height="20px" Width="75px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table id="tblErrMsg" runat="server" visible="false">
                        <tr>
                            <td colspan="4" style="color: red; font-weight: bold;">
                                - Please see the error message above.
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
