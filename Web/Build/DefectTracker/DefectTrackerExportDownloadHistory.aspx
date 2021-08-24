<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerExportDownloadHistory.aspx.cs"
    Inherits="DefectTrackerExportDownloadHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Export Downloads</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMailManager">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader">
                <div id="divHeading" class="divFloatLeft">
                    Download History
                </div>
            </div>
            <br />
            <asp:GridView ID="gvDownloadList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                OnRowCreated="gvDownloadList_RowCreated" OnRowCancelingEdit="gvDownloadList_RowCancelingEdit" OnRowEditing="gvDownloadList_RowEditing"
                OnRowUpdating="gvDownloadList_RowUpdating" OnRowCommand="gvDownloadList_RowCommand"
                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            File No
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUsername" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEFILENO")%>'></asp:Label>
                            <asp:Label ID="lblDTKey" Visible="false" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Downloaded By
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDownloaddBy" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDOWNLOADBY")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Downloaded Date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDownloadedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOWNLOADON") %>'> ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            IP Address
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblIPAddress" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDOWNLOADURL")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Remarks
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblRemarks" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDREMARKS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDREMARKS")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
