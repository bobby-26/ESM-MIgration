<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantCorrespondenceEmail.aspx.cs"
    Inherits="CrewNewApplicantCorrespondenceEmail" %>


<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EmailTemplate" Src="~/UserControls/UserControlEmailTemplate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Correspondence Email</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

    <%--<script runat="server">
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
    </script>--%>
</head>
<body>
    <form id="frmNewApplicantCorrespondenceEmail" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuCorrespondenceEmail" runat="server" OnTabStripCommand="CorrespondenceEmail_TabStripCommand" Title="Email"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
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
                        <%-- <asp:LinkButton ID="lnkAttachment" OnClientClick="OpenWindow();" runat="server">Attachment</asp:LinkButton>--%>
                        <asp:LinkButton runat="server" AlternateText="Delete" ID="lnkAttachment" OnClientClick="OpenWindow();"
                            ToolTip="Add Attachment">
                                <span class="icon"><i class="fas fa-paperclip"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td colspan="2">
                        <telerik:RadListBox ID="lstAttachments" RenderMode="Lightweight" runat="server" CheckBoxes="true" Width="500px">
                        </telerik:RadListBox>
                        <asp:Repeater ID="rpAttachment" runat="server">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkAtt" Target="_blank" Text='<%# Eval("FileName")%>' runat="server"
                                    NavigateUrl='<%#Session["sitepath"] + "/attachments/emailattachments/" + ViewState["mailsessionid"].ToString() + "/" + Eval("FileName")%>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:LinkButton runat="server" AlternateText="Delete" ID="imgDel" OnClick="btnAttDel_Click"
                            ToolTip="Delete Selected Attachment">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                        </asp:LinkButton>
                        <%--<asp:ListBox ID="lstAttachments" runat="server" CssClass="input" Width="500px"></asp:ListBox>
                            <asp:Repeater ID="rpAttachment" runat="server">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkAtt" Target="_blank" Text='<%# Eval("FileName")%>' runat="server"
                                        NavigateUrl='<%#Session["sitepath"] + "/attachments/emailattachments/" + ViewState["mailsessionid"].ToString() + "/" + Eval("FileName")%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:Repeater>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPressDelkeytoremovetheattachement" runat="Server" Text="(Press 'DEL' key to remove the attachment)"></telerik:RadLabel>
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
                        <eluc:Quick ID="ddlCorrespondenceType" runat="server" QuickTypeCode="11" CssClass="input_mandatory" Width="50%"
                            AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTemplate" runat="server" Text="Template"></telerik:RadLabel>
                    </td>
                    <td >
                        <eluc:EmailTemplate ID="ddlEmailTemplate" runat="server" OnTextChangedEvent="ddlEmailTemplate_SelectedIndexChanged"
                            AppendDataBoundItems="true" AutoPostBack="true"
                            EmailType='<%#General.GetNullableInteger(SouthNests.Phoenix.Common.PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 76, "NAP")) %>'></eluc:EmailTemplate>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadEditor ID="txtBody" runat="server" Width="99%" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                <Modules>
                    <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                    <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                    <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                    <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                </Modules>
                <ImageManager ViewPaths="~/Attachments/Crew/Editor"
                    UploadPaths="~/Attachments/Crew/Editor"
                    EnableAsyncUpload="true"></ImageManager>
            </telerik:RadEditor>
            <%--  <tr>
                        <td colspan="4">
                            <cc1:Editor ID="txtBody" runat="server" Width="100%" Height="275px" />
                        </td>
                    </tr>--%>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
