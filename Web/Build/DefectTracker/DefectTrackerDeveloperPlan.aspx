<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerDeveloperPlan.aspx.cs"
    Inherits="DefectTracker_DefectTrackerDeveloperPlan" %>

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
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
                    <eluc:Title runat="server" ID="Title1" Text="Plan" ShowMenu="true" />
                </div>
            </div>
            <div id="divFind" style="margin-top: 0px;">
                <table width="100%" cellpadding="0" cellspacing="0" border="1">
                    <tr>
                        <td width="25%">
                            <table>
                                <tr>
                                    <td>
                                        Bug ID
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIDSearch" runat="server" CssClass="input" OnTextChangedEvent="Filter_Changed"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Assigned To
                                    </td>
                                    <td>
                                        <eluc:SEPTeamMembers ID="ucDeveloperName" AppendDataBoundItems="True" runat="server"
                                            OnTextChangedEvent="Filter_Changed" CssClass="input"></eluc:SEPTeamMembers>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Open Between
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromOpenDate" runat="server" CssClass="input" />
                                        <eluc:Date ID="ucToOpenDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Started Between
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromStartDate" runat="server" CssClass="input" />
                                        <eluc:Date ID="ucToStartDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Finished Between
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromFinishDate" runat="server" CssClass="input" />
                                        <eluc:Date ID="ucToFinishDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        ActualFinish
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromActualFinishDate" runat="server" CssClass="input" />
                                        <eluc:Date ID="ucToActualFinishDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Repored by
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReportedby" CssClass="input" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblmilestone" runat="server" Text="Milestone"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlmilestone" runat="server" CssClass="input" AutoPostBack="true"
                                            MaxLength="100" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblissueflag" runat="server" Text="Issue Flag"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlissueflag" runat="server" CssClass="input" AutoPostBack="true"
                                            MaxLength="100" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="28%">
                            &nbsp; Module<br />
                            <eluc:SEPModule ID="ucSEPModule" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                        <td width="15%">
                            &nbsp; Type<br />
                            <eluc:SEPStatus ID="ucSEPType" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                        <td width="15%">
                            &nbsp; Status<br />
                            <eluc:SEPStatus ID="SEPStatus" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                        <td align="left">
                            &nbsp; Severity<br />
                            <eluc:SEPStatus ID="SEPSeverity" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                        <td align="left">
                            &nbsp; Priority<br />
                            <eluc:SEPStatus ID="SEPPriority" runat="server" AutoPostBack="true" OnTextChangedEvent="Filter_Changed" />
                        </td>
                    </tr>
                </table>
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
                                        Project
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblProjectname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Open Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpenDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDOPENDATE")%>'></asp:Label>
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
                                        Assigned To
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssignedTo" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDASSIGNEDTO")%>'></asp:Label>
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</body>
</form>
</html>
