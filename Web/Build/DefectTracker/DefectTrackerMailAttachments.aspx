<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerMailAttachments.aspx.cs"
    Inherits="DefectTrackerMailAttachments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bug Edit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%= Session["sitepath"]%>/css/<%= Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%= Session["sitepath"]%>/js/phoenixPopup.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <div class="subHeader">
            <div id="divHeading" class="divFloatLeft">
                <eluc:Title runat="server" ID="ucTitle" Text="Attachments" ShowMenu="false"></eluc:Title>
            </div>
            <div style="position: absolute; right: 0px">
                <eluc:TabStrip ID="MenuAttachment" runat="server"></eluc:TabStrip>
            </div>
        </div>
        <table width="100%">
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvAttachment_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="File Name">
                                <ItemStyle HorizontalAlign="Left" Width="80%"></ItemStyle>
                                <HeaderTemplate>
                                    File Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    Download
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                        Height="14px" ToolTip="Download File">
                                    </asp:HyperLink></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
