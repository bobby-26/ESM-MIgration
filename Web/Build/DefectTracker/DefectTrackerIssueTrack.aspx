<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerIssueTrack.aspx.cs"
    Inherits="DefectTrackerIssueTrack" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Issue Track</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>

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
                    <eluc:Title runat="server" ID="ucTitle" Text="Bug Track" ShowMenu="false"></eluc:Title>
                </div>
                <div style="position: absolute; right: 0px">
                    <eluc:TabStrip ID="MenuIssueTrack" runat="server" OnTabStripCommand="MenuIssueTrack_TabStripCommand">
                    </eluc:TabStrip>
                </div>
            </div>
            <asp:GridView ID="gvIssueTrack" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvIssueTrack_RowDataBound">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
                        <HeaderTemplate>
                            Username
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUsername" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDUSERNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Audit Type
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAuditType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITTYPENAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Audit Date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAuditTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITDATETIME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Module
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblModuleName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODULENAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Priority
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPriority" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDPRIORITY")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Severity
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSeverity" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDSEVERITYNAME")%>'></asp:Label>
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
                            Assigned to
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAssignedTo" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDASSIGNEDTO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Assigned date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAssignedDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDASSIGNEDDATE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:GridView ID="gvIssueAssignTrack" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvIssueTrack_RowDataBound">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Audit Type
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAuditType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITTYPENAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Audit Date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAuditTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITDATETIME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Task
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
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
                            Assigned On
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAssignedOn" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDASSIGNEDDATE")%>'></asp:Label>
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
                            <asp:Label ID="lblFinishedDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDFINISHDATE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Actual Start date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActualStartDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDACTUALSTARTDATE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Actual Finish date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActualFinishDate" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>      
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Planned Effort
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPlannedEffort" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDPLANNEDEFFORT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Actual Effort
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActualEffort" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDACTUALEFFORT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                                                       
                </Columns>
            </asp:GridView>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
