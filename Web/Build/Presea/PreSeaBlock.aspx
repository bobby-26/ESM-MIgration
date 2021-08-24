<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBlock.aspx.cs" Inherits="PreSea_PreSeaBlock" %>

<%@ Import Namespace="SouthNests.Phoenix.PreSea" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="InfrastructureType" Src="~/UserControls/UserControlPreSeaInfrastructureType.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pre Sea Block</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaBlock" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSeaBlock">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Infrastructure"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div id="divSearch">
                    <table id="tblSearch" width="100%">
                        <tr>
                            <td>
                                Block Type
                            </td>
                            <td>
                                <eluc:InfrastructureType ID="ddlBlockType" runat="server" AppendDataBoundItems="true"
                                    CssClass="input" />
                            </td>
                            <td>
                                Block name
                            </td>
                            <td>
                                <asp:TextBox ID="txtBlockname" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                Floor
                            </td>
                            <td>
                                <asp:TextBox ID="txtFloor" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaBlock" runat="server" OnTabStripCommand="PreSeaBlock_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvPreSeaBlock" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCancelingEdit="gvPreSeaBlock_RowCancelingEdit"
                        OnRowCommand="gvPreSeaBlock_RowCommand" OnRowDataBound="gvPreSeaBlock_RowDataBound"
                        OnRowDeleting="gvPreSeaBlock_RowDeleting" OnRowEditing="gvPreSeaBlock_RowEditing"
                        OnRowUpdating="gvPreSeaBlock_RowUpdating" ShowFooter="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField HeaderText="Block Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Infrastructure Type
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBlockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINFRASTRUCTURENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:InfrastructureType ID="ddlBlockTypeEdit" runat="server" AppendDataBoundItems="true"
                                        CssClass="gridinput_mandatory" PreSeaInfrastructureTypeList='<%# PhoenixPreSeaInfrastructure.ListPreSeaInfrastructure(null) %>'
                                        SelectedInfrastructureTypeId='<%# DataBinder.Eval(Container,"DataItem.FLDINFRASTRUCTUREID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:InfrastructureType ID="ddlBlockTypeAdd" runat="server" AppendDataBoundItems="true"
                                        CssClass="gridinput_mandatory" PreSeaInfrastructureTypeList='<%# PhoenixPreSeaInfrastructure.ListPreSeaInfrastructure(null) %>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Block Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Block Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBlockId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBLOCKID") %>'></asp:Label>
                                    <asp:Label ID="lblBlockName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBLOCKNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblBlockIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBLOCKID") %>'></asp:Label>
                                    <asp:TextBox ID="txtBlockNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBLOCKNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtBlockNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Floor">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    Floor
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFloor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLOOR") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFloorEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLOOR") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFloorAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Rooms">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    No. of Rooms
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalRooms" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALROOMS") %>'></asp:Label>
                                </ItemTemplate>
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
                                        ToolTip="Edit" Visible="false"></asp:ImageButton>
                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" visible="false" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" visible="false" />
                                    <asp:ImageButton runat="server" AlternateText="Details" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                        CommandName="DETAILS" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDetails"
                                        ToolTip="Room Details"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save" Visible="false"></asp:ImageButton>
                                    <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel" Visible="false"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
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
