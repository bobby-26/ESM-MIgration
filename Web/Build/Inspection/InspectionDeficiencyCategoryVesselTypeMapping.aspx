<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDeficiencyCategoryVesselTypeMapping.aspx.cs" Inherits="InspectionDeficiencyCategoryVesselTypeMapping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Type Mapping</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" height="96%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                    <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand">
                    </eluc:TabStrip>
                    <table width="100%">
                        <tr>
                            <td width="15%">
                                &nbsp;&nbsp<telerik:RadLabel ID="lblCategory" runat="server" Font-Bold="true" Text="Deficiency Category"></telerik:RadLabel>
                            </td>
                            <td width="85%">
                                <telerik:RadTextBox ID="txtCategory" runat="server" ReadOnly="true" Width="300px"
                                    CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="2">
                                &nbsp;&nbsp<telerik:RadLabel ID="lblType" runat="server" Font-Bold="true" Text="Vessel Type"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="2">
                                <div id="dvInspection" runat="server" class="input" style="overflow: auto; width: 55%;
                                    left:1%; position: absolute; height: 350px;">
                                    <telerik:RadCheckBoxList ID="cblType" runat="server" Direction="Vertical" Columns="1">
                                    </telerik:RadCheckBoxList>
                                </div>
                            </td>
                        </tr>
                    </table>
     </telerik:RadAjaxPanel>
    </form>
</body>
</html>
