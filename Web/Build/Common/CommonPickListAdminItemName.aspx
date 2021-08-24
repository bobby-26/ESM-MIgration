<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListAdminItemName.aspx.cs" Inherits="Common_CommonPickListAdminItemName" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Item List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--<div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="User Name List" ShowMenu="false" />
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAssetNameList" runat="server" OnTabStripCommand="MenuAssetNameList_TabStripCommand">
        </eluc:TabStrip>
    </div>--%>
        <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <eluc:Title runat="server" ID="ucTitle" Text="Item List" />
            </div>
        </div>
        <div style="top: 0px; right: 2px; position: absolute">
            <eluc:TabStrip ID="MenuAsset" runat="server" OnTabStripCommand="Asset_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <div class="subHeader" style="top: 1px; right: 0px; position: relative">
            <div id="divHeadings">
                <%--<eluc:Title runat="server" ID="Title1" Text="Asset List" ShowMenu="false" />--%>
            </div>
        </div>
        <div class="Header">
            <div class="navSelect" style="top: 28.4px; right: 0px; position: absolute;">
                <eluc:TabStrip ID="MenuAssetNameList" runat="server" OnTabStripCommand="MenuAssetNameList_TabStripCommand"></eluc:TabStrip>
            </div>
        </div>
        <br />
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlUserNameList">
            <ContentTemplate>
                <div id="search">
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblAssetName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAssetName" CssClass="input" Text=""></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblSerialNumber" runat="server" Text="Serial No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSerialno" CssClass="input" Text=""></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvAssetName" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Font-Size="11px" OnRowCommand="gvAssetName_RowCommand" OnRowEditing="gvAssetName_RowEditing"
                        OnRowDataBound="gvAssetName_ItemDataBound" ShowFooter="true" ShowHeader="true" Width="100%"
                        EnableViewState="false" AllowSorting="true" OnSorting="gvAssetName_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Asset Name">
                                <HeaderStyle HorizontalAlign="Left" Width="45%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkAssetNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAssetId" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' Visible="false"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSETID") %>'></asp:Label>
                                    <asp:Label ID="lblCompanyId" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' Visible="false"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkAssetName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' CommandArgument='<%# Container.DataItemIndex %>'
                                        CommandName="EDIT"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Serial No">
                                <HeaderStyle HorizontalAlign="Left" Width="45%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkheaderserialno" runat="server" CommandName="Sort" CommandArgument="FLDSERIALNO"
                                        ForeColor="White">Serial No&nbsp;</asp:LinkButton>
                                    <img id="FLDSERIALNO" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblserialno" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></asp:Label>
                                </ItemTemplate>
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
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
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
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvAssetName" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
