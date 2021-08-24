<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminITTeam.aspx.cs" Inherits="RegistersAdminITTeam"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ITSupport Team</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server"> 
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersITTeam" autocomplete="off" runat="server">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlITTeamEntry">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading">
                            <eluc:Title runat="server" ID="ucTitle" Text="IT Team" visible="false" />
                        </div>
                    </div>
                    <div id="divFind" style="position: relative; z-index: 2">
                        <table id="tblConfigureITTeam" width="50%">
                            <tr>
                                <td>Name
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100"></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuRegistersITTeam" runat="server" OnTabStripCommand="MenuRegistersITTeam_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0">
                        <asp:GridView ID="gvITTeam" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvITTeam_RowCommand" OnRowDataBound="gvITTeam_ItemDataBound"
                            OnRowCancelingEdit="gvITTeam_RowCancelingEdit" OnRowDeleting="gvITTeam_RowDeleting"
                            OnRowEditing="gvITTeam_RowEditing" ShowFooter="true" ShowHeader="true"
                            OnSorting="gvITTeam_Sorting" AllowSorting="true" OnRowCreated="gvITTeam_RowCreated"
                            OnSelectedIndexChanging="gvITTeam_SelectedIndexChanging" OnRowUpdating="gvITTeam_RowUpdating">

                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />

                            <Columns>
                                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <asp:TemplateField FooterText="New Team Member">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="BindData"
                                            ImageUrl="<%$ PhoenixTheme:images/spacer.png %>"
                                            CommandArgument="1" />
                                        <asp:LinkButton ID="lblITTeamNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME" ForeColor="White">Name&nbsp;</asp:LinkButton>
                                        <img id="FLDNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblITTeamID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITTEAMID") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkITTeamName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblITTeamIDEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITTEAMID") %>'></asp:Label>
                                        <telerik:RadTextBox ID="txtITTeamNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                            CssClass="gridinput_mandatory" Width="98%" MaxLength="200"></telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtITTeamAdd" runat="server" CssClass="gridinput_mandatory"
                                            MaxLength="200" Width="98%" ToolTip="Enter Name"></telerik:RadTextBox>
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
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
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
                                <td width="20px">&nbsp;
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
            <%--        <Triggers>
            <asp:PostBackTrigger ControlID="MenuRegistersDepartmentType" />
        </Triggers>--%>
        </asp:UpdatePanel>
    </form>
</body>
</html>
