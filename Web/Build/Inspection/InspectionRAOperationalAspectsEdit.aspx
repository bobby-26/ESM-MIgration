<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAOperationalAspectsEdit.aspx.cs" Inherits="InspectionRAOperationalAspectsEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aspect</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <%: Scripts.Render("~/bundles/js") %>
            <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" EnableAJAX="false">
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="Details"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                <table width="100%" cellspacing="2">
                    <tr>
                        <td valign="top">
                            <telerik:RadLabel ID="lblAspect" runat="server"  Text="Aspect"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAspect" runat="server" Width="480px" 
                                 TextMode="MultiLine" Rows="2" Resize="Both">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <telerik:RadLabel ID="lblopertaionalhazard" runat="server"  Text="Hazards / Risks"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtopertaionalhazard" runat="server" Width="480px" 
                                 TextMode="MultiLine" Rows="8" Resize="Both">
                            </telerik:RadTextBox>
                        </td>
                    </tr> 
                    <tr>
                        <td valign="top">
                            <telerik:RadLabel ID="lblcontrolprecautions" runat="server"  Text="Controls / Precautions "></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtcontrolprecautions" runat="server" Width="480px" 
                                 TextMode="MultiLine" Rows="8" Resize="Both">
                            </telerik:RadTextBox>
                        </td>
                    </tr>                    
                </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>