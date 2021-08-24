﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerInboxMailList.aspx.cs"
    Inherits="DefectTrackerInboxMailList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPModule" Src="~/UserControls/UserControlSEPbugModuleList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPMailUsername" Src="~/UserControls/UserControlSEPMailUsername.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SEPIncident" Src="~/UserControls/UserControlSEPIncidentList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MailManager</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div class="subHeader">
        <div id="div1" style="vertical-align: top">
            <eluc:Title runat="server" ShowMenu="false" ID="ucInboxMails"></eluc:Title>
        </div>
        <div style="position: absolute; top: 0px; right: 0px">
            <eluc:TabStrip ID="MenuMailInbox" TabStrip="True" runat="server" OnTabStripCommand="MenuMailInbox_TabStripCommand">
            </eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMailManager">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="divFind" style="margin-top: 0px;">
                <table width="50%">
                    <tr>
                        <td>
                            <font color="blue">
                                 <b>Notes :</b>
                                <li>Click the button &nbsp;<img id="Img5" runat="server" src="<%$ PhoenixTheme:images/24.png%>"
                            style="vertical-align: top" />&nbsp; for Maping or Unmaping mails </li>
                               
                            </font>
                            <br />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                        Mailbox
                                    </td>
                                    <td>
                                        <eluc:SEPMailUsername ID="ucSEPMailUsername" runat="server" CssClass="input" AutoPostBack="true"
                                            OnTextChangedEvent="Filter_Changed" AppendDataBoundItems="true" />
                                        &nbsp Call Number
                                        <asp:TextBox ID="txtCallNumber" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        From
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFrom" runat="server" MaxLength="200" CssClass="input"></asp:TextBox>&nbsp;<eluc:SEPVessel
                                            ID="UcVessel" runat="server" CssClass="input" AutoPostBack="true" OnTextChangedEvent="Vessel_Changed"
                                            AppendDataBoundItems="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        To
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTo" runat="server" MaxLength="200" CssClass="input"></asp:TextBox>&nbsp;<eluc:SEPVessel
                                            ID="ucVesselTo" runat="server" CssClass="input" AutoPostBack="true" OnTextChangedEvent="VesselTo_Changed"
                                            AppendDataBoundItems="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Cc
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCc" runat="server" MaxLength="200" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Subject
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubjectText" runat="server" MaxLength="100" CssClass="input"
                                            Width="160px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Body
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBodyText" runat="server" MaxLength="100" CssClass="input" Width="160px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Received Between
                                    </td>
                                    <td>
                                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input" />
                                        <eluc:Date ID="ucToDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            Module<br />
                            <eluc:SEPModule ID="ucSEPModule" runat="server" AutoPostBack="true" OnTextChangedEvent="Module_Changed" />
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkResponse" Text="No Response" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkArchived" runat="server" Text="Archived" AutoPostBack="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuMailManager" runat="server" OnTabStripCommand="MenuMailManager_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0">
                <asp:GridView ID="MailManager" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="MailManager_ItemDataBound" OnRowEditing="MailManager_RowEditing"
                    OnRowCommand="MailManager_RowCommand" OnRowDeleting="MailManager_RowDeleting"
                    OnRowCreated="MailManager_RowCreated" OnSorting="MailManager_Sorting" ShowFooter="True"
                    EnableViewState="False" AllowSorting="True">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="False" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="From">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="Attachments" runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Attachment">
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                From
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUniqueID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAILID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblCallNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCALLNUMBER") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblMessageId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMESSAGEID") %>'
                                    Visible="false"></asp:Label>
                                <%# DataBinder.Eval(Container, "DataItem.FLDFROM")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Subject
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSubject" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECTHINT") %>'></asp:LinkButton>
                                <eluc:Tooltip runat="server" ID="ucSubject" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Received
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRECEIVEDON")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                Responses
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkMessageOut" runat="server" CommandName="MAILSENT" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONSECOUNT") %>'></asp:LinkButton>
                                <asp:Label ID="lblMessageOut" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESPONSECOUNT") %>'></asp:Label>
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
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/24.png %>"
                                    CommandName="MAP" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdMap"
                                    ToolTip="Map/Unmap"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                </asp:GridView>
                <table width="100%" border="0" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap="nowrap" align="center">
                            <asp:Label ID="lblPagenumber" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblPages" runat="server">
                            </asp:Label>
                            <asp:Label ID="lblRecords" runat="server">
                            </asp:Label>&nbsp;&nbsp;
                        </td>
                        <td nowrap="nowrap" align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">
                            &nbsp;
                        </td>
                        <td nowrap="nowrap" align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap="nowrap" align="center">
                            <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                    <eluc:Status runat="server" ID="ucStatus" />
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
