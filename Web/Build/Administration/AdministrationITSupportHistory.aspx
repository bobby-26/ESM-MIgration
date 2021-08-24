<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdministrationITSupportHistory.aspx.cs" Inherits="AdministrationITSupportHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ITSupport History</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                    <eluc:Title runat="server" ID="ucTitle" Text="ITSupport History" ShowMenu="false"></eluc:Title>
                </div>
                <div style="position: absolute; right: 0px">
                    <eluc:TabStrip ID="MenuIssueTrack" runat="server" OnTabStripCommand="MenuIssueTrack_TabStripCommand" TabStrip="true">
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
                    <asp:TemplateField HeaderText="User Name">
                        <ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
                        <HeaderTemplate>
                         <asp:Label ID="lblUser" Text='Username' runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUsername" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDUSERNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Call Type">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                           <asp:Label ID="lblCall" Text='Call Type' runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCallType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCALLTYPE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action Taken">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                           <asp:Label ID="lblAction" Text='Action Taken' runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActionTaken" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIONTAKEN") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remarks">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblRemark" Text='Remarks' runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblStatus" Text='Staus' runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Attended By">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblAttendedBy" Text='Attended By' runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAttendedByName" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDATTENDBY")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>       
                     <asp:TemplateField HeaderText="Date">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblDate" Text='Date' runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>                        
                            <asp:Label ID="lblDate" runat="server" Text=' <%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE"))%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>             
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
