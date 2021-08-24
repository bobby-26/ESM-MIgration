<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewIncidentsEvaluationSuggestion.aspx.cs"
    Inherits="CrewIncidentsEvaluationSuggestion" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmEvaluationSuggestion" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>

            <eluc:TabStrip ID="MenuComment" runat="server" OnTabStripCommand="MenuComment_TabStripCommand"></eluc:TabStrip>

            <table>
                <tr>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblEvaluationPossibleCause" runat="server" Text="Evaluation / Possible Cause"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <telerik:RadEditor ID="txtEvaluation" runat="server" Width="100%" activemode="Design">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblSuggestionsActiontakenforimprovements" runat="server" Text="Suggestions / Action taken for improvements"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <telerik:RadEditor ID="txtSuggestions" runat="server" Width="100%" activemode="Design">
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
        </div>

    </form>
</body>
</html>
