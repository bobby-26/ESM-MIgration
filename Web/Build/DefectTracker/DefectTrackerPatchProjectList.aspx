<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerPatchProjectList.aspx.cs"
    EnableEventValidation="false" Inherits="DefectTrackerPatchProjectList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.DefectTracker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPTeamMembers" Src="~/UserControls/UserControlSEPTeamMembers.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Module" Src="~/UserControls/UserControlSEPModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselList" Src="~/UserControls/UserControlVesselList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Patch Add/Edit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <div class="subHeader">
            <div id="divHeading" class="divFloatLeft">
                <eluc:Title runat="server" ID="ucTitle" Text="Patch Project"></eluc:Title>
            </div>
        </div>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuPatchProjectAdd" runat="server"></eluc:TabStrip>
        </div>
        <eluc:Error ID="ucError" runat="server" Visible="false" />
        <asp:UpdatePanel runat="server" ID="pnlVoyageData">
            <ContentTemplate>
                <asp:GridView ID="gvPatchProject" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvPatchProject_RowDataBound"
                    OnRowCreated="gvPatchProject_RowCreated" OnRowCommand="gvPatchProject_RowCommand"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblDateHeader" runat="server">
                                Date
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCALLDATE").ToString() %>'> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Catalog">
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblCatalogHeader" runat="server">
                                Catalog
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCatalogNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCATALOGNUMBER").ToString().Length > 25 ? DataBinder.Eval(Container, "DataItem.FLDCATALOGNUMBER").ToString().Substring(0, 25) + "..." : DataBinder.Eval(Container, "DataItem.FLDCATALOGNUMBER").ToString()%>'> </asp:Label>
                                <eluc:Tooltip ID="uclblCatalogNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATALOGNUMBER") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Title">
                            <ItemStyle HorizontalAlign="Left" Width="40%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblTitleHeader" runat="server">
                                Title
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTITLE").ToString().Length > 80 ? DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString().Substring(0, 80) + "..." : DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString()%>'> </asp:Label>
                                <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'> </asp:Label>
                                <eluc:Tooltip ID="uclblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Call Number">
                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblCallHeader" runat="server">
                                Call
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCallNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCALLNUMBER").ToString() %>'> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Created By">
                            <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblCreatedByHeader" runat="server">
                                Created By
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME").ToString() %>'> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDITFILE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Patch" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="PROJECTPATCH" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdPatch"
                                    ToolTip="Patch"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
