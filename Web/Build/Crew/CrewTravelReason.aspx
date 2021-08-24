<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelReason.aspx.cs" Inherits="CrewTravelReason" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Reason</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersTravelreason" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlreasonEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Travel Reason" />
                    </div>
                </div><br />
                <div id="divFind" style="position: relative; z-index: 2; top: 0px; left: 0px; width: 100%;">
                    <table id="tblConfigureCity" width="50%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblTravelReason" runat="server" Text="Travel Reason"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearch" runat="server" MaxLength="200" CssClass="input" Width="250px"></asp:TextBox>                                
                              
                            </td>
                        </tr>
                    </table>
                </div>               
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersCity" runat="server" OnTabStripCommand="RegistersCity_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvTravelreason" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvTravelreason_RowCommand" OnRowDataBound="gvTravelreason_ItemDataBound"
                        OnRowCreated="gvTravelreason_RowCreated" OnRowCancelingEdit="gvTravelreason_RowCancelingEdit"
                        OnRowDeleting="gvTravelreason_RowDeleting" OnRowUpdating="gvTravelreason_RowUpdating"
                        OnRowEditing="gvTravelreason_RowEditing" ShowFooter="true" ShowHeader="true"
                        OnSorting="gvTravelreason_Sorting" AllowSorting="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblreadonHeader" Visible="true" runat="server">
                                        <asp:ImageButton runat="server" ID="cmdreason" OnClick="cmdSearch_Click" CommandName="FLDREASON"
                                            ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                    </asp:Label>
                                    <asp:LinkButton ID="lnkreasonHeader" runat="server" CommandName="Sort" CommandArgument="FLDREASON"
                                        ForeColor="White">Travel Reason&nbsp;</asp:LinkButton>
                                    <img id="FLDREASON" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblreasonid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELREASONID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkreason" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblreasonidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELREASONID") %>'></asp:Label>
                                    <asp:TextBox ID="txtreasonedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtreasonadd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                 <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBudgetIdHeader" runat="server">Budget Code</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        CssClass="txtNumber"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListBudgetEdit">
                                        <asp:TextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                            MaxLength="20" CssClass="input_mandatory" Width="80%"></asp:TextBox>
                                        <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" 
                                            OnClientClick="return showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx', true);"/>
                                        <asp:TextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden" Enabled="False"></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                 <span id="spnPickListBudget">
                                        <asp:TextBox ID="txtBudgetCode" runat="server"
                                            MaxLength="20" CssClass="input_mandatory" Width="80%"></asp:TextBox>
                                        <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" 
                                            OnClientClick="return showPickList('spnPickListBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx', true);"/>
                                        <asp:TextBox ID="txtBudgetName" runat="server" Width="0px" CssClass="hidden" Enabled="False"></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                        <asp:TextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                    </span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                 <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActiveYNHeader" runat="server">
                                    Reason For
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReasonfor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONFOR") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:RadioButtonList ID="rbReasonforedit" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Crew Change" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Breakup" Value="2"></asp:ListItem>
                                         <asp:ListItem Text="Supernumarary" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="All" Value="4" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:RadioButtonList ID="rbReasonforadd" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Crew Change" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Breakup" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Supernumarary" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="All" Value="4" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </FooterTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px" ></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
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
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
