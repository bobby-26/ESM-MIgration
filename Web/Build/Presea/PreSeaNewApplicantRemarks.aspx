<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantRemarks.aspx.cs"
    Inherits="PreSeaNewApplicantRemarks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Discussion forum</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

  </telerik:RadCodeBlock>

    <script type="text/javascript">
          function resizediv() {
              var tbl = document.getElementById("tblComments");
              if (tbl != null) {
                  for (var i = 0; i < tbl.rows.length; i++) {
                      tbl.rows[i].cells[2].getElementsByTagName("div")[0].style.width = tbl.rows[i].cells[2].offsetWidth + "px";
                  }
              }
          } //script added for fixing Div width for the comments table
    </script>

</head>
<body>
    <body onload="resizediv();">
        <form id="frmPreSeaGeneralRemarks" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlDiscussion">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                    width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="div2" style="vertical-align: top">
                            <eluc:Title runat="server" ID="Title1" Text="General Remarks" ShowMenu="false"></eluc:Title>
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuDiscussion" runat="server" OnTabStripCommand="MenuDiscussion_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div>
                        <table width="90%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblGroup" runat="server" Visible="false"></asp:Label>
                                    First Name
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    Middle Name
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    Last Name
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    Batch
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBatch" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>                                
                                <td>
                                    <asp:Literal ID='lblActive' Text="Active / In-Active" runat="server"></asp:Literal>
                                </td>
                                <td >
                                    <asp:RadioButtonList ID="rblInActive" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="ActiveRemarks">
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="In-Active" Value="0" ></asp:ListItem> <%--Selected="True"--%>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:Literal ID="lblReason" Text="Reason" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Quick ID="ddlInactiveReason" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                        CssClass="dropdown_mandatory" OnTextChangedEvent="ddlReason_TextChanged" QuickTypeCode="123" />
                                </td>
                                <td>
                                    <asp:Label ID="lblFollowupDate" runat="server" Text="FollowUp Date"></asp:Label>
                                </td>
                                <td>
                                    <eluc:Date ID="txtFollowupDate" runat="server" CssClass="input_mandatory" />
                                </td>
                            </tr>
                        </table>
                    </div>                    
                    <div>
                        <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px;
                            border-style: solid; border-width: 1px;" width="99%">
                            <tr>
                                <td align="left" colspan="2">
                                    Post Your Comments Here
                                </td>
                                <td align="left" style="vertical-align: top;" colspan="2">
                                    <asp:TextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                                        Height="49px" TextMode="MultiLine" Width="692px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid;
                            border-width: 1px;" width="99%" id="tblComments" name="tblComments">
                            <asp:Repeater ID="repDiscussion" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td width="70px" style="border-bottom: 1px solid">
                                            Posted By
                                        </td>
                                        <td width="70px" style="border-bottom: 1px solid">
                                            <b>
                                                <%#DataBinder.Eval(Container, "DataItem.NAME")%>
                                            </b>
                                        </td>
                                        <td align="left" style="border-bottom: 1px solid; border-left: 1px solid;">
                                            Comments -<br />
                                            <div style="height: 34px; float: left; width: 692px; border-width: 1px; overflow-y: auto;
                                                white-space: normal; word-wrap: break-word; font-weight: bold">
                                                <%# Eval("DESCRIPTION")%>
                                            </div>
                                        </td>
                                        <td width="30px" align="left" valign="top" style="border-left: 1px solid; border-bottom: 1px solid">
                                            Date
                                        </td>
                                        <td width="50px" align="left" valign="top" style="border-bottom: 1px solid">
                                            <b>
                                                <%#DataBinder.Eval(Container, "DataItem.POSTEDDATE")%>
                                            </b>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
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
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </form>
    </body>
</body>
</html>
