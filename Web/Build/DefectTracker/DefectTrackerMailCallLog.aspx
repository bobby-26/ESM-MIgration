<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerMailCallLog.aspx.cs"
    Inherits="DefectTrackerMailCallLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPbugModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPMailUsername" Src="~/UserControls/UserControlSEPMailUsername.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPIncident" Src="~/UserControls/UserControlSEPIncidentList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MailCallLog</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="True">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMailCallLog">
        <ContentTemplate>
            <div class="subHeader">
                <div id="div1" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Mail Logs" ShowMenu="false"></eluc:Title>
                </div>
                <div style="position: absolute; top: 0px; right: 0px">
                    <eluc:TabStrip ID="MenuCallNumber" runat="server" OnTabStripCommand="MenuCallNumber_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
            </div>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="divFind" style="margin-top: 0px;">
                <table width="75%">
                    <tr>
                        <td>
                            Call Number
                        </td>
                        <td>
                            <asp:TextBox ID="txtCallNumber" runat="server" CssClass="input" />
                        </td>
                        <td>
                            Importance
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbtnlstImportance" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnTextChanged="rbtnlstImport_TextChanged">
                                <asp:ListItem Text="Low" Value="LOW"></asp:ListItem>
                                <asp:ListItem Text="Medium" Value="MEDIUM"></asp:ListItem>
                                <asp:ListItem Text="High" Value="HIGH"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            Call status
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbtnCallStatus" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnTextChanged="rbtnlstImport_TextChanged">
                                <asp:ListItem Text="Open" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Close" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Closed By
                        </td>
                        <td>
                            <asp:TextBox ID="txtClosedBy" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            Remarks
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="input" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuMailCallLog" runat="server" OnTabStripCommand="MenuMailCallLog_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0">
                <asp:GridView ID="MailCallLog" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="MailCallLog_ItemDataBound" OnRowCommand="MailCallLog_RowCommand"
                    OnRowDeleting="MailCallLog_RowDeleting" OnRowCreated="MailCallLog_RowCreated"
                    ShowFooter="True" EnableViewState="False" AllowSorting="True">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="False" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="From">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                From
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUniqueID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAILINOUTID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblMessageId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMESSAGEID") %>'
                                    Visible="false"></asp:Label>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFROM")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                To
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDTO")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Subject
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSubject" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>'></asp:LinkButton>
                                <eluc:Tooltip runat="server" ID="ucSubject" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSENTRECEIVEDON")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                S/R
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSENTORRECEIVE")%>
                            </ItemTemplate>
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
                                <asp:ImageButton ID="cmdAttachment" runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Attachment">
                                </asp:ImageButton>
                                <img id="Attachment" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Ship Ack." ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                    CommandName="HISTORY" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdArcheiveHistory"
                                    ToolTip="Archive History"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
