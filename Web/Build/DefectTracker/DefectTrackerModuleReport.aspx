<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerModuleReport.aspx.cs"
    Inherits="DefectTrackerModuleReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
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
                    <eluc:Title runat="server" ID="Title1" Text="Module Report" ShowMenu="true" />
                </div>
            </div>
            <table width="30%">
                <tr>
                    <td>
                        <font color="blue">
                            <li>Bugs : Bugs in New and Assigned </li>
                            <li>Next Phase : Issues in Feature deffered</li>
                            <li></li>
                        </font>
                    </td>
                </tr>
                <tr>
                    <td>
                        Report upto
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtToDate" CssClass="input" />
                    </td>
                </tr>
            </table>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuDefectTracker" runat="server" OnTabStripCommand="StrategyReport_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <asp:GridView ID="gvModuleList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="true" ShowHeader="true" EnableViewState="false"
                OnRowCreated="gvModuleList_RowCreated" OnRowDataBound="gvModuleList_ItemDataBound">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField FooterStyle-Font-Bold="true" HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Module Name
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblModuleName" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDMODULENAME")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotal" Text="Total" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                        HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            Issues logged
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="ucSubject" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOGGED") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblloggedTotal" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                        HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            Issues Completed
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCompletedIssues" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETED") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblCompletedTotal" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                        HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            Issues Pending
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPendingIssues" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDPENDING")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblPendingTotal" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                        HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            New Issues
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblNewIssues" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDNEW")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblNewTotal" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                        HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            Bugs
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblBugs" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDBUGS")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblBugsTotal" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                        HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            % Completed
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblIssueCompletion" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDPERCENT")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblPerCompletedTotal" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right"
                        HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            Next Phase
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblNextPhase" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDEFERRED")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblNextPhaseTotal" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
