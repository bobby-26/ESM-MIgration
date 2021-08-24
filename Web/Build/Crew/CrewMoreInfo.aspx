<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewMoreInfo.aspx.cs"
    Inherits="CrewMoreInfo" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Editor" Src="~/UserControls/UserControlEditor.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>More Info.</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComment" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>            
            <eluc:TabStrip ID="MenuComment" runat="server" OnTabStripCommand="MenuComment_TabStripCommand" ></eluc:TabStrip>                      
            <telerik:RadEditor ID="txtComment" runat="server" Width="100%" Height="100%" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                 <Modules>
                    <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                    <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                    <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                    <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                </Modules>
                <ImageManager ViewPaths="~/Attachments/Crew/Editor"
                    UploadPaths="~/Attachments/Crew/Editor"
                    EnableAsyncUpload="true"></ImageManager>
            </telerik:RadEditor>
            <telerik:RadTextBox ID="txtAreaComment" TextMode="MultiLine" runat="server" Visible="false" Height="100%" Width="100%"></telerik:RadTextBox>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
