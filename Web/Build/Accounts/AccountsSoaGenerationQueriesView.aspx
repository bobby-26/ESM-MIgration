<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaGenerationQueriesView.aspx.cs"
    Inherits="AccountsSoaGenerationQueriesView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SOA Queries</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="div" runat="server">
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOwnersAccounts" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuSOALineItems" runat="server" OnTabStripCommand="MenuSOALineItems_TabStripCommand"></eluc:TabStrip>

            <div runat="server" id="divFind">
                <table width="100%">
                    <tr>
                        <td>
                            <%--<asp:LinkButton ID="lnkNumber" runat="server" Font-Size="Medium" ForeColor="Blue"></asp:LinkButton>--%>
                            <telerik:RadLabel ID="lnkNumber" runat="server" Font-Size="Medium" ForeColor="Blue"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; min-height: 750px;">
                            <table width="100%">
                                <tr>
                                    <td style="width: 100%;" valign="top" align="left">
                                        <div id="divGrid" style="position: relative; z-index: 1; width: 100%; height: 550px; top: 10px;">
                                            <asp:GridView ID="gvOwnersAccount" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                CellPadding="3" AllowSorting="true" ShowHeader="true" EnableViewState="false"
                                                OnRowDataBound="gvOwnersAccount_RowDataBound">
                                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                                <RowStyle Height="10px" />
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <HeaderTemplate>
                                                            <asp:Literal ID="lblquerid" runat="server" Text="queryid"></asp:Literal>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container,"DataItem.FLDQUERYID") %>
                                                            <%--<telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCNUMBER") %>'></telerik:RadLabel>--%>
                                                            <telerik:RadLabel ID="lblDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblVoucherDtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDTKEY") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Literal ID="lblVoucherDate" runat="server" Text="Voucher Row"></asp:Literal>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%--<%# DataBinder.Eval(Container,"DataItem.FLDDOCNUMBER") %>--%>
                                                            <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCNUMBER") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Literal ID="lblDescription" runat="server" Text="Questions/Answer"></asp:Literal>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERDETAILS") %>'></telerik:RadLabel><br />
                                                            <telerik:RadLabel ID="lblParticulars" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUERY") %>'></telerik:RadLabel><br />
                                                            <telerik:RadLabel ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGET") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lblActionHeader" runat="server">
                                                        Action
                                                            </telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                                CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                                                ToolTip="Edit" Visible="false"></asp:ImageButton>
                                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                            <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                                                CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAttachment"
                                                                ToolTip="Attachment" Visible="false"></asp:ImageButton>
                                                            <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                                                CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdNoAttachment"
                                                                Visible="false" ToolTip="No Attachment"></asp:ImageButton>
                                                            <asp:ImageButton runat="server" AlternateText="Details" ImageUrl="<%$ PhoenixTheme:images/anyfile.png %>"
                                                                CommandName="DETAILS" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDetails"
                                                                ToolTip="Details"></asp:ImageButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <%-- <td style="width: 50%; vertical-align: top; min-height: 750px; border: 1px solid #CCC;">
                        <div>
                            <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 750px;
                                width: 100%" frameborder="0"></iframe>
                            <asp:HiddenField ID="hdnScroll" runat="server" />
                        </div>
                    </td>--%>
                    </tr>
                </table>
            </div>
            <%--<div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand">
            </eluc:TabStrip>
        </div>--%>
        </div>
    </form>
</body>
</html>
