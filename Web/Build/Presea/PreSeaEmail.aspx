<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaEmail.aspx.cs" Inherits="PreSeaEmail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Mail" Src="~/UserControls/UserControlPreSeaMail.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mail Compose</title>

    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
            //parent.Openpopup('MailAttachment', '', '../Options/OptionsAttachment.aspx?mailsessionid=<%= ViewState["mailsessionid"] %>', 'xdata');

            window.open("Attachment.aspx?mailsessionid=" + '<%= ViewState["mailsessionid"] %>' + "", "Attachment", "height=200,width=950");
        }

        function DeleteFiles(e) {
            var kc = e == null ? event.keyCode : e.keyCode;
            if (kc == 46) {
                var LeftListBox = document.forms[0].lstAttachments;
                for (var i = (LeftListBox.options.length - 1); i >= 0; i--) {
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
       
    </script>
    </telerik:RadCodeBlock>
</head>
<body>

    

    <form id="frmOptionEmail" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Compose Mail" ShowMenu="false"></eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="EmailMenu" runat="server" OnTabStripCommand="EmailMenu_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div style="font-family: Tahoma; font-size: small">
            <asp:Panel ID="pnlMain" runat="server" ScrollBars="None" Width="100%">
                <table id="tblMain">
                    <tr>
                        <td>
                            Priority
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPriority" Width="100px" runat="server" CssClass="input">
                                <asp:ListItem Value="0" Selected="True">Normal</asp:ListItem>
                                <asp:ListItem Value="1">Low</asp:ListItem>
                                <asp:ListItem Value="2">High</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />
                        </td>
                    </tr>
                    <tr id="trMailFormat" runat="server" visible="false">
                        <td>
                            <asp:Literal ID="lblMailFormat" runat="server" Text="Mail Format"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Mail ID="ucMail" runat="server" CssClass="input" AppendDataBoundItems="true"
                                OnTextChangedEvent="ucMail_TextChanged" AutoPostBack="true" ShortNameFilter="OCPSCH,OCPCOL,OCPINS" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            To
                        </td>
                        <td>
                            <asp:TextBox ID="txtTO" Width="700px" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cc
                        </td>
                        <td>
                            <asp:TextBox ID="txtCC" Width="700px" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bcc
                        </td>
                        <td>
                            <asp:TextBox ID="txtBCC" Width="700px" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Subject
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" Width="700px" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td style="text-align: right">
                        <asp:LinkButton ID="lnkAttachment" Visible="false" OnClientClick="OpenWindow();" runat="server">Attachment</asp:LinkButton>
                    </td>
                    <td>
                        <asp:ListBox ID="lstAttachments" Visible="false" runat="server" CssClass="input" Width="650px"></asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblnote" runat="server" Text="Note:Double click to view the attachment"
                            CssClass="guideline_text" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <cc1:Editor ID="edtBody" runat="server" BorderColor="Red" Width="100%" Height="275px" />
                    </td>
                </tr>
            </table>
        </div>
        <eluc:Status ID="ucStatus" runat="server" Visible="false" />
    </div>
    </form>
</body>
</html>
