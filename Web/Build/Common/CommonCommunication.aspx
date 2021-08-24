<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonCommunication.aspx.cs" Inherits="Common_CommonCommunicationn" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Communication</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <eluc:TabStripTelerik ID="MenuSend" runat="server" OnTabStripCommand="MenuSend_TabStripCommand"></eluc:TabStripTelerik>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <table style="width: 99%">
            <tr>
                <td style="width: 20%">
                    <telerik:RadLabel ID="lblcomments" Width="100%" runat="server" Text="Post Your Comments Here"></telerik:RadLabel>
                </td>
                <td style="width: 79%">
                    <telerik:RadTextBox ID="txtcommunication" CssClass="gridinput_mandatory" runat="server" TextMode="multiline" Resize="Both" Height="70px" Width="100%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFileUpload" runat="server" Text="Attachment"></telerik:RadLabel>
                </td>
                <td>
                    <asp:FileUpload ID="txtFileUpload" runat="server" Width="270px" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td colspan="2"></td>
            </tr>
        </table>

        <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px; width: 99%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblsearch" runat="server" Text="Comments"></telerik:RadLabel>
                    <telerik:RadTextBox ID="txtsearch" runat="server"></telerik:RadTextBox>
                    &nbsp;&nbsp<telerik:RadLabel ID="lblDateBetween" runat="server" Text="Date Between"></telerik:RadLabel>
                    <eluc:Date runat="server" ID="txtDateFrom" CssClass="input" />
                    &nbsp;-&nbsp;
                       <eluc:Date runat="server" ID="txtDateTo" CssClass="input" />
                    <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                        OnClick="search_onclick" ID="ImageButton1" ToolTip="Search"></asp:ImageButton>
                </td>

            </tr>
        </table>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px; width: 98.7%"
                id="tblComments">
                <asp:Repeater ID="repCommunication" runat="server" OnItemDataBound="repCommunication_ItemDataBound">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="border-bottom: 1px solid;width:5%">
                                <telerik:RadLabel ID="lblsend" runat="server" Text="Posted By"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcommunicationid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMUNICATIONID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY")%>'></telerik:RadLabel>
                            </td>
                            <td style="border-bottom: 1px solid;width:15%">
                                <br />
                                <telerik:RadLabel ID="lblsender" runat="server" Font-Bold="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENDERNAME")%>'></telerik:RadLabel>

                            </td>

                            <td align="left" style="border-bottom: 1px solid; border-left: 1px solid;">
                                <telerik:RadLabel ID="lblmessage" runat="server" Text="Comments -"></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="attachment" ID="cmdAttachment" CommandName="ATTACHMENT" ToolTip="Attachment">
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/attachment.png %>">
                                </asp:LinkButton>
                                <br />
                                <div style="height: 34px; float: left; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                    <telerik:RadLabel ID="lblmsg" runat="server" Font-Bold="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMESSAGE")%>'></telerik:RadLabel>
                                </div>

                            </td>
                            <td align="left" valign="top" style="border-left: 1px solid; border-bottom: 1px solid;width:3%">
                                <telerik:RadLabel ID="lblsendby" runat="server" Text="Date/Time"></telerik:RadLabel>
                            </td>
                            <td align="Left" valign="top" style="border-bottom: 1px solid;width:10%">
                                <telerik:RadLabel ID="lbldate" runat="server" TextWrap="true" Font-Bold="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></telerik:RadLabel>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
