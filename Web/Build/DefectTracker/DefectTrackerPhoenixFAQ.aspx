<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerPhoenixFAQ.aspx.cs"
    Inherits="DefectTracker_DefectTrackerPhoenixFAQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.DefectTracker" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FAQ</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

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

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewLicenceEntry">
        <ContentTemplate>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Frequently Asked Questions" />
                    </div>
                </div>
                <table cellpadding="1" cellspacing="0" border="1px;" style="padding: 1px; margin: 1px;" width="100%"
                    id="tblComments" name="tblComments">
                    <asp:Repeater ID="repDiscussion" OnItemDataBound="repFAQ_OnItemDataBound" runat="server">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="font-size: small; font-weight: bold;" width="10%">
                                    Subject 
                                </td>
                                <td colspan="3" style="font-size: small; font-weight: bold; color: Blue;" width="70%">
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSUBJECT")%>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: small; font-weight: bold;" width="10%">
                                    Description 
                                </td>
                                <td style="font-weight: bold;" width="70%">
                                    <%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>
                                    </div>
                                </td>
                                <td width="10%">
                                    <b>Attachment :</b>
                                </td>
                                <td width="10%">
                                    <asp:Label ID="lblFile" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDFILENAMES")%>'
                                        Visible="false" />
                                    <asp:Label ID="lblFilePath" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDFILEPATH")%>'
                                        Visible="false" />
                                    <asp:HyperLink ID="lblAttachments" runat="server"> 
                                    </asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td  style="font-size: small; font-weight: bold;" valign="top" width="10%">
                                    <b>Steps</b>
                                </td>
                                <td colspan="3" width="100%">
                                    <br />
                                    <asp:Label ID="lblBody" runat="server" Text='<%# Eval("FLDBODY").ToString().Replace(Environment.NewLine,"<br />") %>'>
                                    </asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
