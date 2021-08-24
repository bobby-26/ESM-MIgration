<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaTraineeCorrespondence.aspx.cs" Inherits="PreSeaTraineeCorrespondence" %>

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
            string destPath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + sessionid + "/" + filename;
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
    <title>Correspondence</title>
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
    <form id="frmCorrespondence" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlDOA">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="ucTitle" Text="Correspondence" ShowMenu="false" />
                            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Text="cmdHiddenSubmit" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuCorrespondence" runat="server" OnTabStripCommand="Correspondence_TabStripCommand"></eluc:TabStrip>
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
                            <td>Batch
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBatch" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>

                            <td runat="server" visible="false">Employee Number
                            </td>
                            <td runat="server" visible="false">
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
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
                                <asp:TextBox ID="txtTO" Width="500px" runat="server" CssClass="input"></asp:TextBox>
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
                            <td>
                                <asp:TextBox ID="txtSubject" Width="500px" runat="server" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Correspondence Type
                            </td>
                            <td>
                                <eluc:Quick ID="ddlCorrespondenceType" runat="server" QuickTypeCode="11" CssClass="input_mandatory" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <cc1:Editor ID="txtBody" runat="server" Width="100%" Height="275px" />
                                <div runat="server" id="txtBodyDiv" style="width: 100%; height: 275px; overflow: auto" class="readonlytextbox">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <font color="blue">Note: &nbsp;For searching click on the filter button below</font>

                            </td>
                        </tr>
                    </table>
                    <div style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuPreSeaCorrespondence" runat="server" OnTabStripCommand="PreSeaCorrespondence_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvCorrespondence" runat="server" AutoGenerateColumns="False" Width="100%"
                            OnRowEditing="gvCorrespondence_RowEditing" OnRowDataBound="gvCorrespondence_RowDataBound"
                            OnRowDeleting="gvCorrespondence_RowDeleting" CellPadding="3" ShowFooter="false"
                            ShowHeader="true" EnableViewState="false">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:ButtonField Text="Click" CommandName="Edit" Visible="false" />
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCorrespondenceId" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDCORRESPONDENCEID") %>'></asp:Label>
                                        <asp:Label ID="lblTraineeId" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDTRAINEEID") %>'></asp:Label>
                                        <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                        <asp:Label ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></asp:Label>
                                        <asp:Label ID="lblLockYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCKEDYN") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkCorrespondence" runat="server" CommandArgument='<%#Container.DataItemIndex%>'
                                            Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREATEDDATE", "{0:dd/MMM/yyyy}")%>'
                                            CommandName="EDIT"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        Subject
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        Type
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDCORRESPONDENCETYPENAME")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        User
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBYNAME") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        Corres/Email
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDTYPEOF")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                        <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="Attachment" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment"></asp:ImageButton>
                                        <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <a runat="server" id="cmdMail" target="filterandsearch">
                                            <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/email.png %>" />
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                    <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">&nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
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
