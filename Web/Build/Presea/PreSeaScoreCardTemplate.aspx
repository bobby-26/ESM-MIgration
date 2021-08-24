<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaScoreCardTemplate.aspx.cs"
    Inherits="PreSeaScoreCardTemplate" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="QuickType" Src="~/UserControls/UserControlQuickType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea ScoreCard Template</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaQuick" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCountryEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Score Card Template"></eluc:Title>
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2;">
                    <table id="tblConfigureQuick" width="100%">
                        <tr>
                            <td width="20%">
                                Score Card Template
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlScoreCard" runat="server" DataTextField="FLDSCORECARDNAME"
                                    AutoPostBack="true" CssClass="dropdown_mandatory" DataValueField="FLDSCORECARDID"
                                    OnSelectedIndexChanged="ddlScoreCard_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaQuick" runat="server" OnTabStripCommand="PreSeaQuick_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPreSeaScoreCard" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvPreSeaScoreCard_RowCommand" OnRowDataBound="gvPreSeaScoreCard_RowDataBound"
                        OnRowCreated="gvPreSeaScoreCard_RowCreated" OnRowCancelingEdit="gvPreSeaScoreCard_RowCancelingEdit"
                        AllowSorting="true" OnRowEditing="gvPreSeaScoreCard_RowEditing" OnRowUpdating="gvPreSeaScoreCard_RowUpdating"
                        OnSorting="gvPreSeaScoreCard_Sorting" ShowFooter="true" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Section
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFieldId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>'></asp:Label>
                                    <asp:Label ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblFieldIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDID") %>'></asp:Label>
                                    <asp:Label ID="lblSectionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTION") %>'></asp:Label>
                                    <asp:Label ID="lblSectionNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlSectionAdd" runat="server" CssClass="gridinput_mandatory">
                                        <asp:ListItem Text="--Select--" Value="DUMMY"></asp:ListItem>
                                        <asp:ListItem Text="Academic" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Written" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Personality and Proficiency" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Serial Number
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDSERIALNO") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSerialNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDSERIALNO") %>'
                                        CssClass="gridinput_mandatory" MaxLength="10"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSerialNoAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="10"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Evaluation Criteria
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFieldDesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDDESCRIPION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFieldDescEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIELDDESCRIPION") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFieldDescAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Academic Subject
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAcademicSubject" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICAIONSUBJECT") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListAcadamicSubjectEdit">
                                        <asp:TextBox ID="txtQualificationEdit" runat="server" CssClass="input" Width="20%"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATION") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtSubjectnameEdit" runat="server" CssClass="input" Width="60%"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTNAME") %>'></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="cmdAcadamicSubjectAdd" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListAcadamicSubjectEdit', 'codehelp1', '', '../PreSea/PreSeaPickListAcademicSubjects.aspx', false);"
                                            Text=".." />
                                        <asp:TextBox ID="txtsubjectIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACADEMICSUBJECT") %>'></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <span id="spnPickListAcadamicSubjectAdd">
                                        <asp:TextBox ID="txtQualificationAdd" runat="server" CssClass="input" Width="20%"></asp:TextBox>
                                        <asp:TextBox ID="txtSubjectnameAdd" runat="server" CssClass="input" Width="60%"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="cmdAcadamicSubjectAdd" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListAcadamicSubjectAdd', 'codehelp1', '', '../PreSea/PreSeaPickListAcademicSubjects.aspx', false);"
                                            Text=".." />
                                        <asp:TextBox ID="txtsubjectIdAdd" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Active Y/N
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkActiveEdit" runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="chkActiveAdd" runat="server" />
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
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
