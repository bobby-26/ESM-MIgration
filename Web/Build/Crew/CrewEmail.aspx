<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewEmail.aspx.cs" Inherits="CrewEmail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mail Compose</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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

        <script language="javascript" type="text/javascript">

            function OpenWindow() {
                openNewWindow('MailAttachment', '', '<%=Session["sitepath"]%>/Options/OptionsAttachment.aspx?mailsessionid=<%= ViewState["mailsessionid"] %>', 'xdata');
            }

            function DeleteFiles(e) {
                var kc = e == null ? event.keyCode : e.keyCode;
                if (kc == 46) {
                    var LeftListBox = document.forms[0].lstAttachments;
                    for (var i = (LeftListBox.options.length - 1) ; i >= 0; i--) {
                        if (LeftListBox.options[i].selected) {
                            PageMethods.Message('<%= ViewState["mailsessionid"] %>', LeftListBox.options[i].text);
                            LeftListBox.options[i] = null;
                        }
                    }
                }
            }
            function Openfile(obj) {
                var path = SitePath + "attachments/" + obj.options[obj.selectedIndex].value;
                Openpopup('download', '', path);
            }

            function OnClientItemDoubleClicked(sender, args) {             
                if (args) {
                    __doPostBack("<%=btnView.UniqueID %>", "");
                }
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>

    <form id="frmOptionEmail" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="EmailMenu" runat="server" OnTabStripCommand="EmailMenu_TabStripCommand"></eluc:TabStrip>
        <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click"  />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblMain">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td>                        
                        <telerik:RadComboBox ID="ddlPriority" Width="100px" runat="server" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value ="0" Text="Normal" Selected="true" />
                                <telerik:RadComboBoxItem Value ="1" Text="Low" />
                                <telerik:RadComboBoxItem Value ="2" Text="High" />
                            </Items>
                        </telerik:RadComboBox>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTO" Width="700px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCc" runat="server" Text="Cc"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCC" Width="700px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBcc" runat="server" Text="Bcc"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBCC" Width="700px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" Width="700px" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="text-align: right">
                        <asp:LinkButton runat="server" ID="lnkAttachment" 
                            ToolTip="Add Attachment">
                                <span class="icon"><i class="fas fa-paperclip"></i></span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <telerik:RadListBox ID="lstAttachments" RenderMode="Lightweight" runat="server" OnClientItemDoubleClicked="OnClientItemDoubleClicked" SelectionMode="Single" Width="700px">
                        </telerik:RadListBox>
                          <asp:LinkButton runat="server"  ID="imgDel" OnClick="btnAttDel_Click"
                            ToolTip="Delete Selected Attachment">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblnote" runat="server" Visible="false" Text="Note:Double click on the attachment name to see the details"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadEditor ID="edtBody" runat="server" Width="99%" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
