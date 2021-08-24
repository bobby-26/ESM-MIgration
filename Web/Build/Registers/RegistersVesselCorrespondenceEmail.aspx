<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselCorrespondenceEmail.aspx.cs"
    Inherits="RegistersVesselCorrespondenceEmail" %>

<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<script runat="server">
    [System.Web.Services.WebMethod]
    public static void Message(string sessionid, string filename)
    {
        try
        {
            string destPath = HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/" + sessionid + "/" + filename);
            System.IO.File.Delete(destPath);
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
        }
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="hdVesselCorrespondence" runat="server">
    <title>Vessel Correspondence</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("");
                grid._gridDataDiv.style.height = (browserHeight - 80) + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <script language="javascript" type="text/javascript">
        function DeleteFiles(e, sessionid) {
            var kc = e == null ? event.keyCode : e.keyCode;
            if (kc == 46) {
                var LeftListBox = document.forms[0].lstAttachments;
                for (var i = (LeftListBox.options.length - 1) ; i >= 0; i--) {
                    if (LeftListBox.options[i].selected) {
                        PageMethods.Message(sessionid, LeftListBox.options[i].text);
                        LeftListBox.options[i] = null;
                    }
                }
            }
        }
    </script>
    <form id="frmCorrespondence" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" CssClass="hidden" />
        <eluc:TabStrip ID="MenuVesselCorrespondenceMail" runat="server" OnTabStripCommand="MenuVesselCorrespondenceMail_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtVesselName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr id="trFrom" runat="server">
                <td>
                    <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtFrom" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr id="trTO" runat="server">
                <td>
                    <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtTO" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr id="trCC" runat="server">
                <td>
                    <telerik:RadLabel ID="lblCc" runat="server" Text="Cc"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr id="trBCC" runat="server">
                <td>
                    <telerik:RadLabel ID="lblBcc" runat="server" Text="Bcc"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtBCC" Width="500px" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr id="trAtt" runat="server">
                <td style="text-align: right">
                    <asp:LinkButton ID="lnkAttachment" OnClientClick="OpenWindow();" runat="server">Attachment</asp:LinkButton>
                </td>
                <td colspan="2">
                    <asp:ListBox ID="lstAttachments" runat="server" CssClass="input" Width="500px"></asp:ListBox>
                    <asp:Repeater ID="rpAttachment" runat="server">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkAtt" Target="_blank" Text='<%# Eval("FileName")%>' runat="server"
                                NavigateUrl='<%#Session["sitepath"] + "/attachments/emailattachments/" + ViewState["mailsessionid"].ToString() + "/" + Eval("FileName")%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPressDELkeytoremovetheattachment" runat="server" Text="(Press &quot;DEL&quot; key to remove the attachment)"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtSubject" Width="500px" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCorrespondenceType" runat="server" Text="Correspondence Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ddlCorrespondenceType" runat="server" QuickTypeCode="11" CssClass="input_mandatory"
                        AppendDataBoundItems="true" Width="40%" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadEditor ID="txtBody" runat="server" Width="99%" EmptyMessage="" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                    </telerik:RadEditor>
                    <%--<cc1:Editor ID="txtBody" runat="server" Width="100%" Height="275px" />
                    <div runat="server" id="txtBodyDiv" style="width: 100%; height: 275px; overflow: auto"
                        class="readonlytextbox">
                    </div>--%>
                </td>
            </tr>
        </table>
        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
