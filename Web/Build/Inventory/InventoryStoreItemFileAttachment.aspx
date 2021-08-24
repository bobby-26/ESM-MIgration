<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreItemFileAttachment.aspx.cs" Inherits="InventoryStoreItemFileAttachment" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Attachment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function pageLoad()
            {
                var lightBox1 = $find('<%= RadLightBox1.ClientID %>');
                lightBox1.show();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        
          <telerik:RadLightBox RenderMode="Lightweight" ID="RadLightBox1" runat="server" Modal="true" LoopItems="true" ZIndex="100000" 
              ShowCloseButton="false" ShowNextButton="false" ShowPrevButton="false" ShowLoadingPanel="false" ShowMaximizeButton="false" >
            <ClientSettings AllowKeyboardNavigation="true" NavigationMode="Zone" FullscreenMode="Emulation" ShowItemsCounter="false">
            </ClientSettings>
            <Items>
            </Items>
        </telerik:RadLightBox>
    </form>
</body>
</html>
