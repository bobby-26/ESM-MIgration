<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerVesselPatch.aspx.cs"
    Inherits="DefectTrackerVesselPatch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patch Release</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMailManager">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader">
                <div id="divHeading" class="divFloatLeft">
                    <eluc:Title runat="server" ID="Title1" Text="Vessel Patch " ShowMenu="true" />
                </div>
            </div>
            <div id="divFind" style="margin-top: 0px;">
                <table width="50%">
                    <tr>
                        <td width="50%">
                            <asp:Label ID="lblVesselname" Text="Vessel Name" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselname" CssClass="input" runat="server" runat="server" />
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuDefectTracker" runat="server" OnTabStripCommand="MenuPatchList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvVesselPatch" OnRowDataBound="gvVesselPatch_ItemDataBound" runat="server"
                    AutoGenerateColumns="False" Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true"
                    EnableViewState="false" OnRowEditing="gvVesselPatch_RowEditing" OnRowCreated="gvVesselPatch_RowCreated">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Vessel Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUniqueID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkVesselName" runat="server" CommandName="cmdEdit" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.VESSELNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                Patch Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPatchName" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDPATCHNAME")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                Patch Applied Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPatchAppliedDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDPATCHAPPLIEDDATE")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="5%" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                                Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Ship Ack." ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                    CommandName="HISTORY" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdPatchHistory"
                                    ToolTip="Patch History"></asp:ImageButton>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
