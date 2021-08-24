<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaTraineeCorrespondenceEmail.aspx.cs" Inherits="PreSeaTraineeCorrespondenceEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script runat="server">
    [System.Web.Services.WebMethod]
    public static void Message(string sessionid, string filename)
    {
        try
        {
            string destPath =  PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/"+ sessionid + "/" + filename;
            System.IO.File.Delete(destPath);
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
        }
    }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Correspondence Email</title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

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

    <form id="frmCorrespondenceEmail" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCorrespondenceEmail">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="ucTitle" Text="Correspondence Email" ShowMenu="false" />
                            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuCorrespondenceEmail" runat="server" OnTabStripCommand="CorrespondenceEmail_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>First Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>Middle Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>Last Name
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Batch
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBatch" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td runat="server" visible="false">Employee Number
                            </td>
                            <td runat="server" visible="false">
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td runat="server" visible="false">Rank
                            </td>
                            <td colspan="3" runat="server" visible="false">
                                <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <br />
                    <table width="100%" cellpadding="1" cellspacing="1">
                        <tr id="trFrom" runat="server">
                            <td>From
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtFrom" Width="500px" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trTO" runat="server">
                            <td>To
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtTO" Width="500px" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trCC" runat="server">
                            <td>Cc
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtCC" Width="500px" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trBCC" runat="server">
                            <td>Bcc
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtBCC" Width="500px" runat="server" CssClass="input"></asp:TextBox>
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
                            <td>(Press "DEL" key to remove the attachment)
                            </td>
                        </tr>
                        <tr>
                            <td>Subject
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtSubject" Width="500px" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Correspondence Type
                            </td>
                            <td>
                                <eluc:Quick ID="ddlCorrespondenceType" runat="server" QuickTypeCode="11" CssClass="input_mandatory"
                                    AppendDataBoundItems="true" />
                            </td>
                            <td>Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <cc1:Editor ID="txtBody" runat="server" Width="100%" Height="275px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <eluc:Status ID="ucStatus" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
