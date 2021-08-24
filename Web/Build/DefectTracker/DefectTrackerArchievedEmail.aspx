<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerArchievedEmail.aspx.cs"
    Inherits="DefectTrackerArchievedEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Archieved Emails</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                    Archive History
                </div>
                <div style="position: absolute; top: 0px; right: 0px">
                    <eluc:TabStrip ID="MenuArchiveHistory" runat="server" OnTabStripCommand="MenuArchiveHistory_TabStripCommand">
                    </eluc:TabStrip>
                </div>
            </div>
            <br />
            <br />
            <br />
            <table id="ArchiveRemarks" runat="server" width="100%">
                <tr>
                    <td>
                        Remarks
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" CssClass="input_mandatory" Width="360px" runat="server" />
                    </td>
                    <td>
                        Archived by
                    </td>
                    <td>
                        <asp:TextBox ID="txtArchiveBy" CssClass="input_mandatory" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvArcheiveMail" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvArcheiveMail_RowDataBound">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            IP Address
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblEmailFrom" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDMODIFIEDIP")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Date
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUniqueID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAILID") %>'
                                Visible="false"></asp:Label>
                            <asp:LinkButton ID="lnkSubject" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODIFIEDDATE") %>'> ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Username
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUsername" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDMODIFIEDBY")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            Archived By
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblArchivedBy" runat="server" Visible="true" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDMODIFIEDBYNAME")%>'></asp:Label>
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
