<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsEmailSent.aspx.cs"
    Inherits="OptionsEmailSent" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Send Mails</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmOptionEmailSent" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
        position: absolute;">
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Sent Mails" ShowMenu="false"></eluc:Title>
        </div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:GridView Width="100%" Font-Size="11px" CellPadding="3" ID="gvSent" runat="server"
            AutoGenerateColumns="False" RowStyle-Wrap="true" OnRowDataBound="gvSent_RowDataBound"
            EnableViewState="false">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <RowStyle Height="10px" Wrap="True" />
            <Columns>
                <asp:TemplateField HeaderText="FLDEMAILID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblEmailId" runat="server" Text='<%# Bind("FLDEMAILID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FLDSESSIONID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSessionId" runat="server" Text='<%# Bind("FLDSESSIONID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FLDPRIORITY" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPriority" runat="server" Text='<%# Bind("FLDPRIORITY") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Image ID="imgPriority" Width="15px" runat="server" ImageUrl="<%$ PhoenixTheme:images/highPriority.png %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="20px" HeaderText="">
                    <ItemTemplate>
                        <asp:Image ID="imgAttachment" Width="15px" runat="server" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="To" ItemStyle-Width="200px" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnTo" runat="server" Text='<%# Eval("FLDTO")%>' OnCommand="ShowSentMail"
                            CommandArgument='<%# Eval("FLDEMAILID")+","+Eval("FLDSESSIONID") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FLDSUBJECT" HeaderText="Subject" SortExpression="FLDSUBJECT">
                </asp:BoundField>
                <asp:TemplateField HeaderText="Sent Date">
                    <ItemTemplate>
                        <asp:Label ID="lblSentDate" runat="server" Text='<%# Eval("FLDCREATEDDATE").ToString() != string.Empty ? ((DateTime)Eval("FLDCREATEDDATE")).ToLocalTime().ToString("dd-MMM-yyyy hh:mm tt") : string.Empty%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Attachment Size">
                    <ItemTemplate>
                        <asp:Label ID="lblFileSize" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div id="divPage" style="position: relative;">
            <table width="100%" border="0" class="datagrid_pagestyle">
                <tr>
                    <td nowrap align="center">
                        <asp:Label ID="lblPagenumber" runat="server">
                        </asp:Label>
                        <asp:Label ID="lblPages" runat="server">
                        </asp:Label>
                        <asp:Label ID="lblRecords" runat="server">
                        </asp:Label>&nbsp;&nbsp;
                    </td>
                    <td nowrap align="left" width="50px">
                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                    </td>
                    <td width="20px">
                        &nbsp;
                    </td>
                    <td nowrap align="right" width="50px">
                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                    </td>
                    <td nowrap align="center">
                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                        </asp:TextBox>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
