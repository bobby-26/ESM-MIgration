<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCompetenceConduct.aspx.cs" Inherits="RegistersCompetenceConduct" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>competence Conduct</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmcompetenceconduct" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlcompetenceconduct">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%; height: 417px;">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Competence Conduct"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect3" style="position: relative;">
                    <table id="tblcompetenceconduct" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ddlcompetenceconduct" runat="server" CssClass="input" HardTypeCode="90"
                                    ShortNameFilter="SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO" HardList='<%# PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO") %>'
                                    AppendDataBoundItems="true" Width="300px" AutoPostBack="true" />
                            </td>
                        </tr>
                    </table>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenucompetenceConduct" runat="server" OnTabStripCommand="MenucompetenceConduct_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative;">
                        <asp:GridView ID="GvcompetenceConduct" runat="server" AutoGenerateColumns="False"
                            CellPadding="3" Font-Size="11px" EnableViewState="false" Style="margin-bottom: 0px"
                            Width="100%" ShowFooter="True" OnRowCancelingEdit="GvcompetenceConduct_RowCancelingEdit"
                            OnRowCommand="GvcompetenceConduct_RowCommand" OnRowDeleting="GvcompetenceConduct_RowDeleting"
                            OnRowEditing="GvcompetenceConduct_RowEditing" OnRowUpdating="GvcompetenceConduct_RowUpdating"
                            OnRowDataBound="GvcompetenceConduct_RowDataBound" OnSorting="GvcompetenceConduct_Sorting"
                            AllowSorting="True">
                            <FooterStyle CssClass="datagrid_footerstyle" />
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkcategory" runat="server" CommandName="Sort" CommandArgument="FLDRANKID"
                                            ForeColor="White">Category&nbsp;</asp:LinkButton>
                                        <img id="FLDRANKID" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblrank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>'></asp:Label>
                                        <asp:Label ID="lblquestionid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPETENCEQUESTIONID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Hard ID="ddlcompetenceconductedit" runat="server" CssClass="input" HardTypeCode="90"
                                            ShortNameFilter="SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO" HardList='<%# PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO") %>'
                                            SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>' AppendDataBoundItems="true"
                                            Width="300px" />
                                        <asp:Label ID="lblquestionidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPETENCEQUESTIONID") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Hard ID="ddlcompetenceconductinsert" runat="server" CssClass="input" HardTypeCode="90"
                                            ShortNameFilter="SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO" HardList='<%# PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO") %>'
                                            AppendDataBoundItems="true" Width="300px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rank List">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Registers/RegistersToolTipAppraisalRankList.aspx?rankcategoryid=" + DataBinder.Eval(Container,"DataItem.FLDRANKID").ToString() %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <%--<asp:LinkButton ID="lnkevaluationitem" runat="server" CommandName="Sort" CommandArgument="FLDCONDUCTQUESTION"
                                            ForeColor="White">--%>Evaluation Item<%--&nbsp;</asp:LinkButton>
                                        <img id="FLDCONDUCTQUESTION" runat="server" visible="false" />--%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblevaluationitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPETENCEQUESTION") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtevaluationitemedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPETENCEQUESTION") %>'
                                            CssClass="gridinput_mandatory"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtevaluationiteminsert" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkordersequence" runat="server" CommandName="Sort" CommandArgument="FLDORDERSEQUENCE"
                                            ForeColor="White">Order Sequence&nbsp;</asp:LinkButton>
                                        <img id="FLDORDERSEQUENCE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblordersequence" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERSEQUENCE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number runat="server" ID="ucordersequenceEdit" CssClass="gridinput_mandatory"
                                            MaxLength="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERSEQUENCE") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number runat="server" ID="ucordersequenceInsert" CssClass="gridinput_mandatory"
                                            MaxLength="2" />
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
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# (Container.DataItemIndex)%>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
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
                        <div id="divPage" style="position: relative;">
                            <table width="100%" border="0" class="datagrid_pagestyle">
                                <tr>
                                    <td nowrap="nowrap" align="center">
                                        <asp:Label ID="lblPagenumber" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblPages" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblRecords" runat="server">
                                        </asp:Label>&nbsp;&nbsp;
                                    </td>
                                    <td nowrap="nowrap" align="left" width="50px">
                                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                    </td>
                                    <td width="20px">
                                        &nbsp;
                                    </td>
                                    <td nowrap="nowrap" align="right" width="50px">
                                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                    </td>
                                    <td nowrap="nowrap" align="center">
                                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                        </asp:TextBox>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                            Width="40px"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <eluc:Status runat="server" ID="Status1" />
                    </div>
                </div>
                <br />
                <br />
                <br />
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
