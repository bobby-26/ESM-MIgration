<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRegulationComplianceStatusFilter.aspx.cs" Inherits="Inspection_InspectionRegulationComplianceStatusFilter" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title>Compliance Filter</title>
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
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="ComplianceStatus" runat="server" OnTabStripCommand="ComplianceStatus_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            </div>
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
          <div id="divFind">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblComplianceStatus" runat="server" Text="Complian Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="chkComplianceStatusAdd" runat="server" AutoPostBack="false" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Text="Complian" Value="true" />
                                    <telerik:ButtonListItem Text="Non Complian" Value="false"/>
                                </Items>
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                </table>
            </div>
    </form>
</body>
</html>