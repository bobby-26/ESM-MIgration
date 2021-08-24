<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaEmailTemplate.aspx.cs"
    Inherits="PreSeaEmailTemplate" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Presea Email Templates</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

         <script language="javascript" type="text/javascript">
        function AssignField() {
            var ddlListBox = document.getElementById(GetClientId('lstField'));
            if (ddlListBox.selectedIndex == -1) //if no field has been selected
            {
                alert("Please select the Field and then move it to Report templete designer editor.");
            }
            else {
                var i = ddlListBox.selectedIndex;
                var strFieldValue = ddlListBox.options[i].text;
                var editor = $find("<%= Editor1.ClientID %>");
                var editPanel = editor.get_editPanel();
                if (editPanel.get_activeMode() == 0) {

                    var designPanel = editPanel.get_activePanel();
                    designPanel._saveContent();
                    designPanel.insertHTML(strFieldValue);

                    setTimeout(function() { designPanel.onContentChanged(); editPanel.updateToolbar(); }, 0);

                    designPanel.focusEditor();
                }
                else if (editPanel.get_activeMode() == 2 || editPanel.get_activeMode() == 1) {
                    alert("You are not in design mode.");
                }
            }
        }

    </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmReportTemplate" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Email Template" ShowMenu="true" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuReportTemplate" runat="server" OnTabStripCommand="MenuReportTemplate_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="padding: 1px;
                    margin: 1px; border-style: solid; border-width: 1px;">
                    <tr>
                        <td>
                            Email Template
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEmail" runat="server" AppendDataBoundItems="true"
                                CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlEmail_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="">--Select--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Subject for the Mail
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" TextMode="SingleLine" runat="server" Width="400px"
                                CssClass="input_mandatory" MaxLength="500"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="padding: 1px;
                    margin: 1px; border-style: solid; border-width: 1px;">
                    <tr>
                        <td align="left" style="vertical-align: top;" width="20%">
                            <asp:ListBox ID="lstField" runat="server" Height="400px" Width="100%" Style="vertical-align: top;"
                                ondblclick="AssignField();"></asp:ListBox>
                        </td>
                        <td align="left" style="vertical-align: top">
                            <cc1:Editor ID="Editor1" runat="server" Height="400px" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Double click for adding the field in Report Template design Editor
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
