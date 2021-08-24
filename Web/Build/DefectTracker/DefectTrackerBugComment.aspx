<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerBugComment.aspx.cs"
    Inherits="DefectTrackerBugComment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bug Edit</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript">
          function resizediv() {
              var tbl = document.getElementById("tblComments");
              if (tbl != null) {
                  for (var i = 0; i < tbl.rows.length; i++) {
                      tbl.rows[i].cells[0].getElementsByTagName("div")[0].style.width = 3*(tbl.rows[i].cells[2].offsetWidth) + "px";
                  }
              }
          } //script added for fixing Div width for the comments table
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader">
        <div id="div1" class="divFloatLeft">
            <eluc:Title runat="server" ID="ucTitle" Text="Comments" ShowMenu="false"></eluc:Title>
        </div>
        <div style="position: absolute; right: 0px">
            <eluc:TabStrip ID="MenuDiscussion" runat="server" OnTabStripCommand="MenuDiscussion_TabStripCommand">
            </eluc:TabStrip>
        </div>
    </div>
    <div class="subHeader">
        <div style="position: absolute; right: 0px">
            <eluc:TabStrip ID="MenuBugDiscussion" runat="server" OnTabStripCommand="MenuBugDiscussion_TabStripCommand">
            </eluc:TabStrip>
        </div>
    </div>
    <div>
        <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px;
            border-style: solid; border-width: 1px;" width="100%">
            <tr>
                <td align="left" colspan="5">
                    <font color="blue" size="0"><b>Post your comments here</b>
                        <li>Comments can be given only on the description of the issue logged</li>
                        <li>Comments cannot include enhancements to the issue logged</li>
                        <li>Enhancements need to be logged as separate defects</li>
                    </font>
                </td>
            </tr>
            <tr>
                <td  colspan="2">
                    Comment <br />
                    <asp:TextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                        Height="49px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid;
            border-width: 1px;" width="100%" id="tblComments" name="tblComments">
            <asp:Repeater ID="repDiscussion" runat="server">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td width="70%" class="input">
                            Comments -<br />
                            <div style="height: 60px; float: left; border-width: 1px; overflow: auto; white-space: normal;
                                word-wrap: break-word; font-weight: bold">
                                <%# Eval("FLDCOMMENTS")%>
                            </div>
                        </td>
                        <td width="15%">
                            Posted By
                            <br />
                            <b>
                                <%#DataBinder.Eval(Container, "DataItem.FLDUSERNAME")%>
                            </b>
                        </td>
                        <td width="15%">
                            Date
                            <br />
                            <b>
                                <%#DataBinder.Eval(Container, "DataItem.FLDPOSTEDDATE")%>
                            </b>    
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
