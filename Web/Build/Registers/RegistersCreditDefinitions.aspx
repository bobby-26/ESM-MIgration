<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersCreditDefinitions.aspx.cs"
    Inherits="RegistersCreditDefinitions" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rank</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersRank" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRankEntry">
        <ContentTemplate>
           
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                 <eluc:Title runat="server" ID="Title1" Text=" Credit Definition Details"></eluc:Title>
                    <div id="divHeading">
                       
                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureRank" width="100%">
                        <tr>
                            <td >
                               <asp:Literal ID="lblCreditDefinition" runat="server" Text="Credit Definition"></asp:Literal>
                            </td>
                            <td >
                                <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input" Width="360px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td >
                                <eluc:Rank ID="ucSerachRank" runat="server"  AppendDataBoundItems="true" CssClass="input"/>
                            </td>
                            <td>
                                <asp:Literal ID="lblPool" runat="server" Text="Pool"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Pool ID="ucSerchPool" runat="server" AppendDataBoundItems="true" CssClass="input"/>
                            </td>
                        </tr>
                        <tr>
                            <td >
                               <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Nationality ID="ucSearchNationality" AppendDataBoundItems="true" CssClass="input" runat="server" />
                            </td>
                            <td>
                            </td>
                            <td >
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersRank" runat="server" OnTabStripCommand="RegistersRank_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvCreditDefinitions" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvCreditDefinitions_RowCommand"
                        OnRowDataBound="gvCreditDefinitions_ItemDataBound" OnRowCancelingEdit="gvCreditDefinitions_RowCancelingEdit"
                        OnRowDeleting="gvCreditDefinitions_RowDeleting" OnRowEditing="gvCreditDefinitions_RowEditing"
                        ShowFooter="true" ShowHeader="true">
                        <FooterStyle ForeColor="#000066" BackColor="#dfdfdf"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle BackColor="#f9f9fa" />
                        <SelectedRowStyle BackColor="#bbddff" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRankCodeHeader" runat="server">Credit Definition Code&nbsp;<asp:ImageButton
                                        runat="server" ID="cmdRankCodeDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                        CommandName="FLDCREDITDEFINITIONSID" CommandArgument="1" />
                                        <asp:ImageButton runat="server" ID="cmdRankCodeAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDCREDITDEFINITIONSID" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreditDefinitionsId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEFINITIONSID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCreditDefinitionsIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEFINITIONSID") %>'
                                        CssClass="gridinput_mandatory" MaxLength="6" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="New Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRankNameHeader" runat="server">Credit Definition &nbsp;<asp:ImageButton
                                        runat="server" ID="cmdCreditDefinitionsNameDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>"
                                        OnClick="cmdSort_Click" CommandName="FLDCREDITDEFINITIONSNAME" CommandArgument="1"
                                        AlternateText="Rank name desc" />
                                        <asp:ImageButton runat="server" ID="cmdCreditDefinitionsNameAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDCREDITDEFINITIONSNAME" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCreditDefinitions" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEFINITIONSID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkCreditDefinitionsName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEFINITIONSNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCreditDefinitionsEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEFINITIONSID") %>'></asp:Label>
                                    <asp:TextBox ID="txtCreditDefinitionsNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITDEFINITIONSNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtCreditDefinitionsNameAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="200" ToolTip="Enter Rank Name"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLevelHeader" runat="server">Rank&nbsp;<asp:ImageButton runat="server"
                                        ID="cmdLevelDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click" CommandName="FLDRANKNAME"
                                        CommandArgument="1" />
                                        <asp:ImageButton runat="server" ID="cmdNationalityAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDRANKNAME" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkairportName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Rank ID="ucRankEdit" AppendDataBoundItems="false" runat="server" RankList="<%#PhoenixRegistersRank.ListRank() %>"
                                        SelectedRank='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>'  CssClass="gridinput" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Rank ID="ucRankAdd" AppendDataBoundItems="false" runat="server" CssClass="gridinput" RankList="<%#PhoenixRegistersRank.ListRank() %>" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCrewSortHeader" runat="server">Pool&nbsp;<asp:ImageButton runat="server"
                                        ID="cmdCrewSortDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                        CommandName="FLDPOOL" CommandArgument="1" />
                                        <asp:ImageButton runat="server" ID="cmdCrewSortAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDPOOL" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPool" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOOL") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkPoolName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOOLNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Pool ID="ucPoolEdit" AppendDataBoundItems="false" runat="server" PoolList="<%#PhoenixRegistersMiscellaneousPoolMaster.ListMiscellaneousPoolMaster()%>"
                                        SelectedPool='<%# DataBinder.Eval(Container,"DataItem.FLDPOOL") %>' CssClass="gridinput" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Pool ID="ucPoolAdd" AppendDataBoundItems="false" runat="server" CssClass="gridinput" PoolList="<%#PhoenixRegistersMiscellaneousPoolMaster.ListMiscellaneousPoolMaster()%>" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblNationalityHeader" runat="server">Nationality&nbsp;<asp:ImageButton
                                        runat="server" ID="cmdGroupRankDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                        CommandName="FLDNATIONALITY" CommandArgument="1" />
                                        <asp:ImageButton runat="server" ID="cmdGroupRankAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDNATIONALITY" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNationality" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkNationalityName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITYNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Nationality ID="ucNationalityEdit" runat="server" NationalityList="<%#PhoenixRegistersCountry.ListNationality() %>"
                                        SelectedNationality='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITY") %>' CssClass="gridinput" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Nationality ID="ucNationalityAdd" runat="server" CssClass="gridinput" NationalityList="<%#PhoenixRegistersCountry.ListNationality() %>" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblGroupHeader" runat="server">Amount&nbsp;<asp:ImageButton runat="server"
                                        ID="cmdGroupDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click" CommandName="FLDAMOUNT"
                                        CommandArgument="1" />
                                        <asp:ImageButton runat="server" ID="cmdGroupAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDAMOUNT" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtAmountAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
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
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
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
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" style="background-color: #88bbee">
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
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="MenuRegistersRank" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
