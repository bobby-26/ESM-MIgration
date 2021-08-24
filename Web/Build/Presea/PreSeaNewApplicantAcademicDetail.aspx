<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantAcademicDetail.aspx.cs"
    Inherits="PreSeaNewApplicantAcademicDetail" %>

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
                <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" />
                    <div class="subHeader">
                        <eluc:Title runat="server" ID="Academic" Text="New ApplicantAcademic Details" ShowMenu="false"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuPreSeaAcademic" runat="server" OnTabStripCommand="PreSeaAcademic_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div>
                        <table cellpadding="3" cellspacing="1" width="80%">
                            <tr>
                                <td>Qualification
                                </td>
                                <td>
                                    <eluc:PreSeaQualificaiton ID="ddlCertificate" runat="server" CssClass="input_mandatory"
                                        Enabled="false" AutoPostBack="true" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>Board
                                </td>
                                <td>
                                    <eluc:Quick ID="ucAcademicBoard" runat="server" CssClass="dropdown_mandatory"
                                        QuickTypeCode="101" AppendDataBoundItems="true" Width="200" />
                                    <%--<asp:TextBox ID="txtBoard" runat="server" CssClass="input" MaxLength="200" Width="200"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Institution
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlInstitute" runat="server"
                                        CssClass="input_mandatory" AutoPostBack="true" Width="200" OnSelectedIndexChanged="ddlInstitute_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%--  <eluc:Institution ID="ucInstitution" runat="server" CssClass="input_mandatory" />--%>
                                </td>
                            </tr>
                            <tr id="univ" runat="server">
                                <td>University
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUniversity" runat="server" CssClass="input" MaxLength="200" Width="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="rollno" runat="server">
                                <td>Exam Rollno
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExamRollno" runat="server" CssClass="input" MaxLength="200" Width="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Years of Education : From
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYearFrom" runat="server" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                    <asp:Literal ID="ltrto" runat="server" Text="To"></asp:Literal>
                                    <asp:DropDownList ID="ddlYearTo" runat="server" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td>Year of passing
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYearPass" runat="server" CssClass="input_mandatory">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>First Attempt
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFirstAttemptYN" runat="server" CssClass="input">
                                        <asp:ListItem Text="-select-" Value="Dummy"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>Still awaiting for the examination result :
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkResultYN" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <hr />
                                    <table>
                                        <tr>
                                            <td>
                                                <b>Contact Address</b>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="4" cellspacing="1" width="90%">
                            <tr>
                                <td colspan="3" rowspan="4">
                                    <eluc:CommonAddress ID="ucAddress" runat="server" />
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:GridView ID="gvMarks" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                            OnRowDataBound="gvMarks_RowDataBound" OnRowCancelingEdit="gvMarks_RowCancelingEdit"
                            OnRowDeleting="gvMarks_RowDeleting" OnRowUpdating="gvMarks_RowUpdating" OnRowEditing="gvMarks_RowEditing"
                            OnRowCommand="gvMarks_RowCommand" Width="100%" CellPadding="3" ShowFooter="true"
                            EnableViewState="false">
                            <FooterStyle CssClass="datagrid_footerstyle" Font-Bold="true" HorizontalAlign="Right"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Subject/Sem">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMarksId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKSID") %>'></asp:Label>
                                        <asp:Label ID="lblSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                        <asp:Label ID="lblSubjectName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlSubjectAdd" runat="server" CssClass="input_mandatory">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mark obtain">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKS") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtMarksEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKS") %>'
                                            Width="100px" MaxLength="7" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="txtMarksAdd" CssClass="input_mandatory" runat="server" Width="100px"
                                            MaxLength="7" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Out Off">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutOff" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOUTOFF") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtOutOffEdit" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOUTOFF") %>'
                                            Width="100px" MaxLength="7" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="txtOutOffAdd" CssClass="input_mandatory" runat="server" Width="100px"
                                            MaxLength="7" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Percentage">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="txtPercentageEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'
                                            Width="100px" MaxLength="7"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="txtPercentageAdd" runat="server" Width="100px"
                                            MaxLength="7"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        Action
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
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
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>                                       
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>                                        
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <br />
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td>Total Score
                                </td>
                                <td>
                                    <eluc:Number ID="txtTotal" runat="server" CssClass="readonlytextbox" />
                                </td>
                                <td>Out Off
                                </td>
                                <td>
                                    <eluc:Number ID="txtoutoff" runat="server" CssClass="readonlytextbox" />
                                </td>                           
                                <td>Percentage
                                </td>
                                <td>
                                    <eluc:Number ID="txtPercentage" runat="server" CssClass="readonlytextbox" />
                                    %
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
