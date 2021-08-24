<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobDetailCheck.aspx.cs" Inherits="PlannedMaintenanceJobDetailCheck" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDetail" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuComponentDetail">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuComponentDetail" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuJobDetail" runat="server" OnTabStripCommand="MenuJobDetail_TabStripCommand"></eluc:TabStrip>
        <telerik:RadEditor ID="txtJobDetail" runat="server" Width="100%" Height="400px" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            <ImageManager ViewPaths="~/Attachments/Purchase/Editor"
                UploadPaths="~/Attachments/Purchase/Editor"
                EnableAsyncUpload="true"></ImageManager>
        </telerik:RadEditor>
        <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
