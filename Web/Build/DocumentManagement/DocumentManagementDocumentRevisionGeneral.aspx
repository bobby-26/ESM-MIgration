<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentRevisionGeneral.aspx.cs" Inherits="DocumentManagementDocumentRevisionGeneral" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Mode</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript">
            function OnSectionClick(event) {                
                var ifMoreInfo = parent.document.getElementById("ifMoreInfo");
                var divDocumentCategory = parent.document.getElementById("divDocumentCategory");                
                //window.parent.resizeFrameFromIframe(ifMoreInfo);
                //window.parent.resizeFrameFromIframe(divDocumentCategory);
                //jQuery(window.parent).scrollTop();
                //window.parent.document.body.scrollTop(0);
                //window.parent.location.href = "#topdiv";
            }
        </script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form runat="server" id="form1">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInvoice">
        <ContentTemplate>
            <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: text-top;
                position: static; height: auto; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader">
                    <div class="subHeader">
                        <eluc:Title runat="server" ID="ttlContent" Text="View" ShowMenu="false"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px;">
                    <eluc:TabStrip ID="MenuClose" runat="server" OnTabStripCommand="MenuClose_TabStripCommand">
                    </eluc:TabStrip>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <br />
    <%--    <input type="button" value="go to next page" onclick="location.href='#425b01a0-869a-e211-8a80-d8d385a9ef98'" ID="B1"  runat="server"/>--%>
    <%--<asp:HyperLink runat="server" NavigateUrl="#425b01a0-869a-e211-8a80-d8d385a9ef98" Text="Click to go to section 1"></asp:HyperLink>--%>
    <div id="divForm" runat="server" style="padding-left: 2%;">
        <span id="span1" runat="server" title="View" style="display: inline-block;"></span>
    </div>
    <br />
    <br />
    </form>
</body>
</html>
