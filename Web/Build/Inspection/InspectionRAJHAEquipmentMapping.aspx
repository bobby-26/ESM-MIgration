<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAJHAEquipmentMapping.aspx.cs" Inherits="InspectionRAJHAEquipmentMapping" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Equipment Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow;
                else if (window.frameElement && window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;
                return oWindow;
            }
            function CloseModal() {
                GetRadWindow().close();
                parent.location.reload();
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <table width="100%" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblengcontrol" runat="server" Text="Equipment"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input"
                                Width="90px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input"
                                Width="270px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowComponents" runat="server" ToolTip="Select Equipment">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                            <asp:LinkButton ID="lnkComponentAdd" runat="server" OnClick="lnkComponentAdd_Click" ToolTip="Add">
                                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                            </asp:LinkButton>
                            <div id="divComponents" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                                <table id="tblcomponents" runat="server">
                                </table>
                            </div>
                        </span>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
