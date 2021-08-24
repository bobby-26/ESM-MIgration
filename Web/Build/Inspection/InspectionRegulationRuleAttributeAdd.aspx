<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationRuleAttributeAdd.aspx.cs" Inherits="Inspection_InspectionRegulationRuleAttributeAdd" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rule Attribute Add</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                <eluc:TabStrip ID="NewAttribute" runat="server" OnTabStripCommand="NewAttribute_TabStripCommand"></eluc:TabStrip>
            </telerik:RadCodeBlock>
        
            <table cellspacing="1" width="100%">
                <tr>
                    <br />
                </tr>
                <tr>
                         <td>
                             <telerik:RadLabel RenderMode="Lightweight" ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                         </td>
                         <td>
                             <telerik:RadTextBox RenderMode="Lightweight" ID="txtAttributeCodeAdd" runat="server"  Enabled="true"></telerik:RadTextBox>
                         </td>
                    </tr>
                       <tr>
                         <td>
                             <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                         </td>
                         <td>
                             <telerik:RadTextBox RenderMode="Lightweight" ID="txtAttributeNameAdd" runat="server"  Enabled="true"></telerik:RadTextBox>
                         </td>
                    </tr>
                    <tr><br /></tr>
                    <tr>
                          <td>
                             <telerik:RadLabel RenderMode="Lightweight" ID="lblInclude" runat="server" Text="Include YN"></telerik:RadLabel>
                         </td>
                         <td>
                             <telerik:RadRadioButtonList runat="server" AutoPostBack="false" ID="chkIncludeYNAdd" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="true" Selected="true" />
                                    <telerik:ButtonListItem Text="No" Value="false" />
                                </Items>
                            </telerik:RadRadioButtonList>
                         </td>
                    </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </div>
    </form>
</body>
</html>

