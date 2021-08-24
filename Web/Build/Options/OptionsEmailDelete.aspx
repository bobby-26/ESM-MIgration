<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsEmailDelete.aspx.cs"
    Inherits="OptionsEmailDelete" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Trash</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmOptionEmailDelete" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
        position: absolute;">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Trash" ShowMenu="false"></eluc:Title>
        </div>
        <asp:GridView Width="100%" Font-Size="11px" CellPadding="3" ID="gvDelete" runat="server"
            AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Left" EnableViewState="false" OnRowDataBound="gvDelete_RowDataBound">
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
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Image ID="imgAttachment" Width="15px" runat="server" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="To" SortExpression="FLDTO">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnTo" runat="server" Text='<%# Eval("FLDTO")%>' OnCommand="ShowDeleteMail"
                            CommandArgument='<%# Eval("FLDEMAILID")+","+Eval("FLDSESSIONID") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FLDSUBJECT" HeaderText="Subject" SortExpression="FLDSUBJECT">
                </asp:BoundField>
                <asp:TemplateField HeaderText="Create Date">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("FLDCREATEDDATE").ToString() != string.Empty ? ((DateTime)Eval("FLDCREATEDDATE")).ToLocalTime().ToString("dd-MMM-yyyy hh:mm tt") : string.Empty%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Attachment Size">
                    <ItemTemplate>
                        <asp:Label ID="lblFileSize" runat="server" Text=""></asp:Label>
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
                        <a href="OptionsEmail.aspx?mailId=<%# Eval("FLDEMAILID") %>&sessionId=<%# Eval("FLDSESSIONID") %>&mailFolderId=2">
                            <img id="Img1" src="<%$ PhoenixTheme:images/te_edit.png %>" runat="server" alt="Edit" title="Edit" /></a>                                               
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
