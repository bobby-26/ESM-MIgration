<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaAcadamicQualificationSubjects.aspx.cs"
    Inherits="PreSeaAcadamicQualificationSubjects" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaQualificaiton" Src="~/UserControls/UserControlPreSeaQualification.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Medical</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaCourseSubjects" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaCourseSubjects">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Acadamic Qualification Subjects"></eluc:Title>
                    </div>
                </div>
                <table id="tblConfigureCity" width="50%">
                    <tr>
                        <td>
                            Qualification
                        </td>
                        <td>
                            <eluc:PreSeaQualificaiton ID="ddlCertificate" runat="server" CssClass="input_mandatory"
                                AutoPostBack="true" AppendDataBoundItems="true" OnTextChangedEvent="ddlPreSeaQualification_OnTextChanged" />
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaCourseSubjects" runat="server" OnTabStripCommand="PreSeaCourseSubjects_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPreSeaCourseSubjects" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" OnRowCreated="gvPreSeaCourseSubjects_RowCreated" Width="100%"
                        CellPadding="3" OnRowCommand="gvPreSeaCourseSubjects_RowCommand" OnRowDataBound="gvPreSeaCourseSubjects_RowDataBound"
                        OnSelectedIndexChanging="gvPreSeaCourseSubjects_SelectedIndexChanging" OnRowCancelingEdit="gvPreSeaCourseSubjects_RowCancelingEdit"
                        OnRowDeleting="gvPreSeaCourseSubjects_RowDeleting" OnRowUpdating="gvPreSeaCourseSubjects_RowUpdating"
                        OnRowEditing="gvPreSeaCourseSubjects_RowEditing" ShowFooter="true" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Subject/Sem">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDNAMEOFMEDICAL"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                    Subject/Sem
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubject" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkSubjectSem" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSubjectIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTID") %>'></asp:Label>
                                    <asp:TextBox ID="txtSubjectEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSubjectAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Subject Code
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubjectCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSubjectCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTCODE") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSubjectCodeAdd" runat="server" CssClass="gridinput"
                                        MaxLength="200"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOrder" runat="server" Text="Order"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERNO") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtOrderNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERNO") %>' CssClass="gridinput"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtOrderNoAdd" runat="server" CssClass="gridinput" ></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
