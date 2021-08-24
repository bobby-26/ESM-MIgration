<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerWeekWisePlan.aspx.cs"
    Inherits="DefectTrackerWeekWisePlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPBugModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPStatus" Src="~/UserControls/UserControlSEPStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPType" Src="~/UserControls/UserControlSEPBugType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPSeverity" Src="~/UserControls/UserControlSEPBugSeverity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPPriority" Src="~/UserControls/UserControlSEPBugPriority.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPTeamMembers" Src="~/UserControls/UserControlSEPTeamMembers.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Developers Plan</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<form id="form1" runat="server">
<body>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMailManager">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader">
                <div id="divHeading" class="divFloatLeft">
                    <eluc:Title runat="server" ID="Title1" Text="Weekly Plan" ShowMenu="true" />
                </div>
                <div style="position: absolute; top: 0px; right: 0px">
                    <eluc:TabStrip ID="MenuWeeklyPlan" runat="server" OnTabStripCommand="MenuWeeklyPlan_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
            </div>
            <div id="divFind" style="margin-top: 0px;">
                <br />
                <table width="50%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            From Date
                        </td>
                        <td>
                            <eluc:Date ID="ucFromFinishDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            To Date
                        </td>
                        <td>
                            <eluc:Date ID="ucToFinishDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuDefectTracker" runat="server" OnTabStripCommand="MenuDefectTracker_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <table width="100%">
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvDeveloperPlan" OnRowDataBound="gvDeveloperPlann_ItemDataBound"
                            runat="server" AutoGenerateColumns="False" Font-Size="11px" Width="100%" CellPadding="3"
                            ShowHeader="true" EnableViewState="false" OnRowEditing="gvDeveloperPlann_RowEditing"
                            OnRowCreated="gvDeveloperPlan_RowCreated">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField  HeaderText="File Name">
                                    <ItemStyle Width="8%" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Assigned To
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssignedTo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDASSIGNEDTO")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Bug Id
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUniqueID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                            Visible="false"></asp:Label>
                                        <asp:LinkButton ID="lnkBugId" runat="server" CommandName="cmdEdit" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUGID") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Open Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpenDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDOPENDATE", "{0:dd/MM/yyyy}")%>'></asp:Label>
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
                                        Subject
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubject" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTHINT") %>' />
                                        <eluc:Tooltip ID="uclblSubject" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBJECT").ToString() %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Status
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Reported By
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportedBy" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDUSERNAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Start Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStartDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDSTARTDATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Finish Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblReopenedDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDFINISHDATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Actual Finish
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosedDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Planned Effort
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPlannedEffort" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDPLANNEDEFFORT")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Actual Effort
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblActualEffort" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDACTUALEFFORT")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</body>
</form>
</html>
