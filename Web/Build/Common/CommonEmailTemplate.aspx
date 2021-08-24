<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonEmailTemplate.aspx.cs" Inherits="CommonEmailTemplate" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Email Template</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <script language="javascript" type="text/javascript">
            function AssignField() {
                var ddlListBox = document.getElementById(GetClientId('lstField'));
                if (ddlListBox.selectedIndex == -1) //if no field has been selected
                {
                    alert("Please select the Field and then move it to email templete designer editor.");
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

                        setTimeout(function () { designPanel.onContentChanged(); editPanel.updateToolbar(); }, 0);

                        designPanel.focusEditor();
                    }
                    else if (editPanel.get_activeMode() == 2 || editPanel.get_activeMode() == 1) {
                        alert("You are not in design mode.");
                    }
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <form id="frmEmailTemplate" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="ucTitle" Text="Email Template" ShowMenu="true" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuEmailTemplate" runat="server" OnTabStripCommand="MenuEmailTemplate_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;">
                        <tr>
                            <td>
                                <asp:Literal ID="lblTemplateType" runat="server" Text="Template Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ddlEmailType" runat="server" CssClass="input_mandatory" HardTypeCode="76" AppendDataBoundItems="true" OnTextChangedEvent="ddlEmailType_SelectedIndexChanged" AutoPostBack="true" />
                                <%-- <asp:DropDownList ID="ddlEmailType" runat="server" AppendDataBoundItems="true" 
                            CssClass="input_mandatory" AutoPostBack="true" 
                            onselectedindexchanged="ddlEmailType_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="">--Select--</asp:ListItem>
                        </asp:DropDownList>--%>
                            </td>
                            <td>
                                <asp:Literal ID="lblTemplate" runat="server" Text="Template"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlEmailTemplate" runat="server"
                                    AppendDataBoundItems="true" CssClass="input" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlEmailTemplate_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblTemplateName" runat="server" Text="Template Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTempleteName" runat="server" CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblTemplateDesc" runat="server" Text="Template Desc"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTempleteDesc" TextMode="SingleLine" runat="server" Width="400px" CssClass="input" MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Literal ID="lblAvailablefields" runat="server" Text="Available fields"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="lblSubject" runat="server" Text="Subject"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtsubject" runat="server" Width="400px" CssClass="input" MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;">
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
                                <asp:Literal ID="lblDoubleclickforaddingthefieldinEmailTemplatedesignEditor" runat="server" Text="Double click for adding the field in Email Template design Editor"></asp:Literal>
                            </td>
                            <td>&nbsp;
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
