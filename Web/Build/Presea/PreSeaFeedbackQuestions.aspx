<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaFeedbackQuestions.aspx.cs" Inherits="Presea_PreSeaFeedbackQuestions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FeedBack Questions</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    
     <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaFeedBackQuestions" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaFeedBackQuestions">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
            </eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" id="ucTitle" Text="FeedBack Questions">
                        </eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaFeedBackQuestions" runat="server" OnTabStripCommand="PreSeaFeedBackQuestions_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPreSeaFeedBackQuestions" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" OnRowCreated="gvPreSeaFeedBackQuestions_RowCreated" Width="100%"
                        CellPadding="3" OnRowCommand="gvPreSeaFeedBackQuestions_RowCommand" OnRowDataBound="gvPreSeaFeedBackQuestions_RowDataBound"
                        OnSelectedIndexChanging="gvPreSeaFeedBackQuestions_SelectedIndexChanging" OnRowCancelingEdit="gvPreSeaFeedBackQuestions_RowCancelingEdit"
                        OnRowDeleting="gvPreSeaFeedBackQuestions_RowDeleting" OnRowUpdating="gvPreSeaFeedBackQuestions_RowUpdating"
                        OnRowEditing="gvPreSeaFeedBackQuestions_RowEditing" ShowFooter="true" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true" >
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Question
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuestionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'></asp:Label>
                                    <asp:Label ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblQuestionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'></asp:Label>
                                    <asp:TextBox ID="txtQuestionEdit" runat="server" TextMode="MultiLine" Width="300px" Height="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>' CssClass="input_mandatory" ></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtQuestionAdd" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Width="300px" Height="50px"  ></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Question Type
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuestionType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rblQuestionTypeEdit" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1">Choose One</asp:ListItem>
                                        <asp:ListItem Value="2">Choose Multiple</asp:ListItem>
                                        <asp:ListItem Value="3">Comments</asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:RadioButtonList ID="rblQuestionTypeAdd" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1">Choose One</asp:ListItem>
                                        <asp:ListItem Value="2">Choose Multiple</asp:ListItem>
                                        <asp:ListItem Value="3">Comments</asp:ListItem>
                                    </asp:RadioButtonList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Options
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOptions" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtOptionsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONS") %>' CssClass="input"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtOptionsAdd" runat="server" CssClass="input"></asp:TextBox>
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
