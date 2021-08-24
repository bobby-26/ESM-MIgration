<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersSetDetails.aspx.cs"
    Inherits="RegistersSetDetails" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rank</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersRank" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRankEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text=" Set Details"></eluc:Title>
                </div>
                <div id="divFind" style="position: relative; z-index: 2; top: 0px; left: 0px; width: 100%;">
                    <table id="tblConfigureRank" width="100%">
                        
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdListOwnerManager" runat="server" OnSelectedIndexChanged="rdListOwnerManager_SelectedIndexChanged"
                                    RepeatDirection="Horizontal" AutoPostBack="True">
                                    <asp:ListItem Text="Manager" Value="126" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Owner" Value="127"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Address ID="ucAddress" AppendDataBoundItems="true" CssClass="input" runat="server" />
                            </td>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Rank ID="ucSerchRank" runat="server" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersRank" runat="server" OnTabStripCommand="RegistersRank_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvSetDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvSetDetails_RowCommand" OnRowDataBound="gvSetDetails_ItemDataBound"
                        OnRowCancelingEdit="gvSetDetails_RowCancelingEdit" OnRowDeleting="gvSetDetails_RowDeleting"
                        OnRowEditing="gvSetDetails_RowEditing" ShowFooter="true" ShowHeader="true">
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
                                    <asp:Label ID="lblSetDetailCodeHeader" runat="server">Set Details Code&nbsp;<asp:ImageButton
                                        runat="server" ID="cmdRankCodeDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>"
                                        OnClick="cmdSort_Click" CommandName="FLDSETDETAILSID" CommandArgument="1" />
                                        <asp:ImageButton runat="server" ID="cmdRankCodeAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDSETDETAILSID" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSetDetailId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSETDETAILSID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSetDetailsIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSETDETAILSID") %>'
                                        CssClass="gridinput_mandatory" MaxLength="6"></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRankNameHeader" runat="server">Set Details Name&nbsp;<asp:ImageButton
                                        runat="server" ID="cmdRankNameDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>"
                                        OnClick="cmdSort_Click" CommandName="FLDSETDETAILNAME" CommandArgument="1" AlternateText="Rank name desc" />
                                        <asp:ImageButton runat="server" ID="cmdRankNameAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDSETDETAILNAME" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRankID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSETDETAILSID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkRankName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                </ItemTemplate>                              
                            </asp:TemplateField>
                              <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRankHeader" runat="server">Rank&nbsp;<asp:ImageButton runat="server"
                                        ID="cmdLevelRank" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                        CommandName="FLDRANKNAME" CommandArgument="1" />
                                        <asp:ImageButton runat="server" ID="cmdRankAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDRANKNAME" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkairportName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'  ></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Rank ID="ucRankEdit" runat="server" AppendDataBoundItems="false" RankList="<%#PhoenixRegistersRank.ListRank() %>"
                                        SelectedRank='<%# DataBinder.Eval(Container,"DataItem.FLDRANK") %>'  CssClass="gridinput_mandatory" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Rank ID="ucRankAdd" runat="server" AppendDataBoundItems="false" RankList="<%#PhoenixRegistersRank.ListRank() %>"
                                        CssClass="gridinput_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblLevelHeader" runat="server">Set Description &nbsp;<asp:ImageButton
                                        runat="server" ID="cmdLevelDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>"
                                        OnClick="cmdSort_Click" CommandName="FLDSETDESCRIPTION" CommandArgument="1" />
                                        <asp:ImageButton runat="server" ID="cmdNationalityAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDSETDESCRIPTION" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                      <eluc:Quick ID="ucSetDescriptionEdit" runat="server"                                        
                                        QuickList='<%#PhoenixRegistersQuick.ListQuick(0,(int)(PhoenixQuickTypeCode.SETDESCRIPTION)) %>'
                                        SelectedQuick='<%# DataBinder.Eval(Container,"DataItem.FLDSETDESCRIPTION") %>'  CssClass="gridinput_mandatory" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                        <eluc:Quick ID="ucSetDescriptionAdd" runat="server" QuickList='<%#PhoenixRegistersQuick.ListQuick(0,(int)(PhoenixQuickTypeCode.SETDESCRIPTION)) %>' QuickTypeCode='<%#((int)(PhoenixQuickTypeCode.SETDESCRIPTION)).ToString()%>'
                                        CssClass="gridinput_mandatory" />
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
