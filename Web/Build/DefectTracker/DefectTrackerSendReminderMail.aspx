<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerSendReminderMail.aspx.cs"
    Inherits="DefectTrackerSendReminderMail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPbugModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPIncident" Src="~/UserControls/UserControlSEPIncidentList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register Src="../UserControls/UserControlMultiColumnCity.ascx" TagName="UserControlMultiColumnCity"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reminder Mail</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div id="divHeading" class="divFloatLeft">
            <eluc:Title runat="server" ID="ucTitle" Text="Send Reminder" ShowMenu="false"></eluc:Title>
        </div>
        <div style="position: absolute; right: 0px">
            <eluc:TabStrip ID="MenuNewMail" runat="server" OnTabStripCommand="MenuNewMail_TabStripCommand">
            </eluc:TabStrip>
        </div>
    </div>
    <table width="100%" border="1">
        <tr>
            <td width="50%">
                <table width="100%">
                    <tr>
                        <td width="10%">
                            To
                        </td>
                        <td>
                            <asp:TextBox ID="txtTo" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cc
                        </td>
                        <td>
                            <asp:TextBox ID="txtCc" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Subject
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="input" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:HyperLink ID="lnkfilename" Target="_blank" runat="server" Width="14px" Height="14px"
                    ToolTip="Download File">
                </asp:HyperLink>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="98%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvAttachment_RowDataBound"
                    OnRowCommand="gvAttachment_RowCommand" OnRowDeleting="gvAttachment_RowDeleting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="File Name">
                            <ItemStyle HorizontalAlign="Left" Width="60%"></ItemStyle>
                            <HeaderTemplate>
                                File Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
                                <asp:Label ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </asp:Label>
                                <asp:Label ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMESSAGEID").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Center" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                Download
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                    Height="14px" ToolTip="Download File">
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2" width="100%">
                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="20" Width="100%"
                    CssClass="input" Font-Size="Small"/>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
