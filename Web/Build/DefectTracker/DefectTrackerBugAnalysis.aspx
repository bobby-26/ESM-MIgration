<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerBugAnalysis.aspx.cs"
    Inherits="DefectTracker_DefectTrackerBugAnalysis" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="ModuleList" Src="~/UserControls/UserControlSepModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugTypes" Src="~/UserControls/UserControlSepBugType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugSeverity" Src="~/UserControls/UserControlSepBugSeverity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugPriority" Src="~/UserControls/UserControlSepBugPriority.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BugStatus" Src="~/UserControls/UserControlSEPBugStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCode" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewLicenceEntry">
        <ContentTemplate>
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <div id="divHeading" class="divFloatLeft">
                        <eluc:Title runat="server" ID="ucTitle" Text="Bug Edit" ShowMenu="false"></eluc:Title>
                    </div>
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuAnalysis" runat="server" OnTabStripCommand="MenuAnalysis_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div class="subHeader">
                    <div style="position: absolute; right: 0px">
                        <eluc:TabStrip ID="MenuBugAnalysis" runat="server" OnTabStripCommand="MenuBugAnalysis_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div style="position: relative">
                    <table width="100%">
                        <tr>
                            <td>
                                Issue Type
                            </td>
                            <td>
                                <asp:Label ID="lblBugid" runat="server" Visible="false" />
                                <eluc:BugTypes ID="ddlBugType" runat="server" MaxLength="100" Width="360px" AppendDataBoundItems="true"
                                    CssClass="dropdown_mandatory" />
                            </td>
                            <td>
                                Severity
                            </td>
                            <td>
                                <eluc:BugSeverity ID="ddlBugSeverity" runat="server" MaxLength="100" Width="360px"
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </td>
                            <td>
                                Priority
                            </td>
                            <td>
                                <eluc:BugPriority ID="ddlBugPriority" runat="server" MaxLength="100" Width="360px"
                                    AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Analysis
                            </td>
                            <td colspan="5">
                                <%-- <asp:TextBox ID="txtDescription" runat="server" MaxLength="4000" TextMode="MultiLine"
                                    Rows="5" Columns="140" CssClass="input"></asp:TextBox>--%>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                New Issue Comment
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtNewIssueComment" Width="90%" runat="server" MaxLength="4000"
                                    TextMode="MultiLine" Rows="5" Columns="140" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gvCommentList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCommand="gvCommentList_RowCommand" Width="100%" CellPadding="3" ShowHeader="true"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td width="10%">
                                                Posted Date
                                            </td>
                                            <td width="15%">
                                                Posted by
                                            </td>
                                            <td width="70%">
                                                Comments
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>
                                                    <%#DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>
                                                </b>
                                            </td>
                                            <td>
                                                <b>
                                                    <%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDDATE")%>
                                                </b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblComments" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCOMMENTS")%>'
                                                        runat="server" />
                                                </b>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Action
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDITITEM" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
