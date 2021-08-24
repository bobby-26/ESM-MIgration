<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerStrategyReport.aspx.cs"
    Inherits="DefectTracker_DefectTrackerStrategyReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Strategy Report</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                    <eluc:Title runat="server" ID="Title1" Text="Strategy Report" ShowMenu="true" />
                </div>
            </div>
            <div>
                <table width="60%">
                    <tr>
                        <font color="blue" size="0">
                            <li>Fixed Bugs : No of bugs closed till selected month</li>
                            <li>Pending Bugs : No of bugs opened till selected month </li>
                            <li>Critical : Critical,Show stopper</li>
                            <li>Not Critical : Not Critical,Minor</li>
                        </font>
                        <td>
                            Report for the Month of
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="input">
                                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                <asp:ListItem Text="December" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Year
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="input" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuDefectTracker" runat="server" OnTabStripCommand="StrategyReport_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <table width="100%">
                <tr>
                    <td colspan="4">
                        <b>Fixed Bugs</b>
                        <br />
                        <asp:GridView ID="gvDefectList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowDataBound="gvDefectList_ItemDataBound" OnRowEditing="gvDefectList_RowEditing"
                            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowCreated="gvDefectList_RowCreated">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        S.No
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDROWNUMBER")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Bug Id
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUniqueID" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDBUGDTKEY")%>'></asp:Label>
                                        <asp:Label ID="lblBugid" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDBUGID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Subject
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSubject" runat="server" CommandName="edit" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTHINT") %>'></asp:LinkButton>
                                        <eluc:Tooltip runat="server" ID="ucSubject" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Module Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblModuleName" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDMODULENAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Severity
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEVERITY") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Status
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Reported By
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportedBy" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDREPORTEDBY")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Open Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpenDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOPENDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Fixed Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFixedDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCLOSEDDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber_fixed" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages_fixed" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords_fixed" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious_fixed" runat="server" OnCommand="PagerButtonClick_fixed"
                                        CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext_fixed" OnCommand="PagerButtonClick_fixed" runat="server"
                                        CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage_fixed" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo_fixed" runat="server" Text="Go" OnClick="cmdGo_Click_fixed"
                                        CssClass="input" Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="4">
                        <b>Pending Bugs</b>
                        <br />
                        <asp:GridView ID="gvDefectListPending" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
                            OnRowEditing="gvDefectListPending_RowEditing" OnRowCreated="gvDefectListPending_RowCreated"
                            OnRowDataBound="gvDefectListPending_ItemDataBound">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        S.No
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDROWNUMBER")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Bug Id
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUniqueID" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDBUGDTKEY")%>'></asp:Label>
                                        <asp:Label ID="lblBugid" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDBUGID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Subject
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSubject" runat="server" CommandName="edit" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTHINT") %>'></asp:LinkButton>
                                        <eluc:Tooltip runat="server" ID="Tooltip1" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Module Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblModuleName" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDMODULENAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Severity
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSeverity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEVERITY") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Status
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Reported By
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportedBy" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDREPORTEDBY")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                    <HeaderTemplate>
                                        Open Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpenDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOPENDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
                                    <HeaderTemplate>
                                        Fixed Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFixedDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCLOSEDDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber_pending" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages_pending" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords_pending" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious_pending" runat="server" OnCommand="PagerButtonClick_pending"
                                        CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext_pending" OnCommand="PagerButtonClick_pending" runat="server"
                                        CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage_pending" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo_pending" runat="server" Text="Go" OnClick="cmdGo_Click_pending"
                                        CssClass="input" Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
