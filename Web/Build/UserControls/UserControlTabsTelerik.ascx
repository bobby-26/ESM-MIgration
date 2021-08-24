<%@ Control Language="C#" AutoEventWireup="false" CodeFile="UserControlTabsTelerik.ascx.cs" Inherits="UserControlTabsTelerik" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadToolBar RenderMode="Lightweight" runat="server" ID="dlstTabs" OnButtonClick="dlstTabs_ButtonClick"
    OnButtonDataBound="dlstTabs_ButtonDataBound" SingleClick="Button"    
    EnableRoundedCorners="true" EnableShadows="true">
</telerik:RadToolBar>

